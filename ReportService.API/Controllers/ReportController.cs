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
            //Request Response Tasarımda bir Sıkıntı var kuyruta mesaj oluşmuyor direct exchange kullanınca da aynı hatayı alıyorum tekrardan bakmak lazım.
            await _reportService.WantReportDetail();
        }

        [HttpGet]
        public async Task TakeReport2()
        {
            await _reportService.WantReportDetails();
        }
    }
}