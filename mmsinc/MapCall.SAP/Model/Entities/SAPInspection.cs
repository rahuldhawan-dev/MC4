using MapCall.Common.Model.Entities;
using MapCall.SAP.service;
using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Entities
{
    public class SAPInspection : SAPEntity, ISAPServiceEntity
    {
        #region Properties

        #region Logical properties

        public virtual string ShortText
        {
            get
            {
                //ACTIVE = 1, CANCELLED= 2, PENDING = 3, RETIRED = 4, INSTALLED = 5, REQUEST_RETIREMENT = 6, REQUEST_CANCELLATION = 7 
                switch (AssetType)
                {
                    case "HYDRANT":
                    case "BLOW-OFF VALVE":
                        return InventoryNumber != null ? InventoryNumber + " " + InspectionType : string.Empty;
                    case "OPENING":
                        return InventoryNumber != null ? InventoryNumber + " " + CodeType : string.Empty;
                    case "STREET VALVE":
                        return InventoryNumber != null ? InventoryNumber + " " + "INSPECTION" : string.Empty;
                    default:
                        return string.Empty;
                }
            }
        }

        //public virtual string ShortText => InventoryNumber != null? InventoryNumber  + " " + InspectionType : string.Empty;

        #endregion

        #region WebService Request Properties

        public virtual string ServiceObjectURL { get; set; }
        public virtual string AssetType { get; set; }
        public virtual string InventoryNumber { get; set; } //As is- Asset ID from Mapcall for Hydrant / Valve/ Opening
        public virtual string InspectionType { get; set; }
        public virtual string NotificationType { get; set; }
        public virtual string NotificationNo { get; set; }
        public virtual string FunctionalLocation { get; set; }
        public virtual string EquipmentNo { get; set; }
        public virtual string CodeGroup { get; set; }
        public virtual string Coding { get; set; }
        public virtual string PlanningPlant { get; set; }
        public virtual string PlannerGroup { get; set; }
        public virtual string DateOfNotification { get; set; }
        public virtual string NotificationTime { get; set; }
        public virtual string ReportedBy { get; set; }
        public virtual string NotificationLongText { get; set; }
        public virtual string Priority { get; set; }
        public virtual string TechInspectionBy { get; set; }
        public virtual string TechInspectionOn { get; set; }
        public virtual string Operated { get; set; }
        public virtual string PositionFound { get; set; }
        public virtual string PositionLeft { get; set; }
        public virtual string OpenDirection { get; set; }
        public virtual string CodeType { get; set; }
        public virtual string RootCutter { get; set; }
        public virtual string ActivityText { get; set; }
        public virtual string StartDate { get; set; }
        public virtual string StartTimeOfActivity { get; set; }
        public virtual string EndDate { get; set; }
        public virtual string EndTimeOfActivity { get; set; }
        public virtual string StaticPressure { get; set; }
        public virtual string Flow1GPM { get; set; }
        public virtual string Flow1Minutes { get; set; }
        public virtual string ResidualChlorine { get; set; }
        public virtual string TotalChlorine { get; set; }
        public virtual string noTurnsOperated { get; set; }
        public virtual string FlushWaterVolume { get; set; }
        public virtual string FootageOfMainCleaned { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string SAPNotificationNumber { get; set; }
        public virtual string SAPErrorCode { get; set; }
        public virtual string Status { get; set; } // is this one needed?
        public virtual string CostCenter { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        #region Constructors

        public SAPInspection() { }

        public SAPInspection(HydrantInspection hydrantInspection)
        {
            ServiceObjectURL = GetShowUrl("HydrantInspection", hydrantInspection.Id);
            AssetType = "HYDRANT";
            InventoryNumber = hydrantInspection.Hydrant?.HydrantNumber?.ToUpper();
            InspectionType = hydrantInspection.HydrantInspectionType?.Description.ToUpper();
            FunctionalLocation = hydrantInspection.Hydrant?.FunctionalLocation?.Description.ToUpper();
            EquipmentNo = hydrantInspection.Hydrant?.SAPEquipmentNumber?.ToUpper();
            DateOfNotification = hydrantInspection.DateInspected != null
                ? hydrantInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            NotificationTime = hydrantInspection.DateInspected != null
                ? hydrantInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            ReportedBy = hydrantInspection.InspectedBy?.UserName?.ToUpper();
            NotificationLongText = hydrantInspection.InspectedBy?.FullName?.ToUpper() + ":\n" +
                                   hydrantInspection.Remarks?.ToUpper();
            TechInspectionBy = hydrantInspection.InspectedBy?.UserName?.ToUpper();
            TechInspectionOn = hydrantInspection.DateInspected != null
                ? hydrantInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            InspectionType = hydrantInspection.HydrantInspectionType?.Description.ToUpper();
            StartDate = hydrantInspection.DateInspected != null
                ? hydrantInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            StartTimeOfActivity = hydrantInspection.DateInspected != null
                ? hydrantInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            EndDate = hydrantInspection.DateInspected != null
                ? hydrantInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            EndTimeOfActivity = hydrantInspection.DateInspected != null
                ? hydrantInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            StaticPressure = hydrantInspection.StaticPressure?.ToString();
            Flow1GPM = hydrantInspection.GPM?.ToString();
            Flow1Minutes = hydrantInspection.MinutesFlowed?.ToString();
            ResidualChlorine = hydrantInspection.ResidualChlorine?.ToString();
            TotalChlorine = hydrantInspection.TotalChlorine?.ToString();
            //noTurnsOperated = HydrantInspection.n
        }

        public SAPInspection(ValveInspection valveInspection)
        {
            ServiceObjectURL = GetShowUrl("ValveInspection", valveInspection.Id);
            AssetType = "STREET VALVE";
            InventoryNumber = valveInspection.Valve?.ValveNumber?.ToUpper();
            FunctionalLocation = valveInspection.Valve?.FunctionalLocation?.Description.ToUpper();
            EquipmentNo = valveInspection.Valve?.SAPEquipmentNumber?.ToUpper();
            DateOfNotification = valveInspection.DateInspected != null
                ? valveInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            NotificationTime = valveInspection.DateInspected != null
                ? valveInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            Operated = valveInspection.Inspected == true ? "YES" : "NO";
            PositionFound = valveInspection.PositionFound?.Description?.ToUpper();
            PositionLeft = valveInspection.PositionLeft?.Description.ToUpper();
            OpenDirection = valveInspection.Valve?.OpenDirection?.Description.ToUpper();
            ReportedBy = valveInspection.InspectedBy?.UserName?.ToUpper();
            NotificationLongText = valveInspection.InspectedBy?.FullName?.ToUpper() + ":\n" +
                                   valveInspection.Remarks?.ToUpper();
            StartDate = valveInspection.DateInspected != null
                ? valveInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            StartTimeOfActivity = valveInspection.DateInspected != null
                ? valveInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            EndDate = valveInspection.DateInspected != null
                ? valveInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            EndTimeOfActivity = valveInspection.DateInspected != null
                ? valveInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            TechInspectionBy = valveInspection.InspectedBy?.UserName?.ToUpper();
            TechInspectionOn = valveInspection.DateInspected != null
                ? valveInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            noTurnsOperated = valveInspection.Turns?.ToString();
        }

        public SAPInspection(BlowOffInspection blowOffInspection)
        {
            ServiceObjectURL = GetShowUrl("BlowOffInspection", blowOffInspection.Id);
            AssetType = "BLOW-OFF VALVE";
            InventoryNumber = blowOffInspection.Valve?.ValveNumber?.ToUpper();
            InspectionType = blowOffInspection.HydrantInspectionType?.Description.ToUpper();
            FunctionalLocation = blowOffInspection.Valve?.FunctionalLocation?.Description.ToUpper();
            EquipmentNo = blowOffInspection.Valve?.SAPEquipmentNumber?.ToUpper();
            DateOfNotification = blowOffInspection.DateInspected != null
                ? blowOffInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            NotificationTime = blowOffInspection.DateInspected != null
                ? blowOffInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            ReportedBy = blowOffInspection.InspectedBy?.UserName?.ToUpper();
            NotificationLongText = blowOffInspection.InspectedBy?.FullName?.ToUpper() + ":\n" +
                                   blowOffInspection.Remarks?.ToUpper();
            StartDate = blowOffInspection.DateInspected != null
                ? blowOffInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            StartTimeOfActivity = blowOffInspection.DateInspected != null
                ? blowOffInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            EndDate = blowOffInspection.DateInspected != null
                ? blowOffInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            EndTimeOfActivity = blowOffInspection.DateInspected != null
                ? blowOffInspection.DateInspected.ToString(SAP_TIME_FORMAT)
                : null;
            TechInspectionBy = blowOffInspection.InspectedBy?.UserName?.ToUpper();
            TechInspectionOn = blowOffInspection.DateInspected != null
                ? blowOffInspection.DateInspected.Date.ToString(SAP_DATE_FORMAT)
                : null;
            noTurnsOperated = blowOffInspection.Valve?.Turns?.ToString();
            OpenDirection = blowOffInspection.Valve?.OpenDirection?.Description.ToUpper();
            ResidualChlorine = blowOffInspection.ResidualChlorine?.ToString();
            TotalChlorine = blowOffInspection.TotalChlorine?.ToString();
            StaticPressure = blowOffInspection.StaticPressure?.ToString();
            Flow1GPM = blowOffInspection.GPM?.ToString();
            Flow1Minutes = blowOffInspection.MinutesFlowed?.ToString();
        }

        public SAPInspection(SewerMainCleaning sewerMainCleaning)
        {
            string opening1Number,
                   opening1Condition,
                   opening1FrameAndCover,
                   opening2Number,
                   opening2Condition,
                   opening2FrameAndCover,
                   hydrantUsed,
                   notes;

            opening1Number = sewerMainCleaning.Opening1 != null
                ? "Opening 1: " + sewerMainCleaning.Opening1?.OpeningNumber.ToUpper()
                : string.Empty;
            opening1Condition = sewerMainCleaning.Opening1Condition != null
                ? "Opening 1 Condition:" + sewerMainCleaning.Opening1Condition?.Description.ToUpper()
                : string.Empty;
            opening1FrameAndCover = sewerMainCleaning.Opening1FrameAndCover != null
                ? "Opening 1 Frame And Cover:" + sewerMainCleaning.Opening1FrameAndCover?.Description.ToUpper()
                : string.Empty;
            opening2Number = sewerMainCleaning.Opening2 != null
                ? "Opening 2: " + sewerMainCleaning.Opening2?.OpeningNumber.ToUpper()
                : string.Empty;
            opening2Condition = sewerMainCleaning.Opening2Condition != null
                ? "Opening 2 Condition: " + sewerMainCleaning.Opening2Condition?.Description.ToUpper()
                : string.Empty;
            opening2FrameAndCover = sewerMainCleaning.Opening2FrameAndCover != null
                ? "Opening 2 Frame And Cover: " + sewerMainCleaning.Opening2FrameAndCover?.Description.ToUpper()
                : string.Empty;
            hydrantUsed = sewerMainCleaning.HydrantUsed != null
                ? "Hydrant Used: " + sewerMainCleaning.HydrantUsed.HydrantNumber.ToUpper()
                : string.Empty;
            notes = sewerMainCleaning.TableNotes != null
                ? "Notes: " + sewerMainCleaning.TableNotes.ToUpper()
                : string.Empty;

            ServiceObjectURL = GetShowUrl("SewerMainCleaning", sewerMainCleaning.Id);
            AssetType = "OPENING";
            InventoryNumber = sewerMainCleaning.Opening1?.OpeningNumber?.ToUpper();
            //InspectionType = SewerMainCleaning.in?.Description;
            FunctionalLocation = sewerMainCleaning.Opening1?.FunctionalLocation?.Description.ToUpper();
            EquipmentNo = sewerMainCleaning.Opening1?.SAPEquipmentNumber?.ToUpper();
            DateOfNotification = sewerMainCleaning.InspectedDate?.Date.ToString(SAP_DATE_FORMAT);
            NotificationTime = sewerMainCleaning.InspectedDate?.ToString(SAP_TIME_FORMAT);
            ReportedBy = sewerMainCleaning.CreatedBy?.UserName?.ToUpper();
            NotificationLongText = "CREATED BY : " + sewerMainCleaning.CreatedBy?.FullName?.ToUpper() + "\n" +
                                   opening1Number + "\n" + opening1Condition + "\n" + opening1FrameAndCover + "\n" +
                                   opening2Number + "\n" + opening2Condition + "\n" + opening2FrameAndCover + "\n" +
                                   hydrantUsed + "\n" + notes;
            StartDate = sewerMainCleaning.InspectedDate?.Date.ToString(SAP_DATE_FORMAT);
            StartTimeOfActivity = sewerMainCleaning.InspectedDate?.ToString(SAP_TIME_FORMAT);
            EndDate = sewerMainCleaning.InspectedDate?.Date.ToString(SAP_DATE_FORMAT);
            EndTimeOfActivity = sewerMainCleaning.InspectedDate?.ToString(SAP_TIME_FORMAT);
            TechInspectionBy = sewerMainCleaning.CreatedBy?.UserName?.ToUpper();
            TechInspectionOn = sewerMainCleaning.InspectedDate?.Date.ToString(SAP_DATE_FORMAT);
            FlushWaterVolume = sewerMainCleaning.GallonsOfWaterUsed?.ToString();
            FootageOfMainCleaned = sewerMainCleaning.FootageOfMainInspected?.ToString();
        }

        #endregion

        public InspectionRecordInspectionRecord[] InspectionRecordCreate()
        {
            InspectionRecordInspectionRecord[] InspectionRecordCreate_Req = new InspectionRecordInspectionRecord[1];

            InspectionRecordCreate_Req[0] = new InspectionRecordInspectionRecord {
                ServiceObjectURL = ServiceObjectURL,
                AssetType = AssetType,
                NotificationType = NotificationType,
                //NotificationNo = NotificationNo,
                ShortText = ShortText,
                FunctionalLocation = FunctionalLocation,
                EquipmentNo = EquipmentNo,
                CodeGroup = CodeGroup,
                Coding = Coding,
                PlanningPlant = PlanningPlant,
                PlannerGroup = PlannerGroup,
                DateOfNotification = DateOfNotification,
                NotificationTime = NotificationTime,
                ReportedBy = ReportedBy,
                NotificationLongText = NotificationLongText != string.Empty ? NotificationLongText : null,
                Priority = Priority,
                TechInspectionBy = TechInspectionBy,
                TechInspectionOn = TechInspectionOn,
                InspectionType = InspectionType,
                Operated = Operated,
                PositionFound = PositionFound,
                PositionLeft = PositionLeft,
                OpenDirection = OpenDirection,
                CodeType = CodeType,
                RootCutter = RootCutter,
                ActivityText = ActivityText,
                StartDate = StartDate,
                StartTimeOfActivity = StartTimeOfActivity,
                EndDate = EndDate,
                EndTimeOfActivity = EndTimeOfActivity,
                StaticPressure = StaticPressure != "0" ? StaticPressure : null,
                Flow1GPM = Flow1GPM != "0" ? Flow1GPM : null,
                Flow1Minutes = Flow1Minutes != "0" ? Flow1Minutes : null,
                ResidualChlorine = ResidualChlorine != "0" ? ResidualChlorine : null,
                TotalChlorine = TotalChlorine != "0" ? TotalChlorine : null,
                noTurnsOperated = noTurnsOperated != "0" ? noTurnsOperated : null,
                FlushWaterVolume = FlushWaterVolume != "0" ? FlushWaterVolume : null,
                FootageOfMainCleaned = FootageOfMainCleaned != "0" ? FootageOfMainCleaned : null
            };
            return InspectionRecordCreate_Req;
        }

        #endregion
    }
}
