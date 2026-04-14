using Microsoft.EntityFrameworkCore;
using MiniAuthSystem.Models;

namespace MiniAuthSystem;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}