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
        Task CreateContactInfo(Guid PersonId, ContactInfoModel contactInfoModel);
        Task DeleteContactInfo(Guid PersonId, ContactInfoDeleteModel contactInfoDeleteModel);
        Task<List<ContactInfoViewModel>> GetContactInfo(Guid PersonId);
    }
}
