using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    public class Book
    {
        [Key]
        public int BookId {  get; set; }

        public string BookName { get; set; }
       
        public string AuthorName { get; set; }
        
        public string PublisherName { get; set; }
        
        public string PublisherYear {  get; set; }
       
        public int UserId {get; set; }

        public User Users { get; set; }

        





    }
}
