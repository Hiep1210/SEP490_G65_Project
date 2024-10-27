using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace verbum_service_domain.DTO.Request
{
    public class CreateJobsRequest
    {
        public List<Guid> WorkIds { get; set; }
        public List<string> DocumentUrls { get; set; }
        public List<string> TargetLanguageIds { get; set; }
    }
}
