using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Customer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Customer.Controllers
{
    [TestClass]
    public class MostRecentlyInstalledServiceControllerTest
        : MapCallMvcControllerTestBase<MostRecentlyInstalledServiceController, MostRecentlyInstalledService>
    {
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IImageToPdfConverter>().Mock();
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly(
                    "~/Customer/" +
                    nameof(MostRecentlyInstalledService) +
                    "/" +
                    nameof(MostRecentlyInstalledServiceController.ByInstallationNumberAndOperatingCenter) + 
                    "/");
            });
        }

        [TestMethod]
        public void Test_ByInstallationNumberAndOperatingCenter_ReturnsJsonOfSingleResult()
        {
            var installation = "installation";
            var opCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            
            var goodPremise = GetEntityFactory<Premise>().Create(new {
                Installation = installation,
                OperatingCenter = opCenter
            });
            var goodPremiseService = GetEntityFactory<Service>().Create(new {
                Premise = goodPremise
            });

            var badPremiseInstallation = GetEntityFactory<Premise>().Create(new {
                Installation = "bad" + installation,
                OperatingCenter = opCenter
            });
            var badPremiseInstallationService = GetEntityFactory<Service>().Create(new {
                Premise = badPremiseInstallation
            });

            var badPremiseOpCenter = GetEntityFactory<Premise>().Create(new {
                Installation = installation,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create()
            });
            var badPremiseOpCenterService = GetEntityFactory<Service>().Create(new {
                Premise = badPremiseOpCenter
            });

            var result = _target.ByInstallationNumberAndOperatingCenter(installation, opCenter.Id)
                as JsonResult;
            var resultData = result.Data.GetPropertyValueByName("Data");
            
            Assert.AreEqual(goodPremiseService.Premise.Id, resultData.GetPropertyValueByName("PremiseId"));
            Assert.AreEqual(goodPremiseService.Id, resultData.GetPropertyValueByName("ServiceId"));
        }

        [TestMethod]
        public void Test_ByInstallationNumberAndOperatingCenter_ReturnsNull_WhenMoreThanOneResult()
        {
            var installation = "installation";
            var opCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            
            var goodPremise1 = GetEntityFactory<Premise>().Create(new {
                Installation = installation,
                OperatingCenter = opCenter
            });
            var goodPremiseService1 = GetEntityFactory<Service>().Create(new {
                Premise = goodPremise1
            });

            var goodPremise2 = GetEntityFactory<Premise>().Create(new {
                Installation = installation,
                OperatingCenter = opCenter
            });
            var goodPremiseService = GetEntityFactory<Service>().Create(new {
                Premise = goodPremise2
            });

            var result = _target.ByInstallationNumberAndOperatingCenter(installation, opCenter.Id)
                as JsonResult;

            Assert.IsNull(result.Data.GetPropertyValueByName("Data"));
        }
    }
}
