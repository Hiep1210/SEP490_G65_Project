using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace verbum_service_domain.DTO.Request
{
    public class CreateJobFileUpload
    {
        public string DocumentUrl { get; set; }
        public int WordCount { get; set; }
    }
}
