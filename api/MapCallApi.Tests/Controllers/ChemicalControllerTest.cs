using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class ChemicalControllerTest : MapCallApiControllerTestBase<ChemicalController, Chemical, IRepository<Chemical>>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/Chemical/Index", ChemicalController.ROLE);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults() { }

        [TestMethod]
        public void Test_Index_ReturnsProperJson()
        {
            var gasStateOfMatter = GetFactory<GasStateOfMatterFactory>().Create();
            var liquidStateOfMatter = GetFactory<LiquidStateOfMatterFactory>().Create();
            var solidStateOfMatter = GetFactory<SolidStateOfMatterFactory>().Create();
            var packagingType = GetEntityFactory<PackagingType>().Create(new {Description = "Bulk"});
            var chemicalType = GetEntityFactory<ChemicalType>().Create();

            _ = GetEntityFactory<Chemical>().Create(new {
                Name = "chem-x",
                PackagingType = packagingType,
                PartNumber = "part-x",
                Appearance = "appearance-x",
                SdsHyperlink = "https://sds.hyperlink.com",
                PackagingUnits = "packaging-units-x",
                WetPoundsPerGal = (float?)2.22,
                SpecificGravityMin = (float?)5.55,
                SpecificGravityMax = (float?)7.55,
                PricePerPoundWet = (decimal?)1.11d,
                ConcentrationLbsPerGal = (float?)4.44,
                RatioResidualProduction = (float?)6.66,
                ChemicalConcentrationLiquid = (float?)3.33,
                ChemicalStates = new List<StateOfMatter> {
                    gasStateOfMatter,
                    liquidStateOfMatter
                },
                ChemicalType = chemicalType
            });

            var chemicalY = GetEntityFactory<Chemical>().Create(new {
                Name = "chem-y",
                PackagingType = packagingType,
                PartNumber = "part-y",
                Appearance = "appearance-y",
                SdsHyperlink = "https://sds.hyperlink.com",
                PackagingUnits = "packaging-units-y",
                WetPoundsPerGal = (float?)2.22,
                SpecificGravityMin = (float?)5.55,
                SpecificGravityMax = (float?)7.55,
                PricePerPoundWet = (decimal?)1.11d,
                ConcentrationLbsPerGal = (float?)4.44,
                RatioResidualProduction = (float?)6.66,
                ChemicalConcentrationLiquid = (float?)3.33,
                ChemicalStates = new List<StateOfMatter> {
                    liquidStateOfMatter,
                    solidStateOfMatter
                },
                ChemicalType = chemicalType
            });

            Session.Flush();

            var searchSet = new SearchChemical {
                Name = new SearchString {
                    Value = "y",
                    MatchType = SearchStringMatchType.Wildcard
                }
            };
            var jsonResponse = _target.Index(searchSet);
            var jsonResultTester = new JsonResultTester(jsonResponse);
            
            jsonResultTester.AreEqual(chemicalY.Id, "Id");
            jsonResultTester.AreEqual(chemicalY.Name, "Name");
            jsonResultTester.AreEqual(chemicalY.PackagingType.Description, "PackagingType");
            jsonResultTester.AreEqual(chemicalY.PartNumber, "PartNumber");
            jsonResultTester.AreEqual(chemicalY.Appearance, "Appearance");
            jsonResultTester.AreEqual(chemicalY.SdsHyperlink, "SdsHyperlink");
            jsonResultTester.AreEqual(chemicalY.PackagingUnits, "PackagingUnits");
            jsonResultTester.AreEqual(chemicalY.SpecificGravityMin, "SpecificGravityMin");
            jsonResultTester.AreEqual(chemicalY.SpecificGravityMax, "SpecificGravityMax");
            jsonResultTester.AreEqual(chemicalY.DisplayChemicalStates, "ChemicalForms");
            jsonResultTester.AreEqual(chemicalY.PricePerPoundWet, "PricePerPoundWet");
            jsonResultTester.AreEqual(chemicalY.WetPoundsPerGal, "WetPoundsPerGallon");
            jsonResultTester.AreEqual(chemicalY.RatioResidualProduction, "RatioResidualProduction");
            jsonResultTester.AreEqual(chemicalY.ConcentrationLbsPerGal, "ConcentrationPoundsPerGallon");
            jsonResultTester.AreEqual(chemicalY.ChemicalConcentrationLiquid, "ChemicalConcentrationLiquid");
            jsonResultTester.AreEqual(chemicalY.ChemicalType.Description, "ChemicalType");
            jsonResultTester.AreEqual(chemicalY.ExtremelyHazardousChemical, "ExtremelyHazardousChemical");
        }

        [TestMethod]
        public void Test_Index_ReturnsOnlyOneRecordWithMultipleStatesOfMatter()
        {
            var liquidStateOfMatter = GetFactory<LiquidStateOfMatterFactory>().Create();
            var solidStateOfMatter = GetFactory<SolidStateOfMatterFactory>().Create();
            var packagingType = GetEntityFactory<PackagingType>().Create(new { Description = "Bulk" });
            var chemicalType = GetEntityFactory<ChemicalType>().Create();

            var chemicalY = GetEntityFactory<Chemical>().Create(new {
                Name = "chem-y",
                PackagingType = packagingType,
                PartNumber = "part-y",
                Appearance = "appearance-y",
                SdsHyperlink = "https://sds.hyperlink.com",
                PackagingUnits = "packaging-units-y",
                WetPoundsPerGal = (float?)2.22,
                SpecificGravityMin = (float?)5.55,
                SpecificGravityMax = (float?)7.55,
                PricePerPoundWet = (decimal?)1.11d,
                ConcentrationLbsPerGal = (float?)4.44,
                RatioResidualProduction = (float?)6.66,
                ChemicalConcentrationLiquid = (float?)3.33,
                ChemicalStates = new List<StateOfMatter> {
                    liquidStateOfMatter,
                    solidStateOfMatter
                },
                ChemicalType = chemicalType
            });

            Session.Flush();

            var searchSet = new SearchChemical {
                Name = new SearchString {
                    Value = "y",
                    MatchType = SearchStringMatchType.Wildcard
                }
            };
            var jsonResponse = _target.Index(searchSet);
            var jsonResultTester = new JsonResultTester(jsonResponse);

            Assert.AreEqual(1, jsonResultTester.Count);
            jsonResultTester.AreEqual(chemicalY.Id, "Id");
        }

        #endregion
    }
}