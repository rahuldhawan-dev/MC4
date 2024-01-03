using System;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Configuration;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.StructureMap;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Tool.hbm2ddl;
using StructureMap;

namespace MMSINC.Testing.SpecFlow.Library
{
    public static class TestHelperProxy
    {
        #region Properties

        /// <summary>
        /// THIS MUST BE THE DISPOSABLE SESSION! The test needs to be able to destroy the session when it's done.
        /// </summary>
        public static ISession Session { get; private set; }

        public static Action LogOutAction { get; private set; }
        private static string _originalDatabaseType;
        private static InMemTestDatabaseFunBag _inMemTestConfig;

        #endregion

        #region Configuration Helper Methods

        public static void EnableTestModeOnGlobal()
        {
            MvcApplication.IsInTestMode = true;
            _originalDatabaseType = ConfigurationManager.AppSettings["DatabaseType"];
            ConfigurationManager.AppSettings["DatabaseType"] = "sqlite";
            SelectAttribute.EnableAsync = false;
        }

        public static void DisableTestModeOnGlobal()
        {
            MvcApplication.IsInTestMode = false;
            ConfigurationManager.AppSettings["DatabaseType"] = _originalDatabaseType;
            SelectAttribute.EnableAsync = true;
        }

        #endregion

        #region Test Connection Helper Methods

        public static void EnableRequestProcessing()
        {
            MMSINC.MvcApplication.AllowTestRequests = true;
        }

        public static void DisableRequestProcessing()
        {
            MMSINC.MvcApplication.AllowTestRequests = false;
        }

        #endregion

        #region Test Database Helper Methods

        public static void CreateSystemWideSession()
        {
            Session = _inMemTestConfig.OpenSession();

            var container = DependencyResolver.Current.GetService<IContainer>();
            var wrapped = new DummyTransactionSessionWrapper(new IndisposableSessionWrapper(Session));
            container.Inject<ISessionFactory>(new DummySessionFactory(wrapped));
            container.Inject<ISession>(wrapped);
        }

        public static void DestroySystemWideSession()
        {
            Session.Clear();
            Session.Connection.Close();
            Session.Dispose();
            Session = null;
        }

        public static void InitializeTestDatabase()
        {
            // I don't quite understand why we're pulling in the Session here unless it's to 
            // dispose of an existing session that was created before the test started. 
            if ((Session ?? (Session = DependencyResolver.Current.GetService<ISession>())) != null)
            {
                DestroyTestDatabase();
            }

            var container = DependencyResolver.Current.GetService<IContainer>();
            var registrar = DependencyResolver.Current.GetService<DependencyRegistrar>();
            var sessionFactory = registrar.CreateSessionFactory(container.GetInstance<ContainerToContextAdapter>());
            _inMemTestConfig = new InMemTestDatabaseFunBag(registrar.NHibernateConfiguration, sessionFactory);
            _inMemTestConfig.Initialize();
            container.Inject(registrar.NHibernateConfiguration);
        }

        public static void DestroyTestDatabase()
        {
            // STRANGE NOTE: The way this entire class is setup as of this commit(2/14/2020),
            // if you continued to try and call Session.Dispose() or some other destruction
            // method on Session or its friends, it would result in iisexpress.exe remaining
            // open. This would prevent you from building the project until you killed iisexpress.exe
            // because there would be a file lock on some dlls. 
            //
            // I believe what was happening was that the Session.Dispose() call was throwing
            // an exception, but either the exception was getting eaten by the test runner or
            // the test runner had already stopped listening and didn't know about the exception.
            // I couldn't figure it out. The moral of the story is: Don't do anything in here besides
            // disposing the config.
            _inMemTestConfig?.Dispose();
        }

        #endregion

        #region Authorization Helper Methods

        public static void SetLogOutAction(Action action)
        {
            LogOutAction = action;
        }

        [Obsolete("This literally can not work. -Ross")]
        public static void ForceLogOut()
        {
            if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (LogOutAction == null)
                {
                    throw new ArgumentNullException(
                        "No log out action defined.  You must call MMSINC.Testing.SpecFlow.Library.TestHelperProxy.SetLogOutAction(), probably from a static constructor in RegressionTests.Steps.Navigation.");
                }

                LogOutAction();
            }
        }

        #endregion

        #region Data Helper Methods

        public static Int32 GetEntityId(object entity)
        {
            var factory = DependencyResolver.Current.GetService<ISessionFactory>();
            var meta = factory.GetClassMetadata(entity.GetType());
            return (Int32)meta.GetIdentifier(entity);
        }

        public static object GetEntityFromPersistence(object entity, Type entityType)
        {
            return GetEntityFromPersistence(entityType,
                GetEntityId(entity));
        }

        public static bool EntityExists(Type entityType, int id)
        {
            var sess = DependencyResolver.Current.GetService<ISession>();

            return sess
                  .CreateCriteria(entityType)
                  .Add(Restrictions.IdEq(id))
                  .SetProjection(Projections.RowCountInt64())
                  .UniqueResult<long>() > 0;
        }

        public static object GetEntityFromPersistence(Type entityType, object id)
        {
            // NOTE: As of NHibernate 4.0, proxy objects do not appear to serialize correctly.
            //       To get around this, we have to create a concrete type instance and then
            //       load the values into it before returning. It is not recommended that you
            //       use this method, though. If Session.Load works, it won't load any lazy-loaded
            //       properties and will leave them nulled out.
            var sess = DependencyResolver.Current.GetService<ISession>();
            var res = Activator.CreateInstance(entityType);

            try
            {
                sess.Load(res, id);
            }
            catch (NonUniqueObjectException)
            {
                res = sess.Get(entityType, id);
            }

            return res;
        }

        public static object GetEntityPropertyValueFromPersistence(object entity, Type entityType, string propertyName)
        {
            return GetEntityPropertyValueFromPersistence(entityType, GetEntityId(entity), propertyName);
        }

        public static object GetEntityPropertyValueFromPersistence(Type entityType, object id, string propertyName)
        {
            var sess = DependencyResolver.Current.GetService<ISession>();
            var entity = sess.CreateCriteria(entityType)
                             .Add(Restrictions.IdEq(id))
                             .UniqueResult();

            // Shouldn't this throw an exception if the entity does not exist?
            if (entity == null)
            {
                return null;
            }

            if (propertyName == "ToString")
            {
                return entity.ToString();
            }

            return entity.GetPropertyValueByName(propertyName);
        }

        #endregion

        #region Regression Test Flags

#if DEBUG

        public static void AddRegressionTestFlag(string flag)
        {
            MMSINC.MvcApplication.RegressionTestFlags.Add(flag);
        }

        public static void ClearAllRegressionTestFlags()
        {
            MMSINC.MvcApplication.RegressionTestFlags.Clear();
        }

#endif

        #endregion

        #region Other/Utility Helper Methods

        public static string GetNHibernateSessionMessage()
        {
            return SessionWrapper.DisposalMessage;
        }

        public static string GetNHibernateSessionFactoryMessage()
        {
            return SessionFactoryWrapper.DisposalMessage;
        }

        public static void ResetNHibernateSessionMessages()
        {
            SessionFactoryWrapper.ResetMessage();
            SessionWrapper.ResetMessage();
        }

        #endregion

        #region Helper classes

        internal class InMemTestDatabaseFunBag
        {
            #region Fields

            private SQLiteConnection _backupDatabaseConnection;
            private readonly global::NHibernate.Cfg.Configuration _config;

            #endregion

            #region Properties

            public ISessionFactory SessionFactory { get; }

            #endregion

            #region Constructor

            internal InMemTestDatabaseFunBag(global::NHibernate.Cfg.Configuration config,
                ISessionFactory sessionFactory)
            {
                SessionFactory = sessionFactory;
                _config = config;
            }

            #endregion

            #region Private Methods

            private void OptimizeSQLiteDatabase(ISession session)
            {
                using (var cmd = session.Connection.CreateCommand())
                {
                    cmd.CommandText = @"
pragma journal_mode = WAL;
pragma synchronous = normal;
pragma temp_store = memory;
pragma map_size = 30000000000;";
                    cmd.ExecuteNonQuery();
                }
            }

            #endregion

            #region Public Methods

            public void Initialize()
            {
                var schemaExport = new SchemaExport(_config);
                var sess = SessionFactory.OpenSession();

                OptimizeSQLiteDatabase(sess);

                // Set the first true(the script param) to false if you don't want SchemaExport to spit out the entire database
                // creation to Console.WriteLine.
                // It's set to false now because total testoutput for MapCallMVC is reaching 600mb with it on.
                schemaExport.Execute(false, true, false, sess.Connection, new StringWriter());

                _backupDatabaseConnection = new SQLiteConnection($"Data Source=:memory:;Version=3;New=True;");
                _backupDatabaseConnection.Open();
                ((SQLiteConnection)sess.Connection).BackupDatabase(_backupDatabaseConnection, "main", "main", -1, null,
                    0);
            }

            public void Dispose()
            {
                SessionFactory.Dispose();
                _backupDatabaseConnection.Dispose();
            }

            public ISession OpenSession()
            {
                var sess = SessionFactory.OpenSession();

                OptimizeSQLiteDatabase(sess);

                _backupDatabaseConnection.BackupDatabase(((SQLiteConnection)sess.Connection), "main", "main", -1, null,
                    0);
                return sess;
            }

            #endregion
        }

        #endregion
    }
}
