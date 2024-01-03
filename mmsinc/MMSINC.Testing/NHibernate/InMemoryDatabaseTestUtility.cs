using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace MMSINC.Testing.NHibernate
{
    /// <summary>
    /// Utility methods/caching/whathaveyouings so the generic InMemoryDatabaseTest can use shared
    /// configurations and SessionFactories.
    /// </summary>
    internal static class InMemoryDatabaseTestUtility
    {
        #region Fields

        private static readonly object _looockBoooox = new object();

        private static readonly Dictionary<Assembly, IInMemTestConfig> _configsByAss =
            new Dictionary<Assembly, IInMemTestConfig>();

        #endregion

        #region Private Methods

        private static InMemTestConfig<TAssemblyOf> CreateConfig<TAssemblyOf>(
            IInMemoryDatabaseTestInterceptor interceptor)
        {
            try
            {
                var nConfig = Fluently
                             .Configure()
                             .Database(new SQLiteConfiguration().Configuration)
                             .Mappings(m => {
                                  m.FluentMappings.AddFromAssemblyOf<TAssemblyOf>()
                                   .AddDynamicMapping<TAssemblyOf>()
                                   .Conventions.AddFromAssemblyOf<TAssemblyOf>();
                                  m.MergeMappings();
                              })
                             .ExposeConfiguration(cfg => { cfg.SetInterceptor(interceptor); })
                             .BuildConfiguration();

                nConfig.AddAuxiliaryDatabaseObjectsInAssemblyOf<TAssemblyOf>();

                return new InMemTestConfig<TAssemblyOf>(nConfig, interceptor);
            }
            catch (FluentConfigurationException e)
            {
                throw new Exception(
                    "An exception has been encountered in configuring the test SQLite database.", e.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "An exception has been encountered in configuring the test SQLite database.", e);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates and/or returns a cached InMemTestConfig for InMemoryDatabaseTest classes to
        /// share when the generic type differs but the assembly is still the same. This prevents
        /// using way too much memory when too many SessionFactory instances are created.
        /// </summary>
        public static IInMemTestConfig GetConfig<TAssemblyOf>(IInMemoryDatabaseTestInterceptor interceptor)
        {
            var ass = typeof(TAssemblyOf).Assembly;

            // Perhaps one day testing will actually be multi-threaded in which case
            // we wanna lock down all the things.
            lock (_looockBoooox)
            {
                if (!_configsByAss.ContainsKey(ass))
                {
                    _configsByAss.Add(ass, CreateConfig<TAssemblyOf>(interceptor));
                }
            }

            return _configsByAss[ass];
        }

        #endregion
    }

    internal interface IInMemTestConfig : IDisposable
    {
        DbConnection OpenConnection();
        global::NHibernate.Cfg.Configuration Configuration { get; }
        ISessionFactory SessionFactory { get; }
        IInMemoryDatabaseTestInterceptor Interceptor { get; }
    }

    internal class InMemTestConfig<TAssemblyOf> : IInMemTestConfig
    {
        #region Fields

        private readonly SchemaExport _schemaExport;

        private static ConcurrentDictionary<string, SQLiteConnection> _backupPathDictionary =
            new ConcurrentDictionary<string, SQLiteConnection>();

        #endregion

        #region Properties

        public global::NHibernate.Cfg.Configuration Configuration { get; }
        public IInMemoryDatabaseTestInterceptor Interceptor { get; }
        public ISessionFactory SessionFactory { get; }

        #endregion

        #region Constructor

        internal InMemTestConfig(global::NHibernate.Cfg.Configuration config,
            IInMemoryDatabaseTestInterceptor interceptor)
        {
            Configuration = config;
            Interceptor = interceptor;

            //try
            //{
            SessionFactory = Configuration.BuildSessionFactory();
            //}
            //catch (InvalidProxyTypeException e)
            //{
            //    throw new Exception(
            //        $"An NHibernate InvalidProxyTypeException has been thrown while setting up the test SQLite database, with the following message: {Environment.NewLine}{e.Message}");
            //}
            //catch (Exception e)
            //{
            //    // If you've reached this point and you're getting an SQLite.Interop.dll can't be found error,
            //    // make sure the project configuration is set to build in x64 and not Any CPU/x86.
            //    // Or more likely it's because your test runner is not explicitly running in x64 mode. 
            //    throw new Exception(
            //        "An exception has been encountered in setting up the test SQLite database.",
            //        e);
            //}

            _schemaExport = new SchemaExport(Configuration);
        }

        #endregion

        public void Dispose()
        {
            SessionFactory.Dispose();
        }

        public DbConnection OpenConnection()
        {
            var sess = SessionFactory.OpenSession();
            var assembly = typeof(TAssemblyOf).Assembly.FullName;

            var sw = System.Diagnostics.Stopwatch.StartNew();
            if (!_backupPathDictionary.ContainsKey(assembly))
            {
                // Set the first true(the script param) to false if you don't want SchemaExport to spit out the entire database
                // creation to Console.WriteLine.
                // It's set to false now because total testoutput for MapCallMVC is reaching 600mb with it on.
                _schemaExport.Execute(false, true, false, sess.Connection, new StringWriter());

                var destination = new SQLiteConnection($"Data Source=:memory:;Version=3;New=True;");
                destination.Open();
                ((SQLiteConnection)sess.Connection).BackupDatabase(destination, "main", "main", -1, null, 0);

                _backupPathDictionary[assembly] = destination;
            }
            else
            {
                var conn = _backupPathDictionary[assembly];
                conn.BackupDatabase(((SQLiteConnection)sess.Connection), "main", "main", -1, null, 0);
            }

            sw.Stop();
            return sess.Connection;
        }
    }
}
