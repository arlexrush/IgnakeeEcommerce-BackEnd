using Ecommerce.Domain;
using Ecommerce.Domain.Commons;
using Ecommerce.Infrastructure.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence
{
    public class EcommerceDbContext : IdentityDbContext<User>
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userName = "System";

            foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = userName;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = userName;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(x => x.Id).HasMaxLength(36);
            builder.Entity<User>().Property(x => x.NormalizedUserName).HasMaxLength(90);
            builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(36);
            builder.Entity<IdentityRole>().Property(x => x.NormalizedName).HasMaxLength(90);

            builder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.product)
                .HasForeignKey(r => r.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasMany(p => p.ProductImages)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasOne(x => x.ProductDimension)
                .WithOne(d => d.product)
                .HasForeignKey<ProductDimension>(p => p.ProductId);

            builder.Entity<ShoppingCart>()
                .HasMany(sc => sc.ShoppingCartItems)
                .WithOne(sci => sci.ShoppingCart)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Tax>()
                .HasOne(t => t.Country)
                .WithOne();

            builder.Entity<Tax>()
                .Property(t => t.Percentage)
                .HasPrecision(18, 2);

            builder.Entity<Country>()
                .HasMany(c => c.Taxes)
                .WithOne(t => t.Country)
                .HasForeignKey(x => x.CountryId);

            builder.Entity<TaxByProduct>()
                .HasKey(pt => new { pt.ProductId, pt.TaxId });

            builder.Entity<TaxByProduct>()
                .HasOne(x => x.Tax)
                .WithMany(x => x.TaxByProducts)
                .HasForeignKey(x => x.TaxId);

            builder.Entity<TaxByProduct>()
                .HasOne(x => x.Product)
                .WithMany(x => x.TaxByProducts)
                .HasForeignKey(x => x.ProductId);

            builder.Entity<Order>()
                .HasMany(x => x.ParTaxItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(z => z.Shipping)
                .WithOne();

            builder.Entity<Order>()
                .Property(x => x.PaymentStatus)
                .HasConversion<string>()
                .HasDefaultValue(PaymentStatus.Pending);

            builder.Entity<ProcessedIntegrationMessage>(entity =>
            {
                entity.HasKey(message => message.MessageId);
                entity.Property(message => message.MessageId).HasMaxLength(36);
                entity.Property(message => message.EventType).HasMaxLength(200).IsRequired();
                entity.Property(message => message.ContractVersion).IsRequired();
                entity.Property(message => message.ProcessedAtUtc).IsRequired();
            });

            builder.Entity<ProcessedBehaviorMessage>(entity =>
            {
                entity.HasKey(message => message.MessageId);
                entity.Property(message => message.MessageId).HasMaxLength(36);
                entity.Property(message => message.EventType).HasMaxLength(200).IsRequired();
                entity.Property(message => message.ContractVersion).IsRequired();
                entity.Property(message => message.ProcessedAtUtc).IsRequired();
            });

            builder.Entity<BehaviorProfile>(entity =>
            {
                entity.HasKey(profile => profile.UserId);
                entity.Property(profile => profile.UserId).HasMaxLength(36);
                entity.Property(profile => profile.HasConsented).IsRequired();
                entity.Property(profile => profile.LowestObservedProductPrice).HasPrecision(20, 2);
                entity.Property(profile => profile.HighestObservedProductPrice).HasPrecision(20, 2);
            });

            builder.Entity<BehaviorEvent>(entity =>
            {
                entity.HasKey(behaviorEvent => behaviorEvent.Id);
                entity.Property(behaviorEvent => behaviorEvent.UserId).HasMaxLength(36).IsRequired();
                entity.Property(behaviorEvent => behaviorEvent.Action).HasConversion<string>().HasMaxLength(40);
                entity.Property(behaviorEvent => behaviorEvent.ProductName).HasMaxLength(100);
                entity.Property(behaviorEvent => behaviorEvent.CategoryName).HasMaxLength(100);
                entity.Property(behaviorEvent => behaviorEvent.ProductPrice).HasPrecision(20, 2);
                entity.HasIndex(behaviorEvent => new { behaviorEvent.UserId, behaviorEvent.OccurredOnUtc });
            });
        }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Address>? Addresses { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Country>? Countries { get; set; }
        public DbSet<Image>? Images { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderAddress>? OrderAddresses { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<ProductDimension>? ProductDimensions { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<ShoppingCart>? ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem>? ShoppingCartItems { get; set; }
        public DbSet<Tax>? Taxs { get; set; }
        public DbSet<TaxByProduct>? TaxByProducts { get; set; }
        public DbSet<ParTaxItem>? parTaxItems { get; set; }
        public DbSet<Shipping>? shippings { get; set; }
        public DbSet<ShippingOperator>? shippingOperators { get; set; }
        public DbSet<ProcessedIntegrationMessage> ProcessedIntegrationMessages => Set<ProcessedIntegrationMessage>();
        public DbSet<ProcessedBehaviorMessage> ProcessedBehaviorMessages => Set<ProcessedBehaviorMessage>();
        public DbSet<BehaviorProfile> BehaviorProfiles => Set<BehaviorProfile>();
        public DbSet<BehaviorEvent> BehaviorEvents => Set<BehaviorEvent>();



    }
}
