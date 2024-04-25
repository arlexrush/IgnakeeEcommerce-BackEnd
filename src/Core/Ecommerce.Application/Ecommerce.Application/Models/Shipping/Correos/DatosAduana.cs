using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class DatosAduana
    {
        // Envío con origen Península/Baleares y destino Canarias, Ceuta o Melilla. También para envíos con origen Canarias, Ceuta y Melilla destino fuera de su territorio. Internacionales
        // Cantidad del articulo contenido en el envio
        // 3 espacios
        public int? Cantidad { get; set; }

        // Envío con origen Península/Baleares y destino Canarias, Ceuta o Melilla. También para envíos con origen Canarias, Ceuta y Melilla destino fuera de su territorio. Internacionales
        // En caso de que se notifique el número tarifario será un campo de texto libre (tamaño 100) y si no, se debe introducir uno de los códigos de la tabla de mercancías del Anexo II  (tamaño 3 en ese caso) Se trunca al tamaño máximo.
        // 100 espacios
        public string? Descripcion { get; set; }

        // Envío con origen Península/Baleares y destino Canarias, Ceuta o Melilla. También para envíos con origen Canarias, Ceuta y Melilla destino fuera de su territorio. Internacionales
        // Peso neto en gramos del artículo contenido en el envío
        // 5 espacios
        public int? Pesoneto { get; set; }

        // Envío con origen Península/Baleares y destino Canarias, Ceuta o Melilla. También para envíos con origen Canarias, Ceuta y Melilla destino fuera de su territorio. Internacionales
        // Valor neto del artículo contenido en el envío, en cántimos de euro. 900,50 = 090050
        // 6 espacios
        public string? Valorneto { get; set; }

        // Envío con origen Península/Baleares y destino Canarias, Ceuta o Melilla. También para envíos con origen Canarias, Ceuta y Melilla destino fuera de su territorio. Internacionales
        // Numero tarifario del S.A. del articulo contenido en el envío. Valores válidos: códigos de 6, 8 o 10 dígitos
        // 10 espacios
        public string? NTarifario { get; set; }

        // Envío con origen Península/Baleares y destino Canarias, Ceuta o Melilla. También para envíos con origen Canarias, Ceuta y Melilla destino fuera de su territorio. Internacionales
        // Código del pais de origen del articulo contenido en el envío (Anexo III)
        // 2 espacios
        public string? PaisOrigen { get; set; }
    }
}
