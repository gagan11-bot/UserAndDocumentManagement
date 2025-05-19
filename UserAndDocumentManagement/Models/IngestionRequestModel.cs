using System.ComponentModel.DataAnnotations;

namespace UserAndDocumentManagement.Models
{
    public class IngestionRequestModel
    {
        [Key]
        public int DocumentId { get; set; }
        public string FilePath { get; set; }
        public string TriggeredBy { get; set; }
    }
}
