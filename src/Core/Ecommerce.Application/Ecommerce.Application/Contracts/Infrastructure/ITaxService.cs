using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts.Infrastructure
{
    public interface ITaxService
    {
        Task<List<Tax>> AddTaxes(List<Tax> taxesIn);
        Task<List<Tax>> UpdateTaxes(List<Tax> taxesIn);
        Task<List<Tax>> GetAllTaxes();
        Task<List<Tax>> GetTaxesByCountryByProduct(int? countryId, int? productId);
        Task<Tax> SelectTax(List<Tax> taxes, int productId, int countryId);
    }
}
