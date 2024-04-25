using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class Product:BaseDomainModel
    {
        [Column(TypeName = "NVARCHAR(100)")]
        public string? ProductCode { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? ProductName { get; set; }


        [Column(TypeName = "NVARCHAR(4000)")]
        public string? Description { get; set; }


        public TypeProduct typeProduct { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? UnitToSell { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? UnitToBuy { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? UnitToStore { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? UnitToProduction { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? Currency { get; set; }


        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? Price { get; set; }


        //[Column(TypeName = "DECIMAL(20,2)")]
        //public decimal? PriceTotal { get; set; }


        //[Column(TypeName = "DECIMAL(20,2)")]
        //public decimal? PriceDiscount { get; set;}


        //[Column(TypeName = "DECIMAL(20,2)")]
        //public decimal? PriceDiscountTotal { get; set; }


        //[Column(TypeName = "DECIMAL(20,2)")]
        //public decimal? PriceTax { get; set;}


        //[Column(TypeName = "DECIMAL(20,2)")]
        //public decimal? PriceTaxTotal { get;set;}


        //[Column(TypeName = "DECIMAL(20,2)")]
        //public decimal? PriceTaxDiscount { get;set;}


        //[Column(TypeName = "DECIMAL(20,2)")]
        //public decimal? PriceTaxDiscountTotal { get;}


        [Column(TypeName = "INT")]
        public int? Rating { get; set;}


        [Column(TypeName = "INT")]
        public int? RatingTotal { get; set;}


        [Column(TypeName = "NVARCHAR(100)")]
        public string? ProviderName { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? SellerName { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? CountrySell { get; set; }

        public PurchaseCriteria PurchaseCriteria { get; set; }

        //public virtual Country? Country { get; set; }
        
        //public TaxStatus TaxStatus { get; set; }


        [Column(TypeName = "INT")]
        public int? Stock { get; set;}


        [Column(TypeName = "INT")]
        public int? PurchaseLot { get; set; }


        [Column(TypeName = "INT")]
        public int? PurchaseLeadTime { get; set;}


        [Column(TypeName = "NVARCHAR(100)")]
        public string? PurchaseLeadTimeUnit { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? ReplenishmentPoint { get; set;}


        [Column(TypeName = "INT")]
        public int? SafetyStock { get; set;}


        [Column(TypeName = "INT")]
        public int? ProductDimensionId { get; set;}
        public virtual ProductDimension? ProductDimension { get; set; }

        public ProductStatus Status { get; set; }


        [Column(TypeName = "INT")]
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? BarCode { get; set; }


        [Column(TypeName = "NVARCHAR(100)")]
        public string? QrCode { get; set; }


        //public Image? ProductPicture { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Image>? ProductImages { get; set; }
        public virtual ICollection<TaxByProduct>? TaxByProducts { get; set;}

    }
}
