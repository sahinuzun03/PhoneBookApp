using ReportService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Business.Models
{
    public class ReportDetailModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Location { get; set; }
        public string PhoneCount { get; set; }
        public string PersonCount { get; set; }
        public Report Report { get; set; }
    }

    public class ReportDetailDTO
    {
        public string Location { get; set; }
        public string PhoneCount { get; set; }
        public string PersonCount { get; set; }

    }


}
