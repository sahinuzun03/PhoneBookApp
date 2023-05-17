using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonService.Business.Models;
using PersonService.Business.Services;
using PersonService.DataAccess.Repo;

namespace PersonService.API.Controllers
{
    [Route("ContactInfo/[action]")]
    [ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoServices _contactInfoServices;
        public ContactInfoController(IContactInfoServices contactInfoServices)
        {
            _contactInfoServices = contactInfoServices;
        }

        [HttpPost]
        public async Task<bool> AddContactInfo(ContactInfoModel model, [FromQuery] Guid Id)
        {
            await _contactInfoServices.CreateContactInfo(Id, model);
            return true;
        }

        [HttpDelete]
        public async Task DeleteContactInfo([FromQuery] Guid contactId)
        {
            await _contactInfoServices.DeleteContactInfo(contactId);
        }

        [HttpGet]
        public async Task<List<ContactInfoViewModel>> GetContactInfo(Guid personId)
        {
            return await _contactInfoServices.GetContactInfo(personId);
        }
    }
}
