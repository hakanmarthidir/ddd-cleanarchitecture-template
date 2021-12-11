using System;
using DDDTemplate.Domain.Entities.UserAggregate;
using DDDTemplate.Domain.Entities.UserAggregate.Enums;
using DDDTemplate.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDTemplate.Persistence.Context.Relational
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.FirstName).IsRequired().HasMaxLength(255);
            builder.Property(b => b.LastName).IsRequired().HasMaxLength(255);
            builder.Property(b => b.Email).IsRequired().HasMaxLength(255);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(b => b.Password).IsRequired().HasMaxLength(255);

            builder.Property(b => b.CreatedDate).IsRequired().HasDefaultValue<DateTimeOffset?>(DateTimeOffset.UtcNow);
            builder.Property(b => b.UserType).HasDefaultValue<UserType>(UserType.User);
            builder.Property(b => b.ModifiedDate);
            builder.Property(b => b.DeletedDate);
            builder.Property(b => b.Status).IsRequired().HasDefaultValue<Status>(Status.Active);
            builder.Property(b => b.Activation.IsActivated).IsRequired().HasDefaultValue<ActivationStatus>(ActivationStatus.NotActivated);
            builder.Property(b => b.Activation.ActivationCode).IsRequired();
            builder.Property(b => b.Activation.ActivationDate);

        }

    }
}