﻿using System.Diagnostics.SymbolStore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bookish.Models;

public class BookViewModel
{
    public static List<Book> books = new List<Book>() {        new Book(
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

    public readonly string ISBN;
    public string title;
    public string authorForename;
    public string authorSurname;
    public string publisher;
}