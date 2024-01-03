using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Entities
{
    public class SAPNewServiceInstallation : SAPEntity, IHasStatus, ISAPServiceEntity
    {
        #region Properties

        public virtual string WorkOrderNumber { get; set; }
        public virtual string MiscInvoice { get; set; }
        public virtual string HeatType { get; set; }
        public virtual string Safety { get; set; }
        public virtual string CurbBoxMeasurementDescription { get; set; }
        public virtual string MeterManufacturerSerialNumber { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string MeterSerialNumber { get; set; }
        public virtual string DeviceCategory { get; set; }
        public virtual string Installation { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string MeterLocation { get; set; }
        public virtual string MeterPositionalLocation { get; set; }
        public virtual string MeterDirectionalLocation { get; set; }
        public virtual string ReadingDevicePosition { get; set; }
        public virtual string ReadingDeviceSupplemental { get; set; }
        public virtual string ReadingDeviceDirectionalInformation { get; set; }
        public IList<SAPRegister> sapRegister { get; set; }
        public virtual string Activity1 { get; set; }
        public virtual string Activity2 { get; set; }
        public virtual string Activity3 { get; set; }
        public virtual string Activity4 { get; set; }
        public virtual string Activity5 { get; set; }
        public virtual string Activity6 { get; set; }
        public virtual string Activity7 { get; set; }
        public virtual string Activity8 { get; set; }
        public virtual string Activity9 { get; set; }
        public virtual string Activity10 { get; set; }
        public virtual string AdditionalWorkNeeded { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string ServiceFound { get; set; }
        public virtual string ServiceLeft { get; set; }
        public virtual string OperatedPointOfControl { get; set; }
        public virtual string ReasonForInstall { get; set; }
        public virtual string ReasonForMiuInstall { get; set; }
        public virtual string AdditionalInformation { get; set; }
        public virtual string InstallationCompletionDate { get; set; }

        public virtual string ActionFlag { get; set; }
        public virtual string ActionFlagMIU1 { get; set; }
        public virtual string ActionFlagMIU2 { get; set; }

        // --------------------------------------	
        public virtual string DeviceLocation { get; set; }
        public virtual string ServiceNumber { get; set; }
        public virtual string ServiceMaterial { get; set; }
        public virtual string SizeofService { get; set; }
        public virtual string SAPWorkOrderNumber { get; set; }
        public virtual string TypeofMain { get; set; }
        public virtual string SizeofMain { get; set; }
        public virtual string TapOrderNotes { get; set; }
        public virtual string LengthOfService { get; set; }
        public virtual string LengthOfServiceIn { get; set; }
        public virtual string DepthMain { get; set; }
        public virtual string DepthMainInches { get; set; }
        public virtual string InstalledDate { get; set; }
        public virtual string CustomerSideMaterial { get; set; }
        public virtual string CustomerSideSize { get; set; }
        public virtual string SAPStatus { get; set; }
        public virtual string SAPErrorCode { get; set; }
        public virtual string EngineerID { get; set; }
        public virtual IEnumerable<string> QualityIssues { get; set; }
        public virtual string FSRComments { get; set; }
        public virtual string CompletionStatus { get; set; }
        public virtual string BackOfficeReview { get; set; }
        public virtual string TechnicalInspectedOn { get; set; }
        public virtual string TechnicalInspectedBy { get; set; }
        public virtual string NotificationItemText { get; set; }
        public virtual string NeedTwoManCrew { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }
        public virtual string LeakDetectedNonCompany { get; set; }
        public virtual string LeakDetectedDate { get; set; }
        public virtual string InspectionDate { get; set; }
        public virtual string InspectionPassed { get; set; }
        public string[] ViolationCodes { get; set; }
        public virtual string LeadInspectionDate { get; set; }
        public virtual string LeadInspectedBy { get; set; }
        public virtual string InternalLeadPipingIndicator { get; set; }
        public virtual string FSRInteraction { get; set; }
        public virtual string SecureAccess { get; set; }

        #endregion

        #region Logical Fields

        public virtual string ServiceTypeSAP
        {
            get
            {
                //ACTIVE = 1, CANCELLED= 2, PENDING = 3, RETIRED = 4, INSTALLED = 5, REQUEST_RETIREMENT = 6, REQUEST_CANCELLATION = 7 
                switch (ServiceType)
                {
                    case "Water Service":
                        return "WT";
                    case "Sewer Service":
                        return "SW";
                    case "Fire Service":
                        return "FS";
                    default:
                        return string.Empty;
                }
            }
        }

        public virtual string ServiceSizeSAP
        {
            get
            {
                switch (SizeofService)
                {
                    case "1 1/2":
                        return "1-1/2";
                    case "1 1/4":
                        return "1-1/4";
                    case "1 3/4":
                        return "1-3/4";
                    case "2 1/2":
                        return "2-1/2";
                    case "2 1/4":
                        return "2-1/4";
                    case "2 3/4":
                        return "2-3/4";
                    case "2 5/8":
                        return "2-5/8";
                    case "3 3/4":
                        return "3-3/4";
                    case "6 3/4":
                        return "6-3/4";
                    case "7 1/2":
                        return "7-1/2";
                    case "7 3/4":
                        return "7-3/4";
                    case "8 3/4":
                        return "8-3/4";
                    default:
                        return SizeofService;
                }
            }
        }

        public virtual string SizeOfMainSAP
        {
            get
            {
                switch (SizeofMain)
                {
                    case "2 1/4":
                        return "2-1/4";
                    case "1 1/4":
                        return "1-1/4";
                    case "2 1/2":
                        return "2-1/2";
                    case "1 1/2":
                        return "1-1/2";
                    default:
                        return SizeofMain;
                }
            }
        }

        public virtual string ServiceMaterialSAP
        {
            get
            {
                switch (ServiceMaterial)
                {
                    case "AC":
                        return "AC";
                    case "Brass":
                        return "Brass";
                    case "Carlon":
                        return "PB";
                    case "Cast Iron":
                        return "CI";
                    case "Copper":
                        return "CU";
                    case "Ductile":
                        return "DI";
                    case "Galvanized":
                        return "GAL";
                    case "Lead":
                        return "LEAD";
                    case "Not Present":
                        return null;
                    case "Plastic":
                        return "PLAS";
                    case "Transite":
                        return "AC";
                    case "Tubeloy":
                        return "LEAD";
                    case "Unknown":
                        return "UNK";
                    case "Vitrified Clay":
                        return "VC";
                    case "WICL":
                        return "WI";
                    case "Galvanized with Lead Gooseneck": //added on 23th Aug 2017 as per Doug request
                        return "GWLG";
                    case "Other with Lead Gooseneck": //added on 23th Aug 2017 as per Doug request
                        return "OWLG";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        #region Exposed Methods

        public SAPNewServiceInstallation() { }

        public SAPNewServiceInstallation(ServiceInstallation serviceInstallation)
        {
            WorkOrderNumber = serviceInstallation.WorkOrder?.SAPWorkOrderNumber?.ToString();
            MeterSerialNumber = serviceInstallation.MeterSerialNumber;
            Manufacturer = serviceInstallation.Manufacturer;
            MeterManufacturerSerialNumber = serviceInstallation.MeterManufacturerSerialNumber;
            Installation = serviceInstallation.WorkOrder?.Installation?.ToString();
            ServiceType =
                serviceInstallation.ServiceType; //WT' - Water Service, 'SW' - Sewer Service, 'FS' - Fire Service
            MeterLocation = serviceInstallation.MeterLocation?.SAPCode;
            MeterPositionalLocation = serviceInstallation.MeterPositionalLocation?.SAPCode;
            MeterDirectionalLocation = serviceInstallation.MeterDirectionalLocation?.SAPCode;
            ReadingDevicePosition = serviceInstallation.ReadingDevicePosition?.SAPCode;
            ReadingDeviceSupplemental = serviceInstallation.ReadingDeviceSupplemental?.SAPCode;
            ReadingDeviceDirectionalInformation = serviceInstallation.ReadingDeviceDirectionalInformation?.SAPCode;
            DeviceCategory = serviceInstallation.MeterDeviceCategory ?? serviceInstallation.MaterialNumber;
            ReasonForMiuInstall = serviceInstallation.MiuInstallReason?.SAPCode;
            //Register1
            sapRegister = new List<SAPRegister>();
            var sapRegister1 = new SAPRegister {
                Dials = serviceInstallation.Register1Dials,
                UOM = serviceInstallation.Register1UnitOfMeasure,
                ReadType = serviceInstallation.Register1ReadType?.SAPCode,
                MIUNumber = serviceInstallation.Register1RFMIU,
                Size = serviceInstallation.Register1Size,
                EncoderId = serviceInstallation.Register1TPEncoderID,
                CurrentRead = serviceInstallation.Register1CurrentRead,
                DeviceCategory = serviceInstallation.Register1DeviceCategory,
                RegType = "1",
                ActivityReasonMUI = serviceInstallation.MiuInstallReason?.SAPCode
            };
            sapRegister.Add(sapRegister1);

            //Register2
            if (serviceInstallation.RegisterTwoDials != null ||
                serviceInstallation.Register2UnitOfMeasure != null ||
                serviceInstallation.Register2ReadType?.SAPCode != null ||
                serviceInstallation.Register2RFMIU != null ||
                serviceInstallation.Register2Size != null ||
                serviceInstallation.Register2TPEncoderID != null ||
                serviceInstallation.Register2CurrentRead != null ||
                serviceInstallation.Register2DeviceCategory != null)
            {
                var sapRegister2 = new SAPRegister {
                    Dials = serviceInstallation.RegisterTwoDials,
                    UOM = serviceInstallation.Register2UnitOfMeasure,
                    ReadType = serviceInstallation.Register2ReadType?.SAPCode,
                    MIUNumber = serviceInstallation.Register2RFMIU,
                    Size = serviceInstallation.Register2Size,
                    EncoderId = serviceInstallation.Register2TPEncoderID,
                    CurrentRead = serviceInstallation.Register2CurrentRead,
                    DeviceCategory = serviceInstallation.Register2DeviceCategory,
                    RegType = "2",
                    ActivityReasonMUI = serviceInstallation.MiuInstallReason?.SAPCode
                };
                sapRegister.Add(sapRegister2);
            }

            ActionFlagMIU1 = "I";
            ActionFlagMIU2 = sapRegister.Count == 2 ? "I" : "S";
            Activity1 = serviceInstallation.Activity1?.SAPCode;
            Activity2 = serviceInstallation.Activity2?.SAPCode;
            Activity3 = serviceInstallation.Activity3?.SAPCode;
            AdditionalWorkNeeded = serviceInstallation.AdditionalWorkNeeded?.SAPCode;
            Purpose = serviceInstallation.Purpose?.Code;
            ServiceFound = serviceInstallation.ServiceFound?.SAPCode;
            ServiceLeft = serviceInstallation.ServiceLeft?.SAPCode;
            OperatedPointOfControl = serviceInstallation.OperatedPointOfControl.ToString() == "True" ? "C01" : "C02";
            ReasonForInstall = serviceInstallation.ServiceInstallationReason?.ToString();
            AdditionalInformation = serviceInstallation.MeterLocationInformation;
            if (serviceInstallation.WorkOrder != null)
                InstallationCompletionDate =
                    serviceInstallation.WorkOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT);
            ActionFlag = "I";
        }

        public SAPNewServiceInstallation(Service service)
        {
            //Service	
            DeviceLocation = service.DeviceLocation;
            ServiceNumber = service.ServiceNumber.ToString();
            ServiceMaterial = service.ServiceMaterial?.Description;
            SizeofService = service.ServiceSize?.Description?.ToString();
            SAPWorkOrderNumber = service.SAPWorkOrderNumber.ToString();
            TypeofMain = service.MainType?.SAPCode;
            SizeofMain = service.MainSize?.Description?.ToString();
            TapOrderNotes = service.TapOrderNotes;
            LengthOfService = Convert.ToInt16(service.LengthOfService).ToString();

            DepthMain = service.DepthMainFeet.ToString();
            DepthMainInches = service.DepthMainInches.ToString();
            InstalledDate = service.DateInstalled?.Date.ToString(SAP_DATE_FORMAT);
            CustomerSideMaterial = service.CustomerSideMaterial?.Description;
            CustomerSideSize = service.CustomerSideSize?.Description?.ToString();
        }

        public W1v_New_ServiceInstallationQuery NewServiceInstallationRequest()
        {
            W1v_New_ServiceInstallationQuery new_ServiceInstallationQuery = new W1v_New_ServiceInstallationQuery();

            var DeviceInstallation = new W1v_New_ServiceInstallationQueryNotificationWorkOrder_DeviceInstallation();

            W1v_New_ServiceInstallationQueryNotificationWorkOrder_DeviceInstallationRegister[] register =
                new W1v_New_ServiceInstallationQueryNotificationWorkOrder_DeviceInstallationRegister[2];
            W1v_New_ServiceInstallationQueryNotificationWorkOrder_DeviceInstallationFieldActivity[] fieldActivityField =
                new W1v_New_ServiceInstallationQueryNotificationWorkOrder_DeviceInstallationFieldActivity[1];

            DeviceInstallation.CurbBoxMeasurementDescription = CurbBoxMeasurementDescription;
            DeviceInstallation.MeterDeviceCategory = DeviceCategory;
            DeviceInstallation.HeatType = HeatType;
            DeviceInstallation.Installation = Installation;
            DeviceInstallation.Manufacturer = Manufacturer;
            DeviceInstallation.MeterDirectionalLocation = MeterDirectionalLocation;
            DeviceInstallation.MeterLocation = MeterLocation;
            DeviceInstallation.MeterManufacturerSerialNumber = MeterManufacturerSerialNumber;
            DeviceInstallation.MeterPositionalLocation = MeterPositionalLocation;
            DeviceInstallation.MeterSerialNumber = MeterSerialNumber;
            DeviceInstallation.MiscInvoice = MiscInvoice;
            DeviceInstallation.ReadingDeviceDirectionalInformation = ReadingDeviceDirectionalInformation;
            DeviceInstallation.ReadingDevicePosition = ReadingDevicePosition;
            DeviceInstallation.ReadingDeviceSupplemental = ReadingDeviceSupplemental;
            DeviceInstallation.Safety = Safety;
            DeviceInstallation.ServiceType = ServiceTypeSAP;
            DeviceInstallation.WorkOrderNumber = WorkOrderNumber;

            //register 1
            if (sapRegister != null && sapRegister.Count > 0)
            {
                register[0] = new W1v_New_ServiceInstallationQueryNotificationWorkOrder_DeviceInstallationRegister();
                register[0].CurrentRead = sapRegister[0].CurrentRead;
                register[0].Dials = sapRegister[0].Dials;
                register[0].EncoderId = sapRegister[0].EncoderId;
                register[0].MIUNumber = sapRegister[0].MIUNumber;
                register[0].ReadType = sapRegister[0].ReadType;
                register[0].Size = sapRegister[0].Size;
                register[0].UOM = sapRegister[0].SAPUnitOfMeasure;
                register[0].MIUDeviceCategory = sapRegister[0].DeviceCategory;
                register[0].NewSourceOfRead = "01"; // default value provided by SAP for Mapcall
                register[0].RegType = "1";
                register[0].ActivityReasonMIU = sapRegister[0].ActivityReasonMUI;
            }

            if (sapRegister != null && sapRegister.Count > 1)
            {
                //register 2
                register[1] = new W1v_New_ServiceInstallationQueryNotificationWorkOrder_DeviceInstallationRegister();
                register[1].CurrentRead = sapRegister[1].CurrentRead;
                register[1].Dials = sapRegister[1].Dials;
                register[1].EncoderId = sapRegister[1].EncoderId;
                register[1].MIUNumber = sapRegister[1].MIUNumber;
                register[1].ReadType = sapRegister[1].ReadType;
                register[1].Size = sapRegister[1].Size;
                register[1].UOM = sapRegister[1].SAPUnitOfMeasure;
                register[1].MIUDeviceCategory = sapRegister[1].DeviceCategory;
                register[1].NewSourceOfRead = "01"; // default value provided by SAP for Mapcall
                register[1].RegType = "2";
                register[1].ActivityReasonMIU = sapRegister[1].ActivityReasonMUI;
            }

            fieldActivityField[0] =
                new W1v_New_ServiceInstallationQueryNotificationWorkOrder_DeviceInstallationFieldActivity();
            fieldActivityField[0].ActionFlagMIU1 = "I";
            // if there's two registers it means it's a compound meter, send "I", otherwise send "S"
            fieldActivityField[0].ActionFlagMIU2 = (register.Length > 1 && register[1] != null) ? "I" : "S";
            fieldActivityField[0].ActionFlagMeter = ActionFlag;
            fieldActivityField[0].ActionFlagMIU = ActionFlag;
            fieldActivityField[0].Activity1 = Activity1;
            fieldActivityField[0].Activity2 = Activity2;
            fieldActivityField[0].Activity3 = Activity3;
            fieldActivityField[0].Activity4 = Activity4;
            fieldActivityField[0].Activity5 = Activity5;
            fieldActivityField[0].Activity6 = Activity6;
            fieldActivityField[0].Activity7 = Activity7;
            fieldActivityField[0].Activity8 = Activity8;
            fieldActivityField[0].Activity9 = Activity9;
            fieldActivityField[0].Activity10 = Activity10;
            fieldActivityField[0].AdditionalInformation = AdditionalInformation;
            fieldActivityField[0].AdditionalWorkNeeded = AdditionalWorkNeeded;
            fieldActivityField[0].InstallationCompletionDate = InstallationCompletionDate;
            fieldActivityField[0].OperatedPointOfControl = OperatedPointOfControl;
            fieldActivityField[0].Purpose = Purpose;
            fieldActivityField[0].ReasonForInstallMeter = ReasonForInstall;
            fieldActivityField[0].ServiceFound = ServiceFound;
            fieldActivityField[0].ServiceLeft = ServiceLeft;
            fieldActivityField[0].ReasonForInstallMIU = ReasonForMiuInstall;
            if (QualityIssues != null && QualityIssues.Any())
            {
                fieldActivityField[0].WorkInformationQualityIssue = QualityIssues.ToArray();
            }

            fieldActivityField[0].NotificationItemText = NotificationItemText;
            fieldActivityField[0].NeedTwoManCrew = NeedTwoManCrew;
            new_ServiceInstallationQuery.Latitude = Latitude;
            new_ServiceInstallationQuery.Longitude = Longitude;
            DeviceInstallation.Register = register;
            DeviceInstallation.FieldActivity = fieldActivityField;

            new_ServiceInstallationQuery.NotificationWorkOrder_DeviceInstallation = DeviceInstallation;

            new_ServiceInstallationQuery.EngineerID = EngineerID;
            //WorkInformationQualityIssue
            new_ServiceInstallationQuery.FSRComments = FSRComments;
            new_ServiceInstallationQuery.CompletionStatus = CompletionStatus;
            new_ServiceInstallationQuery.BackOfficeReview = BackOfficeReview;
            new_ServiceInstallationQuery.TechnicalInspectedOn = TechnicalInspectedOn;
            new_ServiceInstallationQuery.TechnicalInspectedBy = TechnicalInspectedBy;
            new_ServiceInstallationQuery.LeakDetectedDate = LeakDetectedDate;
            new_ServiceInstallationQuery.LeakDetectedNonCompany = LeakDetectedNonCompany;

            new_ServiceInstallationQuery.InspectionPassed = InspectionPassed;
            new_ServiceInstallationQuery.InspectionDate = InspectionDate;
            if (ViolationCodes != null)
            {
                new_ServiceInstallationQuery.ViolationCode = ViolationCodes.Select(x => x).ToArray();
            }

            new_ServiceInstallationQuery.LeadInspectionDate = LeadInspectionDate;
            new_ServiceInstallationQuery.LeadInspectedBy = LeadInspectedBy;
            new_ServiceInstallationQuery.InternalLeadPipingIndicator = InternalLeadPipingIndicator;
            new_ServiceInstallationQuery.CustomerSideMaterial = CustomerSideMaterial;
            new_ServiceInstallationQuery.FSRInteraction = FSRInteraction;
            new_ServiceInstallationQuery.SecureAccess = SecureAccess;

            return new_ServiceInstallationQuery;
        }

        public W1v_New_ServiceInstallationQuery ServiceRequest()
        {
            W1v_New_ServiceInstallationQuery new_ServiceInstallationQuery = new W1v_New_ServiceInstallationQuery();
            W1v_New_ServiceInstallationQueryServiceRecord[]
                ServiceRecord = new W1v_New_ServiceInstallationQueryServiceRecord[1];

            ServiceRecord[0] = new W1v_New_ServiceInstallationQueryServiceRecord();
            ServiceRecord[0].CustomerSideMaterial = CustomerSideMaterial;
            ServiceRecord[0].CustomerSideSize = CustomerSideSize;
            ServiceRecord[0].DepthMain = DepthMain;
            ServiceRecord[0].DepthMainInches = DepthMainInches;
            ServiceRecord[0].DeviceLocation = DeviceLocation;
            ServiceRecord[0].InstalledDate = InstalledDate;
            ServiceRecord[0].LengthOfService = LengthOfService;
            ServiceRecord[0].LengthOfServiceIn = LengthOfServiceIn;
            ServiceRecord[0].SAPWorkOrderNumber = SAPWorkOrderNumber;
            ServiceRecord[0].ServiceMaterial = ServiceMaterialSAP;
            ServiceRecord[0].ServiceNumber = ServiceNumber;
            ServiceRecord[0].SizeofMain = SizeOfMainSAP;
            ServiceRecord[0].SizeofService = ServiceSizeSAP;
            ServiceRecord[0].TapOrderNotes = TapOrderNotes;
            ServiceRecord[0].TypeofMain = TypeofMain;

            new_ServiceInstallationQuery.ServiceRecord = ServiceRecord;

            return new_ServiceInstallationQuery;
        }

        #endregion
    }

    [Serializable]
    public class SAPRegister
    {
        //Register
        public virtual string Dials { get; set; }
        public virtual string UOM { get; set; }
        public virtual string ReadType { get; set; }
        public virtual string MIUNumber { get; set; }
        public virtual string Size { get; set; }
        public virtual string EncoderId { get; set; }
        public virtual string CurrentRead { get; set; }
        public virtual string DeviceCategory { get; set; }
        public virtual string RegType { get; set; }
        public virtual string ActivityReasonMUI { get; set; }

        public virtual string SAPUnitOfMeasure
        {
            get
            {
                switch (UOM)
                {
                    case "Cubic inch":
                        return "Inch3";
                    case "1 Million Gallon":
                        return "1MLGl";
                    case "100 cubit feet":
                        return "CCF";
                    case "Cubic centimeter":
                        return "cm3";
                    case "Cubic decimeter":
                        return "dm3";
                    case "100 gallons":
                        return "CGL";
                    case "Centiliter":
                        return "CI";
                    case "10 CGL":
                        return "10CGL";
                    case "Differential Pres (in H2O)":
                        return "DFP";
                    case "10 Cubic feet":
                        return "DFT";
                    case "Fluid Ounce US":
                        return "foz US";
                    case "Cubic foot":
                        return "1CF";
                    case "US gallon":
                        return "1GL";
                    case "Hectoliter":
                        return "hl";
                    case "Liter":
                        return "l";
                    case "Cubic meter":
                        return "m3";
                    case "Millions of Gallons":
                        return "MG";
                    case "Milliliter":
                        return "ml";
                    case "Cubic millimeter":
                        return "mm3";
                    case "Pint, US liquid":
                        return "pt US";
                    case "Quart, US liquid":
                        return "qt US";
                    case "1000 Cubit Feet":
                        return "1000CF";
                    case "Thousand Gallons":
                        return "TG";
                    case "10 Gallons":
                        return "10GL";
                    case "10 Thousand Gallons":
                        return "10KGL";
                    case "Cubic yard":
                        return "yd3";
                    case "Microliter":
                        return "µl";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
