using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace verbum_service_domain.DTO.Request
{
    public class UploadOrderFileRequest
    {
        public Guid OrderId { get; set; }

        public string ReferenceFileUrl { get; set; } = null!;

        public string Tag { get; set; } //TRANSLATION, REFERENCES, DELIVERABLES
    }
}
