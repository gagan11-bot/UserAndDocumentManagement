using System.ComponentModel.DataAnnotations;

namespace UserAndDocumentManagement.Models
{
    public class IngestionStatus
    {
        [Key]
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string Status { get; set; } // Pending, Ingesting, Completed, Failed, Cancelled
        public string Message { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
