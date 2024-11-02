using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Persistence.Entities;

namespace TodoList.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(a => a.Name)
            .IsRequired();  
        
        builder
            .HasMany(u => u.Notes)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId);
    }
}