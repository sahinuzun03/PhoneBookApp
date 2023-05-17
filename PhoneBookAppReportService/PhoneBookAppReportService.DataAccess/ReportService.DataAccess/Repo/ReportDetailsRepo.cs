using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Core.DataAccess;
using ReportService.DataAccess.Context;
using ReportService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.DataAccess.Repo
{
    public class ReportDetailsRepo : BaseRepo<ReportDetail>, IReportDetailsRepo
    {
        public ReportDetailsRepo(ReportDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
