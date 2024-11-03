using System;
using System.Collections.Generic;

namespace verbum_service_domain.Models;

/// <summary>
/// NEW, ACCEPTED, REJECTED, CANCELED, IN_PROGRESS, PAID, COMPLETED, IN_REVIEW
/// </summary>
public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid ClientId { get; set; }

    public DateTime? DueDate { get; set; }

    public string SourceLanguageId { get; set; } = null!;

    public string? OrderStatus { get; set; }

    public decimal? OrderPrice { get; set; }

    public Guid? DiscountId { get; set; }

    public bool HasTranslateService { get; set; }

    public bool HasEditService { get; set; }

    public bool HasEvaluateService { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? OrderName { get; set; }

    public string? RejectReason { get; set; }

    /// <summary>
    /// 255 char
    /// </summary>
    public string? OrderNote { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual ICollection<OrderReference> OrderReferences { get; set; } = new List<OrderReference>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual Language SourceLanguage { get; set; } = null!;

    public virtual ICollection<Work> Works { get; set; } = new List<Work>();

    public virtual ICollection<Language> TargetLanguages { get; set; } = new List<Language>();
}
