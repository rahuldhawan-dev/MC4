using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class EditSpoilFinalProcessingLocationTest : ViewModelTestBase<SpoilFinalProcessingLocation, EditSpoilFinalProcessingLocation>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IStateRepository>().Use<StateRepository>();
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.Name)
               .CanMapBothWays(x => x.Town)
               .CanMapBothWays(x => x.Street);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.Name);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation() { }

        [TestMethod]
        public override void TestStringLengthValidation() { }

        [TestMethod]
        public void Test_Display_ReturnsOriginalSpoilFinalProcessingLocation()
        {
            var entity = GetEntityFactory<SpoilFinalProcessingLocation>().Create();
            var vm = _viewModelFactory.Build<EditSpoilFinalProcessingLocation, SpoilFinalProcessingLocation>(entity);
            Assert.AreSame(entity, vm.Display);
        }

        #endregion
    }
}