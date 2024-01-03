using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System.Web.Mvc;
using System;
using System.Linq;
using MapCall.Common.Testing.Data;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class NpdesRegulatorsDueInspectionReportControllerTest : MapCallMvcControllerTestBase<
        NpdesRegulatorsDueInspectionReportController, SewerOpening, SewerOpeningRepository>
    {
        #region Constants

        const RoleModules role = RoleModules.FieldServicesAssets;

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const string path = "~/Reports/NpdesRegulatorsDueInspectionReport/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
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

            var search = new SearchNpdesRegulatorsDueInspectionReport {
                OperatingCenter = operatingCenter1.Id,
                OperatingCenterId = operatingCenter1.Id,
                Town = town.Id,
                TownId = town.Id,
                Status = activeStatus.Id,
                StatusId = activeStatus.Id,
                DepartureDateTime = new RequiredDateRange {
                    Start = new DateTime(2023, 7, 19),
                    End = new DateTime(2023, 7, 30),
                    Operator = 0
                }
            };

            _target.ControllerContext = new ControllerContext();

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchNpdesRegulatorsDueInspectionReport)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreEqual(regulator1.OperatingCenter?.Id, resultModel[0].OperatingCenterId);
            Assert.AreEqual(regulator1.Town?.Id, resultModel[0].TownId);
            Assert.AreEqual(regulator1.Status.Id, resultModel[0].StatusId);
        }

        #endregion
    }
}