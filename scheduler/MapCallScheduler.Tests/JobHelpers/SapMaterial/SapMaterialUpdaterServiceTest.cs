using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallScheduler.JobHelpers.SapMaterial;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SapMaterial
{
    [TestClass]
    public class SapMaterialUpdaterServiceTest : SapEntityUpdaterServiceTestBase<SapMaterialFileRecord, ISapMaterialFileParser, Material, IRepository<Material>, SapMaterialUpdaterService>
    {
        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);

            i.For<IPlanningPlantRepository>().Use<PlanningPlantRepository>();
        }

        [TestMethod]
        public void TestProcessActivatesMaterialIfDeletionFlagIsFalse()
        {
            var file = SetupFileAndRecords(
                new SapMaterialFileRecord {
                    PartNumber = "123",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = 6.66m
                });
            GetEntityFactory<Material>().Create(new {
                PartNumber = "123",
                IsActive = false
            });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("123", entity.PartNumber);
            Assert.AreEqual("blah", entity.Description);
            Assert.AreEqual("stone", entity.UnitOfMeasure);
            Assert.IsTrue(entity.IsActive);
        }

        [TestMethod]
        public void TestProcessAddsOperatingCenterStockedMaterialIfDeletionFlagIsFalse()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var planningPlant = GetEntityFactory<PlanningPlant>()
               .Create(new {Code = "abc", OperatingCenter = operatingCenter});
            var file = SetupFileAndRecords(
                new SapMaterialFileRecord {
                    PartNumber = "123",
                    Plant = "abc",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = 6.66m
                });
            GetEntityFactory<Material>().Create(new {
                PartNumber = "123"
            });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.IsTrue(entity.OperatingCenterStockedMaterials.Any(
                ocsm =>
                    ocsm.OperatingCenter == operatingCenter && ocsm.Material == entity &&
                    ocsm.Cost == 6.66m));
        }

        [TestMethod]
        public void TestProcessCreatesMaterialIfItDoesNotExist()
        {
            var file = SetupFileAndRecords(
                new SapMaterialFileRecord {
                    PartNumber = "123",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = 6.66m
                });

            MyAssert.CausesIncrease(() => _target.Process(file), () => Repository.GetAll().Count());

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("123", entity.PartNumber);
            Assert.AreEqual("blah", entity.Description);
            Assert.AreEqual("stone", entity.UnitOfMeasure);
            Assert.IsTrue(entity.IsActive);
        }

        [TestMethod]
        public void TestProcessDoesNotDuplicateMaterialsIfSameNewMaterialIsAddedToDifferentOperatingCentersInTheSameFile()
        {
            var file = SetupFileAndRecords(
                new SapMaterialFileRecord
                {
                    PartNumber = "123",
                    Plant = "abc",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = 6.66m,
                    DeletionFlag = false
                }, new SapMaterialFileRecord
                {
                    PartNumber = "123",
                    Plant = "cba",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = 6.66m,
                    DeletionFlag = false
                });
            var oc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var oc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pp1 = GetEntityFactory<PlanningPlant>().Create(new {Code = "abc", OperatingCenter = oc1});
            var pp2 = GetEntityFactory<PlanningPlant>().Create(new {Code = "cba", OperatingCenter = oc2});

            MyAssert.CausesIncrease(() => _target.Process(file), () => Repository.GetAll().Count(), 1);

            var entity = Repository.GetAll().ToList().Last();

            var opCenters = entity.OperatingCenterStockedMaterials.Map(ocsm => ocsm.OperatingCenter);
            Assert.IsTrue(opCenters.Contains(oc1));
            Assert.IsTrue(opCenters.Contains(oc2));
        }

        [TestMethod]
        public void TestProcessRemovesOperatingCenterStockedMaterialIfDeletionFlagIsTrue()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var planningPlant = GetEntityFactory<PlanningPlant>()
               .Create(new {Code = "abc", OperatingCenter = operatingCenter});
            var file = SetupFileAndRecords(
                new SapMaterialFileRecord
                {
                    PartNumber = "123",
                    Plant = "abc",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = 6.66m,
                    DeletionFlag = true
                });
            var material = GetEntityFactory<Material>().Create(new {PartNumber = "123"});
            material.OperatingCenterStockedMaterials.Add(
                new OperatingCenterStockedMaterial {Material = material, OperatingCenter = operatingCenter});
            Session.Save(material);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(0, entity.OperatingCenters.Count());
        }

        [TestMethod]
        public void TestProcessThrowsExceptionIfPlanningPlantCannotBeFoundByCode()
        {
            foreach (var delete in new[] { true, false })
            {
                var file = SetupFileAndRecords(
                    new SapMaterialFileRecord
                    {
                        PartNumber = "123",
                        Plant = "abc",
                        Description = "blah",
                        UnitOfMeasure = "stone",
                        Cost = 6.66m,
                        DeletionFlag = delete
                    });

                MyAssert.Throws<ArgumentException>(() =>
                        _target.Process(file));
            }
        }

        [TestMethod]
        public void TestProcessThrowsExceptionIfPlanningPlantDoesNotHaveOperatingCenter()
        {
            var planningPlant = GetEntityFactory<PlanningPlant>().Create(new {Code = "abc"});
            planningPlant.OperatingCenter = null;
            Session.Save(planningPlant);
            GetEntityFactory<Material>().Create(new {PartNumber = "123"});

            foreach (var delete in new[] { true, false })
            {
                var file = SetupFileAndRecords(
                    new SapMaterialFileRecord
                    {
                        PartNumber = "123",
                        Plant = "abc",
                        Description = "blah",
                        UnitOfMeasure = "stone",
                        Cost = 6.66m,
                        DeletionFlag = delete
                    });

                MyAssert.Throws<ArgumentException>(() =>
                        _target.Process(file));
            }
        }

        [TestMethod]
        public void TestProcessUpdatesMaterialCostIfItExistsInThePlanningPlant()
        {
            var plantCode = "D216";
            var cost = 6.66m;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ1"});
            var planningPlant = GetEntityFactory<PlanningPlant>()
               .Create(new {Code = plantCode, OperatingCenter = operatingCenter});
            var file = SetupFileAndRecords(
                new SapMaterialFileRecord
                {
                    PartNumber = "123",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = cost,
                    Plant = plantCode
                });
            var material = GetEntityFactory<Material>().Create(new {
                PartNumber = "123",
                IsActive = true
            });
            material.OperatingCenterStockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = operatingCenter, Material = material, Cost = 0m });
            Session.Save(material);
            material = Session.Load<Material>(material.Id);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(cost, entity.OperatingCenterStockedMaterials.First().Cost);
        }

        [TestMethod]
        public void TestProcessUpdatesMaterialIfItDoesExist()
        {
            var file = SetupFileAndRecords(
                new SapMaterialFileRecord
                {
                    PartNumber = "123",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = 6.66m
                });
            GetEntityFactory<Material>().Create(new {
                PartNumber = "123",
                IsActive = true
            });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("123", entity.PartNumber);
            Assert.AreEqual("blah", entity.Description);
            Assert.AreEqual("stone", entity.UnitOfMeasure);
            Assert.IsTrue(entity.IsActive);
        }

        [TestMethod]
        public void TestProcessDoesNotAddMaterialToOperatingCenterIfDeletionFlagIsFalse()
        {
            var plantCode = "D216";
            var cost = 6.66m;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ1"});
            var planningPlant = GetEntityFactory<PlanningPlant>()
               .Create(new {Code = plantCode, OperatingCenter = operatingCenter});
            var file = SetupFileAndRecords(
                new SapMaterialFileRecord
                {
                    PartNumber = "123",
                    Description = "blah",
                    UnitOfMeasure = "stone",
                    Cost = cost,
                    Plant = plantCode,
                    DeletionFlag = true
                });
            GetEntityFactory<Material>().Create(new {
                PartNumber = "123",
                IsActive = true
            });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(0, entity.OperatingCenterStockedMaterials.Count);
        }
    }
}
