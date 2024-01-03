using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class MeasurementPointEquipmentTypeRepositoryTest : InMemoryDatabaseTest<MeasurementPointEquipmentType, MeasurementPointEquipmentTypeRepository>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public Mock<IDateTimeProvider> _dateTimeProvider;
        public User _user;
        
        private readonly DateTime _today = DateTime.Parse("01/15/2030 12:00AM");

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_today));
        }
        
        [TestInitialize]
        public void InitializeTest()
        {
            _user = GetFactory<MapCall.Common.Testing.Data.AdminUserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetAllInUseOnlyReturnsRecordsBeingUsedByProductionWorkOrders()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create();
            var eq = GetFactory<EquipmentFactory>().Create(new { EquipmentType = equipmentType });
            var measurementPoints = GetFactory<MeasurementPointEquipmentTypeFactory>().CreateList(2, new { EquipmentType = equipmentType });

            equipmentType.MeasurementPoints.Add(measurementPoints[0]);
            
            var measurement = GetFactory<ProductionWorkOrderMeasurementPointValueFactory>().Create(new {
                Equipment = eq, 
                ProductionWorkOrder = pwo, 
                MeasurementPointEquipmentType = measurementPoints[0]
            });
            var pwoEq = GetFactory<ProductionWorkOrderEquipmentFactory>().Create(new {
                ProductionWorkOrder = pwo, 
                Equipment = eq
            });
            
            pwo.ProductionWorkOrderMeasurementPointValues.Add(measurement);
            pwo.Equipments.Add(pwoEq);
            eq.ProductionWorkOrderEquipment.Add(pwoEq);

            Session.Flush();

            var actual = Repository.GetAllInUse(equipmentType.Id).ToList();
            
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(measurementPoints[0].Id, actual[0]);
        }

        [TestMethod]
        public void TestIsCurrentlyInUseReturnsTrueIfTheGivenMeasurementPointIsInUse()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create();
            var eq = GetFactory<EquipmentFactory>().Create(new { EquipmentType = equipmentType });
            var measurementPoints = GetFactory<MeasurementPointEquipmentTypeFactory>().CreateList(2, new { EquipmentType = equipmentType });

            equipmentType.MeasurementPoints.Add(measurementPoints[0]);
            
            var measurement = GetFactory<ProductionWorkOrderMeasurementPointValueFactory>().Create(new {
                Equipment = eq, 
                ProductionWorkOrder = pwo, 
                MeasurementPointEquipmentType = measurementPoints[0]
            });
            var pwoEq = GetFactory<ProductionWorkOrderEquipmentFactory>().Create(new {
                ProductionWorkOrder = pwo, 
                Equipment = eq
            });
            
            pwo.ProductionWorkOrderMeasurementPointValues.Add(measurement);
            pwo.Equipments.Add(pwoEq);
            eq.ProductionWorkOrderEquipment.Add(pwoEq);

            Session.Flush();

            var actualGood = Repository.IsCurrentlyInUse(measurementPoints[0].Id, equipmentType.Id);
            var actualBad = Repository.IsCurrentlyInUse(measurementPoints[1].Id, equipmentType.Id);
            
            Assert.AreEqual(true, actualGood);
            Assert.AreEqual(false, actualBad);
        }

        #endregion
    }
}
