using System;
using System.Collections.Generic;

namespace verbum_service_domain.Models;

public partial class Receipt
{
    public Guid ReceiptId { get; set; }

    public DateTime? PayDate { get; set; }

    public Guid? OrderId { get; set; }

    public bool? DepositeOrPayment { get; set; }

    public decimal? Amount { get; set; }

    public virtual Order? Order { get; set; }
}
