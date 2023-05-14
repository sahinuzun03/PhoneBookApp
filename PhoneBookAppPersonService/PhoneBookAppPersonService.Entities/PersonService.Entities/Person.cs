using PhoneBookApp.Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Entities
{
    public class Person : TableBase
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Company { get; set; }
        public List<ContactInfo> ContactInfoList { get; set; }

        public Person()
        {
            ContactInfoList = new List<ContactInfo>();
        }
    }
}
