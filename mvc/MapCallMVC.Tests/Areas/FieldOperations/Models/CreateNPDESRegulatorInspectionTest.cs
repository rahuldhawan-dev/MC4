using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateNpdesRegulatorInspectionTest : NpdesRegulatorInspectionViewModelTestBase<CreateNpdesRegulatorInspection>
    {
        #region Tests

        [TestMethod]
        public void TestSetDefaultsSetsArrivalDateTimeToDateTimeNow()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var sewerOpening = GetEntityFactory<SewerOpening>().Create();

            var target = new CreateNpdesRegulatorInspection(_container) {
                SewerOpening = sewerOpening.Id
            };
            target.SetDefaults();

            Assert.AreEqual(now, target.ArrivalDateTime);
        }

        [TestMethod]
        public void TestMapToEntitySetsInspectedBy()
        {
            Session.Evict(_entity);
            _entity.InspectedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.InspectedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsDepartureDateTimeToNow()
        {
            var now = DateTime.Now;
            _dateTimeProvider
               .Setup(dt => dt.GetCurrentDate())
               .Returns(now);

            _vmTester.MapToEntity();

            Assert.AreEqual(now, _entity.DepartureDateTime);
        }

        #endregion
    }
}
