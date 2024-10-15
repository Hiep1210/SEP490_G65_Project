namespace verbum_service_domain.DTO.Request
{
    public class OrderUpdate
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
        public string SourceLanguageId { get; set; }
        public List<string> TargetLanguageIdList { get; set; }
        public bool TrasnlateService { get; set; }
        public bool EditService { get; set; }
        public bool EvaluateService { get; set; }
        public string? Reference { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? DiscountId { get; set; }
    }
}
