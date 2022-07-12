﻿using Bookish.Models;
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

        Book foundBook = new Book();
        using (var context = new LibraryContext())
        {
            foundBook = context.Books.SingleOrDefault(a => a.ISBN == isbn);
            if (foundBook != null)
            {
                //add one to copies of current book
                var copy = new Copy()
                {
                    BookID = foundBook.BookID,
                    Comments = "This is a comment",
                };
                context.Copies.Add(copy);
                context.SaveChanges();
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
                var copy = new Copy()
                {
                    BookID = book.BookID,
                    Comments = "This is a comment"
                };
                context.Copies.Add(copy);
                context.SaveChanges();
            }
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
        }
        return View(bvm);
    }
    
    
}