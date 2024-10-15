using System;
using System.Collections.Generic;

namespace verbum_service_domain.Models;

public partial class Language
{
    public string LanguageName { get; set; } = null!;

    public string LanguageId { get; set; } = null!;

    public bool Support { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Revelancy? RevelancySourceLanguage { get; set; }

    public virtual Revelancy? RevelancyTargetLanguage { get; set; }

    public virtual ICollection<Order> OrdersNavigation { get; set; } = new List<Order>();
}
