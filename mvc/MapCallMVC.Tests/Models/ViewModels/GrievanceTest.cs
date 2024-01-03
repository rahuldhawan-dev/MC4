using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class GrievanceTest : MapCallMvcInMemoryDatabaseTestBase<Grievance, GrievanceRepository>
    {
        #region Init/Cleanup

        private Mock<IDateTimeProvider> _dateTimeProvider;

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            _dateTimeProvider = i.For<IDateTimeProvider>().Mock();
            i.For<IAuthenticationService<User>>().Mock();
            i.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        #endregion

        [TestMethod]
        public void TestGrievanceIsCreatedWithDateReceivedDefaultingToCurrentDate()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);

            var obj = GetFactory<GrievanceFactory>().BuildWithConcreteDependencies();
            var model = _viewModelFactory.Build<CreateGrievance, Grievance>(obj);
            obj = Repository.Save(model.MapToEntity(obj));

            Assert.AreEqual(now, obj.DateReceived);
        }

        [TestMethod]
        public void TestAllOfTheMapping()
        {
            Assert.Inconclusive("TODO");
        }

        [TestMethod]
        public void TestAllOfTheValidation()
        {
            Assert.Inconclusive("TODO");
        }
        
        [TestMethod]
        public void TestOperatingCenterIdIsRequiredField()
        {
            var model = _viewModelFactory.Build<CreateGrievance>();
            model.OperatingCenter = null;
            ValidationAssert.PropertyIsRequired(model, x => x.OperatingCenter, "The OperatingCenter field is required.");
        }

        [TestMethod]
        public void TestOperatingCenterIdMustMatchExistingEntity()
        {
            var model = _viewModelFactory.Build<CreateGrievance>();
            model.OperatingCenter = -1;
            ValidationAssert.ModelStateHasError(model, x => x.OperatingCenter, "OperatingCenter's value does not match an existing object.");
        }
        
    }
}
