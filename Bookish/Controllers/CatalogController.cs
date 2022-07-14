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
        string publisher = model.BookInput.Publisher;
        DateTime datePublished = model.BookInput.DatePublished;
        string[] authorNames = model.BookInput.Author.Split(" ", 2);
        if (authorNames.Length == 1)
        {
            authorNames = new string[] { authorNames[0], "" };
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
            if (foundBook != null)
            {
                AddCopiesOfBook(newCopies, context, foundBook);
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
    
    [HttpGet]
    public IActionResult BookList()
    {
        BookViewModel bvm = new BookViewModel();
        using (var context = new LibraryContext())
        {
            bvm.CatalogEntries = context.Books.Include(b => b.Author)
                .Include(b => b.CopyList)
                .ToList();
        }
        return View("BookList",bvm);
    }
    /*[HttpPost]
    public IActionResult BookList(int bookId, string operation)
    {
        switch (operation)
        {
            case "delete":
                throw new Exception("HELP DELETE");
                return Delete(new BookViewModel());
                break;
            case "edit":
                throw new Exception("HELP EDIT");
                return Edit(new BookViewModel());
                break;
            default:
                return View("Error");
        }
    }*/
    [HttpGet]
    public IActionResult Delete(int bookId)
    {

        Book? foundBook;
        using (var context = new LibraryContext())
        {
            foundBook = context.Books.SingleOrDefault(a => a.BookID == bookId );
            if (foundBook == null)
            {
                throw new Exception("NO BOOK!");
            }
            else
            {
                context.Remove(foundBook);
                context.SaveChanges();
            }
        }

        return BookList();
    }
    [HttpGet]
    public IActionResult Edit(int bookId)
    {
        Book? foundBook;
        using (var context = new LibraryContext())
        {
            foundBook = context.Books.SingleOrDefault(a => a.BookID == bookId);
            foundBook.Author = context.Authors.SingleOrDefault(a => a.AuthorID == foundBook.AuthorID);
            if (foundBook == null)
            {
                throw new Exception("NO BOOK!");
            }
        }
        return View(foundBook);
    }

    [HttpPost]
    public IActionResult Edit(Book b)
    {
        using (var context = new LibraryContext())
        {
            Book? foundBook = context.Books.SingleOrDefault(a => a.BookID == b.BookID);
            Author? foundAuthor = context.Authors.SingleOrDefault(a =>
                a.AuthorForename == b.Author.AuthorForename && a.AuthorSurname == b.Author.AuthorSurname);
            if (foundBook == null)
            {
                //error stuff
            }
            else
            {
                if (foundAuthor == null)
                {
                    foundAuthor = new Author()
                        { AuthorForename = b.Author.AuthorForename, AuthorSurname = b.Author.AuthorSurname };
                    
                    context.Authors.Add(foundAuthor);
                    context.SaveChanges();
                }
                b.Author = foundAuthor;
                Book _b = context.Books.Where(s => s.BookID == b.BookID).FirstOrDefault<Book>();
                if (_b != null)
                {
                    _b.Title = b.Title;
                    _b.Author = foundAuthor;
                    _b.Publisher = b.Publisher;
                    _b.DatePublished = b.DatePublished;
                    _b.ISBN = b.ISBN;
                    context.Update(_b);
                    context.SaveChanges();
                }
            }
        }
        return BookList();
    }
    /*public IActionResult SortList(BookViewModel modelToSort)
    {
        return View(modelToSort);
    }*/

}