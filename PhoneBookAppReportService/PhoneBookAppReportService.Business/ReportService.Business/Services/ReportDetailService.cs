using AutoMapper;
using ReportService.Business.Models;
using ReportService.DataAccess.Repo;
using ReportService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Business.Services
{
    public class ReportDetailService : IReportDetailService
    {
        private readonly IReportDetailsRepo _reportDetailsRepo;
        private readonly IMapper _mapper;

        public ReportDetailService(IReportDetailsRepo reportDetailsRepo,IMapper mapper)
        {
            _reportDetailsRepo= reportDetailsRepo;
            _mapper = mapper;
        }
        public async Task CreateReportDetail(ReportDetailModel model)
        {
            var addReportDetail = _mapper.Map<ReportDetail>(model);
            await _reportDetailsRepo.InsertAsync(addReportDetail);
            _reportDetailsRepo.SaveChanges();
        }
    }
}
