namespace Agent.Infrastructure.AutofacModules
{
    using Agent.Commands;
    using Autofac;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core.Query;
    using Microsoft.Extensions.Configuration;
    using System.Reflection;
    using Agent.Domain.Query;
    using LeadsPlus.Core.Repositories;
    using LeadsPlus.Core;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LeadsPlus.GoogleApis;

    public class ApplicationModule : Autofac.Module
    {
        public IConfiguration setting;

        public ApplicationModule(IConfiguration setting)
        {
            this.setting = setting;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AgentCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

            //builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
           
            builder.RegisterAssemblyTypes(new Assembly[] { typeof(GetAgentQueryHandler).GetTypeInfo().Assembly })
                .AsClosedTypesOf(typeof(IQueryHandler<,>)).AsImplementedInterfaces();

            //builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
            //.AsClosedTypesOf(typeof(IQueryHandler<,>)).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<QueryExecutor>()
                .As<IQueryExecutor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GoogleApiConnector>()
                .As<IGoogleApiConnector>()
                .InstancePerLifetimeScope();

            builder.Register(c => {
                return ViewModelStoreFactory.Create<Domain.Agent>(setting["DatabaseConnectionString"], setting["DatabaseName"]);
            }).AsSelf().SingleInstance();
            
        }
    }
}
