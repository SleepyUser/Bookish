using System.Globalization;
using System.Net;
using Bookish.Models;

namespace Bookish.API;
using Newtonsoft.Json.Linq;

public class BooksAPIHandler
{
   static readonly HttpClient client = new HttpClient();

   private static async Task<string> MakeApiReq(string uri)
   {
      var responseBody = "";
      try
      {
         HttpResponseMessage response = await client.GetAsync(uri);
         response.EnsureSuccessStatusCode();
         responseBody = await response.Content.ReadAsStringAsync();
      }
      catch(HttpRequestException e)
      {
         Console.WriteLine("\nException Caught!");	
         Console.WriteLine("Message :{0} ",e.Message);
      }

      return responseBody;
   }
   public static async Task<Book> ISBNToBookInfo(string isbn)
   {
      // Make API call
      string uri = $"https://www.googleapis.com/books/v1/volumes?q={isbn}&{Keys.BooksKey}";
      string responseBody = await MakeApiReq(uri);
           
      // Deserialize JSON
      var jsonObject = JObject.Parse(responseBody);
      Book book = new Book();
      var bookInfo = jsonObject["items"]["volumeInfo"];
      book.Title = bookInfo["Title"].ToString();
      book.ISBN = isbn;
      string[] authname = bookInfo["authors"][0].ToString().Split(" ", 2);
      book.Author.AuthorForename = authname[0];
      if (authname.Length == 2)
         book.Author.AuthorSurname = authname[1];
      else
         book.Author.AuthorSurname = "";
      book.Publisher = bookInfo["publisher"].ToString();
      DateTime dt = new DateTime();
      if (DateTime.TryParseExact(bookInfo["publishedDate"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
             DateTimeStyles.None, out dt))
      {
         book.DatePublished = dt;
      };
      
      return book;
   }
   
   // $"https://www.googleapis.com/books/v1/volumes?q={ISBN}&{Keys.BooksKey}"
}