using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MMSINC.Utilities;

namespace SAP.DataTest.Model.Entities
{
    [TestClass()]
    public class SAPNewServiceInstallationTest
    {
        #region Private Methods

        public SAPNewServiceInstallation SetNewServiceInstallation()
        {
            ServiceInstallation serviceInstallation = new ServiceInstallation {
                WorkOrder = new WorkOrder {
                    SAPWorkOrderNumber = 90416501,
                    Installation = 7003444610,
                    DeviceLocation = 6003713600,
                    WorkDescription = new WorkDescription {Description = "INSTALL METER"}
                },
                MeterManufacturerSerialNumber = "12641219",
                Manufacturer = "Neptune",
                MeterSerialNumber = "2006055876",
                MaterialNumber = "1101509",
                ServiceType = "WT",
                MeterLocation = new MeterSupplementalLocation {Description = "Inside"},
                MeterPositionalLocation = new SmallMeterLocation {Description = "Bathroom"},
                MeterDirectionalLocation = new MeterDirection {Description = "Front"},
                ReadingDevicePosition = new SmallMeterLocation {Description = "Bathroom"},
                ReadingDeviceSupplemental = new MeterSupplementalLocation {Description = "Bathroom"},
                ReadingDeviceDirectionalInformation = new MeterDirection {Description = "Front"},
                //Register1
                Register1Dials = "",
                Register1UnitOfMeasure = "Microliter",
                Register1ReadType = new ServiceInstallationReadType {Description = ""},
                Register1RFMIU = "",
                Register1Size = "",
                Register1TPEncoderID = "",
                Register1CurrentRead = "",

                //Register2
                RegisterTwoDials = "",
                Register2UnitOfMeasure = "",
                Register2ReadType = new ServiceInstallationReadType {Description = ""},
                Register2RFMIU = "",
                Register2Size = "",
                Register2TPEncoderID = "",
                Register2CurrentRead = "",

                Activity1 = new ServiceInstallationFirstActivity {SAPCode = "I18"},
                Activity2 = new ServiceInstallationSecondActivity {SAPCode = "I33"},
                Activity3 = new ServiceInstallationThirdActivity {SAPCode = "I01"},
                AdditionalWorkNeeded = new ServiceInstallationWorkType {SAPCode = "F01"},
                Purpose = new SAPWorkOrderPurpose {Code = "I01"},
                ServiceFound = new ServiceInstallationPosition {SAPCode = "I01"},
                ServiceLeft = new ServiceInstallationPosition {SAPCode = "I01"},
                OperatedPointOfControl = false,
                ServiceInstallationReason = new ServiceInstallationReason {Description = "New Service"},
                MeterLocationInformation = "",
            };

            var sapNewServiceInstallation = new SAPNewServiceInstallation(serviceInstallation);

            return sapNewServiceInstallation;
        }

        public SAPNewServiceInstallation SetService()
        {
            Service service = new Service {
                ServiceNumber = 282130,

                ServiceMaterial = new ServiceMaterial {Description = "Not Present"},
                ServiceSize = new ServiceSize {ServiceSizeDescription = "2 1/2"},
                MainSize = new ServiceSize {ServiceSizeDescription = "2 1/4"},
                SAPWorkOrderNumber = 90416576,
                MainType = new MainType {Description = ""},
                TapOrderNotes = "",
                LengthOfService = Convert.ToDecimal(1.00),
                DeviceLocation = "6002049588",
                DepthMainFeet = 0,
                DepthMainInches = 0,
                DateInstalled = DateTime.Now,
                CustomerSideMaterial = new ServiceMaterial {Description = ""},
                CustomerSideSize = new ServiceSize {SAPCode = "0"}
            };

            var sapNewServiceInstallation = new SAPNewServiceInstallation(service);

            return sapNewServiceInstallation;
        }

        public SAPNewServiceInstallation SetServiceForNewServiceMaterial()
        {
            Service service = new Service {
                ServiceNumber = 282130,

                ServiceMaterial = new ServiceMaterial {Description = "Galvanized with Lead Gooseneck"},
                ServiceSize = new ServiceSize {ServiceSizeDescription = "2 1/2"},
                MainSize = new ServiceSize {ServiceSizeDescription = "2 1/4"},
                SAPWorkOrderNumber = 90416576,
                MainType = new MainType {Description = ""},
                TapOrderNotes = "",
                LengthOfService = Convert.ToDecimal(1.00),
                DeviceLocation = "6002049588",
                DepthMainFeet = 0,
                DepthMainInches = 0,
                DateInstalled = DateTime.Now,
                CustomerSideMaterial = new ServiceMaterial {Description = ""},
                CustomerSideSize = new ServiceSize {SAPCode = "0"}
            };

            var sapNewServiceInstallation = new SAPNewServiceInstallation(service);

            return sapNewServiceInstallation;
        }

        private ServiceInstallation GetServiceInstallation()
        {
            return new ServiceInstallation {
                MiuInstallReason = new MiuInstallReasonCode {
                    Description = "New Install AMI",
                    SAPCode = "33"
                },
                MeterDeviceCategory = "01103917",
                Register1DeviceCategory = "01102939",
                Register2DeviceCategory = "027437580"
            };
        }

        private ServiceInstallation GetForLocalTests()
        {
            return new ServiceInstallation {
                WorkOrder = new WorkOrder {
                    SAPWorkOrderNumber = 90416501,
                    Installation = 7003444610,
                    DeviceLocation = 6003713600,
                    WorkDescription = new WorkDescription { Description = "INSTALL METER" }
                },
                MeterManufacturerSerialNumber = "12641219",
                Manufacturer = "Neptune",
                MeterSerialNumber = "2006055876",
                MaterialNumber = "1101509",
                ServiceType = "Water Service",
                MeterLocation = new MeterSupplementalLocation { Description = "Inside", SAPCode = "1" },
                MeterPositionalLocation = new SmallMeterLocation { Description = "Bathroom", SAPCode = "2" },
                MeterDirectionalLocation = new MeterDirection { Description = "Front", SAPCode = "3" },
                ReadingDevicePosition = new SmallMeterLocation { Description = "Bathroom", SAPCode = "4" },
                ReadingDeviceSupplemental = new MeterSupplementalLocation { Description = "Bathroom", SAPCode = "5" },
                ReadingDeviceDirectionalInformation = new MeterDirection { Description = "Front", SAPCode = "6" },
                //Register1
                Register1Dials = "05",
                Register1UnitOfMeasure = "Microliter",
                Register1ReadType = new ServiceInstallationReadType { Description = "Register1ReadType", SAPCode = "7" },
                Register1RFMIU = "Register1RFMIU",
                Register1Size = "Register1Size",
                Register1TPEncoderID = "Register1TPEncoderID",
                Register1CurrentRead = "Register1CurrentRead",
                Register1DeviceCategory = "Register1DeviceCategory",

                //Register2
                RegisterTwoDials = "04",
                Register2UnitOfMeasure = "10 Gallons",
                Register2ReadType = new ServiceInstallationReadType { Description = "Register2ReadType", SAPCode = "8" },
                Register2RFMIU = "Register2RFMIU",
                Register2Size = "Register2Size",
                Register2TPEncoderID = "Register2TPEncoderID",
                Register2CurrentRead = "Register2CurrentRead",
                Register2DeviceCategory = "Register2DeviceCategory",

                Activity1 = new ServiceInstallationFirstActivity { SAPCode = "I18" },
                Activity2 = new ServiceInstallationSecondActivity { SAPCode = "I33" },
                Activity3 = new ServiceInstallationThirdActivity { SAPCode = "I01" },
                AdditionalWorkNeeded = new ServiceInstallationWorkType { SAPCode = "F01" },
                Purpose = new SAPWorkOrderPurpose { Code = "I01" },
                ServiceFound = new ServiceInstallationPosition { SAPCode = "I01" },
                ServiceLeft = new ServiceInstallationPosition { SAPCode = "I01" },
                OperatedPointOfControl = false,
                ServiceInstallationReason = new ServiceInstallationReason { Description = "New Service" },
                MeterLocationInformation = "MeterLocationInformation",
                MiuInstallReason = new MiuInstallReasonCode { Description = "New Install AMI", SAPCode = "33" },
                MeterDeviceCategory = "01103917"
            };
        }

        #endregion

        [TestMethod]
        public void TestContructorSetsPropertiesCorrectly()
        {
            var entity = GetForLocalTests();

            var sapNewServiceInstallation = new SAPNewServiceInstallation(entity);

            Assert.AreEqual(sapNewServiceInstallation.WorkOrderNumber, entity.WorkOrder.SAPWorkOrderNumber.ToString());
            Assert.AreEqual(sapNewServiceInstallation.MeterManufacturerSerialNumber, entity.MeterManufacturerSerialNumber);
            Assert.AreEqual(sapNewServiceInstallation.Manufacturer, entity.Manufacturer);
            Assert.AreEqual(sapNewServiceInstallation.MeterSerialNumber, entity.MeterSerialNumber);
            Assert.AreEqual(sapNewServiceInstallation.Installation, entity.WorkOrder.Installation.ToString());
            Assert.AreEqual(sapNewServiceInstallation.ServiceType, entity.ServiceType);
            Assert.AreEqual(sapNewServiceInstallation.MeterLocation, entity.MeterLocation?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.MeterPositionalLocation, entity.MeterPositionalLocation?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.MeterDirectionalLocation, entity.MeterDirectionalLocation?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.ReadingDevicePosition, entity.ReadingDevicePosition?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.ReadingDeviceSupplemental, entity.ReadingDeviceSupplemental?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.ReadingDeviceDirectionalInformation, entity.ReadingDeviceDirectionalInformation?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.DeviceCategory, entity.MeterDeviceCategory);
            Assert.AreEqual(sapNewServiceInstallation.ReasonForMiuInstall, entity.MiuInstallReason?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.Activity1, entity.Activity1?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.Activity2, entity.Activity2?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.Activity3, entity.Activity3?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.AdditionalWorkNeeded, entity.AdditionalWorkNeeded?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.Purpose, entity.Purpose?.Code);
            Assert.AreEqual(sapNewServiceInstallation.ServiceFound, entity.ServiceFound?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.ServiceLeft, entity.ServiceLeft?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.OperatedPointOfControl, "C02");
            Assert.AreEqual(sapNewServiceInstallation.ReasonForInstall, entity.ServiceInstallationReason?.Description);
            Assert.AreEqual(sapNewServiceInstallation.AdditionalInformation, entity.MeterLocationInformation);
            Assert.AreEqual(sapNewServiceInstallation.ActionFlag, "I");
            Assert.AreEqual(sapNewServiceInstallation.sapRegister.Count, 2);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].Dials, entity.Register1Dials);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].UOM, entity.Register1UnitOfMeasure);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].ReadType, entity.Register1ReadType?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].MIUNumber, entity.Register1RFMIU);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].Size, entity.Register1Size);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].EncoderId, entity.Register1TPEncoderID);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].CurrentRead, entity.Register1CurrentRead);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].DeviceCategory, entity.Register1DeviceCategory);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].Dials, entity.RegisterTwoDials);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].UOM, entity.Register2UnitOfMeasure);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].ReadType, entity.Register2ReadType?.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].MIUNumber, entity.Register2RFMIU);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].Size, entity.Register2Size);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].EncoderId, entity.Register2TPEncoderID);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].CurrentRead, entity.Register2CurrentRead);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].DeviceCategory, entity.Register2DeviceCategory);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].RegType, "1");
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[0].ActivityReasonMUI, entity.MiuInstallReason.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].RegType, "2");
            Assert.AreEqual(sapNewServiceInstallation.sapRegister[1].ActivityReasonMUI, entity.MiuInstallReason.SAPCode);
            Assert.AreEqual(sapNewServiceInstallation.ActionFlagMIU1, "I");
            Assert.AreEqual(sapNewServiceInstallation.ActionFlagMIU2, "I");
        }

        [TestMethod]
        public void TestNewServiceInstallationRequestSentToSAPHasCorrectlyPopulatedValues()
        {
            var entity = GetForLocalTests();

            var sapNewServiceInstallation = new SAPNewServiceInstallation(entity);
            
            var target = sapNewServiceInstallation.NewServiceInstallationRequest();
            var deviceInstallation = target.NotificationWorkOrder_DeviceInstallation;

            // A number of these aren't set, leaving as commented for if and when they are
            //Assert.AreEqual(entity.CurbBoxMeasurementDescription), deviceInstallation.CurbBoxMeasurementDescription);
            Assert.AreEqual(entity.MeterDeviceCategory, deviceInstallation.MeterDeviceCategory);
            //Assert.AreEqual(entity.HeatType, deviceInstallation.HeatType);
            //Assert.AreEqual(entity.Installation, deviceInstallation.Installation);
            Assert.AreEqual(entity.Manufacturer, deviceInstallation.Manufacturer);
            Assert.AreEqual(entity.MeterDirectionalLocation.SAPCode, deviceInstallation.MeterDirectionalLocation);
            Assert.AreEqual(entity.MeterLocation.SAPCode, deviceInstallation.MeterLocation);
            Assert.AreEqual(entity.MeterManufacturerSerialNumber, deviceInstallation.MeterManufacturerSerialNumber);
            Assert.AreEqual(entity.MeterPositionalLocation.SAPCode, deviceInstallation.MeterPositionalLocation);
            Assert.AreEqual(entity.MeterSerialNumber, deviceInstallation.MeterSerialNumber);
            //Assert.AreEqual(entity.MiscInvoice, deviceInstallation.MiscInvoice);
            Assert.AreEqual(entity.ReadingDeviceDirectionalInformation.SAPCode, deviceInstallation.ReadingDeviceDirectionalInformation);
            Assert.AreEqual(entity.ReadingDevicePosition.SAPCode, deviceInstallation.ReadingDevicePosition);
            Assert.AreEqual(entity.ReadingDeviceSupplemental.SAPCode, deviceInstallation.ReadingDeviceSupplemental);
            //Assert.AreEqual(entity.Safety, deviceInstallation.Safety);
            Assert.AreEqual("WT", deviceInstallation.ServiceType); // coverted in private methods
            Assert.AreEqual(entity.WorkOrder.SAPWorkOrderNumber.ToString(), deviceInstallation.WorkOrderNumber);
            Assert.AreEqual(entity.Register1CurrentRead, deviceInstallation.Register[0].CurrentRead);
            Assert.AreEqual(entity.Register1Dials, deviceInstallation.Register[0].Dials);
            Assert.AreEqual(entity.Register1TPEncoderID, deviceInstallation.Register[0].EncoderId);
            Assert.AreEqual(entity.Register1RFMIU, deviceInstallation.Register[0].MIUNumber);
            Assert.AreEqual(entity.Register1ReadType.SAPCode, deviceInstallation.Register[0].ReadType);
            Assert.AreEqual(entity.Register1Size, deviceInstallation.Register[0].Size);
            Assert.AreEqual("µl", deviceInstallation.Register[0].UOM); // coverted in private methods
            Assert.AreEqual(entity.Register1DeviceCategory, deviceInstallation.Register[0].MIUDeviceCategory);
            //Assert.AreEqual(entity.NewSourceOfRead, "01");
            Assert.AreEqual("1", deviceInstallation.Register[0].RegType); // automatically assigned
            Assert.AreEqual(entity.MiuInstallReason.SAPCode, deviceInstallation.Register[0].ActivityReasonMIU);
            Assert.AreEqual(entity.Register2CurrentRead, deviceInstallation.Register[1].CurrentRead);
            Assert.AreEqual(entity.RegisterTwoDials, deviceInstallation.Register[1].Dials);
            Assert.AreEqual(entity.Register2TPEncoderID, deviceInstallation.Register[1].EncoderId);
            Assert.AreEqual(entity.Register2RFMIU, deviceInstallation.Register[1].MIUNumber);
            Assert.AreEqual(entity.Register2ReadType.SAPCode, deviceInstallation.Register[1].ReadType);
            Assert.AreEqual(entity.Register2Size, deviceInstallation.Register[1].Size);
            Assert.AreEqual("10GL", deviceInstallation.Register[1].UOM); // coverted in private methods
            Assert.AreEqual(entity.Register2DeviceCategory, deviceInstallation.Register[1].MIUDeviceCategory);
            //Assert.AreEqual(entity.NewSourceOfRead, "01");
            Assert.AreEqual("2", deviceInstallation.Register[1].RegType); // automatically assigned
            Assert.AreEqual(entity.MiuInstallReason.SAPCode, deviceInstallation.Register[1].ActivityReasonMIU);
            Assert.AreEqual("I", deviceInstallation.FieldActivity[0].ActionFlagMIU);
            Assert.AreEqual("I", deviceInstallation.FieldActivity[0].ActionFlagMIU1);
            Assert.AreEqual("I", deviceInstallation.FieldActivity[0].ActionFlagMIU2);
            Assert.AreEqual("I", deviceInstallation.FieldActivity[0].ActionFlagMeter);
            Assert.AreEqual("I", deviceInstallation.FieldActivity[0].ActionFlagMIU);
            Assert.AreEqual(entity.Activity1.SAPCode, deviceInstallation.FieldActivity[0].Activity1);
            Assert.AreEqual(entity.Activity2.SAPCode, deviceInstallation.FieldActivity[0].Activity2);
            Assert.AreEqual(entity.Activity3.SAPCode, deviceInstallation.FieldActivity[0].Activity3);
            //Assert.AreEqual(entity.AdditionalInformation, deviceInstallation.FieldActivity[0].AdditionalInformation);
            Assert.AreEqual(entity.AdditionalWorkNeeded.SAPCode, deviceInstallation.FieldActivity[0].AdditionalWorkNeeded);
            //Assert.AreEqual(entity.InstallationCompletionDate, deviceInstallation.FieldActivity[0].InstallationCompletionDate);
            Assert.AreEqual("C02", deviceInstallation.FieldActivity[0].OperatedPointOfControl); // logical
            Assert.AreEqual("I01", deviceInstallation.FieldActivity[0].Purpose); // logical
            //Assert.AreEqual(entity.ServiceInstallationReason.SAPCode, deviceInstallation.FieldActivity[0].ReasonForInstallMeter);
            Assert.AreEqual(entity.ServiceFound.SAPCode, deviceInstallation.FieldActivity[0].ServiceFound);
            Assert.AreEqual(entity.ServiceLeft.SAPCode, deviceInstallation.FieldActivity[0].ServiceLeft);
            Assert.AreEqual(entity.MiuInstallReason.SAPCode, deviceInstallation.FieldActivity[0].ReasonForInstallMIU);
            //Assert.AreEqual(entity.QualityIssues deviceInstallation.FieldActivity[0].WorkInformationQualityIssue);
            //Assert.AreEqual(entity.NotificationItemText, deviceInstallation.FieldActivity[0].NotificationItemText);
            //Assert.AreEqual(entity.TwoManCrew, deviceInstallation.FieldActivity[0].NeedTwoManCrew);
            Assert.AreEqual(entity.WorkOrder.Latitude, target.Latitude);
            Assert.AreEqual(entity.WorkOrder.Longitude, target.Longitude);
            // Assert.AreEqual(entity.EngineerId, target.EngineerID);
            // Assert.AreEqual(entity.FSRComments, target.FSRComments);
            // Assert.AreEqual(entity.CompletionStatus, target.CompletionStatus);
            // Assert.AreEqual(entity., target.BackOfficeReview);
            // Assert.AreEqual(entity., target.TechnicalInspectedOn);
            // Assert.AreEqual(entity., target.TechnicalInspectedBy);
            // Assert.AreEqual(entity., target.CompletionStatus);
            // Assert.AreEqual(entity., target.LeakDetectedDate);
            // Assert.AreEqual(entity., target.LeakDetectedNonCompany);
            // Assert.AreEqual(entity., target.InspectionPassed);
            // Assert.AreEqual(entity., target.InspectionDate);
            // Assert.AreEqual(entity., target.ViolationCode);
            // Assert.AreEqual(entity., target.LeadInspectionDate);
            // Assert.AreEqual(entity., target.LeadInspectedBy);
            // Assert.AreEqual(entity.WorkOrder.Service.CustomerSideMaterial, target.CustomerSideMaterial);
            // Assert.AreEqual(entity., target.FSRInteraction);
            // Assert.AreEqual(entity., target.SecureAccess);
        }
    }
}
