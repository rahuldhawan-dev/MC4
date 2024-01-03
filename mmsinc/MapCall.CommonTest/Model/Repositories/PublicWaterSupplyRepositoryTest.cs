using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class PublicWaterSupplyRepositoryTest : MapCallMvcSecuredRepositoryTestBase<PublicWaterSupply,
        PublicWaterSupplyRepository, User>
    {

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetByOperatingCenterId()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pws1 = GetFactory<PublicWaterSupplyFactory>().Create();
            var pws2 = GetFactory<PublicWaterSupplyFactory>().Create();
            var invalid = GetFactory<PublicWaterSupplyFactory>().Create();
            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opc1, PublicWaterSupply = pws1}));
            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opc2, PublicWaterSupply = pws2}));
            Session.Flush();

            // Test getting just one
            var result = Repository.GetByOperatingCenterId(opc1.Id).ToArray();
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(pws1));

            // Test getting the other one
            result = Repository.GetByOperatingCenterId(opc2.Id).ToArray();
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(pws2));

            // Test getting both
            result = Repository.GetByOperatingCenterId(opc1.Id, opc2.Id).ToArray();
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(pws1));
            Assert.IsTrue(result.Contains(pws2));
            Assert.IsFalse(result.Contains(invalid));

            // Test getting all three when params are empty
            result = Repository.GetByOperatingCenterId().ToArray();
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Contains(pws1));
            Assert.IsTrue(result.Contains(pws2));
            Assert.IsTrue(result.Contains(invalid));
        }

        [TestMethod]
        public void TestGetActiveByOperatingCenterId()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activePublicWaterSupplyInOperatingCenter1 = GetFactory<PublicWaterSupplyFactory>().Create();
            var activePublicWaterSupplyInOperatingCenter2 = GetFactory<PublicWaterSupplyFactory>().Create();
            var pendingPublicWaterSupplyInOperatingCenter1 = GetFactory<PublicWaterSupplyFactory>().Create(new {
                Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create()
            });
            var pendingPublicWaterSupplyInOperatingCenter2 = GetFactory<PublicWaterSupplyFactory>().Create(new {
                Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create()
            });

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter1,
                PublicWaterSupply = activePublicWaterSupplyInOperatingCenter1
            }));
            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter2,
                PublicWaterSupply = activePublicWaterSupplyInOperatingCenter2
            }));

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter1,
                PublicWaterSupply = pendingPublicWaterSupplyInOperatingCenter1
            }));
            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter2,
                PublicWaterSupply = pendingPublicWaterSupplyInOperatingCenter2
            }));

            Session.Flush();

            // Test getting just one
            var result = Repository.GetActiveByOperatingCenterId(operatingCenter1.Id).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter1));

            // Test getting the other one
            result = Repository.GetActiveByOperatingCenterId(operatingCenter2.Id).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter2));

            // Test getting both
            result = Repository.GetActiveByOperatingCenterId(operatingCenter1.Id, operatingCenter2.Id).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter2));

            // Test getting all four when params are empty
            result = Repository.GetActiveByOperatingCenterId().ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter2));
            Assert.IsFalse(result.Contains(pendingPublicWaterSupplyInOperatingCenter1));
            Assert.IsFalse(result.Contains(pendingPublicWaterSupplyInOperatingCenter2));
        }

        [TestMethod]
        public void TestGetActiveOrPendingByOperatingCenterId()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activePublicWaterSupplyInOperatingCenter1 = GetFactory<PublicWaterSupplyFactory>().Create();
            var activePublicWaterSupplyInOperatingCenter2 = GetFactory<PublicWaterSupplyFactory>().Create();
            
            var pendingPublicWaterSupplyInOperatingCenter1 = GetFactory<PublicWaterSupplyFactory>().Create(new {
                Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create()
            });
            var pendingPublicWaterSupplyInOperatingCenter2 = GetFactory<PublicWaterSupplyFactory>().Create(new {
                Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create()
            });

            var pendingMergerPublicWaterSupplyInOperatingCenter1 = GetFactory<PublicWaterSupplyFactory>().Create(new {
                Status = GetFactory<PendingMergerPublicWaterSupplyStatusFactory>().Create()
            });
            var pendingMergerPublicWaterSupplyInOperatingCenter2 = GetFactory<PublicWaterSupplyFactory>().Create(new {
                Status = GetFactory<PendingMergerPublicWaterSupplyStatusFactory>().Create()
            });

            var inactivePublicWaterSupplyInOperatingCenter1 = GetFactory<PublicWaterSupplyFactory>().Create(new {
                Status = GetFactory<InactivePublicWaterSupplyStatusFactory>().Create()
            });
            var inactivePublicWaterSupplyInOperatingCenter2 = GetFactory<PublicWaterSupplyFactory>().Create(new {
                Status = GetFactory<InactivePublicWaterSupplyStatusFactory>().Create()
            });

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter1,
                PublicWaterSupply = activePublicWaterSupplyInOperatingCenter1
            }));

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter2,
                PublicWaterSupply = activePublicWaterSupplyInOperatingCenter2
            }));

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter1,
                PublicWaterSupply = pendingPublicWaterSupplyInOperatingCenter1
            }));

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter2,
                PublicWaterSupply = pendingPublicWaterSupplyInOperatingCenter2
            }));

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter1,
                PublicWaterSupply = pendingMergerPublicWaterSupplyInOperatingCenter1
            }));

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter2,
                PublicWaterSupply = pendingMergerPublicWaterSupplyInOperatingCenter2
            }));

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter1,
                PublicWaterSupply = inactivePublicWaterSupplyInOperatingCenter1
            }));

            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                OperatingCenter = operatingCenter2,
                PublicWaterSupply = inactivePublicWaterSupplyInOperatingCenter2
            }));

            Session.Flush();

            // Test getting just one
            var result = Repository.GetActiveOrPendingByOperatingCenterId(operatingCenter1.Id).ToArray();
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(pendingMergerPublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(pendingPublicWaterSupplyInOperatingCenter1));
            
            Assert.AreEqual(3, result.Length);

            // Test getting the other one
            result = Repository.GetActiveOrPendingByOperatingCenterId(operatingCenter2.Id).ToArray();
            Assert.AreEqual(3, result.Length);
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter2));
            Assert.IsTrue(result.Contains(pendingPublicWaterSupplyInOperatingCenter2));
            Assert.IsTrue(result.Contains(pendingMergerPublicWaterSupplyInOperatingCenter2));

            // Test getting both
            result = Repository.GetActiveOrPendingByOperatingCenterId(operatingCenter1.Id, operatingCenter2.Id).ToArray();
            Assert.AreEqual(6, result.Length);
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter2));
            Assert.IsTrue(result.Contains(pendingPublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(pendingPublicWaterSupplyInOperatingCenter2));
            Assert.IsTrue(result.Contains(pendingMergerPublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(pendingMergerPublicWaterSupplyInOperatingCenter2));

            // Test getting all six when params are empty
            result = Repository.GetActiveOrPendingByOperatingCenterId().ToArray();
            Assert.AreEqual(6, result.Length);
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(activePublicWaterSupplyInOperatingCenter2));
            Assert.IsTrue(result.Contains(pendingPublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(pendingPublicWaterSupplyInOperatingCenter2));
            Assert.IsTrue(result.Contains(pendingMergerPublicWaterSupplyInOperatingCenter1));
            Assert.IsTrue(result.Contains(pendingMergerPublicWaterSupplyInOperatingCenter2));
        }

        [TestMethod]
        public void TestGetAWOwnedByOperatingCenterIdsReturnsByOperatingCenterIds()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pws1 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = true});
            var pws2 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = true});
            var pws3 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = false});
            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opc1, PublicWaterSupply = pws1}));
            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opc2, PublicWaterSupply = pws2}));
            Session.Save(GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opc2, PublicWaterSupply = pws3}));
            Session.Flush();

            // Test getting just one
            var result = Repository.GetAWOwnedByOperatingCenterIds(new[] {opc1.Id});
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(pws1));

            // Test getting the other one
            result = Repository.GetAWOwnedByOperatingCenterIds(new[] {opc2.Id});
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(pws2));

            // Test getting both
            result = Repository.GetAWOwnedByOperatingCenterIds(new[] {opc1.Id, opc2.Id});
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(pws1));
            Assert.IsTrue(result.Contains(pws2));
            Assert.IsFalse(result.Contains(pws3), "Should not show up because it's not AW Owned");
        }

        [TestMethod]
        public void TestGetPartialPwsid()
        {
            var pws1 = GetFactory<PublicWaterSupplyFactory>().Create(new {Identifier = "PWS1"});
            var pws2 = GetFactory<PublicWaterSupplyFactory>().Create(new {Identifier = "pws2"});
            var pws3 = GetFactory<PublicWaterSupplyFactory>().Create(new {Identifier = "PwS3"});

            var result = Repository.FindByPartialIdMatch("pws");

            // Test Count and that it contains all pws Identifiers.
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Contains(pws1));
            Assert.IsTrue(result.Contains(pws2));
            Assert.IsTrue(result.Contains(pws3));

            // Test getting single identifier 
            result = Repository.FindByPartialIdMatch("pws1");
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(pws1));
            Assert.IsFalse(result.Contains(pws2));
            Assert.IsFalse(result.Contains(pws3));
        }

        [TestMethod]
        public void TestGetAllFilteredByWaterQualityGeneralRoleFiltersCorrectlyBasedOnOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.WaterQualityGeneral});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            User.DefaultOperatingCenter = opCntr1;
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = User
            });
            User.Roles.Add(role);
            Session.Save(User);
            Session.Refresh(User);

            var pws = GetFactory<PublicWaterSupplyFactory>().Create();
            var invalid = GetFactory<PublicWaterSupplyFactory>().Create();
            var opc1Forpws = GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opCntr1, PublicWaterSupply = pws});
            pws.OperatingCenterPublicWaterSupplies.Add(opc1Forpws);
            var opc1ForInvalid = GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opCntr2, PublicWaterSupply = invalid});
            invalid.OperatingCenterPublicWaterSupplies.Add(opc1ForInvalid);
            Session.Save(opc1Forpws);
            Session.Flush();

            // Assert that this returns for opc1 and opc3, but not for opc2

            Assert.AreSame(pws, Repository.GetAllFilteredByWaterQualityGeneralRole().Single());
        }

        #region GetDistinctOperatingCentersByPublicWaterSupplies

        [TestMethod]
        public void TestGetDistinctOperatingCentersByPublicWaterSuppliesReturnsEmptyResultIfPassedNullOrEmptyParameter()
        {
            Assert.AreEqual(0, Repository.GetDistinctOperatingCentersByPublicWaterSupplies(null).Count());
            Assert.AreEqual(0, Repository.GetDistinctOperatingCentersByPublicWaterSupplies(new int[] { }).Count());
        }

        [TestMethod]
        public void TestGetDistinctOperatingCentersByPublicWaterSuppliesReturnsDistinctOperatingCentersWhenRolesMatter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.WaterQualityGeneral});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            User.DefaultOperatingCenter = opCntr1;
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = User
            });
            User.Roles.Add(role);
            Session.Save(User);
            Session.Refresh(User);

            // Test that this works when a user has a role that only allows access to one operating center.
            var pws = GetFactory<PublicWaterSupplyFactory>().Create();
            var invalid = GetFactory<PublicWaterSupplyFactory>().Create();
            var opc1Forpws = GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opCntr1, PublicWaterSupply = pws});
            pws.OperatingCenterPublicWaterSupplies.Add(opc1Forpws);
            var opc1ForInvalid = GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opCntr2, PublicWaterSupply = invalid});
            invalid.OperatingCenterPublicWaterSupplies.Add(opc1ForInvalid);
            Session.Save(opc1Forpws);
            Session.Flush();

            Assert.AreSame(opCntr1,
                Repository.GetDistinctOperatingCentersByPublicWaterSupplies(new[] {pws.Id, invalid.Id}).Single());
        }

        [TestMethod] 
        public void TestGetDistinctOperatingCentersByPublicWaterSuppliesReturnsAllOperatingCentersWhenUserHasWildcardRole()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.WaterQualityGeneral});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            User.DefaultOperatingCenter = opCntr1;
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = User
            });
            User.Roles.Add(role);
            Session.Save(User);
            Session.Refresh(User);

            // Test that this works when a user has a role that only allows access to one operating center.
            var pws = GetFactory<PublicWaterSupplyFactory>().Create();
            var invalid = GetFactory<PublicWaterSupplyFactory>().Create();
            var opc1Forpws = GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opCntr1, PublicWaterSupply = pws});
            pws.OperatingCenterPublicWaterSupplies.Add(opc1Forpws);
            var opc1ForInvalid = GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opCntr2, PublicWaterSupply = invalid});
            invalid.OperatingCenterPublicWaterSupplies.Add(opc1ForInvalid);
            Session.Save(opc1Forpws);
            Session.Flush();

            var result = Repository.GetDistinctOperatingCentersByPublicWaterSupplies(new[] {pws.Id, invalid.Id});
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(opCntr1));
            Assert.IsTrue(result.Contains(opCntr2));
        }

        [TestMethod]
        public void
            TestGetDistinctOperatingCentersByPublicWaterSuppliesReturnsDistinctOperatingCentersWhenUserIsSiteAdmin()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            User.IsAdmin = true;
            Session.Save(User);

            // Test that this works when a user has a role that only allows access to one operating center.
            var pws = GetFactory<PublicWaterSupplyFactory>().Create();
            var invalid = GetFactory<PublicWaterSupplyFactory>().Create();
            var opc1Forpws = GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opCntr1, PublicWaterSupply = pws});
            pws.OperatingCenterPublicWaterSupplies.Add(opc1Forpws);
            var opc1ForInvalid = GetEntityFactory<OperatingCenterPublicWaterSupply>()
               .Create(new {OperatingCenter = opCntr2, PublicWaterSupply = invalid});
            invalid.OperatingCenterPublicWaterSupplies.Add(opc1ForInvalid);
            Session.Save(opc1Forpws);
            Session.Flush();

            var result = Repository.GetDistinctOperatingCentersByPublicWaterSupplies(new[] {pws.Id, invalid.Id});
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(opCntr1));
            Assert.IsTrue(result.Contains(opCntr2));
        }

        #endregion

        #region SearchYearlyWaterSampleComplianceReport

        [TestMethod]
        public void TestSearchYearlyWaterSampleComplianceReportReturnsExpectedRecordsWithMonthsMappedCorrectly()
        {
            var pwsid1 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = true});
            var janForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 1, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var febForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 2, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var marchForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 3, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var aprilForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 4, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var mayForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 5, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var juneForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 6, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var julyForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 7, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var augForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 8, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var septForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 9, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var octForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 10, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var novForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 11, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var decForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 12, CertifiedYear = 2018, PublicWaterSupply = pwsid1});

            var search = new YearlySearch();
            search.CertifiedYear = 2018;

            var result = Repository.SearchYearlyWaterSampleComplianceReport(search)
                                   .Single(); // Only one result should be coming back anyway

            Assert.AreEqual(pwsid1.Id, result.PublicWaterSupply.Id);
            Assert.AreEqual(janForm.Id, result.JanuaryForm.Id);
            Assert.AreEqual(febForm.Id, result.FebruaryForm.Id);
            Assert.AreEqual(marchForm.Id, result.MarchForm.Id);
            Assert.AreEqual(aprilForm.Id, result.AprilForm.Id);
            Assert.AreEqual(mayForm.Id, result.MayForm.Id);
            Assert.AreEqual(juneForm.Id, result.JuneForm.Id);
            Assert.AreEqual(julyForm.Id, result.JulyForm.Id);
            Assert.AreEqual(augForm.Id, result.AugustForm.Id);
            Assert.AreEqual(septForm.Id, result.SeptemberForm.Id);
            Assert.AreEqual(octForm.Id, result.OctoberForm.Id);
            Assert.AreEqual(novForm.Id, result.NovemberForm.Id);
            Assert.AreEqual(decForm.Id, result.DecemberForm.Id);
        }

        [TestMethod]
        public void TestSearchYearlyWaterSampleComplianceReportReturnsExpectedRecords()
        {
            var pwsid1 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = true});
            var pwsid2 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = true});

            // This one should not show up because it's the wrong year
            var janForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 1, CertifiedYear = 2017, PublicWaterSupply = pwsid1});
            var febForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 2, CertifiedYear = 2018, PublicWaterSupply = pwsid2});
            var marchForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 3, CertifiedYear = 2018, PublicWaterSupply = pwsid2});
            var aprilForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 4, CertifiedYear = 2018, PublicWaterSupply = pwsid2});
            var mayForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 5, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var juneForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 6, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var julyForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 7, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var augForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 8, CertifiedYear = 2018, PublicWaterSupply = pwsid2});
            var septForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 9, CertifiedYear = 2018, PublicWaterSupply = pwsid2});
            var octForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 10, CertifiedYear = 2018, PublicWaterSupply = pwsid1});
            var novForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 11, CertifiedYear = 2018, PublicWaterSupply = pwsid2});
            var decForm = GetFactory<WaterSampleComplianceFormFactory>().Create(new
                {CertifiedMonth = 12, CertifiedYear = 2018, PublicWaterSupply = pwsid1});

            var search = new YearlySearch();
            search.CertifiedYear = 2018;

            var results =
                Repository.SearchYearlyWaterSampleComplianceReport(search)
                          .ToList(); // Only one result should be coming back anyway
            var pwsid1Result = results.Single(x => x.PublicWaterSupply == pwsid1);
            var pwsid2Result = results.Single(x => x.PublicWaterSupply == pwsid2);

            Assert.AreEqual(pwsid1.Id, pwsid1Result.PublicWaterSupply.Id);
            Assert.IsNull(pwsid1Result.JanuaryForm,
                "January shouldn't show up because it's set to 2017 and we searched for 2018.");
            Assert.IsNull(pwsid1Result.FebruaryForm);
            Assert.IsNull(pwsid1Result.MarchForm);
            Assert.IsNull(pwsid1Result.AprilForm);
            Assert.AreEqual(mayForm.Id, pwsid1Result.MayForm.Id);
            Assert.AreEqual(juneForm.Id, pwsid1Result.JuneForm.Id);
            Assert.AreEqual(julyForm.Id, pwsid1Result.JulyForm.Id);
            Assert.IsNull(pwsid1Result.AugustForm);
            Assert.IsNull(pwsid1Result.SeptemberForm);
            Assert.AreEqual(octForm.Id, pwsid1Result.OctoberForm.Id);
            Assert.IsNull(pwsid1Result.NovemberForm);
            Assert.AreEqual(decForm.Id, pwsid1Result.DecemberForm.Id);

            Assert.AreEqual(pwsid2.Id, pwsid2Result.PublicWaterSupply.Id);
            Assert.IsNull(pwsid2Result.JanuaryForm,
                "January shouldn't show up because it's set to 2017 and we searched for 2018.");
            Assert.AreEqual(febForm.Id, pwsid2Result.FebruaryForm.Id);
            Assert.AreEqual(marchForm.Id, pwsid2Result.MarchForm.Id);
            Assert.AreEqual(aprilForm.Id, pwsid2Result.AprilForm.Id);
            Assert.IsNull(pwsid2Result.MayForm);
            Assert.IsNull(pwsid2Result.JuneForm);
            Assert.IsNull(pwsid2Result.JulyForm);
            Assert.AreEqual(augForm.Id, pwsid2Result.AugustForm.Id);
            Assert.AreEqual(septForm.Id, pwsid2Result.SeptemberForm.Id);
            Assert.IsNull(pwsid2Result.OctoberForm);
            Assert.AreEqual(novForm.Id, pwsid2Result.NovemberForm.Id);
            Assert.IsNull(pwsid2Result.DecemberForm);
        }

        [TestMethod]
        public void TestSearchYearlyWaterSampleComplianceReportThrowsExceptionIfCertifiedYearIsNull()
        {
            // This ia a required field, the extra exception throwing in the repository is there
            // to make it obvious that this search method was not written to work outside of
            // searching a single year.

            var search = new YearlySearch();
            search.CertifiedYear = null;

            MyAssert.Throws(() => Repository.SearchYearlyWaterSampleComplianceReport(search));
        }

        [TestMethod]
        public void TestSearchYearlyWaterSampleComplianceReportIncludesPWSIDsWhereNoComplianceFormsExistForTheYear()
        {
            var pwsid1 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = true});

            var search = new YearlySearch();
            search.CertifiedYear = 2018;

            var result = Repository.SearchYearlyWaterSampleComplianceReport(search)
                                   .Single(); // Only one result should be coming back anyway

            Assert.AreSame(pwsid1, result.PublicWaterSupply);
            Assert.IsNull(result.JanuaryForm);
            Assert.IsNull(result.FebruaryForm);
            Assert.IsNull(result.MarchForm);
            Assert.IsNull(result.AprilForm);
            Assert.IsNull(result.MayForm);
            Assert.IsNull(result.JuneForm);
            Assert.IsNull(result.JulyForm);
            Assert.IsNull(result.AugustForm);
            Assert.IsNull(result.SeptemberForm);
            Assert.IsNull(result.OctoberForm);
            Assert.IsNull(result.NovemberForm);
            Assert.IsNull(result.DecemberForm);
        }

        [TestMethod]
        public void TestSearchYearlyWaterSampleComplianceReportOnlyReturnsAWOwnedPWSIDs()
        {
            var pwsid1 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = true});
            var pwsid2 = GetFactory<PublicWaterSupplyFactory>().Create(new {AWOwned = false});

            var search = new YearlySearch();
            search.CertifiedYear = 2018;

            var result = Repository.SearchYearlyWaterSampleComplianceReport(search)
                                   .Single(); // Only one result should be coming back anyway
            Assert.AreSame(pwsid1, result.PublicWaterSupply);
        }

        #endregion

        #endregion

        #region Test classes

        private class YearlySearch : SearchSet<YearlyWaterSampleComplianceReportItem>,
            ISearchYearlyWaterSampleComplianceReport
        {
            public int? State { get; set; }
            public int[] OperatingCenter { get; set; }
            public int[] EntityId { get; set; }
            public int? CertifiedYear { get; set; }
        }

        #endregion
    }
}
