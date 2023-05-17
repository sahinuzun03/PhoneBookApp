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
    public class ContactInfoServices : IContactInfoServices
    {
        private readonly IPersonRepo _personRepo;
        private readonly IMapper _mapper;
        private readonly IContactInfoRepo _contactInfoRepo;
        public ContactInfoServices(IMapper mapper,IPersonRepo personRepo,IContactInfoRepo contactInfoRepo)
        {

            _mapper = mapper;
            _personRepo = personRepo;  
            _contactInfoRepo = contactInfoRepo; 

        }
        public async Task CreateContactInfo(Guid personId, ContactInfoModel contactInfoModel)
        {
            var person = _personRepo.GetById(personId);
            var addContact = _mapper.Map<ContactInfo>(contactInfoModel);
            addContact.Person = person;
            await _contactInfoRepo.InsertAsync(addContact);
            _contactInfoRepo.SaveChanges();
        }

        public async Task DeleteContactInfo(Guid contactID)
        {
            _contactInfoRepo.DeleteById(contactID);
            await _contactInfoRepo.SaveChangesAsync();   
            
        }

        public async Task<List<ContactInfoViewModel>> GetContactInfo(Guid personId)
        {
            var getPersonContacts = await _contactInfoRepo.GetDataWithLinqExp(x => x.DeletedAt == null && x.PersonId == personId).Select(x => new ContactInfoViewModel
            {
                ContactContent = x.ContactContent,
                EmailAddress = x.EmailAddress,
                Id = x.Id,
                Location = x.Location,
                PhoneNumber = x.PhoneNumber,
            }).ToListAsync();

            return getPersonContacts;
        }
    }
}
