using System;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    public class MeterChangeOutExcelExportRecord
    {
        // NOTE: bug 3693 informs us that Ralph/Freddy use a macro to do things with this excel file 
        //       that are dependent on the column order of the excel. The column order is, at the moment,
        //       based on the order of these properties as they appear in this class.

        // OutRead and Start Read are in the file twice for some reason and OutRead has different values in each column

        // NOTE: These columns are in the order they appear in the example excel file for bug-3015.
        public virtual bool ClickAdvantexUpdated { get; set; }
        public MeterChangeOutStatus Status { get; set; }
        public DateTime? Date { get; set; } // DateStatusChanged
        public string CalledInBy { get; set; } // ?????

        [View("SAP Order#")]
        public string SAPOrder { get; set; }

        public string ServiceAddress { get; set; }
        public Town ServiceCity { get; set; }
        public string RemovedSN { get; set; }
        public string NewSN { get; set; }
        public SmallMeterLocation Location { get; set; }
        public MeterSupplementalLocation MeterSupplemental { get; set; }
        public MeterDirection MeterDirection { get; set; }
        public SmallMeterLocation RFPosLocation { get; set; }
        public MeterSupplementalLocation RFSupplemental { get; set; }
        public MeterDirection RFDirectional { get; set; }
        public string NewRF { get; set; }
        public string OutRead { get; set; }
        public string StartRead { get; set; }
        public string PremiseNumber { get; set; }
        public virtual WaterServiceStatus StartStatus { get; set; }
        public virtual WaterServiceStatus EndStatus { get; set; }
        public bool? OperatedPointOfControl { get; set; }
        public ServiceSize MeterSize { get; set; }
        public string AccountName { get; set; }
        public int? MeterInsYear { get; set; }
        public string StreetAddress { get; set; } // Because apparently service address doesn't cover this?
        public bool? InstalledRFOnly { get; set; }
        public int? NumberOfDials { get; set; }
        public string OutRead2 { get; set; }
        public string StartRead2 { get; set; }
        public string EquipmentID { get; set; }
        public string MostRecentNote { get; set; }
        public string MeterReadComment4 { get; set; }
        public string ProjectYear { get; set; }
        public string RouteNumber { get; set; }
        public string OwnerCustomerMeterLocation { get; set; }
        public string TypeOfPlumbing { get; set; }
    }

    public class MeterChangeOutExcelSummaryRecord
    {
        // NOTE: Freddy/Ralph use the excel files generated here with various macros/excel formulas/whatever
        // They don't seem to use the actual header name, they use the column identifiers(A, B, C etc) instead
        // so don't just move things around in here.

        //Account Name	
        public string AccountName { get; set; }

        //Date	
        public DateTime? Date { get; set; } // DateStatusChanged

        //PremiseNumber	
        public string PremiseNumber { get; set; }

        //Service City	
        public Town ServiceCity { get; set; }

        //Street Number	
        public string StreetNumber { get; set; }

        //Prefix	
        public string SAPOrderNumber { get; set; }

        //Street Address	
        public string StreetAddress { get; set; }

        //Suffix	
        public string Suffix { get; set; }

        //AptNo	
        public string AptNo { get; set; }

        //Equipment Id	
        public string EquipmentID { get; set; }

        //Serial Number	
        public string SerialNumber { get; set; } //public string RemovedSN { get; set; }

        //OutRead	
        public string OutRead { get; set; }

        //RFSerialNo	
        public string RFSerialNo { get; set; }

        //EquipmentID	
        // DUPLICATED
        //NewSN	
        public string NewSN { get; set; }

        //StartRead	
        public string StartRead { get; set; }

        //DateInstalled	
        public DateTime? DateInstalled { get; set; } // DateStatusChanged

        //NewRF	
        public string NewRF { get; set; }

        //InstructionCodes	
        public string InstructionCodes { get; set; }

        //Location
        public SmallMeterLocation Location { get; set; }

        //Route Number	
        public string RouteNumber { get; set; }

        //InstalledRFOnly	
        public bool InstalledRFOnly { get; set; }

        //NewWire	
        public bool NewWire { get; set; }

        //TechInit	
        public string TechInit { get; set; } // ?????

        //Meter Size	
        public ServiceSize MeterSize { get; set; }

        //InstalledJumper	
        public bool InstalledJumper { get; set; }

        //LidDrilled	
        public bool LidDrilled { get; set; }

        //ServiceFoundOnLeftOff
        public bool ServiceFoundOnLeftOff { get; set; }

        //HotRodOnNeptune	
        public bool HotRodOnNeptune { get; set; }

        //MeterTurnedOnAfterHours
        public bool MeterTurnedOnAfterHours { get; set; }

        //MovedToPit	
        public bool MovedToPit { get; set; }

        //MeterSupplemental
        public MeterSupplementalLocation MeterSupplemental { get; set; }

        public bool PartialExcavation { get; set; }
        public bool CutBolts { get; set; }

        //public virtual bool ClickAdvantexUpdated { get; set; }
        //public MeterChangeOutStatus Status { get; set; }
        //[View("SAP Order#")]
        //public string SAPOrder { get; set; }
        //public string ServiceAddress { get; set; }
        //public MeterDirection MeterDirection { get; set; }
        //public SmallMeterLocation RFPosLocation { get; set; }
        //public MeterSupplementalLocation RFSupplemental { get; set; }
        //public MeterDirection RFDirectional { get; set; }
        //public virtual WaterServiceStatus StartStatus { get; set; }
        //public virtual WaterServiceStatus EndStatus { get; set; }
        //public bool? OperatedPointOfControl { get; set; }
        //public int? MeterInsYear { get; set; }
        //public int? NumberOfDials { get; set; }
        //public string OutRead2 { get; set; }
        //public string StartRead2 { get; set; }
        [DoesNotExport]
        public OperatingCenter OperatingCenter { get; set; }
    }
}
