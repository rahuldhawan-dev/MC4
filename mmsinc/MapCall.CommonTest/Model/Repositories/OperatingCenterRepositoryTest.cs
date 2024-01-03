using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class OperatingCenterRepositoryTest : InMemoryDatabaseTest<OperatingCenter, OperatingCenterRepository>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        #endregion

        #region Tests

        #region Lookups/Cascades

        [TestMethod]
        public void TestGetAllReturnsAllOperatingCenters()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var result = Repository.GetAll().ToArray();
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(opc));
        }

        [TestMethod]
        public void TestFindFindsOperatingCenter()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            Assert.AreSame(opc, Repository.Find(opc.Id));
        }

        [TestMethod]
        public void TestGetByStateIdReturnsByStateId()
        {
            var nj = GetEntityFactory<State>().Create(new {Name = "New Jersey", Abbreviation = "NJ"});
            var ny = GetEntityFactory<State>().Create(new {Name = "New York", Abbreviation = "NY"});
            var good = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = nj});
            var bad = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = ny});

            var target = Repository.GetByStateId(nj.Id);

            Assert.AreEqual(1, target.Count());
            Assert.IsTrue(target.Contains(good));
            Assert.IsFalse(target.Contains(bad));
        }

        [TestMethod]
        public void TestGetByTownIdReturnsByTownId()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc3 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var opct1 = GetEntityFactory<OperatingCenterTown>()
               .Create(new {OperatingCenter = opc1, Town = town, Abbreviation = "QQ"});
            var opct2 = GetEntityFactory<OperatingCenterTown>()
               .Create(new {OperatingCenter = opc2, Town = town, Abbreviation = "QR"});
            Session.Save(opc1);
            Session.Save(opc2);
            Session.Save(town);
            Session.Flush();

            var result = Repository.GetByTownId(town.Id).ToList();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(opc1));
            Assert.IsTrue(result.Contains(opc2));
            Assert.IsFalse(result.Contains(opc3));
        }

        [TestMethod]
        public void TestSaveAddsAndRemovesAssetTypes()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var assetType = GetFactory<ValveAssetTypeFactory>().Create();
            var opcAssetType = new OperatingCenterAssetType {
                OperatingCenter = opc,
                AssetType = assetType
            };

            opc.OperatingCenterAssetTypes.Add(opcAssetType);

            Session.Save(opc);
            Session.Flush();
            Session.Evict(opc);
            Session.Evict(assetType);

            opc = Session.Query<OperatingCenter>().Single(x => x.Id == opc.Id);
            Assert.IsTrue(opc.OperatingCenterAssetTypes.Any(x => x.Id == opcAssetType.Id),
                "This should have been added.");

            opcAssetType = opc.OperatingCenterAssetTypes.Single(x => x.Id == opcAssetType.Id);
            opc.OperatingCenterAssetTypes.Remove(opcAssetType);
            Session.Save(opc);
            Session.Flush();
            Session.Evict(opc);

            opc = Session.Query<OperatingCenter>().Single(x => x.Id == opc.Id);
            Assert.IsFalse(opc.OperatingCenterAssetTypes.Any(x => x.Id == opcAssetType.Id),
                "This should have been removed.");

            assetType = Session.Query<AssetType>().Single(x => x.Id == assetType.Id);
            Assert.IsNotNull(assetType, "Asset type itself should not have been deleted.");
        }

        #endregion

        #region Reports

        #region OperatingCenterTrainingSummary

        [TestMethod]
        public void
            TestGetOperatingCenterTrainingSummaryReportItemsReturnsOneEmployeeWhoHasNotHadTrainingWhenTrainingRequirementHasOnlyOneTrainingModule()
        {
            var state = GetEntityFactory<State>().Create();
            var sapCompanyCode =
                GetEntityFactory<SAPCompanyCode>().Create(new {Description = "New Jersey-American Water"});
            var positionGroupCommonName =
                GetEntityFactory<PositionGroupCommonName>()
                   .Create(new {Description = "Production Maintenenace TCPA Non-Supv"});
            var positionGroupCommonName2 =
                GetEntityFactory<PositionGroupCommonName>()
                   .Create(new {Description = "Production Maintenenace TCPB Non-Supv"});
            var positionGroup = GetEntityFactory<PositionGroup>().Create(new {
                Group = "MNTMCH4",
                PositionDescription = "",
                BusinessUnit = "181801",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName
            });
            var positionGroup2 = GetEntityFactory<PositionGroup>().Create(new {
                Group = "MNTMCH3",
                PositionDescription = "",
                BusinessUnit = "181802",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName2
            });

            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                   .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var emp1 =
                GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // employee without training
            var emp2 = GetFactory<EmployeeFactory>()
               .Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // so we have an inactive employee
            var emp3 =
                GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // employee that will have the initial training
            var emp4 =
                GetFactory<ActiveEmployeeFactory>()
                   .Create(new {OperatingCenter = opc1, PositionGroup = positionGroup2});
            // employee in a different position group

            var trainingRequirement =
                GetEntityFactory<TrainingRequirement>()
                   .Create(new {Description = "Asbestos Awareness", IsActive = true});
            trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            Session.Save(trainingRequirement);
            trainingRequirement = Session.Load<TrainingRequirement>(trainingRequirement.Id);

            var initialTrainingModule =
                GetEntityFactory<TrainingModule>()
                   .Create(new {Title = "Safety JSA", TrainingRequirement = trainingRequirement});
            trainingRequirement.ActiveInitialTrainingModule = initialTrainingModule;
            Session.Save(trainingRequirement);

            var trainingRecord1 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-2), TrainingModule = initialTrainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp3, LinkedId = trainingRecord1.Id});
            emp3 = Session.Load<Employee>(emp3.Id);
            Session.Evict(trainingRecord1);
            trainingRecord1 = Session.Load<TrainingRecord>(trainingRecord1.Id);

            Assert.AreEqual(0, emp1.AttendedTrainingRecords.Count);
            Assert.AreEqual(1, trainingRecord1.EmployeesAttended.Count);

            emp1 = Session.Load<Employee>(emp1.Id);
            emp2 = Session.Load<Employee>(emp2.Id);
            emp3 = Session.Load<Employee>(emp3.Id);
            emp4 = Session.Load<Employee>(emp4.Id);

            Session.Flush();
            Session.Clear();

            var results =
                Repository.GetOperatingCenterTrainingSummaryReportItems(new TestSearchOperatingCenterTrainingSummary {
                    OperatingCenter = new[] {opc1.Id},
                    TrainingRequirement = trainingRequirement.Id
                });

            Assert.AreEqual(1, results.Count());
            var first = results.First();
            Assert.AreEqual(trainingRequirement.Id, first.TrainingRequirement.Id);
            Assert.AreEqual(2, first.EmployeesInPositionGroups);
            Assert.AreEqual(1, first.EmployeesOverDueInitialTraining);
            Assert.AreEqual(0, first.EmployeesOverDueRecurringTraining);
            Assert.AreEqual(0, first.EmployeesOverDueInitialAndRecurringTraining);
        }

        [TestMethod]
        public void
            TestGetOperatingCenterTrainingSummaryReportItemsReturnsOneEmployeeWhoHasNotHadTrainingWhenTrainingRequirementHasBothTrainingModules()
        {
            var state = GetEntityFactory<State>().Create();
            var sapCompanyCode =
                GetEntityFactory<SAPCompanyCode>().Create(new {Description = "New Jersey-American Water"});
            var positionGroupCommonName =
                GetEntityFactory<PositionGroupCommonName>()
                   .Create(new {Description = "Production Maintenenace TCPA Non-Supv"});
            var positionGroupCommonName2 =
                GetEntityFactory<PositionGroupCommonName>()
                   .Create(new {Description = "Production Maintenenace TCPB Non-Supv"});
            var positionGroup = GetEntityFactory<PositionGroup>().Create(new {
                Group = "MNTMCH4",
                PositionDescription = "",
                BusinessUnit = "181801",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName
            });
            var positionGroup2 = GetEntityFactory<PositionGroup>().Create(new {
                Group = "MNTMCH3",
                PositionDescription = "",
                BusinessUnit = "181802",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName2
            });

            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                   .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var emp1 =
                GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // 
            var emp2 = GetFactory<EmployeeFactory>()
               .Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // so we have an inactive employee
            var emp3 =
                GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // employee that will have the initial training
            var emp4 =
                GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // employee that will have the recurring
            var emp5 =
                GetFactory<ActiveEmployeeFactory>()
                   .Create(new {OperatingCenter = opc1, PositionGroup = positionGroup2});
            // employee in a different position group
            var emp6 =
                GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // employee that is due recurring training and has had both initial and recurring
            var emp7 =
                GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc1, PositionGroup = positionGroup});
            // employee that is due recurring training and has not had recurring at all

            opc1 = Repository.Find(opc1.Id);

            var trainingRequirement =
                GetEntityFactory<TrainingRequirement>()
                   .Create(
                        new {
                            Description = "Arc Flash / Electrical Safety in the Work Place - Awareness",
                            TrainingFrequency = 1,
                            TrainingFrequencyUnit = "Y",
                            IsActive = true
                        });
            trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            Session.Save(trainingRequirement);
            trainingRequirement = Session.Load<TrainingRequirement>(trainingRequirement.Id);

            var initialTrainingModule =
                GetEntityFactory<TrainingModule>()
                   .Create(
                        new {
                            Title = "Electrical Safety in the Workplace (OSHA: 1910.120) - Initial course",
                            TrainingRequirement = trainingRequirement
                        });
            var refresherTrainingModule =
                GetEntityFactory<TrainingModule>()
                   .Create(
                        new {
                            Title = "Electrical Safety in the Workplace (OSHA: 1910.120) - Refresher course",
                            TrainingRequirement = trainingRequirement
                        });
            trainingRequirement.ActiveInitialTrainingModule = initialTrainingModule;
            trainingRequirement.ActiveRecurringTrainingModule = refresherTrainingModule;
            Session.Save(trainingRequirement);

            var trainingRecord1 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-2), TrainingModule = initialTrainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp3, LinkedId = trainingRecord1.Id});
            emp3 = Session.Load<Employee>(emp3.Id);

            var trainingRecord2 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-1), TrainingModule = refresherTrainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp4, LinkedId = trainingRecord2.Id});
            emp4 = Session.Load<Employee>(emp4.Id);

            var trainingRecord3 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-731), TrainingModule = initialTrainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp6, LinkedId = trainingRecord3.Id});
            var trainingRecord4 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-366), TrainingModule = refresherTrainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp6, LinkedId = trainingRecord4.Id});
            var trainingRecord5 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-366), TrainingModule = initialTrainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp7, LinkedId = trainingRecord5.Id});

            Session.Evict(trainingRecord1);
            trainingRecord1 = Session.Load<TrainingRecord>(trainingRecord1.Id);
            Session.Evict(trainingRecord2);
            trainingRecord2 = Session.Load<TrainingRecord>(trainingRecord2.Id);
            Session.Evict(trainingRecord3);
            trainingRecord3 = Session.Load<TrainingRecord>(trainingRecord3.Id);
            Session.Evict(trainingRecord4);
            trainingRecord4 = Session.Load<TrainingRecord>(trainingRecord4.Id);
            Session.Evict(trainingRecord5);
            trainingRecord5 = Session.Load<TrainingRecord>(trainingRecord5.Id);

            Assert.AreEqual(0, emp1.AttendedTrainingRecords.Count);
            Assert.AreEqual(1, trainingRecord1.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord2.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord3.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord4.EmployeesAttended.Count);

            emp1 = Session.Load<Employee>(emp1.Id);
            emp2 = Session.Load<Employee>(emp2.Id);
            emp3 = Session.Load<Employee>(emp3.Id);
            emp4 = Session.Load<Employee>(emp4.Id);
            emp5 = Session.Load<Employee>(emp5.Id);
            emp6 = Session.Load<Employee>(emp6.Id);

            Session.Flush();
            Session.Clear();

            var results =
                Repository.GetOperatingCenterTrainingSummaryReportItems(new TestSearchOperatingCenterTrainingSummary {
                    OperatingCenter = new[] {opc1.Id},
                    TrainingRequirement = trainingRequirement.Id
                });

            // Should be a record for initial and recurring
            Assert.AreEqual(1, results.Count());

            var first = results.First();
            Assert.AreEqual(trainingRequirement.Id, first.TrainingRequirement.Id);
            Assert.AreEqual(5, first.EmployeesInPositionGroups);
            Assert.AreEqual(1, first.EmployeesOverDueInitialTraining);
            Assert.AreEqual(2, first.EmployeesOverDueRecurringTraining);
            Assert.AreEqual(0, first.EmployeesOverDueInitialAndRecurringTraining);
        }

        [TestMethod]
        public void TestGetOperatingCenterTrainingSummaryReportItemsReturnsCorrectEmployeesWithCombinedModule()
        {
            var state = GetEntityFactory<State>().Create();
            var sapCompanyCode =
                GetEntityFactory<SAPCompanyCode>().Create(new {Description = "New Jersey-American Water"});
            var positionGroupCommonName =
                GetEntityFactory<PositionGroupCommonName>()
                   .Create(new {Description = "Production Maintenenace TCPA Non-Supv"});
            var positionGroupCommonName2 =
                GetEntityFactory<PositionGroupCommonName>()
                   .Create(new {Description = "Production Maintenenace TCPB Non-Supv"});
            var positionGroup = GetEntityFactory<PositionGroup>().Create(new {
                Group = "MNTMCH4",
                PositionDescription = "",
                BusinessUnit = "181801",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName
            });
            var positionGroup2 = GetEntityFactory<PositionGroup>().Create(new {
                Group = "MNTMCH3",
                PositionDescription = "",
                BusinessUnit = "181802",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName2
            });

            var opc1 = GetFactory<UniqueOperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var emp1 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {OperatingCenter = opc1, PositionGroup = positionGroup}); // *will require training
            var emp2 = GetFactory<EmployeeFactory>()
               .Create(new {OperatingCenter = opc1, PositionGroup = positionGroup}); // so we have an inactive employee
            var emp3 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // employee that will have the initial training
            var emp4 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // employee that will have the recurring
            var emp5 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup2
                }); // employee in a different position group
            var emp6 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // *employee that is due recurring training and has had both initial and recurring
            var emp7 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // *employee that is due recurring training and has not had recurring at all
            var emp8 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // employ that has retired training module training training record

            opc1 = Repository.Find(opc1.Id);

            var trainingRequirement =
                GetEntityFactory<TrainingRequirement>()
                   .Create(
                        new {
                            Description = "Blood Borne Pathogens",
                            TrainingFrequency = 365,
                            TrainingFrequencyUnit = "D",
                            IsActive = true
                        });
            trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            Session.Save(trainingRequirement);
            trainingRequirement = Session.Load<TrainingRequirement>(trainingRequirement.Id);

            var trainingModule =
                GetEntityFactory<TrainingModule>()
                   .Create(
                        new {
                            Title = "Blood Borne Pathogens (OSHA: 1910.1030) - Initial/Refresher course",
                            TrainingRequirement = trainingRequirement
                        });
            var trainingModuleRetired = GetEntityFactory<TrainingModule>().Create(
                new {
                    Title = "Blood Borne Pathogens (OSHA: 1910.1030) - Initial/Refresher course (2013)",
                    TrainingRequirement = trainingRequirement
                });
            trainingRequirement.ActiveInitialAndRecurringTrainingModule = trainingModule;
            Session.Save(trainingRequirement);

            var trainingRecord1 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-2), TrainingModule = trainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp3, LinkedId = trainingRecord1.Id});
            emp3 = Session.Load<Employee>(emp3.Id);

            var trainingRecord2 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-1), TrainingModule = trainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp4, LinkedId = trainingRecord2.Id});
            emp4 = Session.Load<Employee>(emp4.Id);

            var trainingRecord3 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-731), TrainingModule = trainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp6, LinkedId = trainingRecord3.Id});
            var trainingRecord4 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-366), TrainingModule = trainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp6, LinkedId = trainingRecord4.Id});

            var trainingRecord5 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-366), TrainingModule = trainingModule});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp7, LinkedId = trainingRecord5.Id});

            var trainingRecordRetired =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-1), TrainingModule = trainingModuleRetired});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp8, LinkedId = trainingRecordRetired.Id});
            emp8 = Session.Load<Employee>(emp8.Id);

            Session.Evict(trainingRecord1);
            trainingRecord1 = Session.Load<TrainingRecord>(trainingRecord1.Id);
            Session.Evict(trainingRecord2);
            trainingRecord2 = Session.Load<TrainingRecord>(trainingRecord2.Id);
            Session.Evict(trainingRecord3);
            trainingRecord3 = Session.Load<TrainingRecord>(trainingRecord3.Id);
            Session.Evict(trainingRecord4);
            trainingRecord4 = Session.Load<TrainingRecord>(trainingRecord4.Id);
            Session.Evict(trainingRecord5);
            trainingRecord5 = Session.Load<TrainingRecord>(trainingRecord5.Id);
            Session.Evict(trainingRecordRetired);
            trainingRecordRetired = Session.Load<TrainingRecord>(trainingRecordRetired.Id);

            Assert.AreEqual(0, emp1.AttendedTrainingRecords.Count);
            Assert.AreEqual(1, trainingRecord1.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord2.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord3.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord4.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord5.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecordRetired.EmployeesAttended.Count);

            emp1 = Session.Load<Employee>(emp1.Id);
            emp2 = Session.Load<Employee>(emp2.Id);
            emp3 = Session.Load<Employee>(emp3.Id);
            emp4 = Session.Load<Employee>(emp4.Id);
            emp5 = Session.Load<Employee>(emp5.Id);
            emp6 = Session.Load<Employee>(emp6.Id);
            emp7 = Session.Load<Employee>(emp7.Id);
            emp8 = Session.Load<Employee>(emp8.Id);

            Session.Flush();
            Session.Clear();

            var results =
                Repository.GetOperatingCenterTrainingSummaryReportItems(new TestSearchOperatingCenterTrainingSummary {
                    OperatingCenter = new[] {opc1.Id},
                    TrainingRequirement = trainingRequirement.Id
                });

            // Should be a record for initial and recurring
            Assert.AreEqual(1, results.Count());

            var first = results.First();
            Assert.AreEqual(trainingRequirement.Id, first.TrainingRequirement.Id);
            Assert.AreEqual(6, first.EmployeesInPositionGroups);
            Assert.AreEqual(0, first.EmployeesOverDueInitialTraining);
            Assert.AreEqual(0, first.EmployeesOverDueRecurringTraining);
            Assert.AreEqual(3, first.EmployeesOverDueInitialAndRecurringTraining);
        }

        #endregion

        #region OperatingCenterTrainingOverview

        [TestMethod]
        public void TestOperatingCenterTrainingOverviewReturnsItems()
        {
            // ARRANGE
            var state = GetEntityFactory<State>().Create();
            var sapCompanyCode = GetEntityFactory<SAPCompanyCode>()
               .Create(new {Description = "New Jersey-American Water"});
            var positionGroupCommonName = GetEntityFactory<PositionGroupCommonName>()
               .Create(new {Description = "Production Maintenenace TCPA Non-Supv"});
            var positionGroupCommonName2 = GetEntityFactory<PositionGroupCommonName>()
               .Create(new {Description = "Production Maintenenace TCPB Non-Supv"});
            var positionGroup = GetEntityFactory<PositionGroup>().Create(new {
                Group = "MNTMCH4",
                PositionDescription = "",
                BusinessUnit = "181801",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName
            });
            var positionGroup2 = GetEntityFactory<PositionGroup>().Create(new {
                Group = "MNTMCH3",
                PositionDescription = "",
                BusinessUnit = "181802",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName2
            });
            // training requirement 1
            var trainingRequirement1 = GetEntityFactory<TrainingRequirement>().Create(new {
                Description = "Blood Borne Pathogens", TrainingFrequency = 365, TrainingFrequencyUnit = "D",
                IsActive = true
            });
            trainingRequirement1.PositionGroupCommonNames.Add(positionGroupCommonName);
            Session.Save(trainingRequirement1);
            trainingRequirement1 = Session.Load<TrainingRequirement>(trainingRequirement1.Id);
            var trainingModule1 = GetEntityFactory<TrainingModule>().Create(new {
                Title = "Blood Borne Pathogens (OSHA: 1910.1030) - Initial/Refresher course",
                TrainingRequirement = trainingRequirement1
            });
            trainingRequirement1.ActiveInitialAndRecurringTrainingModule = trainingModule1;
            Session.Save(trainingRequirement1);
            // training requirement 2
            var trainingRequirement2 = GetEntityFactory<TrainingRequirement>().Create(new {
                Description = "Asbestos Awareness", TrainingFrequency = 365, TrainingFrequencyUnit = "D",
                IsActive = true
            });
            trainingRequirement2.PositionGroupCommonNames.Add(positionGroupCommonName);
            Session.Save(trainingRequirement2);
            trainingRequirement2 = Session.Load<TrainingRequirement>(trainingRequirement2.Id);
            var trainingModule2 = GetEntityFactory<TrainingModule>().Create(new {
                Title = "Asbestos Awareness (OSHA: 1910.1001) - Initial/Refresher course",
                TrainingRequirement = trainingRequirement2
            });
            trainingRequirement2.ActiveInitialAndRecurringTrainingModule = trainingModule2;
            Session.Save(trainingRequirement2);
            var opc1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opc2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var emp1 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {OperatingCenter = opc1, PositionGroup = positionGroup}); // *will require training
            var emp2 = GetFactory<EmployeeFactory>()
               .Create(new {OperatingCenter = opc1, PositionGroup = positionGroup}); // so we have an inactive employee
            var emp3 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // employee that will have the initial training
            var emp4 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // employee that will have the recurring
            var emp5 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup2
                }); // employee in a different position group
            var emp6 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // *employee that is due recurring training and has had both initial and recurring
            var emp7 = GetFactory<ActiveEmployeeFactory>()
               .Create(new {
                    OperatingCenter = opc1, PositionGroup = positionGroup
                }); // *employee that is due recurring training and has not had recurring at all
            Session.Flush();
            Session.Clear();
            Session.Evict(opc1);
            Session.Evict(opc2);
            opc1 = Repository.Find(opc1.Id);
            opc2 = Repository.Find(opc2.Id);

            var trainingRecord1 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-2), TrainingModule = trainingModule1});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp3, LinkedId = trainingRecord1.Id});
            emp3 = Session.Load<Employee>(emp3.Id);

            var trainingRecord2 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-1), TrainingModule = trainingModule1});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp4, LinkedId = trainingRecord2.Id});
            emp4 = Session.Load<Employee>(emp4.Id);

            var trainingRecord3 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-731), TrainingModule = trainingModule1});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp6, LinkedId = trainingRecord3.Id});
            var trainingRecord4 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-366), TrainingModule = trainingModule1});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp6, LinkedId = trainingRecord4.Id});

            var trainingRecord5 =
                GetEntityFactory<TrainingRecord>()
                   .Create(new {HeldOn = DateTime.Now.AddDays(-366), TrainingModule = trainingModule1});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = emp7, LinkedId = trainingRecord5.Id});

            Session.Evict(trainingRecord1);
            trainingRecord1 = Session.Load<TrainingRecord>(trainingRecord1.Id);
            Session.Evict(trainingRecord2);
            trainingRecord2 = Session.Load<TrainingRecord>(trainingRecord2.Id);
            Session.Evict(trainingRecord3);
            trainingRecord3 = Session.Load<TrainingRecord>(trainingRecord3.Id);
            Session.Evict(trainingRecord4);
            trainingRecord4 = Session.Load<TrainingRecord>(trainingRecord4.Id);
            Session.Evict(trainingRecord5);
            trainingRecord5 = Session.Load<TrainingRecord>(trainingRecord5.Id);

            Assert.AreEqual(1, trainingRecord1.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord2.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord3.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord4.EmployeesAttended.Count);

            emp1 = Session.Load<Employee>(emp1.Id);
            emp2 = Session.Load<Employee>(emp2.Id);
            emp3 = Session.Load<Employee>(emp3.Id);
            emp4 = Session.Load<Employee>(emp4.Id);
            emp5 = Session.Load<Employee>(emp5.Id);
            emp6 = Session.Load<Employee>(emp6.Id);
            emp7 = Session.Load<Employee>(emp7.Id);

            Assert.AreEqual(0, emp1.AttendedTrainingRecords.Count);

            Session.Flush();
            Session.Clear();
            // ACT
            var results = Repository
                         .GetOperatingCenterTrainingOverviewReportItems(new TestSearchOperatingCenterTrainingOverview())
                         .ToList();

            // ASSERT
            Assert.AreEqual(2, results.Count());
            var first = results[0];
            var last = results[1];
            Assert.AreEqual(opc1.Id, last.OperatingCenter.Id);
            Assert.AreEqual(6, last.Employees);

            Assert.AreEqual(10, last.EmployeeTrainingRecordsRequired);
            Assert.AreEqual(8, last.EmployeeTrainingRecordsDue);
            Assert.AreEqual(2, last.EmployeeTrainingRecordsCompleted);
            Assert.AreEqual(opc2.Id, first.OperatingCenter.Id);
            Assert.AreEqual(0, first.EmployeeTrainingRecordsDue);
        }

        #endregion

        #region OperatingCenterArcFlashCompletions

        [TestMethod]
        public void TestOperatingCenterArcFlashCompletionsReturnsItems()
        {
            var afStatusComplete = GetFactory<CompletedArcFlashStatusFactory>().Create(new { Id = ArcFlashStatus.Indices.COMPLETED, Description = "Completed" });
            var afStatusPending = GetFactory<PendingArcFlashStatusFactory>().Create(new { Id = ArcFlashStatus.Indices.PENDING, Description = "Pending" });
            var afStatusDeferred = GetFactory<DeferredArcFlashStatusFactory>().Create(new { Id = ArcFlashStatus.Indices.DEFFERED, Description = "Deferred" });
            var afStudyCompleted1 = GetFactory<ArcFlashStudyFactory>().Create(new { ArcFlashStatus = afStatusComplete });
            var afStudyCompleted2 = GetFactory<ArcFlashStudyFactory>().Create(new { ArcFlashStatus = afStatusComplete });
            var afStudyCompleted3 = GetFactory<ArcFlashStudyFactory>().Create(new { ArcFlashStatus = afStatusComplete });
            var afStudyCompleted4 = GetFactory<ArcFlashStudyFactory>().Create(new { ArcFlashStatus = afStatusComplete });
            var afStudyPending = GetFactory<ArcFlashStudyFactory>().Create(new { ArcFlashStatus = afStatusPending });
            var afStudyDeferred = GetFactory<ArcFlashStudyFactory>().Create(new { ArcFlashStatus = afStatusDeferred });
            var afStudyList1 = new List<ArcFlashStudy> {
                afStudyCompleted1,
                afStudyCompleted4,
                afStudyPending,
                afStudyDeferred
            };
            var afStudyList2 = new List<ArcFlashStudy> {
                afStudyCompleted2,
                afStudyCompleted3
            };
            var fac1 = GetFactory<FacilityFactory>().Create();
            fac1.ArcFlashStudies = afStudyList1;
            var fac2 = GetFactory<FacilityFactory>().Create();
            fac2.ArcFlashStudies = afStudyList2;
            var facilityList = new List<Facility> {
                fac1,
                fac2
            };

            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            operatingCenter.Facilities = facilityList;
            Session.Save(operatingCenter);
            operatingCenter = Session.Load<OperatingCenter>(operatingCenter.Id);
            Assert.AreEqual(2, operatingCenter.Facilities.Count);

            Session.Flush();
            Session.Clear();

            var afCompletionReportItems = new SearchArcFlashCompletion();

            var results =
                Repository.GetArcFlashCompletions(afCompletionReportItems);

            var arcFlashCompletionReportItems = results as ArcFlashCompletionReportItem[] ?? results.ToArray();
            Assert.AreEqual(4, arcFlashCompletionReportItems.FirstOrDefault()?.NumberCompleted);
            Assert.AreEqual(1, arcFlashCompletionReportItems.FirstOrDefault()?.NumberPending);
            Assert.AreEqual(1, arcFlashCompletionReportItems.FirstOrDefault()?.NumberDeferred);
        }

        #endregion

        #endregion

        #endregion
    }

    public class TestSearchOperatingCenterTrainingSummary : SearchSet<OperatingCenterTrainingSummaryReportItem>,
        ISearchOperatingCenterTrainingSummary
    {
        public int[] OperatingCenter { get; set; }
        public int? TrainingRequirement { get; set; }
        public int? State { get; set; }
        public bool? IsOSHARequirement { get; set; }
        public DateTime? DueBy { get; set; }
    }

    public class TestSearchOperatingCenterTrainingOverview : SearchSet<OperatingCenterTrainingOverviewReportItem>,
        ISearchOperatingCenterTrainingOverview
    {
        public int? State { get; set; }
        public bool? IsOSHARequirement { get; set; }
    }
}
