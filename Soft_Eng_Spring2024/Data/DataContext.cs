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
}

