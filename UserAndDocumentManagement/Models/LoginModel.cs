using System.ComponentModel.DataAnnotations;

namespace UserAndDocumentManagement.Models
{
    public class LoginModel
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
