using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoList.Persistence.Configurations;
using TodoList.Persistence.Entities;

namespace TodoList.Persistence;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options) { }

    public DbSet<NoteEntity> Notes { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new NoteConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}