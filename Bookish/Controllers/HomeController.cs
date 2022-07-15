using System.Diagnostics;
using Bookish.API;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;

namespace Bookish.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        BookOrBorrowerInputModel bbim = new BookOrBorrowerInputModel();
        bbim.BookInput = new BookInputModel();
        return View(bbim);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public async Task<IActionResult> ISBNEntry(string isbn)
    {
        Book testBook = await BooksAPIHandler.ISBNToBookInfo(isbn);
        BookOrBorrowerInputModel bvm = new BookOrBorrowerInputModel();
        bvm.BookInput = new BookInputModel();
        bvm.BookInput.Author = testBook.Author.AuthorForename + " " + testBook.Author.AuthorSurname;
        bvm.BookInput.ISBN = testBook.ISBN;
        bvm.BookInput.Publisher = testBook.Publisher;
        bvm.BookInput.DatePublished = testBook.DatePublished;
        bvm.BookInput.BookName = testBook.Title;
        return View("Index",bvm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
