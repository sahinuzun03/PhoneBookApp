using AutoMapper;
using PersonService.Business.Models;
using PersonService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Business.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ContactInfo, ContactInfoModel>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoViewModel>().ReverseMap();

            CreateMap<Person, PersonViewModel>().ReverseMap();  
            CreateMap<Person, PersonModel>().ReverseMap();
        }
    }
}
