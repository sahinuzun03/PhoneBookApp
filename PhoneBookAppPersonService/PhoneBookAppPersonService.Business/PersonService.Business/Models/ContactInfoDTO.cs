using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Business.Models
{
    public class ContactInfoModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Location { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactContent { get; set; }
    }

    public class ContactInfoDeleteModel
    {
        public Guid Id { get; set; }
    }
    public class ContactInfoViewModel
    {
        public Guid Id { get; set; }
        public string? Location { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactContent { get; set; }
        public string? PersonName { get;set; } 
    }
}
