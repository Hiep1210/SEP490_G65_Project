using System;
using System.Collections.Generic;

namespace verbum_service_domain.Models;

public partial class Job
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    /// <summary>
    /// NEW, IN_PROGRESS, COMPLETED, ACCEPTED
    /// </summary>
    public string Status { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int WordCount { get; set; }

    public string DocumentUrl { get; set; } = null!;

    public string TargetLanguageId { get; set; } = null!;

    public Guid? WorkId { get; set; }

    public virtual Work? Work { get; set; }

    public virtual ICollection<User> Assignees { get; set; } = new List<User>();
}
