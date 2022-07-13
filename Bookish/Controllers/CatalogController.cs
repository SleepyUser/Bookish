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
    public IActionResult BookEntry(BookInputModel model)
    {
        string bookName = model.BookName;
        string isbn = model.ISBN;
        string publisher = model.Publisher;
        DateTime datePublished = model.DatePublished;
        string[] authorNames = model.Author.Split(" ", 2);
        if (authorNames.Length == 1)
        {
            authorNames = new string[] { authorNames[0], "" };
        }
        
        int newCopies = model.NewCopies;
        using (var context = new LibraryContext())
        {
            Book? foundBook = context.Books.SingleOrDefault(a =>
                a.ISBN == isbn &&
                a.Title == bookName &&
                a.Publisher == publisher &&
                a.DatePublished == datePublished &&
                a.Author.AuthorSurname == authorNames[1] &&
                a.Author.AuthorForename == authorNames[0]);
            if (foundBook != null)
            {
                for (int i = 0; i < newCopies; i++)
                {
                    var copy = new Copy()
                    {
                        BookID = foundBook.BookID,
                        Comments = "This is a comment",
                    };
                    context.Copies.Add(copy);
                    context.SaveChanges();
                }
            }
            else
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
                AddXCopies(newCopies, context, book);
            }
        }
        return View();
    }

    public void AddXCopies(int newCopies, LibraryContext context, Book book)
    {
        for (int i = 0; i < newCopies; i++)
        {
            var copy = new Copy()
            {
                BookID = book.BookID,
                Comments = "This is a comment"
            };
            context.Copies.Add(copy);
            context.SaveChanges();
        }
    }
    
    public IActionResult BookList()
    {
        BookViewModel bvm = new BookViewModel();
        using (var context = new LibraryContext())
        {
            bvm.CatalogEntries = context.Books.Include(b => b.Author)
                .Include(b => b.CopyList).ToList();
        }
        return View(bvm);
    }

    public IActionResult EditBook(BookViewModel bvm)
    {
        return View("BookList", bvm);
    }

    public IActionResult DeleteBook(BookViewModel bvm)
    {
        return View("BookList", bvm);
    }
    /*public IActionResult SortList(BookViewModel modelToSort)
    {
        return View(modelToSort);
    }*/
    
}