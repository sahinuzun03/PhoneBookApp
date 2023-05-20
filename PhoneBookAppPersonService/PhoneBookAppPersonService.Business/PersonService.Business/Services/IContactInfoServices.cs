using PersonService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Business.Services
{
    public interface IContactInfoServices
    {
        Task CreateContactInfo(Guid personId, ContactInfoModel contactInfoModel);
        Task DeleteContactInfo(Guid contactId);
        Task<List<ContactInfoViewModel>> GetContactInfo(Guid personId);
        void ListenToQueue();
        List<ReportDetailDTO> GiveReportDetail();
        void SendReportDetailstoRabbitMQ(string reportID);
    }
}
