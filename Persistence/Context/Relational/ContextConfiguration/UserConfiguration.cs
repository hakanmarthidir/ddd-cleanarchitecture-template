using Domain.Entities.UserAggregate;
using Domain.Entities.UserAggregate.Enums;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Context.Relational
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
            builder.Property(b => b.CreatedBy).IsRequired();
            builder.Property(b => b.CreatedDate).IsRequired().HasDefaultValue<DateTimeOffset?>(DateTimeOffset.UtcNow);            
            builder.Property(b => b.UserType).HasDefaultValue<UserTypeEnum>(UserTypeEnum.User);
            builder.Property(b => b.ModifiedBy);
            builder.Property(b => b.ModifiedDate);
            builder.Property(b => b.DeletedBy);
            builder.Property(b => b.DeletedDate);
            builder.Property(b => b.Status).IsRequired().HasDefaultValue<Status>(Status.Active);
            //ValueType Settings
            builder.OwnsOne(o => o.Activation);            
            builder.OwnsOne(p => p.Activation).Property(p => p.IsActivated).IsRequired().HasDefaultValue<ActivationStatusEnum>(ActivationStatusEnum.NotActivated);
            builder.OwnsOne(p => p.Activation).Property(p => p.ActivationCode).IsRequired();
            builder.OwnsOne(p => p.Activation).Property(p => p.ActivationDate);            
        }

    }   
}