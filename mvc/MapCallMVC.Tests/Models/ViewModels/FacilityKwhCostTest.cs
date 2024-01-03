using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class FacilityKwhCostTest : MapCallMvcInMemoryDatabaseTestBase<FacilityKwhCost>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ISensorRepository>().Use<SensorRepository>();
        }
        
        #endregion

        [TestMethod]
        public void TestCannotCreateWhereStartDateWouldOverlap()
        {
            var facility = GetEntityFactory<Facility>().Create();
            var existingStartDate = DateTime.Now;
            GetEntityFactory<FacilityKwhCost>().Create(new {
                Facility = facility,
                StartDate = existingStartDate,
                EndDate = existingStartDate.AddDays(5)
            });

            Session.Flush();
            Session.Clear();

            var newCost = _viewModelFactory.BuildWithOverrides<CreateFacilityKwhCost, FacilityKwhCost>(GetEntityFactory<FacilityKwhCost>().Build(new {
                Facility = facility,
                StartDate = existingStartDate.AddDays(-4),
                EndDate = existingStartDate.AddDays(1)
            }), new {
                OperatingCenter = facility.OperatingCenter.Id
            });

            ValidationAssert.SomethingAboutModelIsNotValid(newCost);

            newCost.StartDate = newCost.StartDate.Value.AddDays(-2);
            newCost.EndDate = newCost.EndDate.Value.AddDays(-2);

            ValidationAssert.WholeModelIsValid(newCost);
        }

        [TestMethod]
        public void TestCannotCreateWhereEndDateWouldOverlap()
        {
            var facility = GetEntityFactory<Facility>().Create();
            var existingEndDate = DateTime.Now;
            GetEntityFactory<FacilityKwhCost>().Create(new {
                Facility = facility,
                StartDate = existingEndDate.AddDays(-5),
                EndDate = existingEndDate
            });

            Session.Flush();
            Session.Clear();

            var newCost = _viewModelFactory.BuildWithOverrides<CreateFacilityKwhCost, FacilityKwhCost>( GetEntityFactory<FacilityKwhCost>().Build(new {
                Facility = facility,
                StartDate = existingEndDate.AddDays(-1),
                EndDate = existingEndDate.AddDays(4)
            }), new {
                OperatingCenter = facility.OperatingCenter.Id
            });

            ValidationAssert.SomethingAboutModelIsNotValid(newCost);

            newCost.StartDate = newCost.StartDate.Value.AddDays(2);
            newCost.EndDate = newCost.EndDate.Value.AddDays(2);

            ValidationAssert.WholeModelIsValid(newCost);
        }

        [TestMethod]
        public void TestCannotUpdateWhereStartDateWouldOverlap()
        {
            var facility = GetEntityFactory<Facility>().Create();
            var existingStartDate = DateTime.Now;
            GetEntityFactory<FacilityKwhCost>().Create(new {
                Facility = facility,
                StartDate = existingStartDate,
                EndDate = existingStartDate.AddDays(5)
            });
            var existing = GetEntityFactory<FacilityKwhCost>().Create(new {
                Facility = facility,
                StartDate = existingStartDate.AddDays(-5),
                EndDate = existingStartDate.AddDays(-1)
            });

            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.BuildWithOverrides<EditFacilityKwhCost, FacilityKwhCost>(existing, new {
                EndDate = existingStartDate.AddDays(1)
            });

            ValidationAssert.SomethingAboutModelIsNotValid(model);

            model.EndDate = existingStartDate.AddDays(-1);

            ValidationAssert.WholeModelIsValid(model);
        }

        [TestMethod]
        public void TestCannotUpdateWhereEndDateWouldOverlap()
        {
            var facility = GetEntityFactory<Facility>().Create();
            var existingEndDate = DateTime.Now;
            GetEntityFactory<FacilityKwhCost>().Create(new {
                Facility = facility,
                StartDate = existingEndDate.AddDays(-5),
                EndDate = existingEndDate
            });
            var existing = GetEntityFactory<FacilityKwhCost>().Create(new {
                Facility = facility,
                StartDate = existingEndDate.AddDays(1),
                EndDate = existingEndDate.AddDays(5)
            });

            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.BuildWithOverrides<EditFacilityKwhCost, FacilityKwhCost>(existing, new {
                StartDate = existingEndDate.AddDays(-1)
            });

            ValidationAssert.SomethingAboutModelIsNotValid(model);

            model.StartDate = existingEndDate.AddDays(1);

            ValidationAssert.WholeModelIsValid(model);
        }
    }
}
