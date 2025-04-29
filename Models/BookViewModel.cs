using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    public class BookViewModel
    {

        public int BookId {  get; set; }

        [Required(ErrorMessage = "Kitap Adı zorunludur.")]
        public string? BookName {  get; set; }

        [Required(ErrorMessage = "Yazar Adı zorunludur.")]
        public string? AuthorName {  get; set; }

        [Required(ErrorMessage = "Yayın Adı zorunludur.")]
        public string? PublisherName {  get; set; }

        [Required(ErrorMessage = "Yayın Yılı zorunludur.")]
        public string? PublisherYear {  get; set; }

     
        public int UserId {  get; set; }


    }
}
