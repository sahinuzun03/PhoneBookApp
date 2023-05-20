using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonService.Business.Services;

namespace PersonService.API.Controllers
{
    [Route("Report/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IContactInfoServices _contactInfoService;
        public ReportController(IContactInfoServices contactInfoServices)
        {
            _contactInfoService = contactInfoServices;
        }

        [HttpGet("{routingKey}")]
        public void GetReportDetail([FromRoute]string routingKey)
        {
            _contactInfoService.SendReportDetailstoRabbitMQ(routingKey);
        }
    }
}
