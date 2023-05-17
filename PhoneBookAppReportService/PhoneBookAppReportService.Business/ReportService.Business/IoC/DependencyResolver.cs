using Autofac;
using AutoMapper;
using ReportService.Business.AutoMapper;
using ReportService.Business.Services;
using ReportService.DataAccess.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Business.IoC
{
    public class DependencyResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<ReportRepo>().As<IReportRepo>().InstancePerLifetimeScope();

            builder.RegisterType<ReportServices>().As<IReportServices>().InstancePerLifetimeScope();    

            builder.RegisterType<ReportDetailsRepo>().As<IReportDetailsRepo>().InstancePerLifetimeScope();

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
