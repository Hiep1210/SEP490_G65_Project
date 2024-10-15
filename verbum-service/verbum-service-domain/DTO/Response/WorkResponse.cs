using System.ComponentModel.DataAnnotations;

namespace verbum_service_domain.DTO.Response
{
    public class WorkResponse
    {
        [Key]
        public Guid WorkId { get; set; }
        public string WorkName { get; set; }
        public string ServiceCode { get; set; }
        public string CreatedDate { get; set; }
        public string DueDate { get; set; }
    }
}
