using System;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Web;
using FluentNHibernate.Cfg;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using NHibernate;
using NHibernate.Event;
using StructureMap;
using StructureMap.Web;
using SessionWrapper = MMSINC.Data.NHibernate.SessionWrapper;

namespace MMSINC.Configuration
{
    public abstract class DependencyRegistrar
    {
        #region Abstract Methods

        public abstract ISessionFactory CreateSessionFactory(IContext container);

        #endregion

        public NHibernate.Cfg.Configuration NHibernateConfiguration { get; protected set; }
    }

    /// <typeparam name="TAssemblyOf">
    /// Type contained in the assembly which should be used for NHibernate
    /// models and fluent mappings.
    /// </typeparam>
    public abstract class DependencyRegistrar<TAssemblyOf, TUser> : DependencyRegistrar
        where TUser : class, IAdministratedUser
    {
        #region Private Methods

        protected virtual Action<NHibernate.Cfg.Configuration> ConfigureSessionFactory(IContext container)
        {
            return configuration => {
                configuration.EventListeners.FlushEventListeners = new IFlushEventListener[] {
                    new FixedDefaultFlushEventListener()
                };
                configuration.SetInterceptor(container.GetInstance<IChangeTrackingInterceptor<TUser>>());
            };
        }

        protected void RegisterNHibernate(ConfigurationExpression i)
        {
            // this should only happen once, when CreateSessionFactory is called.  in regression
            // tests, this will be called again after test mode is enabled to setup the sqlite database
            i.For<DatabaseConfiguration>()
             .AlwaysUnique()
             .Use(() => MvcApplication.IsInTestMode
                  ? new SQLiteConfiguration()
                  : (DatabaseConfiguration)new MsSql2008Configuration(MvcApplication.DEFAULT_CONNECTION_STRING));

            i.For<ISessionFactory>()
             .Singleton()
             .Use(ctx => CreateSessionFactory(ctx));

            i.For<ISession>()
             .HybridHttpOrThreadLocalScoped()
             .Use(ctx => new SessionWrapper(
                  ctx.GetInstance<ISessionFactory>()
                     .OpenSession(ctx.GetInstance<ChangeTrackingInterceptor<TUser>>())));

            i.For<IPrincipal>()
             .HybridHttpOrThreadLocalScoped()
             .Use(() => HttpContext.Current == null ? null : HttpContext.Current.User);

            i.For<NHibernate.Cfg.Configuration>()
             .AlwaysUnique()
             .Use(() => NHibernateConfiguration);

            i.For<IChangeTrackingInterceptor<TUser>>()
             .Use<ChangeTrackingInterceptor<TUser>>();

            // I'm not sure if ControllerFactories are singletons or created per-request.
            // Since I'm doing this for the sake of testing, we can use AlwaysUnique to
            // stop it from caching anything and to always return the one MVC is using.

            //i.For<RequestContext>()
            //    .AlwaysUnique()
            //    .Use(() => HttpContext.Current.Request.RequestContext);
        }

        protected void MakeMappingsHappen(MappingConfiguration conf)
        {
            try
            {
                conf.FluentMappings
                    .AddFromAssemblyOf<TAssemblyOf>()
                    .AddDynamicMapping<TAssemblyOf>()
                    .Conventions.AddFromAssemblyOf<TAssemblyOf>();

                conf.MergeMappings();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (var exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound;
                    if ((exFileNotFound = exSub as FileNotFoundException) != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }

                    sb.AppendLine();
                }

                throw new Exception(sb.ToString(), ex);
            }
        }

        #endregion

        #region Abstract Methods

        protected abstract void RegisterModels(ConfigurationExpression i);
        protected abstract void RegisterUtilities(ConfigurationExpression i);

        #endregion

        #region Exposed Methods

        public override ISessionFactory CreateSessionFactory(IContext container)
        {
            var dbConfig = container.GetInstance<DatabaseConfiguration>();

            // TODO: refactor this
            // it's not designed with testing in mind, so its test is
            // clunky and inaccurate.  it will probably need to be refactored,
            // especially so that inheriting classes can provide extra
            // configuration

            // Leaving a mental note here:
            // If this method is called prior to the regression tests
            // being able to start, then Sessions will never be configured
            // to use SQLite.  Ask Ross he knows!

            var config = Fluently.Configure()
                                 .Database(dbConfig.Configuration)
                                 .Mappings(MakeMappingsHappen)
                                 .ExposeConfiguration(ConfigureSessionFactory(container));
            if (MvcApplication.IsInTestMode)
            {
                // TODO: what is this for?
                config = config
                   .ExposeConfiguration(c => c
                       .SetProperty("hbm2dll.keywords", "none"));
            }

            NHibernateConfiguration = config.BuildConfiguration();
            NHibernateConfiguration.AddAuxiliaryDatabaseObjectsInAssemblyOf<TAssemblyOf>();
            return NHibernateConfiguration.BuildSessionFactory();
        }

        public IContainer EnsureDependenciesRegistered()
        {
            var container = new Container(x => {
                RegisterNHibernate(x);
                RegisterModels(x);
                RegisterUtilities(x);
#if DEBUG
                x.For(typeof(DependencyRegistrar)).Use(GetType());
#endif
            });

            return container;
        }

        #endregion
    }
}
