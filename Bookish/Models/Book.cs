namespace Bookish.Models;


public class Book
{
    public int BookID { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public int AuthorID { get; set; }
    public string Publisher { get; set; }
    public DateTime DatePublished { get; set; }
    public ICollection<Copy> CopyList { get; set; }
}