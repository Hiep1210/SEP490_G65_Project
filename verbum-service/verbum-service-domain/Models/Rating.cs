using System;
using System.Collections.Generic;

namespace verbum_service_domain.Models;

public partial class Rating
{
    public Guid RatingId { get; set; }

    public int? RatingStars { get; set; }

    public Guid? OrderId { get; set; }

    public string? RatingDetail { get; set; }
}
