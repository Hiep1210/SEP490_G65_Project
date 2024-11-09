using System;
using System.Collections.Generic;

namespace verbum_service_domain.Models;

public partial class ClientTransaction
{
    public Guid ClientId { get; set; }

    public Guid TransactionId { get; set; }
}
