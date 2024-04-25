using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Order
{
    public class QuoteResponse
    {
        public decimal? TotalPrice { get; set; } // Precio total de la cotización
        public string? Currency { get; set; } // Moneda en la que se expresa el precio
        public int EstimatedDeliveryDays { get; set; } // Días estimados de entrega
        public string? ServiceType { get; set; } // Tipo de servicio de envío (express, estándar, etc.)
                                                // Otros detalles relevantes de la cotización, como opciones de envío, seguros, etc.
    }
}
