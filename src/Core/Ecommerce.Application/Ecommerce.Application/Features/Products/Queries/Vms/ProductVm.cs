using Ecommerce.Application.Models.Product;
using Ecommerce.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Queries.Vms
{
    public class ProductVm
    {
        public string? Id { get; set; }

        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public TypeProduct typeProduct { get; set; }

        public string? UnitToSell { get; set; }

        public string? UnitToBuy { get; set; }


        public string? UnitToStore { get; set; }


        public string? UnitToProduction { get; set; }


        public string? Currency { get; set; }



        public decimal? Price { get; set; }


        public decimal? PriceTotal { get; set; }


        public decimal? PriceDiscount { get; set; }


        public decimal? PriceDiscountTotal { get; set; }


        public decimal? PriceTax { get; set; }


        public decimal? PriceTaxTotal { get; set; }


        public decimal? PriceTaxDiscount { get; set; }



        public decimal? PriceTaxDiscountTotal { get; }


        public int? Rating { get; set; }


        public int? RatingTotal { get; set; }


        public string? ProviderName { get; set; }


        public string? SellerName { get; set; }

        public PurchaseCriteria PurchaseCriteria { get; set; }


        public int? Stock { get; set; }


        public int? PurchaseLot { get; set; }


        public int? PurchaseLeadTime { get; set; }


        public string? PurchaseLeadTimeUnit { get; set; }


        public string? ReplenishmentPoint { get; set; }


        public int? SafetyStock { get; set; }


        public int? ProductDimensionId { get; set; }

        public string? CategoryNombre { get; set; }

        public int NumeroReviews { get; set; }


        public ProductStatus Status { get; set; }

        public string StatusLabel {
            get {
                switch (Status)
                {
                    case ProductStatus.Active:
                        return ProductStatusLabel.ACTIVE;

                    case ProductStatus.Desactive:
                        return ProductStatusLabel.DESACTIVE;

                    case ProductStatus.Obsolete:
                        return ProductStatusLabel.OBSOLET;

                    default: return ProductStatusLabel.DESACTIVE; ;
                }
            }

        }
        public int? CategoryId { get; set; }

        public virtual CategoryVm? Category { get; set; }

        public string? BarCode { get; set; }


        public string? QrCode { get; set; }


        //public Image? ProductPicture { get; set; }

        public virtual ICollection<ReviewVm>? reviews { get; set; }

        public virtual ICollection<ImageVm>? ProductImages { get; set; }
    }
}
