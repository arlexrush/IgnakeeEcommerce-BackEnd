using CloudinaryDotNet;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.ProductTaxes
{
    public class TaxService : ITaxService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IAuthService _authService;

        public TaxService(IUnitOfWork? unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }


        public async Task<List<Tax>> AddTaxes(List<Tax> taxesIn)
        {
            var allTaxes = await _unitOfWork!.Repository<Tax>().GetAllAsync();
            var newTaxes=new List<Tax>();
            foreach (Tax tax in taxesIn) 
            { 
                if(!allTaxes.Contains(tax))
                {
                    newTaxes.Add(tax);
                }
            }
            
            allTaxes.ToList().AddRange(newTaxes);             
            
            _unitOfWork.Repository<Tax>().AddRange(allTaxes.ToList());
            await _unitOfWork.Complete();

            return newTaxes;
        }

        public async Task<List<Tax>> UpdateTaxes(List<Tax> taxesIn)
        {
            var newTaxes = new List<Tax>();
            foreach (Tax tax in taxesIn)
            {
                try
                {
                    var taxDb = await _unitOfWork!.Repository<Tax>().GetByIdAsync(tax.Id);
                    if (taxDb != null)
                    {
                        taxDb.Name = tax.Name.IsNullOrEmpty() ? taxDb.Name : tax.Name;
                        taxDb.Percentage = tax.Percentage;
                        taxDb.LastModifiedDate = DateTime.UtcNow;
                    }
                    newTaxes.Add(taxDb!);
                    await _unitOfWork.Repository<Tax>().UpdateAsync(taxDb!);
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
                        
            return newTaxes;
        }

        public async Task<List<Tax>> GetAllTaxes()
        {
            var allTaxes = await _unitOfWork!.Repository<Tax>().GetAllAsync();
            return allTaxes.ToList();
        }

        public async Task<List<Tax>> GetTaxesByCountryByProduct(int? countryId, int? productId)
        {
            List<Tax> taxesEntity;
            try
            {
                var taxesByCountry = await _unitOfWork!.Repository<Tax>().GetAsync(x => x.CountryId == countryId);
                if (!taxesByCountry.Any())
                {
                    throw new Exception("Not found these tax for CountryId");
                }
                var taxesByCountryList = taxesByCountry.ToList();
                taxesEntity = new List<Tax>();
                foreach (Tax taxItem in taxesByCountryList)
                {
                    var taxCountryProduct = await _unitOfWork.Repository<TaxByProduct>().GetAsync(x => x.ProductId == productId);
                    if (!taxCountryProduct.Any())
                    {
                        throw new Exception($"Not found these tax for this ProductId {productId}");
                    }
                    foreach (var cp in taxCountryProduct)
                    {
                        if (cp.Tax!.Id == taxItem.Id)
                        {
                            taxesEntity.Add(taxItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage=ex.Message;
                taxesEntity=new List<Tax>();
            }
            
            return taxesEntity;
        }

        public async Task<Tax> SelectTax(List<Tax> taxes, int productId, int countryId)
        {
            if (!taxes.Any())
            {
                throw new Exception("Not Found taxes in his Country");
            }
            var taxesResponse = new List<Tax>();
            foreach(Tax itemTax in taxes)
            {
                var taxByProductTarget=itemTax.TaxByProducts!.Where(x => x.ProductId == productId && x.IsActivated==true).ToList();
                foreach(TaxByProduct itemProduct in taxByProductTarget)
                {
                    var newItem= await _unitOfWork!.Repository<Tax>().GetByIdAsync(itemProduct.TaxId);
                    taxesResponse.Add(newItem);
                }
            }
            Tax taxResponse=new Tax();
            if (taxesResponse.Count() > 1)
            {
                var taxesSorted=taxesResponse.OrderBy(x => x.Percentage).ToList();
                foreach(Tax tax in taxesSorted)
                {
                    foreach(TaxByProduct t in tax.TaxByProducts!)
                    {
                        t.IsActivated = false;
                    }
                }
                taxResponse= taxesSorted.Last();
                foreach(TaxByProduct i in taxResponse.TaxByProducts!)
                {
                    i.IsActivated = true;
                }
                await _unitOfWork!.Repository<Tax>().UpdateAsync(taxResponse);
            }
            if (taxesResponse.Count() == 1)
            {
                taxResponse=taxesResponse.First();
            }
            if (!taxesResponse.Any())
            {
                var country = await _unitOfWork!.Repository<Country>().GetByIdAsync(countryId);
                var taxName = country.Name;
                taxResponse.Percentage = 0;
                taxResponse.Name = $"{taxName!.ToUpper()} IVA-{taxResponse.Percentage}";                
                Tax taxStored;
                try
                {
                    taxStored = await _unitOfWork.Repository<Tax>().AddAsync(taxResponse);
                }
                catch(Exception ex)
                {
                    var errorMessage=ex.Message;
                    throw;
                }                
                var taxByProductNew= new TaxByProduct();
                taxByProductNew.TaxId=taxStored.Id;
                taxByProductNew.ProductId = productId;
                taxByProductNew.IsActivated = true;
                taxStored.TaxByProducts!.Add(taxByProductNew);
                //_unitOfWork!.Repository<TaxByProduct>().AddEntity(taxByProductNew);
                //await _unitOfWork!.Complete();
                taxResponse = taxStored;
                _unitOfWork.Repository<Tax>().UpdateEntity(taxResponse);
                await _unitOfWork!.Complete();
            }
            
            return taxResponse;
        }

    }
}
