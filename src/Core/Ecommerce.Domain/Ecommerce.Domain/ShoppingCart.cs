using Ecommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class ShoppingCart : BaseDomainModel
    {
        public ShoppingCart()
        {
            ShoppingCartItems = new List<ShoppingCartItem>();
        }

        public Guid? ShoppingCartMasterId { get; set; }
        public virtual ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }

        public void AddItem(ShoppingCartItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (item.Quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(item.Quantity), "Quantity must be greater than zero.");
            }

            if (item.Price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(item.Price), "Price cannot be negative.");
            }

            ShoppingCartItems ??= new List<ShoppingCartItem>();

            var existingItem = ShoppingCartItems.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingItem is not null)
            {
                existingItem.Quantity += item.Quantity;
                existingItem.Price = item.Price;
                existingItem.ProductName = item.ProductName;
                existingItem.ProductPicture = item.ProductPicture;
                existingItem.Category = item.Category;
                existingItem.Stock = item.Stock;
                return;
            }

            item.ShoppingCartMasterId = ShoppingCartMasterId ?? item.ShoppingCartMasterId;
            item.ShoppingCartId = Id;
            ShoppingCartItems.Add(item);
        }

        public void UpdateItemQuantity(int productId, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");
            }

            ShoppingCartItems ??= new List<ShoppingCartItem>();
            var item = ShoppingCartItems.FirstOrDefault(x => x.ProductId == productId);
            if (item is null)
            {
                throw new InvalidOperationException("The requested product is not present in the cart.");
            }

            if (quantity == 0)
            {
                ShoppingCartItems.Remove(item);
                return;
            }

            item.Quantity = quantity;
        }

        public void RemoveItem(int productId)
        {
            ShoppingCartItems ??= new List<ShoppingCartItem>();
            var item = ShoppingCartItems.FirstOrDefault(x => x.ProductId == productId);
            if (item is not null)
            {
                ShoppingCartItems.Remove(item);
            }
        }

        public decimal GetSubtotal()
        {
            return ShoppingCartItems?.Sum(item => item.Price * item.Quantity) ?? 0m;
        }
    }
}
