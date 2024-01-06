using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities.OrderAggregate;

namespace Order.Infrastructure.Persistence.EntityConfigurations;

class OrderStatusEntityTypeConfiguration
    : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> orderStatusConfiguration)
    {
        orderStatusConfiguration.ToTable("orderstatus");

        orderStatusConfiguration
            .Property("Value")
            .HasColumnName("Id")
            .ValueGeneratedNever()
            ;

        orderStatusConfiguration.Property(o => o.Name)
            .HasMaxLength(200);
    }
}
