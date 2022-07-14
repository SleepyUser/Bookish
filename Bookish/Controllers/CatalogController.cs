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
    
    [HttpPost]
    public IActionResult AddNewBook(BookOrBorrowerInputModel model)
    {
        string bookName = model.BookInput.BookName;
        string isbn = model.BookInput.ISBN;
        string publisher = (model.BookInput.Publisher == null) ? "" : model.BookInput.Publisher;
        DateTime datePublished = model.BookInput.DatePublished;
        string authorForename = (model.BookInput.AuthorForename == null) ? "" : model.BookInput.AuthorForename;
        string authorSurname = (model.BookInput.AuthorSurname == null) ? "" : model.BookInput.AuthorSurname;
        int newCopies = model.BookInput.NewCopies;
        using (var context = new LibraryContext())
        {
            Book? foundBook = context.Books.SingleOrDefault(a =>
                a.ISBN == isbn &&
                a.Title == bookName &&
                a.Publisher == publisher &&
                a.DatePublished == datePublished &&
                a.Author.AuthorSurname == authorSurname &&
                a.Author.AuthorForename == authorForename);
            Author? foundAuthor = context.Authors.SingleOrDefault(a =>
                a.AuthorForename == authorForename &&
                a.AuthorSurname == authorSurname);
            if (foundBook != null)
            {
                AddCopiesOfBook(newCopies, context, foundBook);
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
                AddCopiesOfBook(newCopies, context, book);
            }
            else{
                var author = new Author()
                {
                    AuthorSurname = authorSurname,
                    AuthorForename = authorForename,
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
                AddCopiesOfBook(newCopies, context, book);
            }
        }
        return View();
    }
    
    public IActionResult AddNewBorrower(BookOrBorrowerInputModel model)
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

    public void AddCopiesOfBook(int newCopies, LibraryContext context, Book book)
    {
        List<Copy> newCopyList = new List<Copy>();
        for (int i = 0; i < newCopies; i++)
        {
            var copy = new Copy()
            {
                BookID = book.BookID,
                Comments = "This is a comment"
            };
            newCopyList.Add(copy);
        }
        context.Copies.AddRange(newCopyList);
        context.SaveChanges();
    }
    
    public IActionResult BookList()
    {
        BookViewModel bvm = new BookViewModel();
        using (var context = new LibraryContext())
        {
            bvm.CatalogEntries = context.Books.Include(b => b.Author)
                .Include(b => b.CopyList)
                .ToList();
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

}