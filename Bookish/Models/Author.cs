namespace Bookish.Models;

public class Author
{
    public int AuthorID{ get; set; }
    public string AuthorSurname { get; set; }
    public string AuthorForename { get; set; }
    public ICollection<Author> Aliases { get; set; }
    public ICollection<Book> AuthoredList { get; set; }
}