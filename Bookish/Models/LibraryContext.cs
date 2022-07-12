using Microsoft.EntityFrameworkCore;

namespace Bookish.Models;

public class LibraryContext: DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrower> Borrowers { get; set; }
    public DbSet<BorrowInstance> BorrowInstances { get; set; }
    public DbSet<Copy> Copies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost;Database=LibraryDB;Trusted_Connection=True;");
    }
}