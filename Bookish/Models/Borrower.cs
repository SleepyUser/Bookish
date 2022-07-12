namespace Bookish.Models;

public class Borrower
{
    public int BorrowerID { get; set; }
    public string Forename { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public ICollection<Copy> CopyList { get; set; }
}