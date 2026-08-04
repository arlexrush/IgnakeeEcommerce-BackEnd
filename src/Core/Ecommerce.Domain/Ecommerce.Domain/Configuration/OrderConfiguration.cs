using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(a => a.OrderAddress, oa => { oa.WithOwner(); });
            builder.HasMany(o => o.OrderItems)
                    .WithOne(oa => oa.Order)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.orderStatus)
                    .HasConversion(os => os.ToString(), os => (OrderStatus)Enum.Parse(typeof(OrderStatus), os));

        }
    }
}
