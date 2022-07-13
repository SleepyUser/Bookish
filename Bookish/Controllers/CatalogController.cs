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
    public IActionResult BookEntry(DefaultInputModel model)
    {
        string bookName = model.BookInput.BookName;
        string isbn = model.BookInput.ISBN;
        string publisher = model.BookInput.Publisher;
        DateTime datePublished = model.BookInput.DatePublished;
        string[] authorNames = model.BookInput.Author.Split(" ", 2);
        if (authorNames.Length == 1)
        {
            authorNames = new [] { authorNames[0], "" };
        }
        int newCopies = model.BookInput.NewCopies;
        using (var context = new LibraryContext())
        {
            Book? foundBook = context.Books.SingleOrDefault(a =>
                a.ISBN == isbn &&
                a.Title == bookName &&
                a.Publisher == publisher &&
                a.DatePublished == datePublished &&
                a.Author.AuthorSurname == authorNames[1] &&
                a.Author.AuthorForename == authorNames[0]);
            Author? foundAuthor = context.Authors.SingleOrDefault(a =>
                a.AuthorForename == authorNames[0] &&
                a.AuthorSurname == authorNames[1]);
            if (foundBook != null)
            {
                AddXCopies(newCopies, context, foundBook);
            }
            else if (foundAuthor != null)
            {
                var book = new Book()
                {
                    ISBN = isbn,
                    Title = bookName,
                    Publisher = publisher,
                    AuthorID = foundAuthor.AuthorID,
                    DatePublished = datePublished
                };
                context.Books.Add(book);
                context.SaveChanges();
                AddXCopies(newCopies, context, book);
            }
            else{
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
    
    public IActionResult BorrowerEntry(DefaultInputModel model)
    {
        string surname = model.BorrowerInput.Surname;
        string forename = model.BorrowerInput.Forename;
        string phonenumber = model.BorrowerInput.PhoneNumber;
        using (var context = new LibraryContext())
        {
            Borrower? foundBorrower = context.Borrowers.SingleOrDefault(a => a.Surname == surname && a.Forename == forename && a.PhoneNumber == phonenumber );
            if (foundBorrower != null)
            {
                //member already exists, do something
            }
            else
            {
                var borrower = new Borrower()
                {
                    Surname = surname,
                    Forename = forename,
                    PhoneNumber = phonenumber
                };
                context.Borrowers.Add(borrower);
                context.SaveChanges();
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
            bvm.EntryQuery = context.Books.Include(b => b.Author)
                .Include(b => b.CopyList);
            bvm.CatalogEntries = bvm.EntryQuery.ToList();
        }
        return View(bvm);
    }
    
    public IActionResult BorrowerList()
    {
        BorrowerViewModel bvm = new BorrowerViewModel();
        using (var context = new LibraryContext())
        {
            bvm.EntryQuery = context.Borrowers.Include(b => b.CopyList).Include(b => b.BorrowList);
            bvm.CatalogEntries = bvm.EntryQuery.ToList();
        }
        return View(bvm);
    }

    /*public IActionResult SortList(BookViewModel modelToSort)
    {
        return View(modelToSort);
    }*/

}