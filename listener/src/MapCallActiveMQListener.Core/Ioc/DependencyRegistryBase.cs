using log4net;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model;
using MapCallActiveMQListener.Library;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ActiveMQ;
using StructureMap;

namespace MapCallActiveMQListener.Ioc
{
    public abstract class DependencyRegistryBase : Registry
    {
        public DependencyRegistryBase()
        {
            Scan(x => {
                x.AssemblyContainingType<DependencyRegistry>();
                x.WithDefaultConventions();
            });
            For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            For<IActiveMQConfiguration>().Use<WorkOrdersConfiguration>();
            For<IActiveMQServiceFactory>().Use<ApacheActiveMQServiceFactory>();
            //For<IActiveMQServiceFactory>().Use<StompDotNetActiveMQServiceFactory>();
            For<ILog>()
                .AlwaysUnique()
                .Use(ctx => LogManager.GetLogger(ctx.ParentType == null ? ctx.RequestedName : ctx.ParentType.Name));
            Scan(x => {
                x.AssemblyContainingType<TownRepository>();
                x.IncludeNamespaceContainingType<TownRepository>();
                x.WithDefaultConventions();
            });
        }
    }
}