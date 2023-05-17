using PhoneBookApp.Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Entities
{

    public enum ReportStatus
    {
        Completed = 1,
        GettingReady = 2,
    }

    public class Report : TableBase
    {
        public Guid Id { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public List<ReportDetail> ReportDetails { get; set; }

        public Report()
        {
            ReportDetails = new List<ReportDetail>();
        }
    }
}
