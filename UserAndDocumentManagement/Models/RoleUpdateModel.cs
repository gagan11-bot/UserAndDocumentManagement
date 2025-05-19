using System.ComponentModel.DataAnnotations;

namespace UserAndDocumentManagement.Models
{
    public class RoleUpdateModel
    {
        [Key]
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
