using PersonService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Business.Services
{
    public interface IPersonServices
    {
        Task CreatePerson(PersonModel createPersonModel);
        Task<List<PersonViewModel>> GetPeople();
        Task DeletePerson(Guid Id);
        Task<PersonDetailModel> GetPersonDetail(Guid Id);
    }
}
