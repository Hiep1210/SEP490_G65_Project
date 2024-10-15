using System.ComponentModel.DataAnnotations;

namespace verbum_service_domain.DTO.Response
{
    public class WorkResponse
    {
        [Key]
        public Guid WorkId { get; set; }
        public string OrderName { get; set; }
        public string SourceLanguageId { get; set; }
        public List<string> TargetLanguageId { get; set; }
        public List<string> TranslationFileUrls { get; set; }
        public List<string> ReferenceFileUrls { get; set; }
        public string OrderStatus { get; set; }
    }
}
