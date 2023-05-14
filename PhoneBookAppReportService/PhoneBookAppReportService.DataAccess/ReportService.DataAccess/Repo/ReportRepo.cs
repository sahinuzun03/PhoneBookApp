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
    public class ReportRepo : BaseRepo<Report>, IReportRepo
    {
        public ReportRepo(ReportDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
