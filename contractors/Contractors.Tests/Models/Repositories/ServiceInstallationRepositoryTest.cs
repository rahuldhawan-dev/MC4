using System.Linq;
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class ServiceInstallationRepositoryTest : ContractorsControllerTestBase<ServiceInstallation, ServiceInstallationRepository>
    {
        #region Private Members

        private OperatingCenter _currentOperatingCenter;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _currentOperatingCenter = GetFactory<OperatingCenterFactory>().Create();
            Repository = _container.GetInstance<ServiceInstallationRepository>();
        }

        #endregion

        #region Exposed Methods

        //public static ServiceInstallation SetLookupIds(ServiceInstallation serviceInstallation)
        //{
        //    // we don't want to push this into the repository so we're doing it here.
        //    // could also possibly go into an onsaved/saving method in the factory.
        //    serviceInstallation.MainFailureTypeID = serviceInstallation.MainFailureType.MainFailureTypeID;
        //    serviceInstallation.ServiceSizeID = serviceInstallation.ServiceSize.Id;
        //    serviceInstallation.MainConditionID = serviceInstallation.MainCondition.MainConditionID;
        //    serviceInstallation.ServiceInstallationMaterialID = serviceInstallation.ServiceInstallationMaterial.ServiceInstallationMaterialID;
        //    serviceInstallation.ServiceInstallationSoilConditionID = serviceInstallation.ServiceInstallationSoilCondition.ServiceInstallationSoilConditionID;
        //    serviceInstallation.ServiceInstallationDisinfectionMethodID =
        //        serviceInstallation.ServiceInstallationDisinfectionMethod.ServiceInstallationDisinfectionMethodID;
        //    serviceInstallation.ServiceInstallationFlushMethodID = serviceInstallation.ServiceInstallationFlushMethod.ServiceInstallationFlushMethodID;
        //    return serviceInstallation;
        //}

        #endregion

        #region Tests

        #region Linq/Criteria

        [TestMethod]
        public void TestLinqOnlyAllowsAccessToTheServiceInstallationsBelongingToWorkOrdersAssignedToTheContractorThatTheCurrentUserBelongsTo()
        {
            var expectedOrders = GetFactory<WorkOrderFactory>().CreateArray(2, new
            {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = _currentOperatingCenter
            });
            var extraOrders = GetFactory<WorkOrderFactory>().CreateArray(2,
                new { OperatingCenter = _currentOperatingCenter });
            var expected = new[] {
                GetFactory<ServiceInstallationFactory>().Create(new {WorkOrder = expectedOrders[0]}),
                GetFactory<ServiceInstallationFactory>().Create(new {WorkOrder = expectedOrders[1]})
            };
            var extra = new[] {
                GetFactory<ServiceInstallationFactory>().Create(new {WorkOrder = extraOrders[0]}),
                GetFactory<ServiceInstallationFactory>().Create(new {WorkOrder = extraOrders[1]})
            };

            var actual = Repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToTheServiceInstallationsBelongingToWorkOrdersAssignedToTheContractorThatTheCurrentUserBelongsTo()
        {
            var expectedOrders = GetFactory<WorkOrderFactory>().CreateArray(2, new
            {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = _currentOperatingCenter
            });
            var extraOrders = GetFactory<WorkOrderFactory>().CreateArray(2,
                new { OperatingCenter = _currentOperatingCenter });
            var expected = new[] {
                GetFactory<ServiceInstallationFactory>().Create(new {WorkOrder = expectedOrders[0]}),
                GetFactory<ServiceInstallationFactory>().Create(new {WorkOrder = expectedOrders[1]})
            };
            var extra = new[] {
                GetFactory<ServiceInstallationFactory>().Create(new {WorkOrder = extraOrders[0]}),
                GetFactory<ServiceInstallationFactory>().Create(new {WorkOrder = extraOrders[1]})
            };

            var actual = Repository.Search(Restrictions.Conjunction()).List<ServiceInstallation>().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        #endregion

        #endregion
    }
}