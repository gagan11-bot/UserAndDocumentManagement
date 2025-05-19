using System.ComponentModel.DataAnnotations;

namespace UserAndDocumentManagement.Models
{
    public class RegisterModel
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "admin", "editor", "viewer"
    }
}
