namespace Bookish.Models;

public class BorrowInstance
{
    public int BorrowInstanceID { get; set; }
    public int BorrowerID { get; set; }
    public int CopyID { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public bool IsOverdue { get; set; }
}