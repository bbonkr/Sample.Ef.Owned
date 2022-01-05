using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sample.Entities;

namespace Sample.Data.TypeConfigurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode();

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode();

        builder.OwnsOne(x => x.Metadata, y =>
        {
            y.WithOwner().HasForeignKey($"{nameof(User)}Id");

            y.Property(a => a.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            y.Property(a => a.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            y.Property(a => a.UpdatedAt);
            y.Property(a => a.DeletedAt);

            y.ToTable($"{nameof(User)}{nameof(Metadata)}");
        });

        builder.OwnsMany(x => x.Addresses, y =>
        {
            y.WithOwner().HasForeignKey($"{nameof(User)}Id");

            y.HasKey(a => a.Id);

            y.Property(a => a.Id).IsRequired();

            y.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

            y.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(500);

            y.Property(a => a.Detail)
            .IsRequired(false)
            .HasMaxLength(500);

            y.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(500);

            y.Property(a => a.State)
            .IsRequired()
            .HasMaxLength(500);

            y.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(500);

            y.Property(a => a.Zipcode)
            .IsRequired(false)
            .HasMaxLength(10);

            y.ToTable($"{nameof(User)}{nameof(Address)}");
        });

        builder.OwnsOne(x => x.Profile, y =>
        {
            y.WithOwner().HasForeignKey($"{nameof(User)}Id");

            y.Property(a => a.Height)
            .IsRequired(false)
            ;

            y.Property(a => a.Weight)
            .IsRequired(false);

            y.ToTable($"{nameof(User)}{nameof(Profile)}");
        });
    }
}
