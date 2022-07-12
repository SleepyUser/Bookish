using Bookish.Models;
using Microsoft.AspNetCore.Mvc;

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
        return View();
    }
}