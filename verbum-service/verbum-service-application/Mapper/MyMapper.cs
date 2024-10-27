using AutoMapper;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.DTO.Response;
using verbum_service_domain.Models;

namespace verbum_service_application.Mapper
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            CreateMap<UserSignUp, User>().ReverseMap();
            CreateMap<Category, CategoryInfoResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Category, CategoryInfo>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ReverseMap();
            CreateMap<Category, CategoryUpdate>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ReverseMap();
            CreateMap<Issue, CreateIssueRequest>().ReverseMap();
            CreateMap<Issue, UpdateIssueRequest>().ReverseMap();
            CreateMap<Language, LanguageResponse>().ReverseMap();
            CreateMap<Order, OrderCreate>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order, OrderDetailsResponse>()
                .ForMember(dest => dest.TargetLanguageId, opt => opt.MapFrom(src => src.TargetLanguages.Select(t => t.LanguageId).ToList()))
                .ForMember(dest => dest.ReferenceFileUrls, opt => opt.MapFrom(src => src.OrderReferences.Where(t => t.Tag == "REFERENCES").Select(t => t.ReferenceFileUrl).ToList()))
                .ForMember(dest => dest.TranslationFileUrls, opt => opt.MapFrom(src => src.OrderReferences.Where(t => t.Tag == "TRANSLATION").Select(t => t.ReferenceFileUrl).ToList()))
                .ForMember(dest => dest.DeliverableFileUrls, opt => opt.MapFrom(src => src.OrderReferences.Where(t => t.Tag == "DELIVERABLES").Select(t => t.ReferenceFileUrl).ToList()));
            CreateMap<OrderReference, UploadOrderFileRequest>().ReverseMap();
            CreateMap<IssueAttachment, UploadIssueAttachmentFiles>().ReverseMap();
            CreateMap<IssueAttachment, UpdateIssueAttachmentFile>().ReverseMap();
            CreateMap<UpdateLanguageSupportRequest, Language>()
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId.ToUpper())).ReverseMap();
            CreateMap<Work, WorkResponse>()
                .ForMember(dest => dest.SourceLanguageId, opt => opt.MapFrom(src => src.Order.SourceLanguageId))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Order.OrderStatus))
                .ForMember(dest => dest.TargetLanguageId, opt => opt.MapFrom(src => src.Order.TargetLanguages.Select(t => t.LanguageId).ToList()))
                .ForMember(dest => dest.ReferenceFileUrls, opt => opt.MapFrom(src => src.Order.OrderReferences.Where(t => t.Tag == "REFERENCES").Select(t => t.ReferenceFileUrl).ToList()))
                .ForMember(dest => dest.TranslationFileUrls, opt => opt.MapFrom(src => src.Order.OrderReferences.Where(t => t.Tag == "TRANSLATION").Select(t => t.ReferenceFileUrl).ToList()));
            CreateMap<Work, WorkCreate>().ReverseMap();
            CreateMap<Work, WorkUpdate>().ReverseMap();
            CreateMap<Discount, DiscountDTO>().ReverseMap();
            CreateMap<Discount, DiscountResponse>().ReverseMap();
            CreateMap<Issue, IssueResponse>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
                .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.Assignee.Name))
                .ForMember(dest => dest.IssueAttachments, opt => opt.MapFrom(src => src.IssueAttachments.Where(a => !a.IsDeleted)))
                .ReverseMap();
            CreateMap<Rating, RatingResponse>().ReverseMap();
            CreateMap<Rating, RatingCreate>().ReverseMap();
            CreateMap<Rating, RatingUpdate>().ReverseMap();
            CreateMap<Job, JobInfoResponse>()
                .ForMember(dest => dest.AssigneeNames, opt => opt.MapFrom(src => src.Assignees.Select(x => x.Name).ToList()))
                .ReverseMap();
        }
    }
}
