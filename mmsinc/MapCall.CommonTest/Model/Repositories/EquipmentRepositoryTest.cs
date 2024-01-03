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
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class EquipmentRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Equipment, EquipmentRepository, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionEquipment;

        #endregion

        #region Private Members

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEquipmentRepository>().Use<EquipmentRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
            e.For<ISensorMeasurementTypeRepository>().Use<SensorMeasurementTypeRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Tests

        #region Linq

        [TestMethod]
        public void TestLinqDoesNotReturnEquipmentFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var facility1 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr1});
            var facility2 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr2});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE});
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

            var equipment1 = GetFactory<EquipmentFactory>().Create(new {Facility = facility1});
            var equipment2 = GetFactory<EquipmentFactory>().Create(new {Facility = facility2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<EquipmentRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(equipment1));
            Assert.IsFalse(result.Contains(equipment2));
        }

        [TestMethod]
        public void TestLinqReturnsAllEquipmentIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var facility1 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr1});
            var facility2 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr2});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE});
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

            var equipment1 = GetFactory<EquipmentFactory>().Create(new {Facility = facility1});
            var equipment2 = GetFactory<EquipmentFactory>().Create(new {Facility = facility2});

            Repository = _container.GetInstance<EquipmentRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(equipment1));
            Assert.IsTrue(result.Contains(equipment2));
        }

        #endregion

        #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnEquipmentFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var facility1 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr1});
            var facility2 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr2});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE});
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

            var equipment1 = GetEntityFactory<Equipment>().Create(new {Facility = facility1});
            var equipment2 = GetEntityFactory<Equipment>().Create(new {Facility = facility2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<EquipmentRepository>();
            var model = new EmptySearchSet<Equipment>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(equipment1));
            Assert.IsFalse(result.Contains(equipment2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllEquipmentIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var facility1 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr1});
            var facility2 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr2});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE});
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

            var equipment1 = GetFactory<EquipmentFactory>().Create(new {Facility = facility1});
            var equipment2 = GetFactory<EquipmentFactory>().Create(new {Facility = facility2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<EquipmentRepository>();
            var model = new EmptySearchSet<Equipment>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(equipment1));
            Assert.IsTrue(result.Contains(equipment2));
        }

        #endregion

        #region SearchMethods

        [TestMethod]
        public void TestSearchLinkedEquipment()
        {
            var eq1 = GetEntityFactory<Equipment>().Create();
            var eq2 = GetEntityFactory<Equipment>().Create();
            var eq3 = GetEntityFactory<Equipment>().Create();
            var eq4 = GetEntityFactory<Equipment>().Create();

            // Same Equipment Purpose and IsReplacement set to true
            eq2.EquipmentPurpose = eq1.EquipmentPurpose;
            eq2.IsReplacement = true;

            // Same Equipment Purpose and Replacement Id
            eq1.ReplacedEquipment = eq3;
            eq3.EquipmentPurpose = eq1.EquipmentPurpose;

            // Different Equipment Purpose and IsReplacement set to true
            eq4.IsReplacement = true;

            eq1.Facility.Equipment.Add(eq2);
            eq1.Facility.Equipment.Add(eq3);
            eq1.Facility.Equipment.Add(eq4);

            Repository.Save(eq1);
            var search = new TestSearchEquipment {
                Facility = eq1.Facility.Id,
                EquipmentPurpose = new[] { eq1.EquipmentPurpose.Id },
                NotEqualEntityId = eq1.Id,
                OriginalEquipmentId = eq1.ReplacedEquipment.Id
            };

            Assert.IsTrue(Repository.SearchLinkedEquipment(search).Contains(eq3));

            eq3.EquipmentPurpose = null;
            eq2.EquipmentPurpose = null;
            Repository.Save(new[] { eq2, eq3 });

            Assert.IsFalse(Repository.SearchLinkedEquipment(search).Contains(eq3));
        }
        
        [TestMethod]
        public void TestGetAllCharacteristicDropDownValuesCurrentlyInUseReturnsOnlyDropDownValuesThatAreInUse()
        {
            var eq = GetEntityFactory<Equipment>().Create();
            var eqtype = GetEntityFactory<EquipmentType>().Create();
            var dropDown = GetFactory<EquipmentCharacteristicFieldTypeFactory>().Create(new {
                DataType = "DropDown"
            });
            var field = GetFactory<EquipmentCharacteristicFieldFactory>().Create(new {
                FieldType = dropDown,
                IsActive = true,
                EquipmentType = eqtype
            });

            var dropdownValues = new [] {
                new EquipmentCharacteristicDropDownValue {
                    Field = field,
                    Value = "Unused1"
                },
                new EquipmentCharacteristicDropDownValue {
                    Field = field,
                    Value = "Unused2"
                },
                new EquipmentCharacteristicDropDownValue {
                    Field = field,
                    Value = "Used"
                },
            };
            
            field.DropDownValues.AddRange(dropdownValues);
            
            foreach (var item in dropdownValues)
            {
                Session.Save(item);
            }
            
            eq.Characteristics.AddRange(new [] {
                new EquipmentCharacteristic {
                    Equipment = eq,
                    Field = field,
                    Value = "3"
                },
            });

            Session.Save(eq);
            Session.Flush();

            var inUse = Repository.GetAllCharacteristicDropDownValuesCurrentlyInUse(field).ToList();
            
            Assert.AreEqual(1, inUse.Count);
            Assert.AreEqual(3, inUse.First());
        }

        #region Private Classes

        private class TestSearchEquipment : SearchSet<Equipment>, ISearchEquipment
        {
            public int? Facility { get; set; }
            public int[] EquipmentPurpose { get; set; }
            [Search(CanMap = false)]
            public int? OriginalEquipmentId { get; set; }
            [Search(CanMap = false)]
            public int? NotEqualEntityId { get; set; }
        }

        #endregion

        #endregion
        
        #endregion
    }
}
