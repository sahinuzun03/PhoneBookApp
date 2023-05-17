using Autofac;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using PersonService.Business.AutoMapper;
using PersonService.Business.Services;
using PersonService.Business.Services.MyBackgroundService;
using PersonService.DataAccess.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Business.IoC
{
    public class DependencyResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // REPO 
            builder.RegisterType<PersonRepo>().As<IPersonRepo>().InstancePerLifetimeScope();
            builder.RegisterType<ContactInfoRepo>().As<IContactInfoRepo>().InstancePerLifetimeScope();

            //SERVİCE

            builder.RegisterType<PersonServices>().As<IPersonServices>().InstancePerLifetimeScope();
            builder.RegisterType<ContactInfoServices>().As<IContactInfoServices>().InstancePerLifetimeScope();

            //BackgrounService 
            builder.RegisterType<ReportBackgroundService>().As<IHostedService>().InstancePerDependency();

            //AUTOMAPPER
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Mapping>();
            }
            )).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
