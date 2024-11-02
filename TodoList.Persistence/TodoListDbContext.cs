using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoList.Persistence.Configurations;
using TodoList.Persistence.Entities;

namespace TodoList.Persistence;

public class TodoListDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public TodoListDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<NoteEntity> Notes { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new NoteConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}