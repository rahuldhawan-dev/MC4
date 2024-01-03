using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterChangeOut : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Consts

        public struct StringLengths
        {
            public const int ACCOUNT_NUMBER = 50,
                             CUSTOMER_NAME = 60,
                             EMAIL = 255,
                             EQUIPMENT_ID = 20,
                             METER_COMMENTS_PREMISE = 255,
                             METER_READ_CODE = 10,
                             METER_READ_COMMENT = 50,
                             NEW_RF_NUMBER = 10,
                             NEW_SERIAL_NUMBER = 8,
                             OWNER_LOCATION_OTHER = 50,
                             PREMISE_NUMBER = 15,
                             REMOVED_SERIAL_NUMBER = 20,
                             ROUTE_NUMBER = 10,
                             SAP_ORDER_NUMBER = 9,
                             SERVICE_STREET = 60,
                             SERVICE_STREET_NUMBER = 10,
                             SERVICE_PHONE = 20,
                             SERVICE_PHONE_EXTENSION = 10,
                             SERVICE_ZIP = 10;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual MeterChangeOutContract Contract { get; set; }

        public virtual string PremiseNumber { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string ServiceStreet { get; set; }
        public virtual string ServiceStreetNumber { get; set; }

        [View(DisplayName = "Service Street Address")]
        public virtual string ServiceStreetAddressCombined { get; set; }

        public virtual Town ServiceCity { get; set; }
        public virtual string ServiceZip { get; set; }
        public virtual string ServicePhone { get; set; }
        public virtual string ServicePhoneExtension { get; set; }
        public virtual string ServicePhone2 { get; set; }
        public virtual string ServicePhone2Extension { get; set; }
        public virtual CustomerMeterLocation OwnerCustomerMeterLocation { get; set; }
        public virtual string OwnerLocationOther { get; set; }
        public virtual int? YearInstalled { get; set; }
        public virtual MeterChangeOutWorkScope WorkScope { get; set; }
        public virtual string ProjectYear { get; set; }

        public virtual TypeOfPlumbing TypeOfPlumbing { get; set; }

        [Multiline]
        public virtual string FieldNotes { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateScheduled { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateStatusChanged { get; set; }

        public virtual ServiceSize MeterSize { get; set; }

        [View("Scheduled Time")]
        public virtual MeterScheduleTime MeterScheduleTime { get; set; }

        public virtual ContractorMeterCrew AssignedContractorMeterCrew { get; set; }

        [DisplayName("Status")]
        public virtual MeterChangeOutStatus MeterChangeOutStatus { get; set; }

        public virtual string Email { get; set; }

        [DisplayName("Called In By")]
        public virtual ContractorMeterCrew CalledInByContractorMeterCrew { get; set; }

        public virtual string RemovedSerialNumber { get; set; }

        public virtual int? OutRead { get; set; }
        public virtual string NewSerialNumber { get; set; }
        public virtual string NewRFNumber { get; set; }
        public virtual int? StartRead { get; set; }
        public virtual SmallMeterLocation MeterLocation { get; set; }
        public virtual MeterSupplementalLocation MeterSupplementalLocation { get; set; }
        public virtual MeterDirection MeterDirection { get; set; }
        public virtual SmallMeterLocation RFLocation { get; set; }
        public virtual MeterSupplementalLocation RFSupplementalLocation { get; set; }
        public virtual MeterDirection RFDirection { get; set; }
        public virtual string EquipmentID { get; set; }
        public virtual string RouteNumber { get; set; }
        public virtual string MeterReadCode1 { get; set; }
        public virtual string MeterReadComment1 { get; set; }
        public virtual string MeterReadCode2 { get; set; }

        [View(DisplayName = "Comment 2")]
        public virtual string MeterReadComment2 { get; set; }

        public virtual string MeterReadCode3 { get; set; }

        [View(DisplayName = "Comment 3")]
        public virtual string MeterReadComment3 { get; set; }

        public virtual string MeterReadCode4 { get; set; }

        [View(DisplayName = "Comment 4")]
        public virtual string MeterReadComment4 { get; set; }

        [View(DisplayName = "Premise")]
        public virtual string MeterCommentsPremise { get; set; }

        public virtual bool? IsNeptuneRFOnly { get; set; }
        public virtual bool? IsHotRodRFOnly { get; set; }
        public virtual bool? OperatedPointOfControlAtAnyValve { get; set; }
        public virtual bool? IsMuellerMeter { get; set; }

        // This is the "Service Found" field 
        public virtual WaterServiceStatus StartStatus { get; set; }

        // This is the "Service Left" field
        public virtual WaterServiceStatus EndStatus { get; set; }

        public virtual bool? TurnedOffWater { get; set; }
        public virtual bool? MeterTurnedOnAfterHours { get; set; }
        public virtual bool? MovedMeterToPit { get; set; }
        public virtual bool? RanNewWire { get; set; }

        [View("Installed Grounding Jumper Bar")]
        public virtual bool? InstalledJumperBar { get; set; }

        public virtual bool? ContractorDrilledLid { get; set; }
        public virtual bool PartialExcavation { get; set; }
        public virtual bool CutBolts { get; set; }
        public virtual bool? Canvassed { get; set; }
        public virtual bool ClickAdvantexUpdated { get; set; }
        public virtual string SAPOrderNumber { get; set; }
        public virtual int? NumberOfDials { get; set; }

        public virtual IList<Document<MeterChangeOut>> Documents { get; set; }
        public virtual IList<Note<MeterChangeOut>> Notes { get; set; }

        #region Logical Properties

        [DoesNotExport]
        public virtual string TableName => nameof(MeterChangeOut) + "s";

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Map(d => (IDocumentLink)d);

        public virtual IList<INoteLink> LinkedNotes =>
            Notes.Map(n => (INoteLink)n).OrderByDescending(n => n.Note.CreatedAt).ToList();

        [View(DisplayName = "Out Read")]
        public virtual string OutReadPadded => $"{OutRead}".PadLeft(6, '0');

        [View(DisplayName = "Out Read")]
        public virtual string StartReadPadded => $"{StartRead}".PadLeft(6, '0');

        public virtual string MostRecentNote
        {
            get { return Notes.OrderByDescending(x => x.Note.CreatedAt).FirstOrDefault()?.Note.Text; }
        }

        #endregion

        #endregion

        #region Constructor

        public MeterChangeOut()
        {
            Notes = new List<Note<MeterChangeOut>>();
            Documents = new List<Document<MeterChangeOut>>();
        }

        #endregion

        #region Public Methods

        public virtual MeterChangeOutExcelExportRecord ToExcelRecord()
        {
            var excel = new MeterChangeOutExcelExportRecord();
            excel.ClickAdvantexUpdated = ClickAdvantexUpdated;
            excel.Status = MeterChangeOutStatus;
            excel.Date = DateStatusChanged;
            excel.CalledInBy = CalledInByContractorMeterCrew?.ToString();
            excel.SAPOrder = SAPOrderNumber;
            excel.ServiceAddress = ServiceStreetAddressCombined;
            excel.ServiceCity = ServiceCity;
            excel.RemovedSN = RemovedSerialNumber;
            excel.NewSN = NewSerialNumber;
            excel.Location = MeterLocation;
            excel.MeterSupplemental = MeterSupplementalLocation;
            excel.MeterDirection = MeterDirection;
            excel.RFPosLocation = RFLocation;
            excel.RFSupplemental = RFSupplementalLocation;
            excel.RFDirectional = RFDirection;
            excel.NewRF = NewRFNumber;
            excel.OutRead = OutReadPadded;
            excel.StartRead = StartReadPadded;
            excel.PremiseNumber = PremiseNumber;
            excel.StartStatus = StartStatus;
            excel.EndStatus = EndStatus;
            excel.OperatedPointOfControl = OperatedPointOfControlAtAnyValve;
            excel.MeterSize = MeterSize;
            excel.AccountName = CustomerName;
            excel.MeterInsYear = YearInstalled;
            excel.StreetAddress = ServiceStreet;
            excel.InstalledRFOnly = IsNeptuneRFOnly;
            excel.NumberOfDials = NumberOfDials;
            excel.OutRead2 = OutReadPadded;
            excel.StartRead2 = StartReadPadded;
            excel.EquipmentID = EquipmentID;
            excel.MostRecentNote = MostRecentNote;
            excel.MeterReadComment4 = MeterReadComment4;
            excel.ProjectYear = ProjectYear;
            excel.RouteNumber = RouteNumber;
            excel.OwnerCustomerMeterLocation = OwnerCustomerMeterLocation?.Description;
            excel.TypeOfPlumbing = TypeOfPlumbing?.Description;
            return excel;
        }

        public virtual MeterChangeOutExcelSummaryRecord ToExcelSummaryRecord()
        {
            var excel = new MeterChangeOutExcelSummaryRecord();
            excel.AccountName = CustomerName;
            excel.Date = DateStatusChanged;
            excel.PremiseNumber = PremiseNumber;
            excel.ServiceCity = ServiceCity;
            excel.StreetNumber = ServiceStreetNumber;
            excel.SAPOrderNumber = SAPOrderNumber;
            excel.StreetAddress = ServiceStreet;
            excel.Suffix = "";
            excel.AptNo = "";
            excel.EquipmentID = EquipmentID;
            excel.SerialNumber = RemovedSerialNumber;
            excel.OutRead = OutReadPadded;
            excel.RFSerialNo = "";
            excel.NewSN = NewSerialNumber;
            excel.StartRead = StartReadPadded;
            excel.DateInstalled = DateStatusChanged;
            excel.NewRF = NewRFNumber;
            excel.InstructionCodes = "";
            excel.Location = MeterLocation;
            excel.RouteNumber = "";
            excel.InstalledRFOnly = IsNeptuneRFOnly.GetValueOrDefault();
            excel.NewWire = RanNewWire.GetValueOrDefault();
            excel.TechInit = CalledInByContractorMeterCrew?.ToString();
            excel.MeterSize = MeterSize;
            excel.InstalledJumper = InstalledJumperBar.GetValueOrDefault();
            excel.LidDrilled = ContractorDrilledLid.GetValueOrDefault();
            excel.ServiceFoundOnLeftOff = TurnedOffWater.GetValueOrDefault();
            excel.MeterTurnedOnAfterHours = MeterTurnedOnAfterHours.GetValueOrDefault();
            excel.HotRodOnNeptune = IsHotRodRFOnly.GetValueOrDefault();
            excel.MovedToPit = MovedMeterToPit.GetValueOrDefault();
            excel.MeterSupplemental = MeterSupplementalLocation;
            excel.PartialExcavation = PartialExcavation;
            excel.CutBolts = CutBolts;
            excel.OperatingCenter = Contract.OperatingCenter;
            return excel;
        }

        public override string ToString()
        {
            //="Order # " & [Order] & ", " & [OpCntr] & ", " & [Service Address] & ", " & [Service City] & ", " & "SN=" & [Serial Number] & ", " & "Equipment ID=" & [Equipment ID]
            return
                $"Order # {SAPOrderNumber}, {Contract.OperatingCenter}, {ServiceStreetNumber} {ServiceStreet}, {ServiceCity} SN={RemovedSerialNumber}, Equipment ID= {EquipmentID}";
        }

        #endregion
    }
}
