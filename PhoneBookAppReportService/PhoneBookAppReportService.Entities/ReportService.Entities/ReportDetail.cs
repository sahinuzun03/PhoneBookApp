using PhoneBookApp.Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Entities
{
    public class ReportDetail : TableBase
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string PhoneCount { get; set; }
        public string PersonCount { get; set; }
        public Guid ReportId { get; set; }
        public Report Report { get; set; }
    }
}
