using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.W1V;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.W1V
{
    [TestClass]
    public class W1VRecordMapperTest
    {
        private W1VRecordMapper _target;
        private Mock<IRepository<Premise>> _premiseRepository;
        private Mock<IRepository<ServiceMaterial>> _serviceMaterialRepository;
        private Mock<IRepository<SmallMeterLocation>> _smallMeterLocationRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            var container = new Container(e => {
                _premiseRepository = e.For<IRepository<Premise>>().Mock();
                _serviceMaterialRepository = e.For<IRepository<ServiceMaterial>>().Mock();
                _smallMeterLocationRepository = e.For<IRepository<SmallMeterLocation>>().Mock();
            });

            _target = container.GetInstance<W1VRecordMapper>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _premiseRepository.VerifyAll();
            _serviceMaterialRepository.VerifyAll();
            _smallMeterLocationRepository.VerifyAll();
        }

        [TestMethod]
        public void Test_Map_SetsPremise_IfEntityPremiseIsNull()
        {
            var premiseNumber = "1234";
            var premise = new Premise {PremiseNumber = premiseNumber};
            var record = new W1VFileParser.ParsedCustomerMaterial {
                PremiseId = premiseNumber
            };
            var entity = new ShortCycleCustomerMaterial();

            _premiseRepository.Setup(x => x.Linq).Returns(new[] { premise }.AsQueryable());
            
            _target.Map(entity, record);
            
            Assert.AreSame(premise, entity.Premise);
        }

        [TestMethod]
        public void Test_Map_DoesNotSetPremise_IfEntityPremiseIsNotNull()
        {
            var premiseNumber = "1234";
            var oldPremise = new Premise();
            var record = new W1VFileParser.ParsedCustomerMaterial {
                PremiseId = premiseNumber
            };
            var entity = new ShortCycleCustomerMaterial {
                Premise = oldPremise
            };
            
            _target.Map(entity, record);
            
            Assert.AreSame(oldPremise, entity.Premise);
        }

        [TestMethod]
        public void Test_Map_SetsAssignmentStart_IfEntityAssignmentStartIsNull()
        {
            var assignmentStart = new DateTime(2018, 2, 15);
            var record = new W1VFileParser.ParsedCustomerMaterial {
                AssignmentStart = assignmentStart 
            };
            var entity = new ShortCycleCustomerMaterial {Premise = new Premise()};
            
            _target.Map(entity, record);
            
            Assert.AreEqual(assignmentStart, entity.AssignmentStart);
        }

        [TestMethod]
        public void Test_Map_DoesNotSetAssignmentStart_IfEntityAssignmentStartIsNotNull()
        {
            var oldAssignmentStart = new DateTime(2018, 2, 15);
            var record = new W1VFileParser.ParsedCustomerMaterial {
                AssignmentStart = DateTime.Now
            };
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise(),
                AssignmentStart = oldAssignmentStart 
            };
            
            _target.Map(entity, record);
            
            Assert.AreEqual(oldAssignmentStart, entity.AssignmentStart);
        }

        [TestMethod]
        public void Test_Map_SetsCustomerSideMaterial_IfRecordCustomerSideMaterialIsNotNull()
        {
            var materialName = "crud";
            var record = new W1VFileParser.ParsedCustomerMaterial {
                CustomerSideMaterial = materialName
            };
            var material = new ServiceMaterial { Description = materialName };
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise() 
            };

            _serviceMaterialRepository.Setup(r => r.Linq).Returns(new[] { material }.AsQueryable());
            
            _target.Map(entity, record);
            
            Assert.AreSame(material, entity.CustomerSideMaterial);
        }

        [TestMethod]
        public void Test_Map_DoesNotSetCustomerSideMaterial_IfRecordCustomerSideMaterialIsNull()
        {
            var materialName = "crud";
            var record = new W1VFileParser.ParsedCustomerMaterial();
            var material = new ServiceMaterial { Description = materialName };
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise(),
                CustomerSideMaterial = material
            };

            _target.Map(entity, record);
            
            Assert.AreSame(material, entity.CustomerSideMaterial);
        }

        [TestMethod]
        public void Test_Map_SetsServiceLineSize_IfRecordServiceLineSizeIsNotNull()
        {
            var size = "big";
            var record = new W1VFileParser.ParsedCustomerMaterial {
                MeterSize = size
            };
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise() 
            };

            _target.Map(entity, record);
            
            Assert.AreEqual(size, entity.ServiceLineSize);
        }

        [TestMethod]
        public void Test_Map_DoesNotSetServiceLineSize_IfRecordServiceLineSizeIsNull()
        {
            var size = "big";
            var record = new W1VFileParser.ParsedCustomerMaterial();
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise(),
                ServiceLineSize = size
            };

            _target.Map(entity, record);
            
            Assert.AreEqual(size, entity.ServiceLineSize);
        }

        [TestMethod]
        public void Test_Map_SetsTechnicalInspectedOn_IfRecordTechnicalInspectedOnIsNotNull()
        {
            var technicalInspectedOn = new DateTime(2022, 9, 28);
            var record = new W1VFileParser.ParsedCustomerMaterial {
                TechnicalInspectedOn = technicalInspectedOn.ToString("yyyyMMdd") 
            };
            var entity = new ShortCycleCustomerMaterial { Premise = new Premise() };
            
            _target.Map(entity, record);
            
            Assert.AreEqual(technicalInspectedOn, entity.TechnicalInspectedOn);
        }

        [TestMethod]
        public void Test_Map_DoesNotSetTechnicalInspectedOn_IfRecordTechnicalInspectedOnIsNull()
        {
            var technicalInspectedOn = new DateTime(2022, 9, 28);
            var record = new W1VFileParser.ParsedCustomerMaterial();
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise(),
                TechnicalInspectedOn = technicalInspectedOn
            };
            
            _target.Map(entity, record);
            
            Assert.AreEqual(technicalInspectedOn, entity.TechnicalInspectedOn);
        }

        [TestMethod]
        public void Test_Map_SetsReadingDevicePositionalLocation_IfRecordReadingDevicePositionalLocationIsNotNull()
        {
            var positionalLocationCode = "WE1";
            var record = new W1VFileParser.ParsedCustomerMaterial {
                ReadingDevicePositionalLocation = positionalLocationCode
            };
            var positionalLocation = new SmallMeterLocation { SAPCode = positionalLocationCode};
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise() 
            };

            _smallMeterLocationRepository.Setup(r => r.Linq)
                                         .Returns(new[] { positionalLocation }.AsQueryable());
            
            _target.Map(entity, record);
            
            Assert.AreSame(positionalLocation, entity.ReadingDevicePositionalLocation);
        }

        [TestMethod]
        public void Test_Map_DoesNotSetReadingDevicePositionalLocation_IfRecordReadingDevicePositionalLocationIsNull()
        {
            var positionalLocationName = "over there";
            var record = new W1VFileParser.ParsedCustomerMaterial();
            var positionalLocation = new SmallMeterLocation { Description = positionalLocationName };
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise(),
                ReadingDevicePositionalLocation = positionalLocation
            };

            _target.Map(entity, record);
            
            Assert.AreSame(positionalLocation, entity.ReadingDevicePositionalLocation);
        }

        [TestMethod]
        public void Test_Map_SetsWorkOrderNumber_IfEntityWorkOrderNumberIsNotSet()
        {
            var workOrderNumber = 1234;
            var record = new W1VFileParser.ParsedCustomerMaterial {
                WorkOrderNumber = workOrderNumber
            };
            var entity = new ShortCycleCustomerMaterial { Premise = new Premise() };
            
            _target.Map(entity, record);
            
            Assert.AreEqual(workOrderNumber, entity.ShortCycleWorkOrderNumber);            
        }

        [TestMethod]
        public void Test_Map_DoesNotSetWorkOrderNumber_IfEntityWorkOrderNumberIsSet()
        {
            var oldWorkOrderNumber = 1234;
            var newWorkOrderNumber = 4321;
            var record = new W1VFileParser.ParsedCustomerMaterial {
                WorkOrderNumber = newWorkOrderNumber
            };
            var entity = new ShortCycleCustomerMaterial {
                Premise = new Premise(),
                ShortCycleWorkOrderNumber = oldWorkOrderNumber
            };
            
            _target.Map(entity, record);
            
            Assert.AreEqual(oldWorkOrderNumber, entity.ShortCycleWorkOrderNumber);            
        }

        [TestMethod]
        public void Test_Map_CreatesPremise_IfItDoesNotExist()
        {
            var record = new W1VFileParser.ParsedCustomerMaterial {
                PremiseId = "1234"
            };
            var entity = new ShortCycleCustomerMaterial();
            _premiseRepository.Setup(x => x.Save(It.Is<Premise>(p => p.PremiseNumber == record.PremiseId)))
                              .Returns(new Premise { Id = 1 });

            _target.Map(entity, record);

            Assert.IsNotNull(entity.Premise);
        }

        [TestMethod]
        public void Test_Map_LinksToPremise_IfInstallationIsNotNull()
        {
            var premiseNumber = "1234";
            var installation = "4321";
            var premise = new Premise { PremiseNumber = premiseNumber, Installation = installation };
            var record = new W1VFileParser.ParsedCustomerMaterial {
                PremiseId = premiseNumber,
                Installation = installation
            };
            var entity = new ShortCycleCustomerMaterial();

            _premiseRepository.Setup(x => x.Linq).Returns(new[] { premise }.AsQueryable());

            _target.Map(entity, record);

            Assert.IsNotNull(entity.Premise);
        }

        [TestMethod]
        public void Test_Map_LinksToPremise_IfInstallationAndFunctionalLocationAreNotNull()
        {
            var premiseNumber = "1234";
            var installation = "4321";
            var functionalLocation = "2022";
            var premise = new Premise { PremiseNumber = premiseNumber, Installation = installation, DeviceLocation = functionalLocation};
            var record = new W1VFileParser.ParsedCustomerMaterial {
                PremiseId = premiseNumber,
                Installation = installation,
                FunctionalLocation = functionalLocation
            };
            var entity = new ShortCycleCustomerMaterial();

            _premiseRepository.Setup(x => x.Linq).Returns(new[] { premise }.AsQueryable());

            _target.Map(entity, record);

            Assert.IsNotNull(entity.Premise);
        }
    }
}
