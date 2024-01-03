using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.JobHelpers.SapProductionWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SapProductionWorkOrder
{
    [TestClass]
    public class SapScheduledProductionWorkOrderServiceTest
    {
        #region Private Members 

        private Mock<ISAPCreatePreventiveWorkOrderRepository> _remoteRepo;
        private Mock<IRepository<ProductionWorkOrder>> _localRepo;
        private Mock<ILog> _log;
        private SapScheduledProductionWorkOrderService _target;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        private Mock<IRepository<PlanningPlant>> _planningPlantRepository;
        private Mock<IRepository<Equipment>> _equipmentRepository;
        private Mock<IRepository<Coordinate>> _coordinateRepository;
        private Mock<IRepository<ProductionWorkOrderPriority>> _workOrderPriorityRepository;
        private Mock<IRepository<WorkOrderPurpose>> _workOrderPurposeRepository;
        private Mock<IRepository<ProductionWorkDescription>> _productionWorkDescriptionRepository;
        private Mock<IRepository<OrderType>> _orderTypeRepository;
        private Mock<IRepository<PlantMaintenanceActivityType>> _plantMaintenanceActivityTypeRepository;
        private Mock<IRepository<ProductionSkillSet>> _productionSkillSetRepository;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();

            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_remoteRepo = new Mock<ISAPCreatePreventiveWorkOrderRepository>()).Object);
            _container.Inject((_localRepo = new Mock<IRepository<ProductionWorkOrder>>()).Object);
            _container.Inject((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);

            _container.Inject((_planningPlantRepository = new Mock<IRepository<PlanningPlant>>()).Object); 
            _container.Inject((_equipmentRepository = new Mock<IRepository<Equipment>>()).Object);
            _container.Inject((_coordinateRepository = new Mock<IRepository<Coordinate>>()).Object);
            _container.Inject((_workOrderPriorityRepository = new Mock<IRepository<ProductionWorkOrderPriority>>()).Object);
            _container.Inject((_workOrderPurposeRepository = new Mock<IRepository<WorkOrderPurpose>>()).Object);
            _container.Inject((_productionWorkDescriptionRepository = new Mock<IRepository<ProductionWorkDescription>>()).Object);
            _container.Inject((_orderTypeRepository = new Mock<IRepository<OrderType>>()).Object);
            _container.Inject((_plantMaintenanceActivityTypeRepository = new Mock<IRepository<PlantMaintenanceActivityType>>()).Object);
            _container.Inject((_productionSkillSetRepository = new Mock<IRepository<ProductionSkillSet>>()).Object);

            _target = _container.GetInstance<SapScheduledProductionWorkOrderService>();

        }

        #endregion

        [TestMethod]
        public void TestProcessDoesNotErrorWhenOrderIsReturnedWithoutOrderNumber()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var expected = new SAPCreatePreventiveWorkOrder { OrderNumber = null, PlanningPlant = "", Equipment = "1" };
            var results = new SAPCreatePreventiveWorkOrderCollection();
            results.Items.Add(expected);

            _localRepo.Setup(x =>
                           x.Any(It.Is<Expression<Func<ProductionWorkOrder, bool>>>(
                               z => z.Compile().Invoke(new ProductionWorkOrder { SAPWorkOrder = expected.OrderNumber }))))
                      .Returns(false);

            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            _target.Process();

            _localRepo.Verify(x => x.Save(It.IsAny<ProductionWorkOrder>()), Times.Never);

            _log.Verify(x => x.Info(SapScheduledProductionWorkOrderService.NO_ORDER_NUMBER), Times.Once);
        }

        [TestMethod]    
        public void TestProcessDownloadsOrdersAndDoesNotCreateIfPlanningPlantNullAndLogsInfo()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var expected = new SAPCreatePreventiveWorkOrder { OrderNumber = "123456", PlanningPlant = "", Equipment = "1" };
            var results = new SAPCreatePreventiveWorkOrderCollection();
            results.Items.Add(expected);

            _localRepo.Setup(x =>
                    x.Any(It.Is<Expression<Func<ProductionWorkOrder, bool>>>(
                        z => z.Compile().Invoke(new ProductionWorkOrder { SAPWorkOrder = expected.OrderNumber }))))
                .Returns(false);

            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            _target.Process();

            _localRepo.Verify(x => x.Save(It.IsAny<ProductionWorkOrder>()), Times.Never);
            var err1 = $"Production Work Order Error: Unable to map order {expected.OrderNumber} for pp/eq: {expected.PlanningPlant}/{expected.Equipment} Child Eq: ";

            _log.Verify(x => x.Info(err1), Times.Once);
        }

        [TestMethod]
        public void TestProcessDownloadsOrdersAndDoesNotCreateIfOrderAlreadyExists()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var results = new SAPCreatePreventiveWorkOrderCollection();
            var existing = new SAPCreatePreventiveWorkOrder { OrderNumber = "123457", PlanningPlant = "P101", Equipment = "2" };

            results.Items.Add(existing);

            _localRepo.Setup(x =>
                    x.Any(It.Is<Expression<Func<ProductionWorkOrder, bool>>>(
                        z => z.Compile().Invoke(new ProductionWorkOrder { SAPWorkOrder = existing.OrderNumber }))))
                .Returns(true);

            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            _target.Process();

            _localRepo.Verify(x => x.Save(It.IsAny<ProductionWorkOrder>()), Times.Never);
            var err2 = $"Production Work Order Error: Unable to map order {existing.OrderNumber} for pp/eq: {existing.PlanningPlant}/{existing.Equipment}";
            _log.Verify(x => x.Info(err2), Times.Never);
        }

        [TestMethod]
        public void TestProcessDownloadsOrdersAndCreatesNewOnesLocallyAndDoesNotCreateIfTheyAlreadyExists()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var priorities = new[] {new ProductionWorkOrderPriority()};
            var purposes = new[] {new WorkOrderPurpose()};
            var plants = new[] {new PlanningPlant{OperatingCenter = new OperatingCenter()}};
            var equipment = new[] {new Equipment { Coordinate = new Coordinate() }};
            _workOrderPurposeRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrderPurpose, bool>>>())).Returns(purposes.AsQueryable());
            _workOrderPriorityRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkOrderPriority, bool>>>())).Returns(priorities.AsQueryable());
            _planningPlantRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanningPlant, bool>>>())).Returns(plants.AsQueryable());
            _equipmentRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Equipment, bool>>>())).Returns(equipment.AsQueryable());
            
            var results = new SAPCreatePreventiveWorkOrderCollection();
            var expected = new SAPCreatePreventiveWorkOrder {
                OrderNumber = "123456", PlanningPlant = "P101", Equipment = "1", SAPNotificationNumber = "12345",
                BasicStart = "20171016", BasicFinish = "20171231"
            };
            var existing = new SAPCreatePreventiveWorkOrder { OrderNumber = "123457", PlanningPlant = "P101", Equipment = "2", SAPNotificationNumber = "12346" };
        
            results.Items.Add(expected);
            results.Items.Add(existing);

            _localRepo.Setup(x =>
                    x.Where(It.Is<Expression<Func<ProductionWorkOrder, bool>>>(
                        z => z.Compile().Invoke(new ProductionWorkOrder {SAPWorkOrder = existing.OrderNumber}))))
                .Returns(new[]{ new ProductionWorkOrder { SAPWorkOrder = existing.OrderNumber } }.AsQueryable());
            
            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            // Act
            _target.Process();

            // Assert
            // Valid Order gets created
            _localRepo.Verify(x => x.Save(It.Is<ProductionWorkOrder>(pwo => 
                pwo.SAPWorkOrder == expected.OrderNumber 
                && pwo.SAPNotificationNumber == long.Parse(expected.SAPNotificationNumber)
                && pwo.BasicStart.Value == DateTime.ParseExact(expected.BasicStart, "yyyyMMdd", CultureInfo.InvariantCulture)
                && pwo.BasicFinish.Value == DateTime.ParseExact(expected.BasicFinish, "yyyyMMdd", CultureInfo.InvariantCulture)
                )), Times.Once);

            // Existing order does not get created
            _localRepo.Verify(x => x.Save(It.Is<ProductionWorkOrder>(pwo => pwo.SAPWorkOrder == existing.OrderNumber)), Times.Never);

        }

        [TestMethod]
        public void TestProcessDownloadsOrdersAndDoesNotCreateLocallyIfNoEquipmentMatchesInMapCall()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            //Annoying setup
            var priorities = new[] { new ProductionWorkOrderPriority() };
            var purposes = new[] { new WorkOrderPurpose() };
            var plants = new[] { new PlanningPlant { OperatingCenter = new OperatingCenter() } };
            var equipment = new[] { new Equipment { Coordinate = new Coordinate() } };
            var equipment2 = new[] {new Equipment {SAPEquipmentId = 31}};
            equipment2[0].SetPropertyValueByName("Id", 4);

            _workOrderPurposeRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrderPurpose, bool>>>())).Returns(purposes.AsQueryable());
            _workOrderPriorityRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkOrderPriority, bool>>>())).Returns(priorities.AsQueryable());
            _planningPlantRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanningPlant, bool>>>())).Returns(plants.AsQueryable());
            //_equipmentRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Equipment, bool>>>())).Returns(equipment.AsQueryable());
            _equipmentRepository.Setup(x => x.Where(It.Is<Expression<Func<Equipment, bool>>>(z => z.Compile().Invoke(new Equipment { SAPEquipmentId = 1 } )))).Returns(equipment.AsQueryable());
            _equipmentRepository.Setup(x => x.Where(It.Is<Expression<Func<Equipment, bool>>>(z => z.Compile().Invoke(new Equipment { SAPEquipmentId = 31 })))).Returns(equipment2.AsQueryable());

            var results = new SAPCreatePreventiveWorkOrderCollection();
            var expected = new SAPCreatePreventiveWorkOrder
            {
                OrderNumber = "123456",
                PlanningPlant = "P101",
                SAPNotificationNumber = "12345",
                BasicStart = "20171016",
                BasicFinish = "20171231"
            };
            var objectList = new List<SAPWorkOrderObjectList>();
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "1" });
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "31" });
            expected.SapWorkOrderObjectList = objectList.ToArray();

            var existing = new SAPCreatePreventiveWorkOrder { OrderNumber = "123457", PlanningPlant = "P101", SAPNotificationNumber = "12346" };
            existing.SapWorkOrderObjectList = objectList.ToArray();

            results.Items.Add(expected);
            results.Items.Add(existing);

            _localRepo.Setup(x =>
                    x.Where(It.Is<Expression<Func<ProductionWorkOrder, bool>>>(
                        z => z.Compile().Invoke(new ProductionWorkOrder { SAPWorkOrder = existing.OrderNumber }))))
                .Returns(new[] { new ProductionWorkOrder { SAPWorkOrder = existing.OrderNumber } }.AsQueryable());

            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            _target.Process();

            _localRepo.Verify(x => x.Save(It.Is<ProductionWorkOrder>(pwo =>
                pwo.SAPWorkOrder == expected.OrderNumber
                && pwo.SAPNotificationNumber == long.Parse(expected.SAPNotificationNumber)
                && pwo.BasicStart.Value == DateTime.ParseExact(expected.BasicStart, "yyyyMMdd", CultureInfo.InvariantCulture)
                && pwo.BasicFinish.Value == DateTime.ParseExact(expected.BasicFinish, "yyyyMMdd", CultureInfo.InvariantCulture)
                )), Times.Once);

            _localRepo.Verify(x => x.Save(It.Is<ProductionWorkOrder>(pwo => pwo.SAPWorkOrder == existing.OrderNumber)), Times.Never);

        }

        [TestMethod]
        public void TestProcessAddsAllEquipmentInTheObjectListAndGrabsFacilityFromChildObject()
        {
            // ARRANGE
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var priorities = new[] { new ProductionWorkOrderPriority() };
            var purposes = new[] { new WorkOrderPurpose() };
            var plants = new[] { new PlanningPlant { OperatingCenter = new OperatingCenter() } };
            var facility = new Facility();
            var equipment2 = new[] { new Equipment { SAPEquipmentId = 31, Facility = facility } };
            _workOrderPurposeRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrderPurpose, bool>>>())).Returns(purposes.AsQueryable());
            _workOrderPriorityRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkOrderPriority, bool>>>())).Returns(priorities.AsQueryable());
            _planningPlantRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanningPlant, bool>>>())).Returns(plants.AsQueryable());
            _equipmentRepository.Setup(x => x.Where(It.Is<Expression<Func<Equipment, bool>>>(z => z.Compile().Invoke(new Equipment { SAPEquipmentId = 31 })))).Returns(equipment2.AsQueryable());

            var results = new SAPCreatePreventiveWorkOrderCollection();
            var objectList = new List<SAPWorkOrderObjectList>();
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "1" });
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "31" });
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "231" });
            
            var expected = new SAPCreatePreventiveWorkOrder {
                OrderNumber = "123456",
                PlanningPlant = "P101",
                SAPNotificationNumber = "12345",
                BasicStart = "20171016",
                BasicFinish = "20171231",
                SapWorkOrderObjectList = objectList.ToArray()
            };
            results.Items.Add(expected);
            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            // ACT
            _target.Process();

            // ASSERT
            _localRepo.Verify(x => x.Save(It.Is<ProductionWorkOrder>(pwo => pwo.SAPWorkOrder == expected.OrderNumber && pwo.Facility == facility && pwo.Equipments.Count == 3)), 
                Times.Once);
        }

        [TestMethod]
        public void TestProcessAddsSingleEquipmentFromTheObjectListWithIsParentProperlySet()
        {
            // ARRANGE
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var priorities = new[] { new ProductionWorkOrderPriority() };
            var purposes = new[] { new WorkOrderPurpose() };
            var plants = new[] { new PlanningPlant { OperatingCenter = new OperatingCenter() } };
            var facility = new Facility();
            var equipment2 = new[] { new Equipment { SAPEquipmentId = 31, Facility = facility } };
            _workOrderPurposeRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrderPurpose, bool>>>())).Returns(purposes.AsQueryable());
            _workOrderPriorityRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkOrderPriority, bool>>>())).Returns(priorities.AsQueryable());
            _planningPlantRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanningPlant, bool>>>())).Returns(plants.AsQueryable());
            _equipmentRepository.Setup(x => x.Where(It.Is<Expression<Func<Equipment, bool>>>(z => z.Compile().Invoke(new Equipment { SAPEquipmentId = 31 })))).Returns(equipment2.AsQueryable());
            var results = new SAPCreatePreventiveWorkOrderCollection();
            var objectList = new List<SAPWorkOrderObjectList>();
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "31" });

            var expected = new SAPCreatePreventiveWorkOrder {
                OrderNumber = "123456",
                PlanningPlant = "P101",
                SAPNotificationNumber = "12345",
                BasicStart = "20171016",
                BasicFinish = "20171231",
                Equipment = "31",
                SapWorkOrderObjectList = objectList.ToArray()
            };

            results.Items.Add(expected);
            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            // ACT
            _target.Process();

            // ASSERT
            _localRepo.Verify(
                x => x.Save(It.Is<ProductionWorkOrder>(pwo =>
                    pwo.SAPWorkOrder == expected.OrderNumber && pwo.Facility == facility &&
                    pwo.Equipments.Count(e => e.IsParent == true) == 1)), Times.Once);
        }

        [TestMethod]
        public void TestProcessReturnsProductionWorkDescriptionIfItAlreadyExists()
        {
            // ARRANGE
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var priorities = new[] { new ProductionWorkOrderPriority() };
            var purposes = new[] { new WorkOrderPurpose() };
            var plants = new[] { new PlanningPlant { OperatingCenter = new OperatingCenter() } };
            var facility = new Facility();
            var equipment2 = new[] { new Equipment { SAPEquipmentId = 31, Facility = facility } };
            var workDescription = new[] {new ProductionWorkDescription{Description = "Testy"}};
            _workOrderPurposeRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrderPurpose, bool>>>())).Returns(purposes.AsQueryable());
            _workOrderPriorityRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkOrderPriority, bool>>>())).Returns(priorities.AsQueryable());
            _planningPlantRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanningPlant, bool>>>())).Returns(plants.AsQueryable());
            _productionWorkDescriptionRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkDescription, bool>>>())).Returns(workDescription.AsQueryable);
            _equipmentRepository.Setup(x => x.Where(It.Is<Expression<Func<Equipment, bool>>>(z => z.Compile().Invoke(new Equipment { SAPEquipmentId = 31 })))).Returns(equipment2.AsQueryable());

            var results = new SAPCreatePreventiveWorkOrderCollection();
            var objectList = new List<SAPWorkOrderObjectList>();
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "1" });
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "31" });
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "231" });

            var expected = new SAPCreatePreventiveWorkOrder
            {
                OrderNumber = "123456",
                PlanningPlant = "P101",
                SAPNotificationNumber = "12345",
                BasicStart = "20171016",
                BasicFinish = "20171231",
                SapWorkOrderObjectList = objectList.ToArray()
            };
            results.Items.Add(expected);
            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            // ACT
            _target.Process();

            // ASSERT
            _localRepo.Verify(x => x.Save(It.Is<ProductionWorkOrder>(pwo => pwo.SAPWorkOrder == expected.OrderNumber && pwo.Facility == facility && pwo.Equipments.Count == 3 && pwo.ProductionWorkDescription.Description == workDescription[0].Description)),
                Times.Once);
            _productionWorkDescriptionRepository.Verify(x => x.Save(It.IsAny<ProductionWorkDescription>()), Times.Never);
        }

        [TestMethod]
        public void TestProcessDoesNotSaveWorkDescriptionWhenEquipmentNull()
        {
            // ARRANGE
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var priorities = new[] { new ProductionWorkOrderPriority() };
            var purposes = new[] { new WorkOrderPurpose() };
            var plants = new[] { new PlanningPlant { OperatingCenter = new OperatingCenter() } };
            var facility = new Facility();
            _workOrderPurposeRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrderPurpose, bool>>>())).Returns(purposes.AsQueryable());
            _workOrderPriorityRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkOrderPriority, bool>>>())).Returns(priorities.AsQueryable());
            _planningPlantRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanningPlant, bool>>>())).Returns(plants.AsQueryable());

            var results = new SAPCreatePreventiveWorkOrderCollection();
            var objectList = new List<SAPWorkOrderObjectList>();
            objectList.Add(new SAPWorkOrderObjectList());
            objectList.Add(new SAPWorkOrderObjectList());
            objectList.Add(new SAPWorkOrderObjectList());

            var expected = new SAPCreatePreventiveWorkOrder
            {
                OrderNumber = "123456",
                PlanningPlant = "P101",
                SAPNotificationNumber = "12345",
                BasicStart = "20171016",
                BasicFinish = "20171231",
                SapWorkOrderObjectList = objectList.ToArray()
            };
            results.Items.Add(expected);
            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            // ACT
            _target.Process();

            // ASSERT
            _localRepo.Verify(x => x.Save(It.Is<ProductionWorkOrder>(pwo => pwo.SAPWorkOrder == expected.OrderNumber && pwo.Facility == facility && pwo.Equipments.Count == 3 && pwo.ProductionWorkDescription == null)),
                Times.Never);
            _productionWorkDescriptionRepository.Verify(x => x.Save(It.IsAny<ProductionWorkDescription>()), Times.Never);
        }

        [TestMethod]
        public void TestProcessCreatesNewWorkDescriptionWhenOneDoesntExist()
        {
            // ARRANGE
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var priorities = new[] { new ProductionWorkOrderPriority() };
            var purposes = new[] { new WorkOrderPurpose() };
            var plants = new[] { new PlanningPlant { OperatingCenter = new OperatingCenter() } };
            var facility = new Facility();
            var equipment2 = new[] { new Equipment { SAPEquipmentId = 31, Facility = facility } };
            //var workDescription = new[] { new ProductionWorkDescription { Description = "Testy" } };
            //_workOrderPurposeRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrderPurpose, bool>>>())).Returns(purposes.AsQueryable());
            _workOrderPriorityRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkOrderPriority, bool>>>())).Returns(priorities.AsQueryable());
            _planningPlantRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanningPlant, bool>>>())).Returns(plants.AsQueryable());
           // _productionWorkDescriptionRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProductionWorkDescription, bool>>>())).Returns(workDescription.AsQueryable);
            _equipmentRepository.Setup(x => x.Where(It.Is<Expression<Func<Equipment, bool>>>(z => z.Compile().Invoke(new Equipment { SAPEquipmentId = 31 })))).Returns(equipment2.AsQueryable());

            var results = new SAPCreatePreventiveWorkOrderCollection();
            var objectList = new List<SAPWorkOrderObjectList>();
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "1" });
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "31" });
            objectList.Add(new SAPWorkOrderObjectList { SAPEquipmentNumber = "231" });

            var expected = new SAPCreatePreventiveWorkOrder
            {
                OrderNumber = "123456",
                PlanningPlant = "P101",
                SAPNotificationNumber = "12345",
                BasicStart = "20171016",
                BasicFinish = "20171231",
                SapWorkOrderObjectList = objectList.ToArray()
            };
            results.Items.Add(expected);
            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            // ACT
            _target.Process();

            // ASSERT
            _localRepo.Verify(x => x.Save(It.Is<ProductionWorkOrder>(pwo => pwo.SAPWorkOrder == expected.OrderNumber && pwo.Facility == facility && pwo.Equipments.Count == 3)),
                Times.Once);
            _productionWorkDescriptionRepository.Verify(x => x.Save(It.IsAny<ProductionWorkDescription>()), Times.Once);
        }

    }
}
