using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Core.Validators;
using TodoList.Persistence.Entities;

namespace TodoList.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<NoteEntity>
{
    public void Configure(EntityTypeBuilder<NoteEntity> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title)
            .HasMaxLength(NotesValidator.MAX_TITLE_LENGTH)
            .IsRequired();
        
        builder.Property(n => n.Description)
            .HasMaxLength(NotesValidator.MAX_DESCRIPTION_LENGTH);
        
        builder
            .HasOne(n => n.User)
            .WithMany(n => n.Notes);
    }
}