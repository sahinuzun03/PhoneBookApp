using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Business.Services.MyBackgroundService
{
    public class ReportBackgroundService : BackgroundService
    {
        private readonly IContactInfoServices _contactInfoServices;
        public ReportBackgroundService(IContactInfoServices contactInfoServices)
        {

            _contactInfoServices = contactInfoServices;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    // ListenToQueue metodunu burada çağırabilirsiniz
            //    //_contactInfoServices.ListenToQueue();

            //    // Uygun bir bekleme süresi belirleyin
            //    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            //}
        }
    }
}
