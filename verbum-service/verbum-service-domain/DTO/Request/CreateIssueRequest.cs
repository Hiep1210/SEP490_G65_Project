using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace verbum_service_domain.DTO.Request
{
    public class CreateIssueRequest
    {
        public string? IssueName { get; set; }
        public Guid? OrderId { get; set; }

        public string? IssueDescription { get; set; }
    }
}
