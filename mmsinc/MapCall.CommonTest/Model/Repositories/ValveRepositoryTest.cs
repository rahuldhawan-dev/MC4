using System;
using System.Collections.Generic;
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
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ValveRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Valve, ValveRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesAssets;

        #endregion

        #region Fields

        private List<ValveZone> valveZones;
        private Mock<IDateTimeProvider> _dateProvider;

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
            i.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            i.For<IDateTimeProvider>().Use((_dateProvider = new Mock<IDateTimeProvider>()).Object);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            GetFactory<TownAbbreviationTypeFactory>().Create();
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
            valveZones = GetEntityFactory<ValveZone>().CreateList(7);
        }

        #endregion

        #region Tests

        #region Linq/Criteria

        [TestMethod]
        public void TestLinqDoesNotReturnValvesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });

            Session.Save(user);

            var val1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var val2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<ValveRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(val1));
            Assert.IsFalse(result.Contains(val2));
        }

        [TestMethod]
        public void TestLinqReturnsAllValvesIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var val1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var val2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<ValveRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(val1));
            Assert.IsTrue(result.Contains(val2));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnValvesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });

            Session.Save(user);

            var val1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var val2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<ValveRepository>();
            var model = new EmptySearchSet<Valve>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(val1));
            Assert.IsFalse(result.Contains(val2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllValvesIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var val1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var val2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<ValveRepository>();
            var model = new EmptySearchSet<Valve>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(val1));
            Assert.IsTrue(result.Contains(val2));
        }

        #endregion

        #region Find By

        [TestMethod]
        public void TestFindByStreetIdReturnsValvesWithMatchingStreets()
        {
            var street1 = GetFactory<StreetFactory>().Create();
            var street2 = GetFactory<StreetFactory>().Create();

            var valve1 = GetFactory<ValveFactory>().Create(new {Street = street1});
            var valve2 = GetFactory<ValveFactory>().Create(new {Street = street2});

            Assert.AreSame(valve1, Repository.FindByStreetId(street1.Id).Single());
            Assert.AreSame(valve2, Repository.FindByStreetId(street2.Id).Single());
        }

        [TestMethod]
        public void TestFindByTownIdReturnsValvesWithMatchingTowns()
        {
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();

            var valve1 = GetFactory<ValveFactory>().Create(new {Town = town1});
            var valve2 = GetFactory<ValveFactory>().Create(new {Town = town2});

            Assert.AreSame(valve1, Repository.FindByTownId(town1.Id).Single());
            Assert.AreSame(valve2, Repository.FindByTownId(town2.Id).Single());
        }

        [TestMethod]
        public void TestFindByTownIdOtherReturnsValvesWithMatchingTowns()
        {
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();

            var valve1 = GetFactory<ValveFactory>().Create(new {Town = town1});
            var valve2 = GetFactory<ValveFactory>().Create(new {Town = town2});

            Assert.AreEqual(valve1.ValveNumber, Repository.FindByTownIdOther(town1.Id).Single().Description);
            Assert.AreEqual(valve2.ValveNumber, Repository.FindByTownIdOther(town2.Id).Single().Description);
        }

        [TestMethod]
        public void TestFindByOperatingCenterAndValveNumberReturnsWhatItsNameImplies()
        {
            var expectedValNum = "1234";
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var valve1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opc, ValveNumber = expectedValNum});

            var result = Repository.FindByOperatingCenterAndValveNumber(opc, expectedValNum);
            Assert.AreSame(valve1, result.Single());
        }

        [TestMethod]
        public void TestRouteByTownId()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var valveValid1 = GetEntityFactory<Valve>().Create(new {Town = towns[0], Route = 1});
            var valveValid2 = GetEntityFactory<Valve>().Create(new {Town = towns[0], Route = 2});
            var valveInvalid1 = GetEntityFactory<Valve>().Create(new {Town = towns[0]});
            var valveInvalid2 = GetEntityFactory<Valve>().Create(new {Town = towns[1], Route = 3});

            var result = Repository.RouteByTownId(towns[0].Id);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(valveValid1.Route, result.First());
            Assert.AreEqual(valveValid2.Route, result.Last());
            Assert.AreNotEqual(valveInvalid2.Route, result.First());
        }

        [TestMethod]
        public void TestFindByTownIdAndOperatingCenterId()
        {
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();
            var oc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var oc2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var valve1 = GetFactory<ValveFactory>().Create(new {Town = town1, OperatingCenter = oc1});
            var valve2 = GetFactory<ValveFactory>().Create(new {Town = town2, OperatingCenter = oc2});

            Assert.AreSame(valve1, Repository.FindByTownIdAndOperatingCenterId(town1.Id, oc1.Id).Single());
            Assert.AreSame(valve2, Repository.FindByTownIdAndOperatingCenterId(town2.Id, oc2.Id).Single());
            Assert.IsNull(Repository.FindByTownIdAndOperatingCenterId(town1.Id, oc2.Id).SingleOrDefault());
        }

        [TestMethod]
        public void TestGetByOperatingCenter()
        {
            var operatingCenterA = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterC = GetFactory<UniqueOperatingCenterFactory>().Create();

            var valveAInOperatingCenterA = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenterA });
            var valveBInOperatingCenterA = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenterA });
            var valveCInOperatingCenterB = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenterB });
            var valveDInOperatingCenterC = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenterC });

            Session.Flush();

            var hydrantsInOperatingCenterA = Repository.GetValvesByOperatingCenter(operatingCenterA.Id).ToList();
            Assert.AreEqual(2, hydrantsInOperatingCenterA.Count);
            CollectionAssert.Contains(hydrantsInOperatingCenterA, valveAInOperatingCenterA);
            CollectionAssert.Contains(hydrantsInOperatingCenterA, valveBInOperatingCenterA);

            var hydrantsInOperatingCenterB = Repository.GetValvesByOperatingCenter(operatingCenterB.Id).ToList();
            Assert.AreEqual(1, hydrantsInOperatingCenterB.Count);
            CollectionAssert.Contains(hydrantsInOperatingCenterB, valveCInOperatingCenterB);

            var allValvesById = Repository.GetValvesByOperatingCenter(operatingCenterA.Id, operatingCenterB.Id, operatingCenterC.Id).ToList();
            Assert.AreEqual(4, allValvesById.Count);
            CollectionAssert.Contains(allValvesById, valveAInOperatingCenterA);
            CollectionAssert.Contains(allValvesById, valveBInOperatingCenterA);
            CollectionAssert.Contains(allValvesById, valveCInOperatingCenterB);
            CollectionAssert.Contains(allValvesById, valveDInOperatingCenterC);

            var allValvesByNone = Repository.GetValvesByOperatingCenter().ToList();
            Assert.AreEqual(4, allValvesByNone.Count);
            CollectionAssert.Contains(allValvesByNone, valveAInOperatingCenterA);
            CollectionAssert.Contains(allValvesByNone, valveBInOperatingCenterA);
            CollectionAssert.Contains(allValvesByNone, valveCInOperatingCenterB);
            CollectionAssert.Contains(allValvesByNone, valveDInOperatingCenterC);

            Assert.IsNull(Repository.GetValvesByOperatingCenter(108).SingleOrDefault());
        }

        #endregion

        #endregion

        #region GetValvesWithSapIssues

        [TestMethod]
        public void TestGetValvesWithSapIssuesReturnsHydrantsWithSapIssues()
        {
            var valveValid1 = GetFactory<ValveFactory>().Create(new {SAPErrorCode = "RETRY::Something went wrong"});
            var valveInvalid1 = GetFactory<ValveFactory>().Create(new {SAPErrorCode = ""});
            var valveInvalid2 = GetFactory<ValveFactory>().Create();

            var result = Repository.GetValvesWithSapRetryIssues();

            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #region Valve Numbers

        #region GetUnusedValveNumbers

        [TestMethod]
        public void TestGetUnusedValveNumbersReturnsEmptyIfThereAreNomatchingValvesInTheSystem()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCenters.Add(opc1);
            var result = Repository.GetUnusedValveNumbers(opc1, town, null);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestGetUnusedValveNumbersReturnsEmptyIfThereIsOnlyOneMatchingValveAndItsSuffixEqualsOne()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCenters.Add(opc1);
            var valve = GetEntityFactory<Valve>().Create(new {Town = town, ValveSuffix = 1});
            var result = Repository.GetUnusedValveNumbers(opc1, town, null);
            Assert.IsFalse(result.Any(), "1 would be both the min and max so there should not be any returned.");
        }

        [TestMethod]
        public void TestGetUnusedValveNumbersReturnsUnusedNumbersForMatchingTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var valve = GetFactory<ValveFactory>().Create(new {Town = town, ValveSuffix = 3, OperatingCenter = opc1});
            var result = Repository.GetUnusedValveNumbers(opc1, town, null).ToArray();
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(1), "1 is unused and should appear.");
            Assert.IsTrue(result.Contains(2), "2 is unused and should appear.");
        }

        [TestMethod]
        public void TestGetUnusedValveNumbersDoesNotCareAboutTownSectionAtAllWhenTownAbbreviationTypeIsTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {AbbreviationType = typeof(TownAbbreviationTypeFactory)});
            town.OperatingCenters.Add(opc1);
            var townSect = GetFactory<TownSectionFactory>().Create(new {Town = town});
            Assert.IsTrue(town.TownSections.Contains(townSect), "Sanity check");

            var valveInTown = GetFactory<ValveFactory>()
               .Create(new {Town = town, ValveSuffix = 3, OperatingCenter = opc1});
            var valveInTownSection = GetFactory<ValveFactory>().Create(new
                {Town = town, TownSection = townSect, ValveSuffix = 2, OperatingCenter = opc1});

            var result = Repository.GetUnusedValveNumbers(opc1, town, townSect).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(1));

            // Results should be identical regardless of whether townSection was supplied or not.
            result = Repository.GetUnusedValveNumbers(opc1, town, null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(1));
        }

        [TestMethod]
        public void
            TestGetUnusedValveNumbersReturnsUnusedNumbersForMatchingTownAndIncludesNumbersThatAreUsedForTownSectionInSameTownWhenAbbreviationTypeForTownIsTownSectionButTheTownSectionIsNotSelected()
        {
            /* 
             * I hate reading long test names
             * 
             * This is testing that if a town uses the Town Section for creating valve numbers, but the number being used 
             * is not requiring a town section, then any number that matches the town section should be ignored.
             */

            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            var townSect = GetFactory<TownSectionFactory>().Create(new {Town = town});
            Assert.IsTrue(town.TownSections.Contains(townSect), "Sanity check");

            var valveInTown = GetFactory<ValveFactory>().Create(new {Town = town, ValveSuffix = 3});
            var valveInTownSection = GetFactory<ValveFactory>()
               .Create(new {Town = town, TownSection = townSect, ValveSuffix = 2});

            var result = Repository.GetUnusedValveNumbers(valveInTown.OperatingCenter, town, null)
                                   .ToArray(); // Do not include town section!

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result.Contains(2));
        }

        [TestMethod]
        public void
            TestGetUnusedValveNumberReturnsUnusedNumbersForTOWNSECTIONIfTownSectionIsSuppliedAndTownAbbreviationTypeIsTownSection()
        {
            /* 
            * This is testing that if a town uses the Town Section for creating valve numbers, and a town section is included,
            * then it should ignore the town's numbers.
            */

            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            var townSect = GetFactory<TownSectionFactory>().Create(new {Town = town});
            Assert.IsTrue(town.TownSections.Contains(townSect), "Sanity check");

            var valveInTown = GetFactory<ValveFactory>().Create(new {Town = town, ValveSuffix = 3});
            var valveInTownSection = GetFactory<ValveFactory>()
               .Create(new {Town = town, TownSection = townSect, ValveSuffix = 2});

            var result = Repository.GetUnusedValveNumbers(valveInTown.OperatingCenter, town, townSect).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(1));
        }

        #endregion

        #region Generating Valve Numbers

        #region Getting the correct abbreviation for the valve prefix

        [TestMethod]
        public void TestAbbreviationIsTownAbbreviationIfTownAbbreviationTypeIsTown()
        {
            // This needs to exist.
            GetFactory<FireDistrictAbbreviationTypeFactory>().Create();

            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(TownAbbreviationTypeFactory)
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});

            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town,
                Abbreviation = "SS"
            });

            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, null);
            Assert.AreEqual("VQQ-1", result.FormattedNumber);

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, townSection);
            Assert.AreEqual("VQQ-1", result.FormattedNumber, "Town abbreviation must be used in this instance.");
        }

        /// <summary>
        ///  This scenario si for when the town section valves has surpassed the town valves, the next test is the opposite
        /// </summary>
        [TestMethod]
        public void
            TestAbbreviationIsTownSectionAbbreviationIfTownAbbreviationTypeIsTownSectionAndTownSectionIsNotNullAndTownSectionAbbreviationIsNotNullOrEmptyOrWhiteSpace()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town,
                Abbreviation = "SS"
            });
            var valve1 = GetEntityFactory<Valve>().Create(new
                {OperatingCenter = opc1, Town = town, ValveSuffix = 1, ValveNumber = "VQQ-1"});
            var valve2 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = opc1, Town = town, TownSection = townSection, ValveSuffix = 5, ValveNumber = "VSS-5"
            });

            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VSS-6", result.FormattedNumber,
                "Town section abbreviation must be used in this instance.");

            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, null);
            Assert.AreEqual("VQQ-2", result.FormattedNumber,
                "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");

            townSection.Abbreviation = null;
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-2", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is null and the abbreviation type is town section.");

            townSection.Abbreviation = string.Empty;
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-2", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is string.Empty and the abbreviation type is town section.");

            townSection.Abbreviation = "   ";
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-2", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is whitespace and the abbreviation type is town section.");
        }

        /// <summary>
        ///  This scenario si for when the town valves has surpassed the town section valves, the previous test is the opposite
        /// </summary>
        [TestMethod]
        public void
            TestAbbreviationIsTownSectionAbbreviationIfTownAbbreviationTypeIsTownSectionAndTownSectionIsNotNullAndTownSectionAbbreviationIsNotNullOrEmptyOrWhiteSpaceReversed()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town,
                Abbreviation = "SS"
            });
            var valve1 = GetEntityFactory<Valve>().Create(new
                {OperatingCenter = opc1, Town = town, ValveSuffix = 5, ValveNumber = "VQQ-5"});
            var valve2 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = opc1, Town = town, TownSection = townSection, ValveSuffix = 1, ValveNumber = "VSS-1"
            });

            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VSS-2", result.FormattedNumber,
                "Town section abbreviation must be used in this instance.");

            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, null);
            Assert.AreEqual("VQQ-6", result.FormattedNumber,
                "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");

            townSection.Abbreviation = null;
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-6", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is null and the abbreviation type is town section.");

            townSection.Abbreviation = string.Empty;
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-6", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is string.Empty and the abbreviation type is town section.");

            townSection.Abbreviation = "   ";
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-6", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is whitespace and the abbreviation type is town section.");
        }

        /// <summary>
        ///  This scenario si for when the town section valves has surpassed the town valves, the next test is the opposite
        /// </summary>
        [TestMethod]
        public void
            TestAbbreviationIsFireDistrictActsLikeTownSectionAbbreviationIfTownAbbreviationTypeIsTownSectionAndTownSectionIsNotNullAndTownSectionAbbreviationIsNotNullOrEmptyOrWhiteSpace()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(FireDistrictAbbreviationTypeFactory)
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town,
                Abbreviation = "SS"
            });
            var valve1 = GetEntityFactory<Valve>().Create(new
                {OperatingCenter = opc1, Town = town, ValveSuffix = 1, ValveNumber = "VQQ-1"});
            var valve2 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = opc1, Town = town, TownSection = townSection, ValveSuffix = 5, ValveNumber = "VSS-5"
            });

            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VSS-6", result.FormattedNumber,
                "Town section abbreviation must be used in this instance.");

            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, null);
            Assert.AreEqual("VQQ-2", result.FormattedNumber,
                "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");

            townSection.Abbreviation = null;
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-2", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is null and the abbreviation type is town section.");

            townSection.Abbreviation = string.Empty;
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-2", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is string.Empty and the abbreviation type is town section.");

            townSection.Abbreviation = "   ";
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-2", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is whitespace and the abbreviation type is town section.");
        }

        /// <summary>
        ///  This scenario si for when the town valves has surpassed the town section valves, the previous test is the opposite
        /// </summary>
        [TestMethod]
        public void
            TestAbbreviationIsFireDistrictActsLikeTownSectionAbbreviationIfTownAbbreviationTypeIsTownSectionAndTownSectionIsNotNullAndTownSectionAbbreviationIsNotNullOrEmptyOrWhiteSpaceReversed()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(FireDistrictAbbreviationTypeFactory)
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town,
                Abbreviation = "SS"
            });
            var valve1 = GetEntityFactory<Valve>().Create(new
                {OperatingCenter = opc1, Town = town, ValveSuffix = 5, ValveNumber = "VQQ-5"});
            var valve2 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = opc1, Town = town, TownSection = townSection, ValveSuffix = 1, ValveNumber = "VSS-1"
            });

            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VSS-2", result.FormattedNumber,
                "Town section abbreviation must be used in this instance.");

            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, null);
            Assert.AreEqual("VQQ-6", result.FormattedNumber,
                "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");

            townSection.Abbreviation = null;
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-6", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is null and the abbreviation type is town section.");

            townSection.Abbreviation = string.Empty;
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-6", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is string.Empty and the abbreviation type is town section.");

            townSection.Abbreviation = "   ";
            result = Repository.GenerateNextValveNumber(rawRepo, valve1.OperatingCenter, town, townSection);
            Assert.AreEqual("VQQ-6", result.FormattedNumber,
                "Town abbreviation must be used when the town section's abbreviation is whitespace and the abbreviation type is town section.");
        }

        #endregion

        #region Getting the correct suffix

        [TestMethod]
        public void TestGenerateNextValveNumber_TownNumberingSystemReturns_1_IfThereAreNoValvesForTown()
        {
            // This needs to exist.
            GetFactory<FireDistrictAbbreviationTypeFactory>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {AbbreviationType = typeof(TownAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, null);
            Assert.AreEqual(1, result.Suffix);
        }

        [TestMethod]
        public void TestGenerateNextValveNumber_TownNumberingSystemReturns_MAX_IfThereIsAValveForThatTown()
        {
            // This needs to exist.
            GetFactory<FireDistrictAbbreviationTypeFactory>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {AbbreviationType = typeof(TownAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            GetFactory<ValveFactory>().Create(new {Town = town, ValveSuffix = 12, OperatingCenter = opc1});
            GetFactory<ValveFactory>().Create(new {Town = town, ValveSuffix = 41, OperatingCenter = opc1});

            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, null);
            Assert.AreEqual(42, result.Suffix);
        }

        [TestMethod]
        public void
            TestGenerateNextValveNumber_TownSectionNumberingSystemReturns_1_IfThereAreNoValvesThatMatchBothTownAndTownSection()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town});
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, townSection);
            Assert.AreEqual(1, result.Suffix,
                "1 should be returned when there are no valves that match both the town and town section.");

            var valveSameTownNoTownSection = GetFactory<ValveFactory>().Create(new {Town = town});

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, townSection);
            Assert.AreEqual(1, result.Suffix,
                "1 should be returned if there is a valve for the same town but different town section.");
        }

        [TestMethod]
        public void
            TestGenerateNextValveNumber_TownSectionNumberingSystemReturns_MAX_IfThereIsAMatchingValveForTownANDTownSection()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town, Abbreviation = "AB"});
            var valveWithTownAndTownSection = GetFactory<ValveFactory>().Create(new
                {Town = town, TownSection = townSection, ValveSuffix = 13, OperatingCenter = opc1});
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, townSection);
            Assert.AreEqual(14, result.Suffix);

            var valveWithTownButNoTownSection =
                GetFactory<ValveFactory>().Create(new {Town = town, ValveSuffix = 28});

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, townSection);
            Assert.AreEqual(14, result.Suffix, "This should be 14. TownSection must be taken into account.");
        }

        [TestMethod]
        public void
            TestGenerateNextValveNumber_TownSectionNumberingSystemReturns_1_IfTownSectionIsNullAndThereAreNoValvesThatMatchTheTownAndNullTownSection()
        {
            // This is testing for when a town has the TownSection abbreviation type but no town section was selected 
            // for a valve. This is allowed. See Neptune(town, null town section) vs Ocean Grove(town is Neptune, OG is town section).
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town, Abbreviation = "AB"});

            // This should not be found in the query.
            var valveWithTownAndTownSection =
                GetFactory<ValveFactory>().Create(new {Town = town, TownSection = townSection});
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, null);
            Assert.AreEqual(1, result.Suffix);
        }

        [TestMethod]
        public void
            TestGenerateNextValveNumber_TownSectionNumberingSystemReturns_MAX_IfTownSectionIsNullAndThereAreValvesThatMatchTheTownAndNullTownSection()
        {
            // This is testing for when a town has the TownSection abbreviation type but no town section was selected 
            // for a valve. This is allowed. See Neptune(town, null town section) vs Ocean Grove(town is Neptune, OG is town section).
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town, Abbreviation = "AB"});
            var valveWithTownOnly = GetFactory<ValveFactory>()
               .Create(new {Town = town, ValveSuffix = 13, OperatingCenter = opc1});
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, null);
            Assert.AreEqual(14, result.Suffix);

            var valveWithTownSection =
                GetFactory<ValveFactory>().Create(new
                    {Town = town, TownSection = townSection, ValveSuffix = 19, OperatingCenter = opc1});

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, null);

            Assert.AreEqual(14, result.Suffix, "The valve with a town section should not be factored into this.");
        }

        [TestMethod]
        public void TestTheNextSuffixDoesNotFailWhenTownSectionAbbreviationTypeAndTownValveIsAttempted()
        {
            // town with abbrevation type of town section
            // a town valve gets the correct next town number
            // a town section valve with no abbreviation for the town section gets the next town number
            // a town section valve with an abbrevation gets the town section for the next town section number
            // this was a real world example that was failing. 
            // This needs to exist.
            GetFactory<FireDistrictAbbreviationTypeFactory>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var townAbbrevationType = GetFactory<TownAbbreviationTypeFactory>().Create();
            var townSectionAbbreviationType = GetFactory<TownSectionAbbreviationTypeFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {AbbreviationType = townSectionAbbreviationType});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = opc1, Abbreviation = "TR"});
            Session.Save(town.OperatingCentersTowns.First());
            var townSection = GetFactory<TownSectionFactory>()
               .Create(new {Town = town, Abbreviation = "DT2", Name = "Ortley Beach"});
            var townSection2 = GetFactory<TownSectionFactory>().Create(new {Town = town, Name = "Pelican Island"});
            townSection2.Abbreviation = null;
            GetFactory<ValveFactory>().Create(new
                {Town = town, ValveSuffix = 2853, ValveNumber = "VTR-2853", OperatingCenter = opc1});
            GetFactory<ValveFactory>().Create(new
                {Town = town, ValveSuffix = 2858, ValveNumber = "VTR-2858", OperatingCenter = opc1});
            GetFactory<ValveFactory>().Create(new {
                Town = town, TownSection = townSection, ValveSuffix = 2854, ValveNumber = "VDT2-2854",
                OperatingCenter = opc1
            });
            GetFactory<ValveFactory>().Create(new {
                Town = town, TownSection = townSection, ValveSuffix = 2855, ValveNumber = "VDT2-2855",
                OperatingCenter = opc1
            });
            GetFactory<ValveFactory>().Create(new {
                Town = town, TownSection = townSection, ValveSuffix = 2856, ValveNumber = "VDT2-2856",
                OperatingCenter = opc1
            });
            GetFactory<ValveFactory>().Create(new {
                Town = town, TownSection = townSection, ValveSuffix = 2857, ValveNumber = "VDT2-2857",
                OperatingCenter = opc1
            });
            Session.Flush();
            Session.Clear();
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, null);
            Assert.AreEqual(2859, result.Suffix);

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, townSection);
            Assert.AreEqual(2858, result.Suffix);

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, town, townSection2);
            Assert.AreEqual(2859, result.Suffix);
        }

        [TestMethod]
        public void TestSharedTownGetsCorrectSuffixForCorrectOperatingCenter()
        {
            // This needs to exist.
            GetFactory<FireDistrictAbbreviationTypeFactory>().Create();
            var ew1 = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterName = "Netherwood", OperatingCenterCode = "EW1"});
            var ew4 = GetEntityFactory<OperatingCenter>().Create(new
                {OperatingCenterName = "Edison Water Company", OperatingCenterCode = "EW4"});

            var town = GetEntityFactory<Town>().Create(new {ShortName = "Edison Twp", FullName = "TOWNSHIP OF EDISON"});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = ew1, Abbreviation = "EDI"});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = town, OperatingCenter = ew4, Abbreviation = "EDS"});
            Session.Save(town);
            town = Session.Load<Town>(town.Id);

            var valve1 = GetFactory<ValveFactory>().Create(new {Town = town, ValveSuffix = 645, OperatingCenter = ew1});
            var valve2 = GetFactory<ValveFactory>()
               .Create(new {Town = town, ValveSuffix = 3003, OperatingCenter = ew4});
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            Assert.AreEqual(2, town.OperatingCenters.Count);
            Assert.AreEqual(646, Repository.GenerateNextValveNumber(rawRepo, ew1, town, null).Suffix);
            Assert.AreEqual("VEDI", Repository.GenerateNextValveNumber(rawRepo, ew1, town, null).Prefix);
            Assert.AreEqual(3004, Repository.GenerateNextValveNumber(rawRepo, ew4, town, null).Suffix);
            Assert.AreEqual("VEDS", Repository.GenerateNextValveNumber(rawRepo, ew4, town, null).Prefix);
        }

        #endregion

        #endregion

        #region Bug #2722

        [TestMethod]
        public void TestNeptuneReturnsTheNextValveNumberProperly()
        {
            // This needs to exist.
            GetFactory<FireDistrictAbbreviationTypeFactory>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var abbreviationType = GetFactory<TownSectionAbbreviationTypeFactory>().Create();
            var neptune = GetFactory<TownFactory>().Create(new
                {ShortName = "Neptune", FullName = "Neptune Twp", AbbreviationType = abbreviationType});
            neptune.OperatingCentersTowns.Add(new OperatingCenterTown
                {Town = neptune, OperatingCenter = opc1, Abbreviation = "NT"});
            Session.Save(neptune.OperatingCentersTowns.First());
            var sharkRiverIsland = GetFactory<TownSectionFactory>()
               .Create(new {Town = neptune, Abbreviation = "", Name = "Shark River Island"});
            var oceanGrove = GetFactory<TownSectionFactory>()
               .Create(new {Town = neptune, Abbreviation = "OG", Name = "Ocean Grove"});
            var sharkRiverHills = GetFactory<TownSectionFactory>()
               .Create(new {Town = neptune, Name = "Shark River Hills"});
            sharkRiverHills.Abbreviation = null;
            Session.Save(sharkRiverHills);
            Session.Flush();
            Session.Clear();

            sharkRiverHills = Session.Load<TownSection>(sharkRiverHills.Id);
            Assert.IsNull(sharkRiverHills.Abbreviation);

            var neptuneCity = GetFactory<TownSectionFactory>()
               .Create(new {Town = neptune, Abbreviation = "NC", Name = "Neptune City"});

            var neptuneValve1 = GetFactory<ValveFactory>().Create(new
                {Town = neptune, ValveNumber = "VNT-1", ValveSuffix = 1, OperatingCenter = opc1});
            var neptuneValve2 = GetFactory<ValveFactory>().Create(new
                {Town = neptune, ValveNumber = "VNT-2", ValveSuffix = 2, OperatingCenter = opc1});
            var neptuneValveSRI = GetFactory<ValveFactory>().Create(new {
                Town = neptune, TownSection = sharkRiverIsland, ValveNumber = "VNT-3", ValveSuffix = 3,
                OperatingCenter = opc1
            });
            var neptuneValvesOG1 = GetFactory<ValveFactory>().Create(new {
                Town = neptune, TownSection = oceanGrove, ValveNumber = "VOG-1", ValveSuffix = 1, OperatingCenter = opc1
            });
            var neptuneValvesOG2 = GetFactory<ValveFactory>().Create(new {
                Town = neptune, TownSection = oceanGrove, ValveNumber = "VOG-2", ValveSuffix = 2, OperatingCenter = opc1
            });
            var neptuneValvesSRH = GetFactory<ValveFactory>().Create(new {
                Town = neptune, TownSection = sharkRiverHills, ValveNumber = "VNT-4", ValveSuffix = 4,
                OperatingCenter = opc1
            });
            var neptuneValvesNC = GetFactory<ValveFactory>().Create(new {
                Town = neptune, TownSection = neptuneCity, ValveNumber = "VNC-1", ValveSuffix = 1,
                OperatingCenter = opc1
            });
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();

            neptune = Session.Load<Town>(neptune.Id);

            var result = Repository.GenerateNextValveNumber(rawRepo, opc1, neptune, null);
            Assert.AreEqual(5, result.Suffix);

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, neptune, sharkRiverIsland);
            Assert.AreEqual(5, result.Suffix);

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, neptune, oceanGrove);
            Assert.AreEqual(3, result.Suffix);

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, neptune, sharkRiverHills);
            Assert.AreEqual(5, result.Suffix);

            result = Repository.GenerateNextValveNumber(rawRepo, opc1, neptune, neptuneCity);
            Assert.AreEqual(2, result.Suffix);
        }

        #endregion

        #endregion

        #region Reports

        [TestMethod]
        public void TestGetValveBPUCountsReturnsCorrectAssetCountsByGroup()
        {
            var factory = GetEntityFactory<Valve>();
            var billing1 = GetFactory<PublicValveBillingFactory>().Create();
            var billing2 = GetFactory<MunicipalValveBillingFactory>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var smallSize = GetEntityFactory<ValveSize>().Create(new {Size = 2.0m});
            var largeSize = GetEntityFactory<ValveSize>().Create(new {Size = 12.0m});
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var val1 = factory.Create(new
                {ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1});
            var val2 = factory.Create(new
                {ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1});
            var val3 = factory.Create(new
                {ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1});
            var valdoesntmatter = factory.Create(new {
                ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1,
                BPUKPI = true
            });
            var args = new TestValveBPUReport();

            var result = Repository.GetValveBPUCounts(args).ToList();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[0].SizeRange);

            //by operating center
            args.OperatingCenter = opc2.Id;

            result = Repository.GetValveBPUCounts(args).ToList();

            Assert.AreEqual(0, result.Count);

            factory.Create(new
                {ValveBilling = billing2, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1});
            factory.Create(new
                {ValveBilling = billing2, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1});

            // multiple billing/sort
            args.OperatingCenter = null;
            result = Repository.GetValveBPUCounts(args).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result[0].Total);
            Assert.AreEqual(billing2.Description, result[0].ValveBilling);
            Assert.AreEqual(3, result[1].Total);
            Assert.AreEqual(billing1.Description, result[1].ValveBilling);

            // addt sizes
            factory.Create(new
                {ValveBilling = billing2, ValveSize = largeSize, Status = activeStatus, OperatingCenter = opc1});

            result = Repository.GetValveBPUCounts(args).ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0].Total);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[0].SizeRange);
            Assert.AreEqual(1, result[1].Total);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[1].SizeRange);
            Assert.AreEqual(3, result[2].Total);
        }

        private class TestValveBPUReport : SearchSet<ValveBPUReportItem>
        {
            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }

            public int? Town { get; set; }
        }

        #region Valve Routes

        [TestMethod]
        public void TestGetValveRoutes()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var valvesRoute1a = GetEntityFactory<Valve>().CreateList(3,
                new {Route = 1, OperatingCenter = operatingCenter, Town = town, Status = activeStatus});
            var valvesRoute1r = GetEntityFactory<Valve>().CreateList(2,
                new {Route = 1, OperatingCenter = operatingCenter, Town = town, Status = retiredStatus});
            var valvesRoute2a = GetEntityFactory<Valve>().CreateList(5,
                new {Route = 2, OperatingCenter = operatingCenter, Town = town, Status = activeStatus});
            var valvesRoute2r = GetEntityFactory<Valve>().CreateList(4,
                new {Route = 2, OperatingCenter = operatingCenter, Town = town, Status = retiredStatus});

            var args = new TestValveRouteReport();

            var results = Repository.GetRoutes(args).ToList();

            Assert.AreEqual(4, results.Count);

            Assert.AreEqual(1, results[0].Route);
            Assert.AreEqual(town, results[0].Town);
            Assert.AreEqual(operatingCenter, results[0].OperatingCenter);
            Assert.AreEqual(3, results[0].Total);
            Assert.AreEqual(activeStatus, results[0].ValveStatus);

            Assert.AreEqual(1, results[1].Route);
            Assert.AreEqual(town, results[1].Town);
            Assert.AreEqual(operatingCenter, results[1].OperatingCenter);
            Assert.AreEqual(2, results[1].Total);
            Assert.AreEqual(retiredStatus, results[1].ValveStatus);

            Assert.AreEqual(2, results[2].Route);
            Assert.AreEqual(town, results[2].Town);
            Assert.AreEqual(operatingCenter, results[2].OperatingCenter);
            Assert.AreEqual(5, results[2].Total);
            Assert.AreEqual(activeStatus, results[0].ValveStatus);

            Assert.AreEqual(2, results[3].Route);
            Assert.AreEqual(town, results[3].Town);
            Assert.AreEqual(operatingCenter, results[3].OperatingCenter);
            Assert.AreEqual(4, results[3].Total);
            Assert.AreEqual(retiredStatus, results[3].ValveStatus);
        }

        private class TestValveRouteReport : SearchSet<ValveRouteReportItem>
        {
            public int? OperatingCenter { get; set; }
            public int? Town { get; set; }
            public int? ValveStatus { get; set; }
        }

        #endregion

        #region AgedPendingAssets

        [TestMethod]
        public void TestAgedPendingAssetsReturnsDataCorrectlyForTheHorde()
        {
            var now = DateTime.Now;
            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pending = GetFactory<PendingAssetStatusFactory>().Create();
            Assert.AreEqual(AssetStatus.Indices.PENDING, pending.Id);
            var active = GetFactory<ActiveAssetStatusFactory>().Create();
            var args = new TestAgedAssetSearch {OperatingCenter = opc1.Id};

            #region Good

            var val1a = GetEntityFactory<Valve>()
               .CreateList(4, new {OperatingCenter = opc1, Status = pending, CreatedAt = now});
            var val1b = GetEntityFactory<Valve>().CreateList(4,
                new {OperatingCenter = opc1, Status = pending, CreatedAt = now.AddDays(-1)});
            var val1c = GetEntityFactory<Valve>().CreateList(4,
                new {OperatingCenter = opc1, Status = pending, CreatedAt = now.AddDays(-90)});

            var val91 = GetEntityFactory<Valve>().CreateList(2,
                new {OperatingCenter = opc1, Status = pending, CreatedAt = now.AddDays(-91)});
            var val180 = GetEntityFactory<Valve>().CreateList(2,
                new {OperatingCenter = opc1, Status = pending, CreatedAt = now.AddDays(-180)});

            var val181 = GetEntityFactory<Valve>().CreateList(5,
                new {OperatingCenter = opc1, Status = pending, CreatedAt = now.AddDays(-181)});
            var val360 = GetEntityFactory<Valve>().CreateList(3,
                new {OperatingCenter = opc1, Status = pending, CreatedAt = now.AddDays(-360)});

            var val361 = GetEntityFactory<Valve>().CreateList(3,
                new {OperatingCenter = opc1, Status = pending, CreatedAt = now.AddDays(-361)});

            #endregion

            #region Bad

            var valBadStatus = GetEntityFactory<Valve>().Create(new {OperatingCenter = opc1, Status = active});
            var valBadOpCntr = GetEntityFactory<Valve>().Create(new {OperatingCenter = opc2, Status = pending});

            #endregion

            var result = Repository.GetAgedPendingAssets(args).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(12, result[0].ZeroToNinety);
            Assert.AreEqual(4, result[0].NinetyOneToOneEighty);
            Assert.AreEqual(8, result[0].OneEightyToThreeSixty);
            Assert.AreEqual(3, result[0].ThreeSixtyPlus);
            Assert.AreEqual(27, result[0].Total);
            Assert.AreEqual("Valve", result[0].AssetType);
        }

        private class TestAgedAssetSearch : SearchSet<AgedPendingAssetReportItem>
        {
            public int? OperatingCenter { get; set; }
        }

        #endregion

        #endregion

        #region GetValveAssetCoordinates

        [TestMethod]
        public void TestGetValveAssetCoordinatesOnlyReturnsValvesThatAreNotBlowOffs()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var coord = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m});
            var valve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = coord,
            });
            var valveBad = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = coord
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetValveAssetCoordinates(search).Single();
            Assert.AreEqual(valve.Id, result.Id);
        }

        [TestMethod]
        public void TestGetValveAssetCoordinatesCorrectlyFiltersOnOperatingCenter()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var coord = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m});
            var valve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = coord
            });
            var valveBad = GetFactory<ValveFactory>().Create(new {
                Coordinate = coord
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetValveAssetCoordinates(search).Single();
            Assert.AreEqual(valve.Id, result.Id);
        }

        [TestMethod]
        public void TestGetValveAssetCoordinateFiltersByExtent()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var valve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m})
            });
            var valveBad = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = -1m, Longitude = 50m}),
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetValveAssetCoordinates(search).Single();
            Assert.AreEqual(valve.Id, result.Id);
        }

        [TestMethod]
        public void TestGetValveAssetCoordinateDoesNotReturnValvesWithoutCoordinates()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var valve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m})
            });
            var valveBad = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
            });
            valveBad.Coordinate = null;
            Session.Save(valveBad);
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetValveAssetCoordinates(search).Single();
            Assert.AreEqual(valve.Id, result.Id);
        }

        [TestMethod]
        public void TestGetValveAssetCoordinateHandlesActiveStatusCase()
        {
            var allStatuses = GetFactory<AssetStatusFactory>().CreateAll();
            var activeStatuses = allStatuses.Where(x => AssetStatus.ACTIVE_STATUSES.Contains(x.Id)).ToList();
            var inactiveStatuses = allStatuses.Except(activeStatuses).ToList();

            var valve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {valve.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            foreach (var status in activeStatuses)
            {
                valve.Status = status;
                Session.Save(valve);
                Session.Flush();
                var result = Repository.GetValveAssetCoordinates(search).Single();
                Assert.IsTrue(result.IsActive, $"Valve should be active with status '{status.Description}'.");
            }

            foreach (var status in inactiveStatuses)
            {
                valve.Status = status;
                Session.Save(valve);
                Session.Flush();
                var result = Repository.GetValveAssetCoordinates(search).Single();
                Assert.IsFalse(result.IsActive, $"Valve should not be active with status '{status.Description}'.");
            }
        }

        [TestMethod]
        public void TestGetValveAssetCoordinateHandlesPublicBillingCase()
        {
            var valve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
                ValveBilling = GetFactory<PublicValveBillingFactory>().Create()
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {valve.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetValveAssetCoordinates(search).Single();
            Assert.IsTrue(result.IsPublic);

            valve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
                ValveBilling = GetFactory<MunicipalValveBillingFactory>().Create()
            });
            search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {valve.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            result = Repository.GetValveAssetCoordinates(search).Single();
            Assert.IsFalse(result.IsPublic);
        }

        [TestMethod]
        public void TestGetValveAssetCoordinateWorksCorrectlyWhenNormalPositionIsNotSet()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var valveWithNormPos = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
                NormalPosition = GetFactory<ValveNormalPositionFactory>().Create()
            });
            var valveWithoutNormPos = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
            });
            Assert.IsNull(valveWithoutNormPos.NormalPosition, "Sanity");

            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetValveAssetCoordinates(search).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(valveWithNormPos.NormalPosition.Description, result[0].NormalPosition.Description);
            Assert.IsNull(result[1].NormalPosition);
            Assert.IsNull(result[1].InNormalPosition, "This should have mapped to null rather than true/false.");
        }

        #endregion

        #region GetBlowOffAssetCoordinates

        [TestMethod]
        public void TestGetBlowOffAssetCoordinatesOnlyReturnsBlowOffValves()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var coord = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m});
            var valve = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = coord,
            });
            Session.Clear();
            var valveBad = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = coord
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetBlowOffAssetCoordinates(search).Single();
            Assert.AreEqual(valve.Id, result.Id);
        }

        [TestMethod]
        public void TestGetBlowOffAssetCoordinatesCorrectlyFiltersOnOperatingCenter()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var coord = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m});
            var valve = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = coord,
            });
            Session.Clear();
            var valveBad = GetFactory<BlowOffValveFactory>().Create(new {
                Coordinate = coord
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetBlowOffAssetCoordinates(search).Single();
            Assert.AreEqual(valve.Id, result.Id);
        }

        [TestMethod]
        public void TestGetBlowOffAssetCoordinateFiltersByExtent()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var valve = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m})
            });
            var valveBad = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = -1m, Longitude = 50m}),
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetBlowOffAssetCoordinates(search).Single();
            Assert.AreEqual(valve.Id, result.Id);
        }

        [TestMethod]
        public void TestGetBlowOffAssetCoordinateDoesNotReturnValvesWithoutCoordinates()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var valve = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m})
            });
            var valveBad = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = opc,
            });
            valveBad.Coordinate = null;
            Session.Save(valveBad);
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetBlowOffAssetCoordinates(search).Single();
            Assert.AreEqual(valve.Id, result.Id);
        }

        [TestMethod]
        public void TestGetBlowOffAssetCoordinateHandlesActiveStatusCase()
        {
            var allStatuses = GetFactory<AssetStatusFactory>().CreateAll();
            var activeStatuses = allStatuses.Where(x => AssetStatus.ACTIVE_STATUSES.Contains(x.Id)).ToList();
            var inactiveStatuses = allStatuses.Except(activeStatuses).ToList();

            var valve = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {valve.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            foreach (var status in activeStatuses)
            {
                valve.Status = status;
                Session.Save(valve);
                Session.Flush();
                var result = Repository.GetBlowOffAssetCoordinates(search).Single();
                Assert.IsTrue(result.IsActive, $"BlowOff should be active with status '{status.Description}'.");
            }

            foreach (var status in inactiveStatuses)
            {
                valve.Status = status;
                Session.Save(valve);
                Session.Flush();
                var result = Repository.GetBlowOffAssetCoordinates(search).Single();
                Assert.IsFalse(result.IsActive, $"BlowOff should not be active with status '{status.Description}'.");
            }
        }

        [TestMethod]
        public void TestGetBlowOffAssetCoordinateHandlesPublicBillingCase()
        {
            var valve = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
                ValveBilling = GetFactory<PublicValveBillingFactory>().Create()
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {valve.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetBlowOffAssetCoordinates(search).Single();
            Assert.IsTrue(result.IsPublic);

            valve = GetFactory<BlowOffValveFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
                ValveBilling = GetFactory<MunicipalValveBillingFactory>().Create()
            });
            search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {valve.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            result = Repository.GetBlowOffAssetCoordinates(search).Single();
            Assert.IsFalse(result.IsPublic);
        }

        #endregion

        #region Valves Due Inspection Town Counts

        [TestMethod]
        public void TestGetValvesDueInspectionReturnsTownCounts()
        {
            var valveZones = GetEntityFactory<ValveZone>().CreateList(7);
            var valveControls = GetFactory<MainValveControlFactory>().Create();
            var valveBilling = GetFactory<PublicValveBillingFactory>().Create();
            var activeValveStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredValveStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();

            var val1 =
                GetFactory<ValveFactory>()
                   .Create(
                        new {
                            OperatingCenter = opc1,
                            Town = town1,
                            Status = activeValveStatus,
                            ValveZone = valveZones[6],
                            ValveControls = valveControls,
                            ValveBilling = valveBilling
                        });
            var val2 =
                GetFactory<ValveFactory>()
                   .Create(
                        new {
                            OperatingCenter = opc2,
                            Town = town2,
                            Status = activeValveStatus,
                            ValveZone = valveZones[6],
                            ValveControls = valveControls,
                            ValveBilling = valveBilling
                        });
            var val3 =
                GetFactory<ValveFactory>()
                   .Create(new {OperatingCenter = opc2, Town = town2, Status = retiredValveStatus});

            var result = Repository.GetValvesDueInspection(new TestSearchValvesDueInspectionReport());

            Assert.AreEqual(1, result.Where(x => x.Town == town1.ShortName).Single().Count);
            Assert.AreEqual(1, result.Where(x => x.Town == town2.ShortName).Single().Count);
        }

        #endregion

        #region Total/Inspection Counts

        [TestMethod]
        public void TestGetCountOfExistingValvesThatAreInspectableForYearReturnsCorrectNumberOfExistingValvesForYear()
        {
            var year = 2011;
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var publicBilling = GetFactory<PublicValveBillingFactory>().Create();
            var pendingStatus = GetFactory<PendingAssetStatusFactory>().Create();
            var muniBilling = GetFactory<MunicipalValveBillingFactory>().Create();

            var oc = GetEntityFactory<OperatingCenter>().Create();
            // the good
            var val1 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus,
                DateInstalled = new DateTime(year, 1, 1)
            });
            // the bad
            var val1Installed = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus,
                DateInstalled = new DateTime(year + 1, 1, 1)
            });
            var val1BillingInvalid = GetEntityFactory<Valve>().Create(new
                {OperatingCenter = oc, ValveBilling = muniBilling, DateInstalled = new DateTime(year, 1, 1)});
            var val1StatusInvalid = GetEntityFactory<Valve>().Create(new
                {OperatingCenter = oc, Status = pendingStatus, DateInstalled = new DateTime(year, 1, 1)});

            Assert.AreEqual(1,
                Repository.GetCountOfExistingValvesThatAreInspectableForYear(year, null, oc.Id).Single().Count);
        }

        [TestMethod]
        public void
            TestGetCountOfExistingValvesThatAreInspectableForYearReturnsCorrectNumberOfExistingValvesForYearAndSize()
        {
            var year = 2011;
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var publicBilling = GetFactory<PublicValveBillingFactory>().Create();
            var pendingStatus = GetFactory<PendingAssetStatusFactory>().Create();
            var muniBilling = GetFactory<MunicipalValveBillingFactory>().Create();

            var oc = GetEntityFactory<OperatingCenter>().Create();
            var valveSizeSmall = GetEntityFactory<ValveSize>().Create(new {Size = 2m});
            var valveSizeLarge = GetEntityFactory<ValveSize>().Create(new {Size = 12m});
            // the good
            // small
            var val1 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus,
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeSmall
            });
            // large
            var val2 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus,
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeLarge
            });
            var val3 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus,
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeLarge
            });

            // the bad
            var val1Installed = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus,
                DateInstalled = new DateTime(year + 1, 1, 1), ValveSize = valveSizeSmall
            });
            var val2Installed = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus,
                DateInstalled = new DateTime(year + 1, 1, 1), ValveSize = valveSizeLarge
            });
            var val3Installed = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus,
                DateInstalled = new DateTime(year + 1, 1, 1), ValveSize = valveSizeLarge
            });
            var val1BillingInvalid = GetEntityFactory<Valve>().Create(new
                {OperatingCenter = oc, ValveBilling = muniBilling, DateInstalled = new DateTime(year, 1, 1)});
            var val1StatusInvalid = GetEntityFactory<Valve>().Create(new
                {OperatingCenter = oc, Status = pendingStatus, DateInstalled = new DateTime(year, 1, 1)});

            Assert.AreEqual(3,
                Repository.GetCountOfExistingValvesThatAreInspectableForYear(year, null, oc.Id).Single().Count);
            Assert.AreEqual(1,
                Repository.GetCountOfExistingValvesThatAreInspectableForYear(year, false, oc.Id).Single().Count);
            Assert.AreEqual(2,
                Repository.GetCountOfExistingValvesThatAreInspectableForYear(year, true, oc.Id).Single().Count);
        }

        [TestMethod]
        public void TestGetCountOfInspectionsRequiredForYearReturnsCorrectNumberOfRequiredInspections()
        {
            var year = 2011;
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var publicBilling = GetFactory<PublicValveBillingFactory>().Create();
            var pendingStatus = GetFactory<PendingAssetStatusFactory>().Create();
            var muniBilling = GetFactory<MunicipalValveBillingFactory>().Create();

            var oc = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});

            var val1 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[0],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val2 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[1],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val3 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[2],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val4 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[3],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val5 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[4],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val6 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[5],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val7 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[6],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val1BillingInvalid = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = muniBilling, ValveZone = valveZones[0],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val1StatusInvalid = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, Status = pendingStatus, ValveZone = valveZones[0],
                DateInstalled = new DateTime(year, 1, 1)
            });
            var val1InstalledInvalid = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[0],
                DateInstalled = new DateTime(year + 1, 1, 1)
            });

            //   Assert.AreEqual(6, Repository.GetCountOfInspectionsRequiredForYear(year, new[] { oc.Id, oc2.Id }));
            Assert.AreEqual(3, Repository.GetCountOfInspectionsRequiredForYear(year, null, oc.Id).Single().Count);
        }

        [TestMethod]
        public void TestGetCountOfInspectionsRequiredForYearAndSizeReturnsCorrectNumberOfRequiredInspections()
        {
            var year = 2011;
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var publicBilling = GetFactory<PublicValveBillingFactory>().Create();
            var pendingStatus = GetFactory<PendingAssetStatusFactory>().Create();
            var muniBilling = GetFactory<MunicipalValveBillingFactory>().Create();
            var oc = GetEntityFactory<OperatingCenter>().Create();
            var valveSizeSmall = GetEntityFactory<ValveSize>().Create(new {Size = 2m});
            var valveSizeLarge = GetEntityFactory<ValveSize>().Create(new {Size = 12m});

            // the good
            var val1 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[0],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeLarge
            });
            var val5 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[4],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeLarge
            });
            var val7 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[6],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeSmall
            });
            // the bad
            var val2 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[1],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeLarge
            });
            var val3 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[2],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeLarge
            });
            var val4 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[3],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeSmall
            });
            var val6 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[5],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeLarge
            });
            var val1BillingInvalid = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = muniBilling, ValveZone = valveZones[0],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeSmall
            });
            var val1StatusInvalid = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, Status = pendingStatus, ValveZone = valveZones[0],
                DateInstalled = new DateTime(year, 1, 1), ValveSize = valveSizeLarge
            });
            var val1InstalledInvalid = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = oc, ValveBilling = publicBilling, Status = activeStatus, ValveZone = valveZones[0],
                DateInstalled = new DateTime(year + 1, 1, 1), ValveSize = valveSizeLarge
            });

            Assert.AreEqual(3, Repository.GetCountOfInspectionsRequiredForYear(year, null, oc.Id).Single().Count);
            Assert.AreEqual(2, Repository.GetCountOfInspectionsRequiredForYear(year, true, oc.Id).Single().Count);
            Assert.AreEqual(1, Repository.GetCountOfInspectionsRequiredForYear(year, false, oc.Id).Single().Count);
        }

        #endregion

        #region Private Classes

        private class TestSearchValvesDueInspectionReport : SearchSet<ValveDueInspectionReportItem>
        {
            #region Properties

            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }

            #endregion
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
