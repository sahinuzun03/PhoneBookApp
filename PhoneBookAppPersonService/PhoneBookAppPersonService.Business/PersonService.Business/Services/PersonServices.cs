using PersonService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Business.Services
{
    public class PersonServices : IPersonServices
    {
        public Task CreatePerson(PersonModel createPersonModel)
        {
            throw new NotImplementedException();
        }

        public Task DeletePerson(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PersonViewModel>> GetPeople()
        {
            throw new NotImplementedException();
        }
    }
}
