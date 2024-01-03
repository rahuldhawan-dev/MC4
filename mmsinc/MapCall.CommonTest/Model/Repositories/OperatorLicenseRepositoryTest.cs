using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate;
using NHibernate.Util;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    /// <summary>
    ///     Summary description for OperatorLicenseRepositoryTest
    /// </summary>
    [TestClass]
    public class
        OperatorLicenseRepositoryTest : MapCallEmployeeSecuredRepositoryTestBase<OperatorLicense,
            OperatorLicenseRepository>
    {
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSearchOperatorLicenseReportCanHandleExpiredProperty()
        {
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.HumanResourcesEmployeeLimited});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            Session.Save(user);

            var date = DateTime.Today;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            var now = DateTime.Now.Date;
            var yesterday = now.AddDays(-1);

            var opcPrime = GetFactory<OperatingCenterFactory>().Create();
            var operatorTypePrime = GetFactory<OperatorLicenseTypeFactory>().Create(new {Description = "Yes"});
            var validOperatorLicense = GetFactory<OperatorLicenseFactory>().Create(new
                {OperatingCenter = opcPrime, OperatorLicenseType = operatorTypePrime});
            var expiredOperatorLicense = GetFactory<OperatorLicenseFactory>().Create(new {
                OperatingCenter = opcPrime, OperatorLicenseType = operatorTypePrime,
                ExpirationDate = yesterday
            });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<OperatorLicenseRepository>();
            //test for Expired not initialized (null)
            var search = new TestSearchOperatorLicenseReport();
            var result = Repository.SearchOperatorLicenseReport(search);
            Assert.IsTrue(result.Contains(validOperatorLicense));
            Assert.IsTrue(result.Contains(expiredOperatorLicense));

            //test for Expired = true
            search = new TestSearchOperatorLicenseReport {
                Expired = true
            };
            result = Repository.SearchOperatorLicenseReport(search);
            Assert.IsFalse(result.Contains(validOperatorLicense));
            Assert.IsTrue(result.Contains(expiredOperatorLicense));

            //test for Expired = false
            search = new TestSearchOperatorLicenseReport {
                Expired = false
            };
            result = Repository.SearchOperatorLicenseReport(search);
            Assert.IsTrue(result.Contains(validOperatorLicense));
            Assert.IsFalse(result.Contains(expiredOperatorLicense));
        }

        #endregion

        public class TestSearchOperatorLicenseReport : SearchSet<OperatorLicense>, ISearchOperatorLicenseReport
        {
            public int? State { get; set; }
            public int? OperatingCenter { get; set; }
            public bool? Expired { get; set; }
            public int? EmployeeStatus { get; set; }
        }
    }
}
