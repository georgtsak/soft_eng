using Microsoft.EntityFrameworkCore;
using Soft_Eng_Spring2024.Models;
namespace Soft_Eng_Spring2024.Data;

using Microsoft.EntityFrameworkCore;
using Soft_Eng_Spring2024.Models;

public class DataContext : DbContext
{
    public DataContext() { }    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
    }

    public DbSet<User> Users { get; set; }

public DbSet<Soft_Eng_Spring2024.Models.Announcement> Announcement { get; set; } = default!;

public DbSet<Soft_Eng_Spring2024.Models.Event> Event { get; set; } = default!;

public DbSet<Soft_Eng_Spring2024.Models.Poll> Poll { get; set; } = default!;

    
}

