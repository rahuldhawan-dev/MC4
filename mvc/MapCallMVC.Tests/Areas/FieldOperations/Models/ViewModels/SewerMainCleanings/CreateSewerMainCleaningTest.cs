using System;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings
{
    [TestClass]
    public class CreateSewerMainCleaningTest : SewerMainCleaningViewModelTestBase<CreateSewerMainCleaning>
    {
        #region Tests

        [TestMethod]
        public void TestSetDefaultsSetsDateToDateTimeNow()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);

            var target = new CreateSewerMainCleaning(_container);
            target.SetDefaults();

            Assert.AreEqual(now, target.Date);
        }

        #endregion

        [TestMethod]
        public void TestSetValuesFromNewSewerOpeningMapsPropertiesFromModelWhenCalled()
        {
            var _entity1 = new SewerMainCleaning();
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var street = GetEntityFactory<Street>().Create();
            var interesectingStreet = GetEntityFactory<Street>().Create();
            var model = GetEntityFactory<SewerOpening>().Create(new {OperatingCenter = opc, Street = street, Town = town, IntersectingStreet = interesectingStreet });
            var _viewModel = new CreateSewerMainCleaning(_container);
            var _vmTester1 = new ViewModelTester<CreateSewerMainCleaning, SewerMainCleaning>(_viewModel, _entity1);

            _viewModel.SetValuesFromNewSewerOpening(model);

            _vmTester1.MapToEntity();

            Assert.AreEqual(_viewModel.Opening1, _entity1.Opening1.Id);
            Assert.AreEqual(_viewModel.Street, _entity1.Street.Id);
            Assert.AreEqual(_viewModel.CrossStreet, _entity1.CrossStreet.Id);
            Assert.AreEqual(_viewModel.Town, _entity1.Town.Id);
            Assert.AreEqual(_viewModel.OperatingCenter, _entity1.OperatingCenter.Id);
            Assert.IsTrue(_entity1.NeedsToSync);
        }
    }
}