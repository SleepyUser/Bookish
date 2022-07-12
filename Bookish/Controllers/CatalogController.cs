using Bookish.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Controllers;

public class CatalogController : Controller
{
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }
    
    // GET
    public IActionResult BookEntry()
    {
        using (var context = new LibraryContext())
        {
            var author = new Author()
            {
                AuthorSurname = "Sinha",
                AuthorForename = "Sanjib",
            };
            
            context.Authors.Add(author);
            context.SaveChanges();
            
            var book = new Book()
            {
                ISBN = "9781720025191",
                Title = "How to Build A PHP 7 Framework: With an Introduction to Composer, Interface, Trait, Horizontal Reuse of code, PDO, and MVC Pattern",
                Publisher = "independent",
                AuthorID = author.AuthorID,
                DatePublished = new DateTime(2018,09,01)
            };
            
            context.Books.Add(book);
            context.SaveChanges();
        }
        return View();
    }
    
    public IActionResult BookList()
    {
        BookViewModel bvm = new BookViewModel();
        using (var context = new LibraryContext())
        {
            bvm.CatalogEntries = context.Books.Include(b => b.Author)
                .Include(b => b.CopyList).ToList();
            /*var query = (from b in context.Books
                join a in context.Authors on b.AuthorID equals a.AuthorID
                select (
                    //b.Title
                    a.AuthorSurname
                    //Forename = a.AuthorForename,
                    //b.Publisher,
                    //b.DatePublished,
                    //b.ISBN
                    //b.CopyList.Count
                ));
            //bvm.RawCatalogEntries = query;
            var list = query.ToList();*/
            
        }
        return View(bvm);
    }
    
    
}