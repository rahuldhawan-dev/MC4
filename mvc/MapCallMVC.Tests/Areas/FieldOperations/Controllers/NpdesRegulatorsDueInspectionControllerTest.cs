using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System.Web.Mvc;
using System;
using System.Linq;
using MapCall.Common.Testing.Data;
using MMSINC.Data;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class NpdesRegulatorsDueInspectionControllerTest : MapCallMvcControllerTestBase<NpdesRegulatorsDueInspectionController, SewerOpening>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/NpdesRegulatorsDueInspection/Index/", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var npdesRegulatorType = GetFactory<NpdesRegulatorSewerOpeningTypeFactory>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();

            var regulator1 = GetFactory<SewerOpeningFactory>().Create(new {
                OperatingCenter = operatingCenter1,
                Town = town,
                SewerOpeningType = npdesRegulatorType,
                Status = activeStatus
            });
            var regulator2 = GetFactory<SewerOpeningFactory>().Create(new {
                OperatingCenter = operatingCenter1,
                Town = town,
                SewerOpeningType = npdesRegulatorType,
                Status = activeStatus
            });
            var inspection1 = GetFactory<NpdesRegulatorInspectionFactory>().Create(new {
                SewerOpening = regulator1,
                ArrivalDateTime = new DateTime(2023, 6, 20),
                DepartureDateTime = new DateTime(2023, 6, 21)
            });
            var inspection2 = GetFactory<NpdesRegulatorInspectionFactory>().Create(new {
                SewerOpening = regulator2,
                ArrivalDateTime = new DateTime(2023, 7, 20),
                DepartureDateTime = new DateTime(2023, 7, 21)
            });

            var search = new SearchNpdesRegulatorsDueInspection {
                OperatingCenter = operatingCenter1.Id,
                Town = town.Id,
                Status = activeStatus.Id,
                DepartureDateTime = new RequiredDateRange {
                    Start = new DateTime(2023, 7, 19),
                    End = new DateTime(2023, 7, 30),
                    Operator = 0
                }
            };

            _target.ControllerContext = new ControllerContext();

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchNpdesRegulatorsDueInspection)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreEqual(regulator1.Id, resultModel[0].Id);
        }
    }
}