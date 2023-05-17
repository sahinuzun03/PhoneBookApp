using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonService.Business.Models;
using PersonService.Business.Services;

namespace PersonService.API.Controllers
{
    [Route("Person/[action]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonServices _personServices;
        public PersonController(IPersonServices personServices)
        {
            _personServices = personServices;
        }

        [HttpPost]
        public async Task<bool> AddPerson(PersonModel model)
        {
            await _personServices.CreatePerson(model);
            return true;
        }

        [HttpDelete]
        public async Task DeletePerson(Guid Id)
        {
            await _personServices.DeletePerson(Id);
        }

        [HttpGet]
        public async Task<PersonDetailModel> GetPersonDetail(Guid Id)
        {
            return await _personServices.GetPersonDetail(Id);
        }

        [HttpGet]
        public async Task<List<PersonViewModel>> GetAllPerson()
        {
            return await _personServices.GetPeople();
        }
    }
}
