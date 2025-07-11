using Microsoft.EntityFrameworkCore;
using PhoneBook.Core.Models;

namespace PhoneBook.DataAccess.DatabaseOperations;

public class PhoneBookContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public PhoneBookContext(DbContextOptions<PhoneBookContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

