using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Validation;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        SewerOpeningRepositoryTest : MapCallMvcSecuredRepositoryTestBase<SewerOpening, SewerOpeningRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesAssets;

        #endregion

        #region Fields

        private Mock<IDateTimeProvider> _dateProvider;

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateProvider = e.For<IDateTimeProvider>().Mock<IDateTimeProvider>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        #region Tests

        #region Linq/Criteria

        [TestMethod]
        public void TestLinqDoesNotReturnValvesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opCntr1 });
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });
            Session.Save(user);
            var sewerOpening1 = GetEntityFactory<SewerOpening>()
               .Create(new { OperatingCenter = opCntr1 });
            var sewerOpening2 = GetEntityFactory<SewerOpening>()
               .Create(new { OperatingCenter = opCntr2 });
            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<SewerOpeningRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(sewerOpening1));
            Assert.IsFalse(result.Contains(sewerOpening2));
        }

        [TestMethod]
        public void TestLinqReturnsAllSewerOpeningsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });
            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);
            var sewerOpening1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            Repository = _container.GetInstance<SewerOpeningRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(sewerOpening1));
            Assert.IsTrue(result.Contains(sewerOpening2));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnValvesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opCntr1 });
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });
            Session.Save(user);
            var sewerOpening1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<SewerOpeningRepository>();
            var model = new EmptySearchSet<SewerOpening>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(sewerOpening1));
            Assert.IsFalse(result.Contains(sewerOpening2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllSewerOpeningsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });
            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);
            var sewerOpening1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            Repository = _container.GetInstance<SewerOpeningRepository>();
            var model = new EmptySearchSet<SewerOpening>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(sewerOpening1));
            Assert.IsTrue(result.Contains(sewerOpening2));
        }

        #endregion

        #region GetValvesWithSapIssues

        [TestMethod]
        public void TestGetSewerOpeningsWithSapIssuesReturnsHydrantsWithSapIssues()
        {
            var sewerOpeningValid1 = GetFactory<SewerOpeningFactory>()
               .Create(new { SAPErrorCode = "RETRY::Something went wrong" });
            var sewerOpeningInvalid1 = GetFactory<SewerOpeningFactory>()
               .Create(new { SAPErrorCode = "" });
            var sewerOpeningInvalid2 = GetFactory<SewerOpeningFactory>().Create();

            var result = Repository.GetSewerOpeningsWithSapRetryIssues();

            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #region Opening Number

        [TestMethod]
        public void TestGenerateNextHydrantNumberReturnsOneIfThereAreNoOpeningsForTheTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                { Town = town, OperatingCenter = opc1, Abbreviation = "OO" });

            var result = Repository.GenerateNextOpeningNumber(_container.GetInstance<AbbreviationTypeRepository>(),
                _container.GetInstance<RepositoryBase<SewerOpening>>(), opc1, town, null);

            Assert.AreEqual(1, result.Suffix);
        }

        [TestMethod]
        public void TestGenerateNextHydrantNumberReturnsMaxIfThereAreExistingOpeningsForTheTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                { Town = town, OperatingCenter = opc1, Abbreviation = "OO" });
            var sm1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = town, OperatingCenter = opc1, OpeningNumber = "MOO-1", OpeningSuffix = 1
            });
            var sm2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = town, OperatingCenter = opc1, OpeningNumber = "MOO-2", OpeningSuffix = 2
            });

            var result = Repository.GenerateNextOpeningNumber(_container.GetInstance<AbbreviationTypeRepository>(),
                _container.GetInstance<RepositoryBase<SewerOpening>>(), opc1, town, null);

            Assert.AreEqual(3, result.Suffix);
            Assert.AreEqual("MOO", result.Prefix);
        }

        #endregion

        #region Getting the correct abbreviation for the Opening prefix

        [TestMethod]
        public void TestAbbreviationIsTownAbbreviationIfTownAbbreviationTypeIsTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(TownAbbreviationTypeFactory)
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                OperatingCenter = opc1, Town = town, Abbreviation = "QQ"
            });

            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town, Abbreviation = "SS"
            });

            var Opening1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = town, OpeningSuffix = 1, OpeningNumber = "MQQ-1", OperatingCenter = opc1
            });

            var rawRepo = _container.GetInstance<RepositoryBase<SewerOpening>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();

            var result = Repository.GenerateNextOpeningNumber(abbrRepo, rawRepo, opc1, town, null);
            Assert.AreEqual("MQQ-2", result.FormattedNumber);

            result = Repository.GenerateNextOpeningNumber(abbrRepo, rawRepo, opc1, town, townSection);
            Assert.AreEqual("MQQ-2", result.FormattedNumber, "Town abbreviation must be used in this instance.");
        }

        [TestMethod]
        public void
            TestAbbreviationIsTownSectionAbbreviationIfTownAbbreviationTypeIsTownSectionAndTownSectionIsNotNullAndTownSectionAbbreviationIsNotNullOrEmptyOrWhiteSpace()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(TownSectionAbbreviationTypeFactory),
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                { OperatingCenter = opc1, Town = town, Abbreviation = "QQ" });
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town,
                Abbreviation = "SS"
            });
            var townSection2 = GetFactory<TownSectionFactory>().Create(new { Town = town, Abbreviation = "" });

            var opening1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = town, OpeningSuffix = 10, OpeningNumber = "MQQ-10", OperatingCenter = opc1
            });
            var opening2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = town, TownSection = townSection, OpeningSuffix = 5, OpeningNumber = "HSS-5",
                OperatingCenter = opc1
            });
            var opening3 = GetEntityFactory<SewerOpening>().Create(new {
                Town = town, TownSection = townSection2, OpeningSuffix = 11, OpeningNumber = "MQQ-11",
                OperatingCenter = opc1
            });

            //var result = Repository.GenerateNextOpeningNumber(town, null);
            //Assert.AreEqual("MQQ-12", result.FormattedNumber, "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");

            //result = Repository.GenerateNextOpeningNumber(town, townSection2);
            //Assert.AreEqual("MQQ-12", result.FormattedNumber, "Town abbreviation must be used when the town section's abbreviation is null and the abbreviation type is town section.");

            //result = Repository.GenerateNextOpeningNumber(town, townSection);
            //Assert.AreEqual("HSS-6", result.FormattedNumber, "Town section abbreviation must be used in this instance.");

            var opening4 = GetEntityFactory<SewerOpening>().Create(new
                { Town = town, TownSection = townSection, OpeningSuffix = 20, OpeningNumber = "HSS-20" });

            var rawRepo = _container.GetInstance<RepositoryBase<SewerOpening>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();

            var result = Repository.GenerateNextOpeningNumber(abbrRepo, rawRepo, opc1, town, null);
            Assert.AreEqual("MQQ-12", result.FormattedNumber,
                "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");
        }

        #endregion

        #region FindByOperatingCenterAndNumber

        [TestMethod]
        public void TestFindByOperatingCenterAndOpeningNumberDoesExactlyThat()
        {
            var sm1 = GetEntityFactory<SewerOpening>().Create(new {
                OpeningNumber = "MOO-1", OpeningSuffix = 1
            });
            var sm2 = GetEntityFactory<SewerOpening>().Create(new {
                OpeningNumber = "MOO-2", OpeningSuffix = 2
            });

            var result = Repository.FindByOperatingCenterAndOpeningNumber(sm1.OperatingCenter, sm1.OpeningNumber)
                                   .Single();

            Assert.AreSame(sm1, result);
        }

        [TestMethod]
        public void
            TestTestFindByOperatingCenterAndOpeningNumberDoesNotBlowUpTheSpotIfYouUseLinqAnyExtensionOnAResultsReutnrsForANonAdminUser()
        {
            var sm1 = GetEntityFactory<SewerOpening>().Create(new {
                OpeningNumber = "MOO-1", OpeningSuffix = 1
            });

            Repository = _container.GetInstance<SewerOpeningRepository>();

            MyAssert.DoesNotThrow(() =>
                Repository.FindByOperatingCenterAndOpeningNumber(sm1.OperatingCenter, sm1.OpeningNumber)
                          .Any());
        }

        #endregion

        #region RouteByTownId

        [TestMethod]
        public void TestRouteByTownId()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var smv1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Route = 1
            });
            var smv2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Route = 2
            });
            var smiv1 = GetEntityFactory<SewerOpening>().Create(new { Town = towns[0] });
            var smiv2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[1], Route = 3
            });

            var result = Repository.RouteByTownId(towns[0].Id);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(smv1.Route, result.First());
            Assert.AreEqual(smv2.Route, result.Last());
            Assert.AreNotEqual(smiv1.Route, result.First());
        }

        #endregion

        #region ByTownId

        [TestMethod]
        public void TestByTownIdReturnsOpeningsForTown()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var smv1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Route = 1
            });
            var smv2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Route = 2
            });
            var smv3 = GetEntityFactory<SewerOpening>().Create(new { Town = towns[0] });
            var smiv1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[1], Route = 3
            });

            var result = Repository.FindByTownId(towns[0].Id);

            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Contains(smv1));
            Assert.IsTrue(result.Contains(smv2));
            Assert.IsTrue(result.Contains(smv3));
            Assert.IsFalse(result.Contains(smiv1));
        }

        [TestMethod]
        public void TestActiveByTownIdReturnsOpeningsForTown()
        {
            var assetStatus1 = GetFactory<ActiveAssetStatusFactory>().Create();
            var assetStatus2 = GetFactory<PendingAssetStatusFactory>().Create();
            var towns = GetEntityFactory<Town>().CreateList(2);
            var smv1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Route = 1, Status = assetStatus1
            });
            var smv2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Route = 2, Status = assetStatus1
            });
            var smiv1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Status = assetStatus2
            });
            var smiv2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[1], Route = 3
            });

            var result = Repository.FindActiveByTownId(towns[0].Id);

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(smv1));
            Assert.IsTrue(result.Contains(smv2));
            Assert.IsFalse(result.Contains(smiv1));
            Assert.IsFalse(result.Contains(smiv2));
        }

        [TestMethod]
        public void TestGetAllForDropDownReturnsAllForDropDown()
        {
            var assetStatus = GetEntityFactory<AssetStatus>().CreateList(5);
            var towns = GetEntityFactory<Town>().CreateList(2);
            var smv1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Route = 1, Status = assetStatus[0]
            });
            var smv2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Route = 2, Status = assetStatus[0]
            });
            var smiv1 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[0], Status = assetStatus[1]
            });
            var smiv2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[1], Route = 3
            });

            var result = Repository.GetAllForDropDown();

            Assert.AreEqual(4, result.Count());
        }

        #endregion

        #region AutoComplete

        [TestMethod]
        public void TestFindByPartialOpeningMatchByTownReturnsMatch()
        {
            var towns = GetEntityFactory<Town>().CreateList(5);
            var assetStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            var sewerList1 = GetEntityFactory<SewerOpening>().CreateList(3, new { Town = towns[0] });
            var sewer1 = GetEntityFactory<SewerOpening>().Create(new {
                OpeningNumber = "whiter0s3", Town = towns[0]
            });
            var sewer2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[1], OpeningNumber = "Tyr3ll"
            });
            var sewer3 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[1], OpeningNumber = "Tyr3ll", Status = assetStatus
            });

            var result = Repository.FindByPartialOpeningMatchByTown("r0", towns[0].Id);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(sewer1.OpeningNumber, result.First().ToString());

            result = Repository.FindByPartialOpeningMatchByTown("r3", towns[1].Id);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(sewer2.OpeningNumber, result.First().ToString());
        }

        [TestMethod]
        public void TestFindByPartialOpeningMatchByTownReturnsNoMatch()
        {
            var towns = GetEntityFactory<Town>().CreateList(5);
            var sewerList1 = GetEntityFactory<SewerOpening>().CreateList(3, new { Town = towns[0] });
            var sewer1 = GetEntityFactory<SewerOpening>().Create(new {
                OpeningNumber = "whiter0se", Town = towns[0]
            });
            var sewer2 = GetEntityFactory<SewerOpening>().Create(new {
                Town = towns[1], OpeningNumber = "Tyr3ll"
            });

            var result = Repository.FindByPartialOpeningMatchByTown("100", towns[0].Id);

            Assert.AreEqual(0, result.Count());

            result = Repository.FindByPartialOpeningMatchByTown("425", towns[1].Id);

            Assert.AreEqual(0, result.Count());
        }

        #endregion

        #region GetAssetCoordinates

        [TestMethod]
        public void TestGetAssetCoordinatesCorrectlyFiltersOnOperatingCenter()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var coord = GetFactory<CoordinateFactory>().Create(new { Latitude = 50m, Longitude = 50m });
            var sewerOpening = GetFactory<SewerOpeningFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = coord
            });
            var sewerOpeningBad = GetFactory<HydrantFactory>().Create(new {
                Coordinate = coord
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] { opc.Id },
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetAssetCoordinates(search).Single();
            Assert.AreEqual(sewerOpening.Id, result.Id);
        }

        [TestMethod]
        public void TestGetAssetCoordinateDoesNotReturnHydrantsWithoutCoordinates()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var sewerOpening = GetFactory<SewerOpeningFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new { Latitude = 50m, Longitude = 50m })
            });
            var sewerOpeningBad = GetFactory<SewerOpeningFactory>().Create(new {
                OperatingCenter = opc,
            });
            sewerOpeningBad.Coordinate = null;
            Session.Save(sewerOpeningBad);
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] { opc.Id },
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetAssetCoordinates(search).Single();
            Assert.AreEqual(sewerOpening.Id, result.Id);
        }

        #endregion

        #region SearchForMap

        [TestMethod]
        public void TestSearchForMapReturnsTheCorrectResults()
        {
            var opcenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var npdesRegulatorType = GetFactory<NpdesRegulatorSewerOpeningTypeFactory>().Create();
            var regulators = GetEntityFactory<SewerOpening>().CreateList(5, new {
                SewerOpeningType = npdesRegulatorType,
                OperatingCenter = opcenter,
                Town = town
            });

            var results = Repository.SearchForMap(new TestSearchSewerOpeningForMap {
                OperatingCenter = opcenter.Id,
                Town = town.Id,
            }).ToList();

            Assert.AreEqual(5, results.Count);
            foreach (var result in results)
            {
                Assert.IsTrue(regulators.Any(x => x.Id == result.Id));
            }
        }

        #endregion

        #region NpdesRegulatorsDueInspection

        [TestMethod]
        public void TestGetNpdesRegulatorsDueInspectionReturnsTheCorrectResults()
        {
            var today = DateTime.Today; // We can do this since the actual date/time itself is irrelevant.

            var opcenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var npdesRegulatorType = GetFactory<NpdesRegulatorSewerOpeningTypeFactory>().Create();
            var regulators = GetEntityFactory<SewerOpening>().CreateList(5, new {
                SewerOpeningType = npdesRegulatorType,
                OperatingCenter = opcenter,
                Town = town
            });

            var start = today.Subtract(TimeSpan.FromDays(30));
            var end = today.AddDays(30);

            var results = Repository.GetNpdesRegulatorsDueInspection(new TestSearchNpdesRegulatorsDueInspection {
                OperatingCenter = opcenter.Id,
                Town = town.Id,
                DepartureDateTime = new RequiredDateRange { Start = start, End = end, Operator = RangeOperator.Between },
            }).ToList();

            Assert.AreEqual(5, results.Count);
            Assert.IsTrue(regulators.All(x => results.Contains(x)));
        }

        [TestMethod]
        public void TestSearchNpdesRegulatorsDueInspectionForMapReturnsTheSameResultsAsRegularDueInspectionSearch()
        {
            var today = DateTime.Today; // We can do this since the actual date/time itself is irrelevant.

            var opcenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var npdesRegulatorType = GetFactory<NpdesRegulatorSewerOpeningTypeFactory>().Create();
            var regulators = GetEntityFactory<SewerOpening>().CreateList(5, new {
                SewerOpeningType = npdesRegulatorType,
                OperatingCenter = opcenter,
                Town = town
            });

            var start = today.Subtract(TimeSpan.FromDays(30));
            var end = today.AddDays(30);

            var regularSearch = Repository.GetNpdesRegulatorsDueInspection(new TestSearchNpdesRegulatorsDueInspection {
                OperatingCenter = opcenter.Id,
                Town = town.Id,
                DepartureDateTime = new RequiredDateRange { Start = start, End = end, Operator = RangeOperator.Between },
            }).ToList();

            var mapSearch = Repository.SearchNpdesRegulatorsDueInspectionForMap(
                new TestSearchNpdesRegulatorsDueInspectionForMap {
                    OperatingCenter = opcenter.Id,
                    Town = town.Id,
                    DepartureDateTime = new RequiredDateRange
                        { Start = start, End = end, Operator = RangeOperator.Between },
                }).ToList();

            Assert.AreEqual(regularSearch.Count, mapSearch.Count);
            foreach (var result in mapSearch)
            {
                Assert.IsTrue(regularSearch.Any(x => x.Id == result.Id));
            }
        }

        #endregion

        #endregion

        #region Test Classes

        private class TestSearchSewerOpeningForMap : SearchSet<SewerOpeningAssetCoordinate>, ISearchSewerOpeningForMap
        {
            #region Properties

            public int? OperatingCenter { get; set; }

            public int? Town { get; set; }

            public int? WasteWaterSystem { get; set; }

            public int? SewerOpeningType { get; set; }

            public int? TownSection { get; set; }

            public string StreetNumber { get; set; }

            public int? Street { get; set; }

            [View("Cross Street")]
            public int? IntersectingStreet { get; set; }

            [View("SAP Equipment")]
            public int? SAPEquipmentId { get; set; }

            [Search(CanMap = false)]
            public bool? HasSAPErrorCode { get; set; }

            [View(SewerOpening.Display.CRITICAL)]
            public bool? Critical { get; set; }

            public string SAPErrorCode { get; set; }

            public int? Status { get; set; }

            [View(SewerOpening.Display.TASK_NUMBER)]
            public string TaskNumber { get; set; }

            public SearchString OpeningNumber { get; set; }

            public int? OpeningSuffix { get; set; }

            public string FunctionalLocationDescription { get; set; }

            public int? FunctionalLocation { get; set; }

            public bool? IsDoghouseOpening { get; set; }

            public DateRange DateInstalled { get; set; }

            [View("Date Added")]
            public DateRange CreatedAt { get; set; }

            public int? Route { get; set; }

            public SearchString GeoEFunctionalLocation { get; set; }

            public IntRange InspectionFrequency { get; set; }

            [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
            public int? InspectionFrequencyUnit { get; set; }

            [View("Legacy ID")]
            public virtual string OldNumber { get; set; }

            public DateRange DateRetired { get; set; }

            public int? BodyOfWater { get; set; }

            public SearchString OutfallNumber { get; set; }

            public SearchString LocationDescription { get; set; }

            #endregion

            #region Exposed Methods

            public override void ModifyValues(ISearchMapper mapper)
            {
                base.ModifyValues(mapper);

                if (!HasSAPErrorCode.HasValue)
                {
                    return;
                }

                mapper.MappedProperties["SAPErrorCode"].Value = HasSAPErrorCode.Value
                    ? SearchMapperSpecialValues.IsNotNull
                    : SearchMapperSpecialValues.IsNull;
            }

            #endregion
        }

        private class TestSearchNpdesRegulatorsDueInspection : SearchSet<SewerOpening>, ISearchNpdesRegulatorsDueInspectionItem
        {
            [Search(CanMap = false)]
            public int? SewerOpeningId { get; set; }

            [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }

            [EntityMap, EntityMustExist(typeof(Town))]
            [SearchAlias("Town", "town", "Id")]
            public int? Town { get; set; }

            [EntityMap, EntityMustExist(typeof(AssetStatus))]
            public int? Status { get; set; }

            [Search(CanMap = false)]
            public string NpdesPermitNumber { get; set; }

            [Search(CanMap = false)]
            public string SewerOpeningNumber { get; set; }

            [Search(CanMap = false)]
            public string BodyOfWater { get; set; }

            [Required]
            public RequiredDateRange DepartureDateTime { get; set; }
        }

        private class TestSearchNpdesRegulatorsDueInspectionForMap : SearchSet<SewerOpeningAssetCoordinate>, ISearchNpdesRegulatorsDueInspectionForMap
        {
            #region Consts

            public const int MAX_MAP_RESULT_COUNT = 10000;

            #endregion

            #region Properties

            /// <remarks>
            /// Returns false and is not settable, because map coordinates shouldn't be paged.
            /// </remarks>
            public override bool EnablePaging => false;

            #endregion

            [Search(CanMap = false)]
            public int? SewerOpeningId { get; set; }

            [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }

            [EntityMap, EntityMustExist(typeof(Town))]
            [SearchAlias("Town", "town", "Id")]
            public int? Town { get; set; }

            [EntityMap, EntityMustExist(typeof(AssetStatus))]
            public int? Status { get; set; }

            [Search(CanMap = false)]
            public string NpdesPermitNumber { get; set; }

            [Search(CanMap = false)]
            public string SewerOpeningNumber { get; set; }

            [Search(CanMap = false)]
            public string BodyOfWater { get; set; }

            [Required]
            public RequiredDateRange DepartureDateTime { get; set; }
        }

        private class TestAssetCoordinateSearch : SearchSet<AssetCoordinate>, IAssetCoordinateSearch
        {
            public int[] OperatingCenter { get; set; }
            public decimal? LatitudeMin { get; set; }
            public decimal? LatitudeMax { get; set; }
            public decimal? LongitudeMin { get; set; }
            public decimal? LongitudeMax { get; set; }
        }

        #endregion
    }
}
