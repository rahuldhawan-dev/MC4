using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorEngine.Templating;
using System.Configuration;

namespace MapCall.CommonTest.Model.Migrations
{
    [TestClass]
    [DeploymentItem("Microsoft.Data.SqlClient.SNI.x64.dll")]
    public class MigrationTest
    {
        #region Constants

        // Set to localhost so that locally it runs against your db and on fatman for ci it runs there.
        public string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["MCDev"].ToString();

        #endregion

        private long GetStartingMigration(Assembly assembly)
        {
            var sixMonthsAgo =
                decimal.Parse(DateTime.Now.AddMonths(-6).Year +
                              DateTime.Now.AddMonths(-6).Month.ToString().PadLeft(2, '0')) * 100000000000;
            var classes = from t in assembly.GetTypes()
                          let attributes = t.GetCustomAttributes(typeof(MigrationAttribute), true)
                          where attributes != null && attributes.Length > 0
                          select new {Type = t, attributes.Cast<MigrationAttribute>().First().Version};
            if (sixMonthsAgo < 20150520143934612)
                sixMonthsAgo = 20150520143934612;
            var lastSixMonths = classes.OrderBy(x => x.Version).Where(x => x.Version > sixMonthsAgo);
            return lastSixMonths.OrderByDescending(x => x.Version).Take(lastSixMonths.Count() / 6).Last().Version;
        }

        /// <summary>
        /// Note: We are relying on the migration number to be 17 digits long. 
        /// If this changes, the private method above needs to have the factor changed.
        /// </summary>
        [TestMethod]
        public void TestMigrationsRunDownUpDownUp()
        {
            if (Environment.MachineName.ToLowerInvariant().Contains("njs-ls") || Environment.MachineName.Contains("WINDOWS-RO5MTDA"))
                Assert.Inconclusive("This test doesn't run locally");

            var type = typeof(Common.Model.Migrations.AddHasAccessColumnToTblPermissions);
            var assembly = Assembly.GetAssembly(type);
            var startingMigration = GetStartingMigration(assembly);
            var runnerContext = new RunnerContext(new TextWriterAnnouncer(Console.Out)) {Namespace = type.Namespace};
            runnerContext.Tags = new[] {"Production"};

            var processor = new SqlServer2008ProcessorFactory().Create(CONNECTION_STRING,
                new TextWriterAnnouncer(Console.Out),
                new ProcessorOptions());
            var runner = new FluentMigrator.Runner.MigrationRunner(assembly, runnerContext, processor);
            MyAssert.DoesNotThrow(() => runner.MigrateDown(startingMigration, false),
                "Failed 1st Down Migration using connection string '{0}'", CONNECTION_STRING);
            MyAssert.DoesNotThrow(() => runner.MigrateUp(false),
                "Failed 1st Up Migration using connection string '{0}'", CONNECTION_STRING);
            MyAssert.DoesNotThrow(() => runner.MigrateDown(startingMigration, false),
                "Failed 2nd Down Migration using connection string '{0}'", CONNECTION_STRING);
            MyAssert.DoesNotThrow(() => runner.MigrateUp(false),
                "Failed 2nd Up Migration using connection string '{0}'", CONNECTION_STRING);

            // It'd be nice if this could do a simple select of every table we have mapped at the end. That way an exception
            // could be thrown if we're still mapping to properties that no longer exist, as the in-mem database tests will
            // continue to create the fields if they're still mapped.
        }
    }
}
