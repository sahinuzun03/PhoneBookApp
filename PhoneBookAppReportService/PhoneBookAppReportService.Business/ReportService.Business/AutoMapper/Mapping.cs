using AutoMapper;
using ReportService.Business.Models;
using ReportService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Business.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Report, ReportModel>().ReverseMap();
            CreateMap<ReportDetailModel,ReportDetail>().ReverseMap(); 
            CreateMap<ReportDetailDTO,ReportDetailModel>().ReverseMap();
        }
    }
}
