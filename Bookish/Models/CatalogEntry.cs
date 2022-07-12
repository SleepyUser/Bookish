namespace Bookish.Models;

public class CatalogEntry
{
    public CatalogEntry(string title, string authorSurname, string authorForename, string publisher, DateTime datePublished, string isbn)//, int copies = 0)
    {
        Title = title;
        Publisher = publisher;
        DatePublished = datePublished;
        Isbn = isbn;
        //Copies = copies;
        AuthorSurname = authorSurname;
        AuthorForename = authorForename;
    }
    public string Title;
    public string AuthorSurname;
    public string AuthorForename;
    public string Publisher;
    public DateTime DatePublished;
    public string Isbn;
    //public int Copies;
}