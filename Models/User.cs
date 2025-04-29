using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Kullanıcı Adı zorunludur.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Kullanıcı Soyadı zorunludur.")]
        public string? UserSurname{get; set; }

        public string? UserAd
        {
            get
            {

                return UserName+" "+UserSurname;
            }
        }

        public List<Book> Books { get; set; } = new List<Book>();


       


    }
}
