namespace verbum_service_domain.DTO.Request
{
    public class CreateJobsRequest
    {
        public List<Guid> WorkIds { get; set; }
        public List<CreateJobFileUpload> UploadFiles { get; set; }
        public List<string> TargetLanguageIds { get; set; }
    }
}
