namespace Bookish.Models;

public class BookInputModel
{
    public string BookName { get; set; }
    public string ISBN { get; set; }
    public string Publisher { get; set; }
    public DateTime DatePublished { get; set; }
    public string AuthorForename { get; set; }
    public string AuthorSurname { get; set; }
    public int NewCopies { get; set; }
}