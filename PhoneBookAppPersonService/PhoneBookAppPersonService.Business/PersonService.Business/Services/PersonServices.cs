using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonService.Business.Models;
using PersonService.DataAccess.Repo;
using PersonService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Business.Services
{
    public class PersonServices : IPersonServices
    {
        private readonly IPersonRepo _personRepo;
        private readonly IMapper _mapper;
        public PersonServices(IPersonRepo personRepo, IMapper mapper)
        {
            _personRepo = personRepo;
            _mapper = mapper;
        }
        public async Task CreatePerson(PersonModel createPersonModel)
        {
            var addPerson = _mapper.Map<Person>(createPersonModel);
            await _personRepo.InsertAsync(addPerson);
            _personRepo.SaveChanges();
        }

        public async Task DeletePerson(Guid Id)
        {
            _personRepo.DeleteById(Id);
            await _personRepo.SaveChangesAsync();
        }

        public async Task<List<PersonViewModel>> GetPeople()
        {
            var personList = await _personRepo.GetDataWithLinqExp(x => x.DeletedAt == null).Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Company = x.Company != null ? x.Company : "",
            }).ToListAsync();

            return personList;
        }

        public async Task<PersonDetailModel> GetPersonDetail(Guid Id)

        {
            var personDetail = await _personRepo.GetDataWithLinqExp(x => x.DeletedAt == null && x.Id == Id, "ContactInfoList").Select(x => new PersonDetailModel
            {
                Company = x.Company,
                Id = x.Id,
                LastName = x.LastName,
                FirstName = x.FirstName,
                PersonContacts = x.ContactInfoList
            }).FirstOrDefaultAsync();

            return personDetail;
        }
    }
}
