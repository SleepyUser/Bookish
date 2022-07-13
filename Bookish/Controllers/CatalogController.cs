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
        string publisher = model.BookInput.Publisher ?? "";
        DateTime datePublished = model.BookInput.DatePublished;
        string authorForename = model.BookInput.AuthorForename ?? "";
        string authorSurname = model.BookInput.AuthorSurname ?? "";
        int newCopies = model.BookInput.NewCopies;
        
        using (var context = new LibraryContext())
        {
            Book? foundBook = context.Books.SingleOrDefault(b =>
                b.ISBN == isbn &&
                b.Title == bookName &&
                b.Publisher == publisher &&
                b.DatePublished == datePublished &&
                b.Author.AuthorSurname == authorSurname &&
                b.Author.AuthorForename == authorForename);
            
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
            else
            {
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
    
    [HttpPost]
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
    
    
    [HttpGet]
    public IActionResult DisplayBookList()
    {
        BookViewModel bvm = new BookViewModel();
        using (var context = new LibraryContext())
        {
            bvm.CatalogEntries = context.Books.Include(b => b.Author)
                .Include(c => c.CopyList)
                .ToList();
        }
        return View(bvm);
    }
    
    [HttpGet]
    public IActionResult DisplayBorrowerList()
    {
        BorrowerViewModel bvm = new BorrowerViewModel();
        using (var context = new LibraryContext())
        {
            bvm.CatalogEntries = context.Borrowers.Include(b => b.CopyList)
                .Include(c => c.BorrowList)
                .ToList();
        }
        return View(bvm);
    }
    
    [HttpGet]
    public IActionResult GetCopyList(int bookInputId)
    {
        CopyViewModel cvm = new CopyViewModel();
        using (var context = new LibraryContext())
        {
            cvm.Book = context.Books
                .Include(b => b.CopyList)
                .Include(b => b.Author)
                .Single(b => b.BookID == bookInputId);
        }
        return View(cvm);
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