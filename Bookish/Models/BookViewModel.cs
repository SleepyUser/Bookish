using System.Diagnostics.SymbolStore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bookish.Models;

public class BookViewModel
{
    public List<Book> books = new List<Book>() {        new Book(
        "Charlie and the Chocolate Factory", 
        "0142410314", 
        "Usborne", 
        "Roald", 
        "Dahl"),};
}

public class Book
{
    public Book(string title, string isbn, string publisher, string authorSurname, string authorForename)
    {
        this.title = title;
        this.ISBN = isbn;
        this.publisher = publisher;
        this.authorSurname = authorSurname;
        this.authorForename = authorForename;
    }

    private readonly string ISBN;
    private string title;
    private string authorForename;
    private string authorSurname;
    private string publisher;
}