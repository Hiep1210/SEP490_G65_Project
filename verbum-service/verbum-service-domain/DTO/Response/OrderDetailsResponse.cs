namespace verbum_service_domain.DTO.Response
{
    public class OrderDetailsResponse
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
        public string CreatedDate { get; set; }
        public string DueDate { get; set; }
        public string SourceLanguageId { get; set; }
        public List<string> TargetLanguageId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderPrice { get; set; }
        public string DiscountId { get; set; }
        public bool HasTranslateService { get; set; }
        public bool HasEditService { get; set; }
        public bool HasEvaluateService { get; set; }
        public List<string> TranslationFileUrls { get; set; }
        public List<string> ReferenceFileUrls { get; set; }
        public List<string> DeliverableFileUrls { get; set; }
        public List<string> DeleteddFileUrls { get; set; }
        public string PaymentStatus { get; set; }
    }
}
