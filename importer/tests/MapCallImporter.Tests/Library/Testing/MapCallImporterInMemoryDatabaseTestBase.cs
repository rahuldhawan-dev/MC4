using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallImporter.Library.TypeRegistration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.V2;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Validation;
using NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace MapCallImporter.Library.Testing
{
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
    [DeploymentItem(@"SampleFiles\", "SampleFiles")]
    public class MapCallImporterInMemoryDatabaseTestBase : InMemoryDatabaseTest<Street>
    {
        #region Private Members

        protected DateTime _now;
        protected TestDateTimeProvider _dateTimeProvider;

        #endregion

        #region Private Methods

        protected override IInMemoryDatabaseTestInterceptor CreateInterceptor()
            => _container.GetInstance<MapCallImporterInMemoryDatabaseTestInterceptorWithChangeTracking>();

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(TownFactory).Assembly);
        }

        [TestInitialize]
        public void MapCallImporterInMemoryDatabaseTestBaseTestInitialize()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(EntityMustExistAttribute),
                (metadata, context, attribute) =>
                    Container.With(metadata).With(context).With(attribute as EntityMustExistAttribute).GetInstance<EntityMustExistAttributeAdapter>());
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            i.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));

            base.InitializeObjectFactory(i);

            i.AddRegistry<TestStructureMapRegistry>();
        }

        #endregion

        #region Exposed Methods

        [DebuggerStepThrough]
        public string GetRelativePath(string filePath)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), filePath));
        }

        [DebuggerStepThrough]
        public void WithOpenExcelFile(string filePath, Action<string> fn)
        {
            using (var spreadSheet = SpreadsheetDocument.Open(filePath, true))
            {
                try
                {
                    fn(filePath);
                }
                finally
                {
                    spreadSheet.Close();
                }
            }
        }

        [DebuggerStepThrough]
        public void ExpectCountChange<TEntity>(int increase, Action fn)
        {
            var countBefore = _container.GetInstance<ISession>().Query<TEntity>().Count();

            fn();

            var expected = countBefore + increase;
            var actual = Session.Query<TEntity>().Count();
            var difference = expected - actual;

            if (difference == 0)
            {
                return;
            }

            Assert.AreEqual(expected, actual,
                $"{(increase > 0 ? "Increase" : "Decrease")} in count of {typeof(TEntity).Name} records was {(difference > 0 ? "less" : "greater")} than expected.");
        }

        [DebuggerStepThrough]
        public void WithUnitOfWork(Action<IUnitOfWork> fn)
        {
            using (var uow = Container.GetInstance<IUnitOfWorkFactory>().Build())
            {
                fn(uow);
            }
        }

        #endregion
    }

    public class MapCallImporterInMemoryDatabaseTestBase<TTarget> : MapCallImporterInMemoryDatabaseTestBase
    {
        #region Private Members

        protected TTarget _target;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void MapCallImporterInMemoryDatabaseTestBaseWithTargetTestInitialize()
        {
            _target = _container.GetInstance<TTarget>();
        }

        #endregion
    }
}