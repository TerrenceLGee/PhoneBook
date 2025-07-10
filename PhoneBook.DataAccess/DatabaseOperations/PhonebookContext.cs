using Microsoft.EntityFrameworkCore;
using PhoneBook.Core.Models;

namespace PhoneBook.DataAccess.DatabaseOperations;

public class PhonebookContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public PhonebookContext(DbContextOptions<PhonebookContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

