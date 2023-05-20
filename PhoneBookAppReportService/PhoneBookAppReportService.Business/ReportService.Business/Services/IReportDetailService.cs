using ReportService.Business.Models;
using ReportService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Business.Services
{
    public interface IReportDetailService 
    {
        Task CreateReportDetail(ReportDetailModel model);
    }
}
