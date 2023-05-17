using ReportService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Business.Models
{
    public class ReportModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ReportStatus ReportStatus { get; set; } = ReportStatus.GettingReady;
    }
}
