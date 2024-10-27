using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verbum_service_domain.Models;

namespace verbum_service_domain.DTO.Response
{
    public class JobInfoResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        /// <summary>
        /// NEW, IN_PROGRESS, COMPLETED
        /// </summary>
        public long Status { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int WordCount { get; set; }

        public string DocumentUrl { get; set; } = null!;

        public string TargetLanguageId { get; set; } = null!;

        public Guid? WorkId { get; set; }
        public List<string> AssigneeNames { get; set; }
    }
}
