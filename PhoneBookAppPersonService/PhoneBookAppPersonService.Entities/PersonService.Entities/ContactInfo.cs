using PhoneBookApp.Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Entities
{
    public class ContactInfo : TableBase
    {
        public Guid Id { get; set; }
        public string? Location { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactContent { get; set; }
        public Guid? PersonId { get; set; }
        public Person Person { get; set; }
    }
}
