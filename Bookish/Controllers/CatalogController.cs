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
                Author? foundAuthor = context.Authors.SingleOrDefault(a =>
                    a.AuthorForename == author.AuthorForename && a.AuthorSurname == author.AuthorSurname);
                if (foundAuthor != null)
                {
                    author = foundAuthor;
                }
                else
                {
                    context.Authors.Add(author);
                }

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

    [HttpGet]
    public IActionResult Delete(int bookId)
    {

        Book? foundBook;
        using (var context = new LibraryContext())
        {
            foundBook = context.Books.SingleOrDefault(a => a.BookID == bookId);
            if (foundBook == null)
            {
                //Do Nothing
            }
            else
            {
                context.Remove(foundBook);
                context.SaveChanges();
            }
        }

        return RedirectToAction("BookList");
    }

    [HttpGet]
    public IActionResult Edit(int bookId)
    {
        Book? foundBook;
        using (var context = new LibraryContext())
        {
            foundBook = context.Books.SingleOrDefault(a => a.BookID == bookId);
            if (foundBook == null)
            {
                throw new Exception("NO BOOK!");
            }
            foundBook.Author = context.Authors.SingleOrDefault(a => a.AuthorID == foundBook.AuthorID);
        }
        return View(foundBook);
    }

    [HttpPost]
    public IActionResult Edit(Book book)
    {
        using (var context = new LibraryContext())
        {
            Book? foundBook = context.Books.SingleOrDefault(b => b.BookID == book.BookID);
            Author? foundAuthor = context.Authors.SingleOrDefault(a =>
                a.AuthorForename == book.Author.AuthorForename && a.AuthorSurname == book.Author.AuthorSurname);
            if(foundBook != null)
            {
                if (foundAuthor == null)
                {
                    foundAuthor = new Author()
                        { AuthorForename = book.Author.AuthorForename, AuthorSurname = book.Author.AuthorSurname };
                    
                    context.Authors.Add(foundAuthor);

                }
                book.Author = foundAuthor;
                Book freshBookCopy = context.Books.SingleOrDefault(b => b.BookID == book.BookID);
                if (freshBookCopy != null)
                {
                    freshBookCopy.Title = book.Title;
                    freshBookCopy.Author = foundAuthor;
                    freshBookCopy.Publisher = book.Publisher;
                    freshBookCopy.DatePublished = book.DatePublished;
                    freshBookCopy.ISBN = book.ISBN;
                    context.Update(freshBookCopy);
                }
                context.SaveChanges();
            }
        }
        return RedirectToAction("BookList");
    }
    /*public IActionResult SortList(BookViewModel modelToSort)
    {
        return View(modelToSort);
    }*/

}