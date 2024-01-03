using System;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Testing.Data;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EquipmentTest : InMemoryDatabaseTest<Equipment>
    {
        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
        }

        [TestInitialize]
        public void TestInitialize()
        {
            CreateTablesForBug1891.FIELD_TYPES.Each(o =>
                GetEntityFactory<EquipmentCharacteristicFieldType>().Create(o));
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(TownFactory).Assembly);
        }

        #endregion

        [TestMethod]
        public void TestIsGeneratorReturnsCorrectValues()
        {
            var equipmentPurpose = new EquipmentPurpose();
            var equipmentLifespan = new EquipmentLifespan();
            equipmentLifespan.SetPropertyValueByName("Id", EquipmentLifespan.Indices.GENERATOR);

            var target = new Equipment();
            Assert.IsFalse(target.IsGenerator);

            target.EquipmentPurpose = equipmentPurpose;
            Assert.IsFalse(target.IsGenerator);

            equipmentPurpose.EquipmentLifespan = new EquipmentLifespan();
            Assert.IsFalse(target.IsGenerator);

            equipmentPurpose.EquipmentLifespan = equipmentLifespan;
            Assert.IsTrue(target.IsGenerator);
        }

        [TestMethod]
        public void TestToStringReturnsIdentifier()
        {
            var facility = new Facility {
                Id = 1, OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJSB"},
                FacilityName = "Test Facility"
            };
            var expected = "NJSB-1-TEST-1";
            var eq = new Equipment
                {Id = 1, Facility = facility, EquipmentPurpose = new EquipmentPurpose {Abbreviation = "TEST"}};

            Assert.AreEqual(expected, eq.ToString());
        }

        [TestMethod]
        public void TestDisplayDoesntErrorOnNullFacility()
        {
            var target = new Equipment();

            Assert.IsNotNull(target.Display);
        }

        [TestMethod]
        public void TestDeletingEquipmentWithCharacteristicsAlsoDeletesThoseCharacteristics()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = equipmentType,
                EquipmentPurpose = equipmentPurpose
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "String"),
                FieldName = "SomeField"
            });
            equipment.Characteristics.Add(GetEntityFactory<EquipmentCharacteristic>().Build(new {
                Equipment = equipment,
                Field = field,
                Value = "foo"
            }));
            Session.SaveOrUpdate(equipment);

            Session.Flush();
            Session.Clear();

            equipment = Session.Load<Equipment>(equipment.Id);

            MyAssert.CausesDecrease(() => _container.GetInstance<RepositoryBase<Equipment>>().Delete(equipment),
                () => _container.GetInstance<RepositoryBase<EquipmentCharacteristic>>().GetAll().Count());
        }

        [TestMethod]
        public void TestLocalizedRiskOfFailureText()
        {
            var equipment = new Equipment();

            Assert.IsNull(equipment.LocalizedRiskOfFailureText);
            for (var i = 1; i < 10; i++)
            {
                equipment.LocalizedRiskOfFailure = i;
                if (i <= 2)
                    Assert.AreEqual(Equipment.Risks.LOW, equipment.LocalizedRiskOfFailureText);
                if (i > 2 && i < 6)
                    Assert.AreEqual(Equipment.Risks.MEDIUM, equipment.LocalizedRiskOfFailureText);
                if (i >= 6)
                    Assert.AreEqual(Equipment.Risks.HIGH, equipment.LocalizedRiskOfFailureText);
            }
        }

        [TestMethod]
        public void TestStandardOperatingProcedureDocumentIdReturnsCorrectId()
        {
            var entity = new Equipment();
            var documentType = new DocumentType {Id = DocumentType.Indices.STANDARD_OPERATING_PROCEDURE};
            var document = new Document {CreatedAt = DateTime.Now, Id = 5, DocumentType = documentType};
            var equipmentDocument = new EquipmentDocument {Id = 4, Document = document, DocumentType = documentType};
            entity.EquipmentDocuments.Add(equipmentDocument);

            Assert.AreEqual(document.Id, entity.StandardOperatingProcedureDocumentId);
        }

        [TestMethod]
        public void TestCanBeCopiedReturnsTrueForCorrectStatuses()
        {
            foreach (var status in typeof(EquipmentStatus.Indices).GetFields())
            {
                var equipmentStatusId = (int)status.GetValue(status);
                var equipment = new MapCall.Common.Model.Entities.Equipment
                    {EquipmentStatus = new EquipmentStatus {Id = equipmentStatusId}};

                if (EquipmentStatus.CanBeCopiedStatuses.Contains(equipmentStatusId))
                {
                    Assert.IsTrue(equipment.CanBeCopied);
                }
                else
                {
                    Assert.IsFalse(equipment.CanBeCopied);
                }
            }
        }

        [TestMethod]
        public void TestRemainingUsefulLifeSubtractsAgeFromEstimatedLifespan()
        {
            var target = new Equipment {
                EquipmentPurpose = new EquipmentPurpose {
                    EquipmentLifespan = new EquipmentLifespan {
                        EstimatedLifespan = 20
                    }
                },
                DateInstalled = _now.AddYears(-10)
            };

            _container.BuildUp(target);

            Assert.AreEqual(10, target.RemainingUsefulLife);
        }

        [TestMethod]
        public void TestRemainingUsefulLifeAllowsNegative()
        {
            var target = new Equipment {
                EquipmentPurpose = new EquipmentPurpose {
                    EquipmentLifespan = new EquipmentLifespan {
                        EstimatedLifespan = 20
                    }
                },
                DateInstalled = _now.AddYears(-30)
            };

            _container.BuildUp(target);

            Assert.AreEqual(-10, target.RemainingUsefulLife);
        }

        [TestMethod]
        public void TestRemainingUsefulLifeReturnsNullWhenEquipmentPurposeIsNull()
        {
            var target = new Equipment {
                DateInstalled = _now.AddYears(-10)
            };

            _container.BuildUp(target);

            Assert.IsNull(target.RemainingUsefulLife);
        }

        [TestMethod]
        public void TestRemainingUsefulLifeReturnsNullWhenEquipmentPurposeHasNoEquipmentLifespan()
        {
            var target = new Equipment {
                EquipmentPurpose = new EquipmentPurpose(),
                DateInstalled = _now.AddYears(-10)
            };

            _container.BuildUp(target);

            Assert.IsNull(target.RemainingUsefulLife);
        }

        [TestMethod]
        public void TestRemainingUsefulLifeReturnsNullWhenEquipmentLifespanEstimatedLifespanIsNull()
        {
            var target = new Equipment {
                EquipmentPurpose = new EquipmentPurpose {
                    EquipmentLifespan = new EquipmentLifespan()
                },
                DateInstalled = _now.AddYears(-10)
            };

            _container.BuildUp(target);

            Assert.IsNull(target.RemainingUsefulLife);
        }

        [TestMethod]
        public void TestRemainingUsefulLifeReturnsNullWhenDateInstalledIsNull()
        {
            var target = new Equipment {
                EquipmentPurpose = new EquipmentPurpose {
                    EquipmentLifespan = new EquipmentLifespan {
                        EstimatedLifespan = 20
                    }
                }
            };

            _container.BuildUp(target);

            Assert.IsNull(target.RemainingUsefulLife);
        }
    }
}
