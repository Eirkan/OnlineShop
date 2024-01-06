using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Order.Domain.Common.Errors.Errors;

namespace Order.Infrastructure.Persistence.EntityConfigurations;

class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order.Domain.Entities.OrderAggregate.Order>
{
    public void Configure(EntityTypeBuilder<Order.Domain.Entities.OrderAggregate.Order> orderConfiguration)
    {
        orderConfiguration.ToTable("orders");

        orderConfiguration.Ignore(b => b.DomainEvents);

        orderConfiguration.Property(o => o.Id)
            .UseHiLo("orderseq");

        orderConfiguration
            .OwnsOne(o => o.Address);

        orderConfiguration
            .Property("_buyerId")
            .HasColumnName("BuyerId");

        orderConfiguration
            .Property(o => o.OrderDate)
            .HasColumnName("OrderDate");

        orderConfiguration
            .Property("_orderStatusId")
            .HasColumnName("OrderStatusId");

        orderConfiguration
            .Property("_paymentMethodId")
            .HasColumnName("PaymentMethodId");

        orderConfiguration.Property<string>("Description");

        //orderConfiguration.HasOne<PaymentMethod>()
        //.WithMany()
        //    .HasForeignKey("_paymentMethodId")
        //    .OnDelete(DeleteBehavior.Restrict);

        //orderConfiguration.HasOne<Buyer>()
        //    .WithMany()
        //    .HasForeignKey("_buyerId");

        orderConfiguration.HasOne(o => o.OrderStatus)
            .WithMany()
            .HasForeignKey("_orderStatusId");
    }
}
