using System;
using System.Collections.Generic;

namespace verbum_service_domain.Models;

/// <summary>
/// NEW, ACCEPTED, REJECTED, CANCELED, DEPOSITED, PAID
/// </summary>
public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid ClientId { get; set; }

    public DateTime? DueDate { get; set; }

    public string SourceLanguageId { get; set; } = null!;

    public string? OrderStatus { get; set; }

    public decimal? OrderPrice { get; set; }

    public bool? HasDiscount { get; set; }

    public Guid? DiscountId { get; set; }

    public bool HasTranslateService { get; set; }

    public bool HasEditService { get; set; }

    public bool HasEvaluateService { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? OrderName { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual ICollection<OrderReference> OrderReferences { get; set; } = new List<OrderReference>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Language SourceLanguage { get; set; } = null!;

    public virtual ICollection<Work> Works { get; set; } = new List<Work>();

    public virtual ICollection<Language> TargetLanguages { get; set; } = new List<Language>();
}
