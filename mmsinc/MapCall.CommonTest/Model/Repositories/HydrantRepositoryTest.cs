using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class HydrantRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Hydrant, HydrantRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesAssets;

        #endregion

        #region Private Members

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        private Mock<IDateTimeProvider> _dateProvider;
        private HydrantBilling _publicBilling, _companyBilling, _privateBilling, _municipalBilling;

        private AssetStatus _active,
                            _cancelled,
                            _pending,
                            _retired;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IHydrantBillingRepository>().Use<HydrantBillingRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {

            _active = GetFactory<ActiveAssetStatusFactory>().Create();
            _cancelled = GetFactory<CancelledAssetStatusFactory>().Create();
            _pending = GetFactory<PendingAssetStatusFactory>().Create();
            _retired = GetFactory<RetiredAssetStatusFactory>().Create();
            // These just need to exist for some tests:
            GetFactory<TownAbbreviationTypeFactory>().Create();
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
            _municipalBilling = GetFactory<MunicipalHydrantBillingFactory>().Create();
            _publicBilling = GetFactory<PublicHydrantBillingFactory>().Create();
            _companyBilling = GetFactory<CompanyHydrantBillingFactory>().Create();
            _privateBilling = GetFactory<PrivateHydrantBillingFactory>().Create();

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        #endregion

        #region Tests

        #region Linq

        [TestMethod]
        public void TestLinqDoesNotReturnHydrantsFromOtherOperatingCentersForUser()
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

            var hyd1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<HydrantRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(hyd1));
            Assert.IsFalse(result.Contains(hyd2));
        }

        [TestMethod]
        public void TestLinqReturnsAllHydrantsIfUserHasMatchingRoleWithWildcardOperatingCenter()
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

            var hyd1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<HydrantRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(hyd1));
            Assert.IsTrue(result.Contains(hyd2));
        }

        #endregion

        #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnHydrantsFromOtherOperatingCentersForUser()
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

            var hyd1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<HydrantRepository>();
            var model = new EmptySearchSet<Hydrant>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(hyd1));
            Assert.IsFalse(result.Contains(hyd2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllHydrantsIfUserHasMatchingRoleWithWildcardOperatingCenter()
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

            var hyd1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<HydrantRepository>();
            var model = new EmptySearchSet<Hydrant>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(hyd1));
            Assert.IsTrue(result.Contains(hyd2));
        }

        [TestMethod]
        public void TestCriteriaReturnsHydrantsThatHaveNullCoordinatesDueToLeftJoin()
        {
            var hydWithCoord = GetFactory<HydrantFactory>().Create(new {Coordinate = typeof(CoordinateFactory)});
            var hydWithoutCoord = GetFactory<HydrantFactory>().Create();
            hydWithoutCoord.Coordinate = null;
            Assert.IsNotNull(hydWithCoord.Coordinate, "Sanity check");
            Assert.IsNull(hydWithoutCoord.Coordinate, "Sanity check");

            var model = new EmptySearchSet<Hydrant>();
            var result = Repository.Search(model);
            Assert.IsTrue(result.Contains(hydWithCoord));
            Assert.IsTrue(result.Contains(hydWithoutCoord));
        }

        #endregion

        #region Delete

        [TestMethod]
        public void TestDeleteThrowsNotSupportedException()
        {
            MyAssert.Throws<NotSupportedException>(() => Repository.Delete(null));
        }

        #endregion

        #region FindByOperatingCenterAndNumber

        [TestMethod]
        public void TestFindByOperatingCenterAndHydrantNumberDoesExactlyThat()
        {
            var hydrantOne = GetFactory<HydrantFactory>().Create(new {HydrantNumber = "Neat"});
            var hydrantTwo = GetFactory<HydrantFactory>().Create(new {HydrantNumber = "Not neat"});

            Assert.AreSame(hydrantOne,
                Repository.FindByOperatingCenterAndHydrantNumber(hydrantOne.OperatingCenter, hydrantOne.HydrantNumber)
                          .Single());
        }

        [TestMethod]
        public void
            TestFindByOperatingCenterAndHydrantNumberDoesNotBlowUpIfYouUseTheLinqAnyExtensionOnAResultReturnedFromANonAdminUser()
        {
            // This test exists because non-admin users get an extra Where added to the query which
            // makes nhibernate choke and die.
            var hydrant = GetFactory<HydrantFactory>().Create();

            Repository = _container.GetInstance<HydrantRepository>();

            MyAssert.DoesNotThrow(
                () =>
                    Repository.FindByOperatingCenterAndHydrantNumber(hydrant.OperatingCenter, hydrant.HydrantNumber)
                              .Any());
        }

        #endregion

        #region FindByTown

        [TestMethod]
        public void TestFindByTownIdReturnsHydrantsWithMatchingTowns()
        {
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();

            var hyd1 = GetFactory<HydrantFactory>().Create(new {Town = town1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {Town = town2});

            Assert.AreSame(hyd1, Repository.FindByTownId(town1.Id).Single());
            Assert.AreSame(hyd2, Repository.FindByTownId(town2.Id).Single());
        }

        #endregion

        #region GetUnusedHydrantNumbers

        [TestMethod]
        public void TestGetUnusedHydrantNumbersReturnsEmptyIfThereAreNoMatchingHydrantsInTheSystem()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCenters.Add(opc1);
            var result = Repository.GetUnusedHydrantNumbers(opc1, town, null, null);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestGetUnusedHydrantNumbersReturnsEmptyIfThereIsOnlyOneMatchingHydrantAndItsSuffixEqualsOne()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCenters.Add(opc1);
            var hydrant = GetFactory<HydrantFactory>()
               .Create(new {Town = town, HydrantSuffix = 1, OperatingCenter = opc1});
            var result = Repository.GetUnusedHydrantNumbers(opc1, town, null, null);
            Assert.IsFalse(result.Any(), "1 would be both the min and max so there should not be any returned.");
        }

        [TestMethod]
        public void TestGetUnusedHydrantNumbersReturnsUnusedNumbersForMatchingTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCenters.Add(opc1);
            var hydrant = GetFactory<HydrantFactory>()
               .Create(new {Town = town, HydrantSuffix = 3, OperatingCenter = opc1});
            var result = Repository.GetUnusedHydrantNumbers(opc1, town, null, null).ToArray();
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(1), "1 is unused and should appear.");
            Assert.IsTrue(result.Contains(2), "2 is unused and should appear.");
        }

        [TestMethod]
        public void TestGetUnusedHydrantNumbersDoesNotCareAboutTownSectionAtAllWhenTownAbbreviationTypeIsTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {AbbreviationType = typeof(TownAbbreviationTypeFactory)});
            town.OperatingCenters.Add(opc1);
            var townSect = GetFactory<TownSectionFactory>().Create(new {Town = town});
            Assert.IsTrue(town.TownSections.Contains(townSect), "Sanity check");

            var hydrantInTown = GetFactory<HydrantFactory>()
               .Create(new {Town = town, HydrantSuffix = 3, OperatingCenter = opc1});
            var hydrantInTownSection = GetFactory<HydrantFactory>().Create(new
                {Town = town, TownSection = townSect, HydrantSuffix = 2, OperatingCenter = opc1});

            var result = Repository.GetUnusedHydrantNumbers(opc1, town, townSect, null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(1));

            // Results should be identical regardless of whether townSection was supplied or not.
            result = Repository.GetUnusedHydrantNumbers(opc1, town, null, null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(1));
        }

        [TestMethod]
        public void
            TestGetUnusedHydrantNumbersReturnsUnusedNumbersForMatchingTownAndIncludesNumbersThatAreUsedForTownSectionInSameTownWhenAbbreviationTypeForTownIsTownSectionButTheTownSectionIsNotSelected()
        {
            /* 
             * I hate reading long test names
             * 
             * This is testing that if a town uses the Town Section for creating hydrant numbers, but the number being used 
             * is not requiring a town section, then any number that matches the town section should be ignored.
             */
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCenters.Add(opc1);
            var townSect = GetFactory<TownSectionFactory>().Create(new {Town = town});
            Assert.IsTrue(town.TownSections.Contains(townSect), "Sanity check");

            var hydrantInTown = GetFactory<HydrantFactory>()
               .Create(new {Town = town, HydrantSuffix = 3, OperatingCenter = opc1});
            var hydrantInTownSection = GetFactory<HydrantFactory>().Create(new
                {Town = town, TownSection = townSect, HydrantSuffix = 2, OperatingCenter = opc1});

            var result =
                Repository.GetUnusedHydrantNumbers(opc1, town, null, null).ToArray(); // Do not include town section!

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result.Contains(2));
        }

        [TestMethod]
        public void
            TestGetUnusedHydrantNumberReturnsUnusedNumbersForTOWNSECTIONIfTownSectionIsSuppliedAndTownAbbreviationTypeIsTownSection()
        {
            /* 
            * This is testing that if a town uses the Town Section for creating hydrant numbers, and a town section is included,
            * then it should ignore the town's numbers.
            */
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCenters.Add(opc1);
            var townSect = GetFactory<TownSectionFactory>().Create(new {Town = town});
            Assert.IsTrue(town.TownSections.Contains(townSect), "Sanity check");

            var hydrantInTown = GetFactory<HydrantFactory>()
               .Create(new {Town = town, HydrantSuffix = 3, OperatingCenter = opc1});
            var hydrantInTownSection = GetFactory<HydrantFactory>().Create(new
                {Town = town, TownSection = townSect, HydrantSuffix = 2, OperatingCenter = opc1});

            var result = Repository.GetUnusedHydrantNumbers(opc1, town, townSect, null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(1));
        }

        #endregion

        #region Generating hydrant numbers

        #region Getting the correct abbreviation for the hydrant prefix

        [TestMethod]
        public void TestAbbreviationIsTownAbbreviationIfTownAbbreviationTypeIsTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(TownAbbreviationTypeFactory),
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});

            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town,
                Abbreviation = "SS"
            });
            var hydrant1 = GetEntityFactory<Hydrant>().Create(new
                {Town = town, HydrantSuffix = 1, HydrantNumber = "HQQ-1", OperatingCenter = opc1});

            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();

            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, null, null);
            Assert.AreEqual("HQQ-2", result.FormattedNumber);

            result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, townSection, null);
            Assert.AreEqual("HQQ-2", result.FormattedNumber, "Town abbreviation must be used in this instance.");
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
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town,
                Abbreviation = "SS"
            });
            var townSection2 = GetFactory<TownSectionFactory>().Create(new {Town = town, Abbreviation = ""});

            var hydrant1 = GetEntityFactory<Hydrant>().Create(new
                {Town = town, HydrantSuffix = 10, HydrantNumber = "HQQ-10", OperatingCenter = opc1});
            var hydrant2 = GetEntityFactory<Hydrant>().Create(new {
                Town = town, TownSection = townSection, HydrantSuffix = 5, HydrantNumber = "HSS-5",
                OperatingCenter = opc1
            });
            var hydrant3 = GetEntityFactory<Hydrant>().Create(new {
                Town = town, TownSection = townSection2, HydrantSuffix = 11, HydrantNumber = "HQQ-11",
                OperatingCenter = opc1
            });

            //var result = Repository.GenerateNextHydrantNumber(town, null);
            //Assert.AreEqual("HQQ-12", result.FormattedNumber, "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");

            //result = Repository.GenerateNextHydrantNumber(town, townSection2);
            //Assert.AreEqual("HQQ-12", result.FormattedNumber, "Town abbreviation must be used when the town section's abbreviation is null and the abbreviation type is town section.");

            //result = Repository.GenerateNextHydrantNumber(town, townSection);
            //Assert.AreEqual("HSS-6", result.FormattedNumber, "Town section abbreviation must be used in this instance.");

            var hydrant4 = GetEntityFactory<Hydrant>().Create(new
                {Town = town, TownSection = townSection, HydrantSuffix = 20, HydrantNumber = "HSS-20"});

            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();

            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, null, null);
            Assert.AreEqual("HQQ-12", result.FormattedNumber,
                "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");
        }

        [TestMethod]
        public void
            TestAbbreviationIsFireDistrictAbbreviationIfTownAbbreviationTypeIsFireDistrictAndFireDistrictIsNotNullAndFireDistrictAbbreviationIsNotNullOrEmptyOrWhiteSpace()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {
                AbbreviationType = typeof(FireDistrictAbbreviationTypeFactory),
            });
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            var fireDistrict = GetFactory<FireDistrictFactory>().Create(new {
                Abbreviation = "SS"
            });
            var fireDistrict2 = GetFactory<FireDistrictFactory>().Create(new {Abbreviation = ""});
            GetEntityFactory<FireDistrictTown>().Create(new {Town = town, FireDistrict = fireDistrict});
            GetEntityFactory<FireDistrictTown>().Create(new {Town = town, FireDistrict = fireDistrict2});

            var hydrant1 = GetEntityFactory<Hydrant>().Create(new
                {Town = town, HydrantSuffix = 10, HydrantNumber = "HQQ-10", OperatingCenter = opc1});
            var hydrant2 = GetEntityFactory<Hydrant>().Create(new {
                Town = town, FireDistrict = fireDistrict, HydrantSuffix = 5, HydrantNumber = "HSS-5",
                OperatingCenter = opc1
            });
            var hydrant3 = GetEntityFactory<Hydrant>().Create(new {
                Town = town, FireDistrict = fireDistrict2, HydrantSuffix = 11, HydrantNumber = "HQQ-11",
                OperatingCenter = opc1
            });

            //var result = Repository.GenerateNextHydrantNumber(town, null);
            //Assert.AreEqual("HQQ-12", result.FormattedNumber, "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");

            //result = Repository.GenerateNextHydrantNumber(town, FireDistrict2);
            //Assert.AreEqual("HQQ-12", result.FormattedNumber, "Town abbreviation must be used when the town section's abbreviation is null and the abbreviation type is town section.");

            //result = Repository.GenerateNextHydrantNumber(town, FireDistrict);
            //Assert.AreEqual("HSS-6", result.FormattedNumber, "Town section abbreviation must be used in this instance.");

            var hydrant4 = GetEntityFactory<Hydrant>().Create(new
                {Town = town, FireDistrict = fireDistrict, HydrantSuffix = 20, HydrantNumber = "HSS-20"});

            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();

            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, null, null);
            Assert.AreEqual("HQQ-12", result.FormattedNumber,
                "Town abbreviation must be used when the town section is null and the abbreviation type is town section.");
        }

        #endregion

        #region Getting the correct suffix

        [TestMethod]
        public void TestGenerateNextHydrantNumber_TownNumberingSystemReturns_1_IfThereAreNoHydrantsForTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {AbbreviationType = typeof(TownAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();
            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, null, null);
            Assert.AreEqual(1, result.Suffix);
        }

        [TestMethod]
        public void TestGenerateNextHydrantNumber_TownNumberingSystemReturns_MAX_IfThereIsAHydrantForThatTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {AbbreviationType = typeof(TownAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            GetFactory<HydrantFactory>().Create(new {Town = town, HydrantSuffix = 12, OperatingCenter = opc1});
            GetFactory<HydrantFactory>().Create(new {Town = town, HydrantSuffix = 41, OperatingCenter = opc1});
            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();
            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, null, null);
            Assert.AreEqual(42, result.Suffix);
        }

        [TestMethod]
        public void
            TestGenerateNextHydrantNumber_TownSectionNumberingSystemReturns_1_IfThereAreNoHydrantsThatMatchBothTownAndTownSection()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town});
            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();
            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, townSection, null);
            Assert.AreEqual(1, result.Suffix,
                "1 should be returned when there are no hydrants that match both the town and town section.");

            var hydrantSameTownNoTownSection = GetFactory<HydrantFactory>().Create(new {Town = town});
            result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, townSection, null);
            Assert.AreEqual(1, result.Suffix,
                "1 should be returned if there is a hydrant for the same town but different town section.");
        }

        [TestMethod]
        public void
            TestGenerateNextHydrantNumber_TownSectionNumberingSystemReturns_MAX_IfThereIsAMatchingHydrantForTownANDTownSection()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town, Abbreviation = "AB"});
            var hydrantWithTownAndTownSection = GetFactory<HydrantFactory>().Create(new
                {Town = town, TownSection = townSection, HydrantSuffix = 13, OperatingCenter = opc1});
            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();
            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, townSection, null);
            Assert.AreEqual(14, result.Suffix);

            var hydrantWithTownButNoTownSection =
                GetFactory<HydrantFactory>().Create(new {Town = town, HydrantSuffix = 28, OperatingCenter = opc1});

            result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, townSection, null);
            Assert.AreEqual(14, result.Suffix, "This should be 14. TownSection must be taken into account.");
        }

        [TestMethod]
        public void
            TestGenerateNextHydrantNumber_TownSectionNumberingSystemReturns_1_IfTownSectionIsNullAndThereAreNoHydrantsThatMatchTheTownAndNullTownSection()
        {
            // This is testing for when a town has the TownSection abbreviation type but no town section was selected 
            // for a hydrant. This is allowed. See Neptune(town, null town section) vs Ocean Grove(town is Neptune, OG is town section).
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town, Abbreviation = "AB"});

            // This should not be found in the query.
            var hydrantWithTownAndTownSection = GetFactory<HydrantFactory>()
               .Create(new {Town = town, TownSection = townSection, OperatingCenter = opc1});
            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();
            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, null, null);
            Assert.AreEqual(1, result.Suffix);
        }

        [TestMethod]
        public void
            TestGenerateNextHydrantNumber_TownSectionNumberingSystemReturns_MAX_IfTownSectionIsNullAndThereAreHydrantsThatMatchTheTownAndNullTownSection()
        {
            // This is testing for when a town has the TownSection abbreviation type but no town section was selected 
            // for a hydrant. This is allowed. See Neptune(town, null town section) vs Ocean Grove(town is Neptune, OG is town section).
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>()
               .Create(new {AbbreviationType = typeof(TownSectionAbbreviationTypeFactory)});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town, Abbreviation = "AB"});
            var hydrantWithTownOnly = GetFactory<HydrantFactory>()
               .Create(new {Town = town, HydrantSuffix = 13, OperatingCenter = opc1});
            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();
            var result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, null, null);
            Assert.AreEqual(14, result.Suffix);

            var hydrantWithTownSection =
                GetFactory<HydrantFactory>().Create(new
                    {Town = town, TownSection = townSection, HydrantSuffix = 19, OperatingCenter = opc1});
            result = Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, opc1, town, null, null);
            Assert.AreEqual(14, result.Suffix, "The hydrant with a town section should not be factored into this.");
        }

        [TestMethod]
        public void TestSharedTownGetsCorrectSuffixForCorrectOperatingCenter()
        {
            // EW1 - EDI - 211
            // EW4 - EDS - 17789
            var ew1 = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterName = "Netherwood", OperatingCenterCode = "EW1"});
            var ew4 = GetEntityFactory<OperatingCenter>().Create(new
                {OperatingCenterName = "Edison Water Company", OperatingCenterCode = "EW4"});
            var town = GetEntityFactory<Town>().Create(new {ShortName = "Edison Twp", FullName = "TOWNSHIP OF EDISON"});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = ew1, Town = town, Abbreviation = "EDI"});
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = ew4, Town = town, Abbreviation = "EDS"});
            Session.Save(town);
            town = Session.Load<Town>(town.Id);

            var hydrant1 = GetFactory<HydrantFactory>()
               .Create(new {Town = town, HydrantSuffix = 211, OperatingCenter = ew1});
            var hydrant2 = GetFactory<HydrantFactory>()
               .Create(new {Town = town, HydrantSuffix = 17789, OperatingCenter = ew4});

            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<AbbreviationTypeRepository>();

            Assert.AreEqual(2, town.OperatingCenters.Count);
            Assert.AreEqual(212, Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, ew1, town, null, null).Suffix);
            Assert.AreEqual("HEDI",
                Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, ew1, town, null, null).Prefix);
            Assert.AreEqual(17790,
                Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, ew4, town, null, null).Suffix);
            Assert.AreEqual("HEDS",
                Repository.GenerateNextHydrantNumber(abbrRepo, rawRepo, ew4, town, null, null).Prefix);
        }

        #endregion

        #endregion

        #region GetActiveHydrantCounts

        [TestMethod]
        public void
            TestGetActiveHydrantCountsReturnsHydrantCountsGroupedByOperatingCenterAndHydrantBillingForActiveHydrants()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            Assert.AreNotSame(opc1, opc2, "Sanity check because I was using the wrong factory.");

            var hydrantOPC1Billing1 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc1, HydrantBilling = _publicBilling});
            var hydrantOPC1Billing2 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc1, HydrantBilling = _privateBilling});
            var hydrantOPC2Billing1 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc2, HydrantBilling = _publicBilling});
            var hydrantOPC2Billing2 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc2, HydrantBilling = _privateBilling});
            var hydrantOPC2Billing2_again = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc2, HydrantBilling = _privateBilling});
            var hydrantOPC2Billing2_again_shouldnotmatter = GetFactory<HydrantFactory>().Create(new
                {OperatingCenter = opc2, HydrantBilling = _privateBilling, IsNonBPUKPI = true});

            // This should not appear in any counts.
            var hydrantOPC1Billing1_inactive = GetFactory<HydrantFactory>().Create(new
                {OperatingCenter = opc1, HydrantBilling = _publicBilling, Status = _retired});

            // Null op center and null town should return all hydrants 
            var args = new TestSearchActiveHydrantReport {
                OperatingCenter = null,
                Town = null
            };
            var result = Repository.GetActiveHydrantCounts(args).ToList();
            Assert.AreEqual(4, result.Count(), "There should only be 4 items returned.");
            Assert.AreEqual(1,
                result.Where(x => x.OperatingCenter == opc1.OperatingCenterCode &&
                                  x.HydrantBilling == _publicBilling.Description).Single().Count);
            Assert.AreEqual(1,
                result.Where(x => x.OperatingCenter == opc1.OperatingCenterCode &&
                                  x.HydrantBilling == _privateBilling.Description).Single().Count);
            Assert.AreEqual(1,
                result.Where(x => x.OperatingCenter == opc2.OperatingCenterCode &&
                                  x.HydrantBilling == _publicBilling.Description).Single().Count);
            Assert.AreEqual(2,
                result.Where(x => x.OperatingCenter == opc2.OperatingCenterCode &&
                                  x.HydrantBilling == _privateBilling.Description).Single().Count);
        }

        [TestMethod]
        public void TestGetActiveHydrantCountsOnlyCountsHydrantsWithOneOfTheActiveStatuses()
        {
            var assetStatuses = GetFactory<AssetStatusFactory>().CreateAll();
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var hydrant = GetFactory<HydrantFactory>().Create(new {HydrantBilling = _publicBilling});

            // Null op center and null town should return all hydrants 
            var args = new TestSearchActiveHydrantReport {
                OperatingCenter = null,
                Town = null
            };

            foreach (var assetStatus in assetStatuses)
            {
                var expectedCount = expectedAssetStatusIds.Contains(assetStatus.Id) ? 1 : 0;
                hydrant.Status = assetStatus;
                Session.Save(hydrant);
                Session.Flush();
                var result = Repository.GetActiveHydrantCounts(args).ToList();
                Assert.AreEqual(expectedCount, result.Count(),
                    $"Invalid result for hydrant with asset status {assetStatus.Description}.");
            }
        }

        [TestMethod]
        public void TestGetActiveHydrantCountsFiltersByOperatingCenter()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            Assert.AreNotSame(opc1, opc2, "Sanity check because I was using the wrong factory.");

            var hydrantOPC1Billing1 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc1, HydrantBilling = _publicBilling});
            var hydrantOPC1Billing2 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc1, HydrantBilling = _privateBilling});
            var hydrantOPC2Billing1 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc2, HydrantBilling = _publicBilling});
            var hydrantOPC2Billing2 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc2, HydrantBilling = _privateBilling});
            var hydrantOPC2Billing2_again = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = opc2, HydrantBilling = _privateBilling});

            var args = new TestSearchActiveHydrantReport {
                OperatingCenter = opc1.Id,
                Town = null
            };
            var result = Repository.GetActiveHydrantCounts(args).ToList();
            Assert.AreEqual(2, result.Count(), "There should only be 2 items returned.");
            Assert.AreEqual(1,
                result.Where(x => x.OperatingCenter == opc1.OperatingCenterCode &&
                                  x.HydrantBilling == _publicBilling.Description).Single().Count);
            Assert.AreEqual(1,
                result.Where(x => x.OperatingCenter == opc1.OperatingCenterCode &&
                                  x.HydrantBilling == _privateBilling.Description).Single().Count);
        }

        [TestMethod]
        public void TestGetActiveHydrantCountsFiltersByTown()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();

            var hydrantOPC1Billing1 = GetFactory<HydrantFactory>().Create(new
                {OperatingCenter = opc1, HydrantBilling = _publicBilling, Town = town1});
            var hydrantOPC1Billing2 = GetFactory<HydrantFactory>().Create(new
                {OperatingCenter = opc1, HydrantBilling = _privateBilling, Town = town1});
            var hydrantOPC2Billing1 = GetFactory<HydrantFactory>().Create(new
                {OperatingCenter = opc1, HydrantBilling = _publicBilling, Town = town2});
            var hydrantOPC2Billing2 = GetFactory<HydrantFactory>().Create(new
                {OperatingCenter = opc1, HydrantBilling = _privateBilling, Town = town2});
            var args = new TestSearchActiveHydrantReport {
                OperatingCenter = opc1.Id,
                Town = town1.Id
            };
            var result = Repository.GetActiveHydrantCounts(args).ToList();
            Assert.AreEqual(2, result.Count(), "There should only be 2 items returned.");
            Assert.AreEqual(1,
                result.Where(x => x.OperatingCenter == opc1.OperatingCenterCode &&
                                  x.HydrantBilling == _publicBilling.Description).Single().Count);
            Assert.AreEqual(1,
                result.Where(x => x.OperatingCenter == opc1.OperatingCenterCode &&
                                  x.HydrantBilling == _privateBilling.Description).Single().Count);
        }

        #endregion

        #region GetActiveHydrantDetailCount

        [TestMethod]
        public void TestGetActiveHydrantDetailCountsWithAllNullSearchParameters()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var hydrant1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opc1});
            var hydrant2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opc2});
            var hydrant3ThatShouldNotMatter =
                GetFactory<HydrantFactory>().Create(new {OperatingCenter = opc2, IsNonBPUKPI = true});
            var args = new TestSearchActiveHydrantDetailReport();
            var result = Repository.GetActiveHydrantDetailCounts(args).ToList();

            Assert.AreEqual(1, result.Where(x => x.OperatingCenter == opc1.OperatingCenterCode).Single().Count);
            Assert.AreEqual(1, result.Where(x => x.OperatingCenter == opc2.OperatingCenterCode).Single().Count);
        }

        [TestMethod]
        public void TestGetActiveHydrantDetailCountsOnlyCountsHydrantsWithOneOfTheActiveStatuses()
        {
            var assetStatuses = GetFactory<AssetStatusFactory>().CreateAll();
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var hydrant = GetFactory<HydrantFactory>().Create(new {HydrantBilling = _publicBilling});
            var args = new TestSearchActiveHydrantDetailReport();

            foreach (var assetStatus in assetStatuses)
            {
                var expectedCount = expectedAssetStatusIds.Contains(assetStatus.Id) ? 1 : 0;
                hydrant.Status = assetStatus;
                Session.Save(hydrant);
                Session.Flush();
                var result = Repository.GetActiveHydrantDetailCounts(args).ToList();
                Assert.AreEqual(expectedCount, result.Count(),
                    $"Invalid result for hydrant with asset status {assetStatus.Description}.");
            }
        }

        [TestMethod]
        public void TestGetActiveHydrantDetailCountsFiltersByOperatingCenter()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var hydrant1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opc1});
            var hydrant2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opc2});
            var args = new TestSearchActiveHydrantDetailReport {
                OperatingCenter = opc1.Id
            };
            var result = Repository.GetActiveHydrantDetailCounts(args).ToList();

            Assert.AreEqual(1, result.Where(x => x.OperatingCenter == opc1.OperatingCenterCode).Single().Count,
                "There should be one for opc1");
            Assert.AreEqual(0, result.Count(x => x.OperatingCenter == opc2.OperatingCenterCode),
                "There should be nothing for opc2");
        }

        [TestMethod]
        public void TestGetActiveHydrantDetailCountsFiltersByTown()
        {
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();
            var hydrant1 = GetFactory<HydrantFactory>().Create(new {Town = town1});
            var hydrant2 = GetFactory<HydrantFactory>().Create(new {Town = town2});
            var args = new TestSearchActiveHydrantDetailReport {
                Town = town1.Id
            };
            var result = Repository.GetActiveHydrantDetailCounts(args).ToList();

            Assert.AreEqual(1, result.Where(x => x.Town == town1.ShortName).Single().Count,
                "There should be one for town1");
            Assert.AreEqual(0, result.Count(x => x.Town == town2.ShortName), "There should be nothing for town2");
        }

        [TestMethod]
        public void TestGetActiveHydrantDetailCountsFiltersByLateralSize()
        {
            var ls1 = GetFactory<LateralSizeFactory>().Create();
            var ls2 = GetFactory<LateralSizeFactory>().Create();
            var hydrant1 = GetFactory<HydrantFactory>().Create(new {LateralSize = ls1});
            var hydrant2 = GetFactory<HydrantFactory>().Create(new {LateralSize = ls2});
            var args = new TestSearchActiveHydrantDetailReport {
                LateralSize = ls1.Id
            };
            var result = Repository.GetActiveHydrantDetailCounts(args).ToList();

            Assert.AreEqual(1, result.Where(x => x.LateralSize == ls1.Description).Single().Count,
                "There should be one for ls1");
            Assert.AreEqual(0, result.Count(x => x.LateralSize == ls2.Description), "There should be nothing for ls2");
        }

        [TestMethod]
        public void TestGetActiveHydrantDetailCountsFiltersByHydrantSize()
        {
            var hs1 = GetFactory<HydrantSizeFactory>().Create();
            var hs2 = GetFactory<HydrantSizeFactory>().Create();
            var hydrant1 = GetFactory<HydrantFactory>().Create(new {HydrantSize = hs1});
            var hydrant2 = GetFactory<HydrantFactory>().Create(new {HydrantSize = hs2});
            var args = new TestSearchActiveHydrantDetailReport {
                HydrantSize = hs1.Id
            };
            var result = Repository.GetActiveHydrantDetailCounts(args).ToList();

            Assert.AreEqual(1, result.Where(x => x.HydrantSize == hs1.Description).Single().Count,
                "There should be one for hs1");
            Assert.AreEqual(0, result.Count(x => x.HydrantSize == hs2.Description), "There should be nothing for hs2");
        }

        #endregion

        #region GetAssetCoordinates

        [TestMethod]
        public void TestGetAssetCoordinatesCorrectlyFiltersOnOperatingCenter()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var coord = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m});
            var hydrant = GetFactory<HydrantFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = coord
            });
            var hydrantBad = GetFactory<HydrantFactory>().Create(new {
                Coordinate = coord
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetAssetCoordinates(search).Single();
            Assert.AreEqual(hydrant.Id, result.Id);
        }

        [TestMethod]
        public void TestGetAssetCoordinateFiltersByExtent()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var hydrant = GetFactory<HydrantFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m})
            });
            var hydrantBad = GetFactory<HydrantFactory>().Create(new {
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

            var result = Repository.GetAssetCoordinates(search).Single();
            Assert.AreEqual(hydrant.Id, result.Id);
        }

        [TestMethod]
        public void TestGetAssetCoordinateDoesNotReturnHydrantsWithoutCoordinates()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var hydrant = GetFactory<HydrantFactory>().Create(new {
                OperatingCenter = opc,
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m})
            });
            var hydrantBad = GetFactory<HydrantFactory>().Create(new {
                OperatingCenter = opc,
            });
            hydrantBad.Coordinate = null;
            Session.Save(hydrantBad);
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {opc.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetAssetCoordinates(search).Single();
            Assert.AreEqual(hydrant.Id, result.Id);
        }

        [TestMethod]
        public void TestGetAssetCoordinateHandlesActiveStatusCase()
        {
            var assetStatusesById = GetFactory<AssetStatusFactory>().CreateAll();
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var hydrant = GetFactory<HydrantFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {hydrant.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            foreach (var assetStatus in assetStatusesById)
            {
                hydrant.Status = assetStatus;
                Session.Save(hydrant);
                Session.Flush();
                var expectedResult = expectedAssetStatusIds.Contains(assetStatus.Id);
                var result = Repository.GetAssetCoordinates(search).Single();
                Assert.AreEqual(expectedResult, result.IsActive,
                    $"Unexpected IsActive value when hydrant status is {assetStatus.Description}");
            }
        }

        [TestMethod]
        public void TestGetAssetCoordinateHandlesPublicBillingCase()
        {
            var hydrant = GetFactory<HydrantFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
                HydrantBilling = GetFactory<PublicHydrantBillingFactory>().Create()
            });
            var search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {hydrant.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            var result = Repository.GetAssetCoordinates(search).Single();
            Assert.IsTrue(result.IsPublic);

            hydrant = GetFactory<HydrantFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Coordinate = GetFactory<CoordinateFactory>().Create(new {Latitude = 50m, Longitude = 50m}),
                HydrantBilling = GetFactory<MunicipalHydrantBillingFactory>().Create()
            });
            search = new TestAssetCoordinateSearch {
                OperatingCenter = new[] {hydrant.OperatingCenter.Id},
                LatitudeMin = 0,
                LongitudeMin = 0,
                LatitudeMax = 100,
                LongitudeMax = 100
            };

            result = Repository.GetAssetCoordinates(search).Single();
            Assert.IsFalse(result.IsPublic);
        }

        #endregion

        #region GetCountofInspectionsRequiredForYearReturnsCorrectNumberOfRequiredInspections

        /// <summary>
        // active, public, 
        // if feq year then frequency
        // if feq month then 12\frequency
        // if feq week then 52\frequency
        // if feq day then 365\frequency
        // was installed <= the year
        /// </summary>
        [TestMethod]
        public void TestGetCountofInspectionsRequiredForYearReturnsCorrectNumberOfRequiredInspections()
        {
            var yearlyFrequency = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            var monthlyFrequency = GetFactory<MonthlyRecurringFrequencyUnitFactory>().Create();
            var weeklyFrequency = GetFactory<WeeklyRecurringFrequencyUnitFactory>().Create();
            var dailyFrequency = GetFactory<DailyRecurringFrequencyUnitFactory>().Create();
            var inActiveStatus = _pending;

            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var invalidOperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var year = 2011;
            // the good
            var hydActivePublicYearly = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year, 1, 1),
                InspectionFrequencyUnit = yearlyFrequency, InspectionFrequency = 1, OperatingCenter = operatingCenter
            }); // 1
            var hydActivePublicBiYearly = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year, 1, 1),
                InspectionFrequencyUnit = yearlyFrequency, InspectionFrequency = 2, OperatingCenter = operatingCenter
            }); // 1
            var hydActivePublicMonthly = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year, 1, 1),
                InspectionFrequencyUnit = monthlyFrequency, InspectionFrequency = 1, OperatingCenter = operatingCenter
            }); // 12
            // active public bi-monthly
            var hydActivePublicBiMonthly = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year, 1, 1),
                InspectionFrequencyUnit = monthlyFrequency, InspectionFrequency = 2, OperatingCenter = operatingCenter
            }); // 6
            var hydActivePublicWeekly = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year, 1, 1),
                InspectionFrequencyUnit = weeklyFrequency, InspectionFrequency = 1, OperatingCenter = operatingCenter
            }); // 52
            var hydActivePublicDaily = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year, 1, 1),
                InspectionFrequencyUnit = dailyFrequency, InspectionFrequency = 1, OperatingCenter = operatingCenter
            }); // 365

            // the bad
            var hydInstalledAfterYear = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year + 1, 1, 1),
                InspectionFrequencyUnit = yearlyFrequency, InspectionFrequency = 1, OperatingCenter = operatingCenter
            });
            var hydNotActive = GetEntityFactory<Hydrant>().Create(new {
                Status = inActiveStatus, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year, 1, 1),
                InspectionFrequencyUnit = yearlyFrequency, InspectionFrequency = 1, OperatingCenter = operatingCenter
            });
            var hydNotPublic = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _municipalBilling, DateInstalled = new DateTime(year, 1, 1),
                OperatingCenter = operatingCenter
            });
            var hydOtherOperatingCenter = GetEntityFactory<Hydrant>().Create(new {
                Status = _active, HydrantBilling = _publicBilling, DateInstalled = new DateTime(year, 1, 1),
                InspectionFrequencyUnit = yearlyFrequency, InspectionFrequency = 1,
                OperatingCenter = invalidOperatingCenter
            });
            var badIds = new Dictionary<string, int> {
                {nameof(hydInstalledAfterYear), hydInstalledAfterYear.Id},
                {nameof(hydNotActive), hydNotActive.Id},
                {nameof(hydNotPublic), hydNotPublic.Id},
                {nameof(hydOtherOperatingCenter), hydOtherOperatingCenter.Id},
            };
            Session.Flush();
            Session.Clear();

            var subResult = Repository.GetHydrantsRequiringInspectionInYear(year, operatingCenter.Id);
            Assert.IsFalse(subResult.Any(h =>
                    badIds.Values.Contains(h.Id)),
                $"Found bad id(s) '{string.Join(", ", subResult.Where(h => badIds.Values.Contains(h.Id)).Select(h => h.Id))}'. ({string.Join(", ", badIds.Select(b => b.Key + ":  " + b.Value))})");
            Assert.AreEqual(436, Repository.GetCountOfInspectionsRequiredForYear(year, operatingCenter.Id));
        }

        [TestMethod]
        public void TestInspectionsPerYearReturnsCorrectAmount()
        {
            var yearlyFrequency = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            var monthlyFrequency = GetFactory<MonthlyRecurringFrequencyUnitFactory>().Create();
            var weeklyFrequency = GetFactory<WeeklyRecurringFrequencyUnitFactory>().Create();
            var dailyFrequency = GetFactory<DailyRecurringFrequencyUnitFactory>().Create();

            var hydrant = GetEntityFactory<Hydrant>().Create(new {
                InspectionFrequencyUnit = yearlyFrequency, InspectionFrequency = 1,
                DateInstalled = new DateTime(2010, 1, 1)
            });
            Session.Evict(hydrant);
            hydrant = Repository.Find(hydrant.Id);

            Assert.AreEqual(1, hydrant.InspectionsPerYear);
        }

        #endregion

        #region GetHydrantsDueInspection

        [TestMethod]
        public void TestGetHydrantsDueInspectionReturnsTownCounts()
        {
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();
            var hydrant1 = GetFactory<HydrantFactory>().Create(new {Town = town1});
            var hydrant2 = GetFactory<HydrantFactory>().Create(new {Town = town2});

            // Should not be included in count
            var hydrantIsNotRequired = GetFactory<HydrantFactory>().Create(new {Town = town2});
            GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = hydrantIsNotRequired,
                DateInspected = DateTime.Today
            });

            var result = Repository.GetHydrantsDueInspection(new TestSearchHydrantsDueInspectionReport());

            Assert.AreEqual(1, result.Where(x => x.Town == town1.ShortName).Single().Count);
            Assert.AreEqual(1, result.Where(x => x.Town == town2.ShortName).Single().Count);
        }

        #endregion

        #region GetHydrantsDuePainting

        [TestMethod]
        public void TestGetHydrantsDuePaintingReturnsTownCounts()
        {
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();
            var hydrant1 = GetFactory<HydrantFactory>().Create(new {Town = town1});
            var hydrant2 = GetFactory<HydrantFactory>().Create(new {Town = town2});

            // Should not be included in count
            var hydrantIsNotRequired = GetFactory<HydrantFactory>().Create(new {Town = town2});
            GetFactory<HydrantPaintingFactory>().Create(new {
                Hydrant = hydrantIsNotRequired,
                PaintedAt = DateTime.Today
            });

            var result = Repository.GetHydrantsDuePainting(new TestSearchHydrantsDuePaintingReport());

            Assert.AreEqual(1, result.Where(x => x.Town == town1.ShortName).Single().Count);
            Assert.AreEqual(1, result.Where(x => x.Town == town2.ShortName).Single().Count);
        }

        #endregion

        #region GetHydrantsWithSapIssues

        [TestMethod]
        public void TestGetHydrantsWithSapIssuesReturnsHydrantsWithSapIssues()
        {
            var hydrantValid1 = GetFactory<HydrantFactory>().Create(new {SAPErrorCode = "RETRY::Something went wrong"});
            var hydrantInvalid1 = GetFactory<HydrantFactory>().Create(new {SAPErrorCode = ""});
            var hydrantInvalid2 = GetFactory<HydrantFactory>().Create();

            var result = Repository.GetHydrantsWithSapRetryIssues();

            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #region GetPublicHydrantCounts

        [TestMethod]
        public void TestGetPublicHydrantCountsReturnsExpectedResultsGroupedCorrectly()
        {
            var hydrant1 = GetFactory<HydrantFactory>().Create();
            var hydrant2 = GetFactory<HydrantFactory>()
               .Create(new {OperatingCenter = hydrant1.OperatingCenter, Town = hydrant1.Town});

            var args = new EmptySearchSet<PublicHydrantCountReportItem>();

            var result = Repository.GetPublicHydrantCounts(args);
            Assert.AreEqual(1,
                result.Count()); // NOTE: Don't use args.Count because the count will be wrong due to the group by being stripped.
            Assert.AreEqual(2, result.First().Total);
        }

        #endregion

        #region GetTotalNumberOfActiveHydrantsForPremise

        [TestMethod]
        public void TestGetTotalNumberOfActiveHydrantsForPremiseGetsCorrectCountOfHydrants()
        {
            var townGood = GetEntityFactory<Town>().Create();
            var townBad = GetEntityFactory<Town>().Create();
            var fireDistrictGood = GetEntityFactory<FireDistrict>().Create(new {PremiseNumber = "123456789"});
            GetEntityFactory<FireDistrictTown>().Create(new {Town = townGood, FireDistrict = fireDistrictGood});
            var fireDistrictBad = GetEntityFactory<FireDistrict>().Create(new {PremiseNumber = "987654321"});
            GetEntityFactory<FireDistrictTown>().Create(new {Town = townBad, FireDistrict = fireDistrictBad});

            var hydrant1 = GetEntityFactory<Hydrant>().Create(new
                {Town = townGood, FireDistrict = fireDistrictGood, HydrantBilling = _publicBilling});
            var hydrant2 = GetEntityFactory<Hydrant>().Create(new
                {Town = townGood, FireDistrict = fireDistrictGood, HydrantBilling = _companyBilling});
            var hydrant3 = GetEntityFactory<Hydrant>().Create(new
                {Town = townGood, FireDistrict = fireDistrictGood, HydrantBilling = _municipalBilling});
            var hydrant4 = GetEntityFactory<Hydrant>().Create(new
                {Town = townGood, FireDistrict = fireDistrictGood, HydrantBilling = _privateBilling});
            var hydrant5 = GetEntityFactory<Hydrant>().Create(new
                {Town = townGood, FireDistrict = fireDistrictGood, HydrantBilling = _publicBilling, Status = _pending});
            var hydrant6 = GetEntityFactory<Hydrant>().Create(new
                {Town = townGood, FireDistrict = fireDistrictGood, HydrantBilling = _publicBilling, Status = _retired});

            var result = Repository.GetTotalNumberOfActiveHydrantsForPremise(fireDistrictGood.PremiseNumber);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestGetTotalNumberOfActiveHydrantsForPremiseCountsIncludesAllActiveStatusesForHydrants()
        {
            var assetStatusesById = GetFactory<AssetStatusFactory>().CreateAll().ToDictionary(x => x.Id, x => x);
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var town = GetEntityFactory<Town>().Create();
            var fireDistrict = GetEntityFactory<FireDistrict>().Create(new {PremiseNumber = "123456789"});
            GetEntityFactory<FireDistrictTown>().Create(new {Town = town, FireDistrict = fireDistrict});

            var hydrant = GetEntityFactory<Hydrant>().Create(new
                {Town = town, FireDistrict = fireDistrict, HydrantBilling = _publicBilling});

            foreach (var assetStatus in assetStatusesById)
            {
                hydrant.Status = assetStatus.Value;
                Session.Save(hydrant);
                Session.Flush();

                var expectedResult = expectedAssetStatusIds.Contains(assetStatus.Key) ? 1 : 0;
                var result = Repository.GetTotalNumberOfActiveHydrantsForPremise(fireDistrict.PremiseNumber);
                Assert.AreEqual(expectedResult, result);
            }
        }

        #endregion

        #region TestGetByOperatingCenter

        [TestMethod]
        public void TestGetByOperatingCenter()
        {
            var operatingCenterA = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterC = GetFactory<UniqueOperatingCenterFactory>().Create();

            var hydrantAInOperatingCenterA = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenterA });
            var hydrantBInOperatingCenterA = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenterA });
            var hydrantCInOperatingCenterB = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenterB });
            var hydrantDInOperatingCenterC = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenterC });

            Session.Flush();

            var hydrantsInOperatingCenterA = Repository.GetHydrantsByOperatingCenter(operatingCenterA.Id).ToList();
            Assert.AreEqual(2, hydrantsInOperatingCenterA.Count);
            CollectionAssert.Contains(hydrantsInOperatingCenterA, hydrantAInOperatingCenterA);
            CollectionAssert.Contains(hydrantsInOperatingCenterA, hydrantBInOperatingCenterA);

            var hydrantsInOperatingCenterB = Repository.GetHydrantsByOperatingCenter(operatingCenterB.Id).ToList();
            Assert.AreEqual(1, hydrantsInOperatingCenterB.Count);
            CollectionAssert.Contains(hydrantsInOperatingCenterB, hydrantCInOperatingCenterB);

            var allHydrantsById = Repository.GetHydrantsByOperatingCenter(operatingCenterA.Id, operatingCenterB.Id, operatingCenterC.Id).ToList();
            Assert.AreEqual(4, allHydrantsById.Count);
            CollectionAssert.Contains(allHydrantsById, hydrantAInOperatingCenterA);
            CollectionAssert.Contains(allHydrantsById, hydrantBInOperatingCenterA);
            CollectionAssert.Contains(allHydrantsById, hydrantCInOperatingCenterB);
            CollectionAssert.Contains(allHydrantsById, hydrantDInOperatingCenterC);

            var allHydrantsByNone = Repository.GetHydrantsByOperatingCenter().ToList();
            Assert.AreEqual(4, allHydrantsByNone.Count);
            CollectionAssert.Contains(allHydrantsByNone, hydrantAInOperatingCenterA);
            CollectionAssert.Contains(allHydrantsByNone, hydrantBInOperatingCenterA);
            CollectionAssert.Contains(allHydrantsByNone, hydrantCInOperatingCenterB);
            CollectionAssert.Contains(allHydrantsByNone, hydrantDInOperatingCenterC);

            Assert.IsNull(Repository.GetHydrantsByOperatingCenter(54).SingleOrDefault());
        }

        #endregion

        #region Save

        [TestMethod]
        public void TestSaveInsertsNewHydrantOutOfServiceRecordsForHydrant()
        {
            var target = GetFactory<HydrantFactory>().Create();
            // NOTE: If you set Hydrant outside of BuildWithConcreteDependencies then saving
            //       resets the Hydrant reference back to the default one that BWCD created.
            var oos = GetFactory<HydrantOutOfServiceFactory>().BuildWithConcreteDependencies(new {
                Hydrant = target
            });
            target.OutOfServiceRecords.Add(oos);
            Repository.Save(target);

            Session.Evict(target);
            Session.Evict(oos);

            target = Repository.Find(target.Id);
            Assert.AreEqual(1, target.OutOfServiceRecords.Count);
            Assert.IsTrue(target.OutOfServiceRecords.Any(x => x.Id == oos.Id));
        }

        [TestMethod]
        public void TestSaveRefreshesFormulaColumnsOnEntity()
        {
            var target = GetFactory<HydrantFactory>().Create();
            // NOTE: If you set Hydrant outside of BuildWithConcreteDependencies then saving
            //       resets the Hydrant reference back to the default one that BWCD created.
            var oos = GetFactory<HydrantOutOfServiceFactory>().BuildWithConcreteDependencies(new {
                Hydrant = target
            });
            target.OutOfServiceRecords.Add(oos);
            Session.Save(oos);
            Assert.IsFalse(target.OutOfService);
            Repository.Save(target);
            Assert.IsTrue(target.OutOfService);
        }

        #endregion

        #region RouteByTownId

        [TestMethod]
        public void TestRouteByTownId()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var hydrantValid1 = GetEntityFactory<Hydrant>().Create(new {Town = towns[0], Route = 1});
            var hydrantValid2 = GetEntityFactory<Hydrant>().Create(new {Town = towns[0], Route = 2});
            var hydrantInvalid1 = GetEntityFactory<Hydrant>().Create(new {Town = towns[0]});
            var hydrantInvalid2 = GetEntityFactory<Hydrant>().Create(new {Town = towns[1], Route = 3});

            var result = Repository.RouteByTownId(towns[0].Id);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(hydrantValid1.Route, result.First());
            Assert.AreEqual(hydrantValid2.Route, result.Last());
            Assert.AreNotEqual(hydrantInvalid2.Route, result.First());
        }

        #endregion

        #region Reports

        #region Hydrant Routes

        [TestMethod]
        public void TestGetHydrantRoutes()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var hydrantsRoute1a = GetEntityFactory<Hydrant>().CreateList(3,
                new {Route = 1, OperatingCenter = operatingCenter, Town = town, Status = _active});
            var hydrantsRoute1r = GetEntityFactory<Hydrant>().CreateList(2,
                new {Route = 1, OperatingCenter = operatingCenter, Town = town, Status = _retired});
            var hydrantsRoute2a = GetEntityFactory<Hydrant>().CreateList(5,
                new {Route = 2, OperatingCenter = operatingCenter, Town = town, Status = _active});
            var hydrantsRoute2r = GetEntityFactory<Hydrant>().CreateList(4,
                new {Route = 2, OperatingCenter = operatingCenter, Town = town, Status = _retired});

            var args = new TestHydrantRouteReport();

            var results = Repository.GetRoutes(args).ToList();

            Assert.AreEqual(4, results.Count);

            Assert.AreEqual(1, results[0].Route);
            Assert.AreEqual(town, results[0].Town);
            Assert.AreEqual(operatingCenter, results[0].OperatingCenter);
            Assert.AreEqual(3, results[0].Total);
            Assert.AreEqual(_active, results[0].HydrantStatus);

            Assert.AreEqual(1, results[1].Route);
            Assert.AreEqual(town, results[1].Town);
            Assert.AreEqual(operatingCenter, results[1].OperatingCenter);
            Assert.AreEqual(2, results[1].Total);
            Assert.AreEqual(_retired, results[1].HydrantStatus);

            Assert.AreEqual(2, results[2].Route);
            Assert.AreEqual(town, results[2].Town);
            Assert.AreEqual(operatingCenter, results[2].OperatingCenter);
            Assert.AreEqual(5, results[2].Total);
            Assert.AreEqual(_active, results[2].HydrantStatus);

            Assert.AreEqual(2, results[3].Route);
            Assert.AreEqual(town, results[3].Town);
            Assert.AreEqual(operatingCenter, results[3].OperatingCenter);
            Assert.AreEqual(4, results[3].Total);
            Assert.AreEqual(_retired, results[3].HydrantStatus);
        }

        private class TestHydrantRouteReport : SearchSet<HydrantRouteReportItem>
        {
            public int? OperatingCenter { get; set; }
            public int? Town { get; set; }
            public int? HydrantStatus { get; set; }
        }

        #endregion

        #region AgedPendingAssets

        [TestMethod]
        public void TestAgedPendingAssetsReturnsDataCorrectlyForTheHorde()
        {
            AuthenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            var now = DateTime.Now;
            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var args = new TestAgedAssetSearch {OperatingCenter = opc1.Id};

            #region Good

            var hyd1 = GetEntityFactory<Hydrant>().CreateList(2,
                new {OperatingCenter = opc1, Status = _pending, CreatedAt = now.AddDays(-1)});
            var hyd90 = GetEntityFactory<Hydrant>().CreateList(2,
                new {OperatingCenter = opc1, Status = _pending, CreatedAt = now.AddDays(-90)});

            var hyd91 = GetEntityFactory<Hydrant>().CreateList(3,
                new {OperatingCenter = opc1, Status = _pending, CreatedAt = now.AddDays(-91)});
            var hyd180 = GetEntityFactory<Hydrant>().CreateList(3,
                new {OperatingCenter = opc1, Status = _pending, CreatedAt = now.AddDays(-180)});

            var hyd181 = GetEntityFactory<Hydrant>().CreateList(4,
                new {OperatingCenter = opc1, Status = _pending, CreatedAt = now.AddDays(-181)});
            var hyd360 = GetEntityFactory<Hydrant>().CreateList(4,
                new {OperatingCenter = opc1, Status = _pending, CreatedAt = now.AddDays(-360)});

            var hyd361 = GetEntityFactory<Hydrant>().CreateList(5,
                new {OperatingCenter = opc1, Status = _pending, CreatedAt = now.AddDays(-361)});

            #endregion

            #region Bad

            var hydBadStatus = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opc2, Status = _pending});
            var hydBadOpCntr = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opc1, Status = _active});

            #endregion

            var result = Repository.GetAgedPendingAssets(args).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(4, result[0].ZeroToNinety);
            Assert.AreEqual(6, result[0].NinetyOneToOneEighty);
            Assert.AreEqual(8, result[0].OneEightyToThreeSixty);
            Assert.AreEqual(5, result[0].ThreeSixtyPlus);
            Assert.AreEqual(23, result[0].Total);
            Assert.AreEqual("Hydrant", result[0].AssetType);
        }

        #endregion

        #endregion

        #region Search

        [TestMethod]
        public void TestSearchCorrectlyReturnsResultsForHydrantsWithOpenWorkOrdersThatHaveASpecificWorkDescription()
        {
            var hyd1 = GetFactory<HydrantFactory>().Create();
            var hyd2 = GetFactory<HydrantFactory>().Create();
            var workDesc = GetFactory<HydrantNoDripWorkDescriptionFactory>().Create();
            var woMatch1 = GetFactory<WorkOrderFactory>().Create(new {
                Hydrant = hyd1,
                WorkDescription = workDesc
            });
            var woMatch2 = GetFactory<WorkOrderFactory>().Create(new {
                Hydrant = hyd1,
                WorkDescription = workDesc
            });
            var woNoMatch1 = GetFactory<WorkOrderFactory>().Create(new {
                Hydrant = hyd1,
                WorkDescription = typeof(HydrantRepairWorkDescriptionFactory)
            });
            var woNoMatch2 = GetFactory<WorkOrderFactory>().Create(new {
                Hydrant = hyd2,
                WorkDescription = workDesc,
                DateCompleted = DateTime.Today,
                CancelledAt = DateTime.Today,
            });

            var search = new TestSearchHydrant();
            search.OpenWorkOrderWorkDescription = workDesc.Id;
            var result = Repository.Search(search);

            Assert.AreSame(hyd1, result.Single());
        }

        [TestMethod]
        public void TestSearchForMapReturnsCorrectListOfHydrants()
        {
            var state = GetEntityFactory<State>().Create();
            var hydList = GetEntityFactory<Hydrant>().CreateList(4, new {
                State = state
            }).Select(x => x.Id);

            var search = new TestSearchHydrantForMap();
            var result = Repository.SearchForMap(search).Select(x => x.Id);

            foreach (var id in hydList)
            {
                Assert.IsTrue(result.Contains(id));
            }
        }

        #endregion

        #endregion

        #region Test classes

        private class TestSearchActiveHydrantReport : SearchSet<ActiveHydrantReportItem>
        {
            #region Properties

            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }

            public int? Town { get; set; }

            #endregion
        }

        private class TestSearchActiveHydrantDetailReport : SearchSet<ActiveHydrantDetailReportItem>
        {
            #region Properties

            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }

            [SearchAlias("Town", "town", "Id")]
            public int? Town { get; set; }

            [SearchAlias("LateralSize", "latSize", "Id")]
            public int? LateralSize { get; set; }

            [SearchAlias("HydrantSize", "hydSize", "Id")]
            public int? HydrantSize { get; set; }

            #endregion
        }

        private class TestSearchHydrantsDueInspectionReport : SearchSet<HydrantDueInspectionReportItem>
        {
            #region Properties

            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }

            #endregion
        }

        private class TestSearchHydrantsDuePaintingReport : SearchSet<HydrantDuePaintingReportItem>
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

        private class TestAgedAssetSearch : SearchSet<AgedPendingAssetReportItem>
        {
            public int? OperatingCenter { get; set; }
        }

        private class TestSearchHydrant : SearchSet<Hydrant>, ISearchHydrant
        {
            public int? OpenWorkOrderWorkDescription { get; set; }
        }

        private class TestSearchHydrantForMap : SearchSet<HydrantAssetCoordinate>, ISearchHydrantForMap
        {
            public int? OpenWorkOrderWorkDescription { get; set; }
        }

        #endregion
    }
}
