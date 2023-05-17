using Microsoft.EntityFrameworkCore;
using PersonService.DataAccess.Context;
using PersonService.Entities;
using PhoneBookApp.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.DataAccess.Repo
{
    public class ContactInfoRepo : BaseRepo<ContactInfo>, IContactInfoRepo
    {
        public ContactInfoRepo(PersonDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
