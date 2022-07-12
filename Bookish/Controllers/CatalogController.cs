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
    public IActionResult BookEntry(BookInputModel model)
    {
        string bookName = model.BookName;
        string isbn = model.ISBN;
        string publisher = model.Publisher;
        DateTime datePublished = model.DatePublished;
        string[] authorNames = model.Author.Split(" ");
        using (var context = new LibraryContext())
        {
            var author = new Author()
            {
                AuthorSurname = authorNames[1],
                AuthorForename = authorNames[0],
            };
            context.Authors.Add(author);
            context.SaveChanges();
            var book = new Book()
            {
                ISBN = isbn,
                Title = bookName,
                Publisher = publisher,
                AuthorID = author.AuthorID,
                DatePublished = datePublished
            };
            context.Books.Add(book);
            context.SaveChanges();
        }
        /*using (var context = new LibraryContext())
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
        }*/
        return View();
    }
    
    public IActionResult BookList()
    {
        return View();
    }
}