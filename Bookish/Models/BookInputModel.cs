namespace Bookish.Models;

public class BookInputModel
{
    public string BookName { get; set; }
    public string ISBN { get; set; }
    public string Publisher { get; set; }
    public DateTime DatePublished { get; set; }
    public string Author { get; set; }
}