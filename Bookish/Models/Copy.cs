namespace Bookish.Models;

public class Copy
{
    public int CopyID { get; set; }
    public int BookID{ get; set; }
    public virtual Book Book { get; set; }
    public string Comments { get; set; }
    public ICollection<BorrowInstance> BorrowInstanceList { get; set; }
}