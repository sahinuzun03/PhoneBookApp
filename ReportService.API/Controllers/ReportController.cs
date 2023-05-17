using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportService.Business.Services;

namespace ReportService.API.Controllers
{
    [Route("Report/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportServices _reportService;
        public ReportController(IReportServices reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task TakeReport()
        {
            await _reportService.WantReportDetail();
        }

    }
}