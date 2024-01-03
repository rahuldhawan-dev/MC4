using System;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    // NSI - New Service Installation 
    // This happens when they install a new meter
    // Links to a work order and a service through the order.
    [Serializable]
    public class ServiceInstallation : IEntity, ISAPEntity //: IThingWithOperatingCenter
    {
        #region Constants

        public struct StringLengths
        {
            public const int
                METER_MANUFACTURER_SERIAL_NUMBER = 18,
                SERVICE_TYPE = 30,
                MANUFACTURER = 30,
                METER_SERIAL_NUMBER = 18,
                TP_ENCODER_ID = 30,
                CURRENT_READ = 17,
                RFMIU = 30,
                SIZE = 30,
                DIALS = 30,
                UNIT_OF_MEASURE = 30,
                SAP_CODE = 3,
                CODE_GROUP = 10,
                DESCRIPTION = 50,
                REGISTER_DIALS = 40,
                REGISTER_UNIT_OF_MEASURE = 40,
                REGISTER_READ_TYPE = 2,
                REGISTER_MIU = 30,
                REGISTER_SIZE = 40,
                REGISTER_ENCODER = 30,
                REGISTER_CURRENT_READ = 17,
                METER_SIZE = 30,
                MATERIAL_NUMBER = 18;
        }

        public struct Display
        {
            public const string METER_MANUFACTURER_SERIAL_NUMBER = "Meter Serial Number",
                                MIU_INSTALL_REASON = "Reason for MIU Install",
                                READING_DEVICE_SUPPLEMENTAL = "Reading Device Location",
                                READING_DEVICE_POSITION = "Reading Positional Location",
                                READING_DEVICE_DIRECTIONAL_INFORMATION = "Reading Device Directional Location",
                                REGISTER_1RF_MIU = "Register 1 RF/MIU",
                                REGISTER_2RF_MIU = "Register 2 RF/MIU",
                                REGISTER_1_ENCODERID = "Register 1 TP/EncoderID",
                                REGISTER_2_ENCODERID = "Register 2 TP/EncoderID",
                                REGISTER_2_DIALS = "Register 2 Dials",
                                SERVICE_INSTALLATION_REASON = "Reason for Install/Replace/Remove";
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        public virtual string MeterManufacturerSerialNumber { get; set; }

        public virtual string Manufacturer { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string MeterSerialNumber { get; set; }
        public virtual string MaterialNumber { get; set; }

        public virtual string MeterSize { get; set; }
        public virtual string MeterDeviceCategory { get; set; }

        // Meter Location
        public virtual MeterSupplementalLocation MeterLocation { get; set; }
        public virtual SmallMeterLocation MeterPositionalLocation { get; set; }

        public virtual MeterDirection MeterDirectionalLocation { get; set; }

        // RF Location
        [View(Display.READING_DEVICE_SUPPLEMENTAL)]
        public virtual MeterSupplementalLocation ReadingDeviceSupplemental { get; set; }

        [View(Display.READING_DEVICE_POSITION)]
        public virtual SmallMeterLocation ReadingDevicePosition { get; set; }

        [View(Display.READING_DEVICE_DIRECTIONAL_INFORMATION)]
        public virtual MeterDirection ReadingDeviceDirectionalInformation { get; set; }

        // Register1
        public virtual string Register1Dials { get; set; }
        public virtual string Register1UnitOfMeasure { get; set; }
        public virtual ServiceInstallationReadType Register1ReadType { get; set; }

        [View(Display.REGISTER_1RF_MIU)]
        public virtual string Register1RFMIU { get; set; }

        public virtual string Register1Size { get; set; }

        [View(Display.REGISTER_1_ENCODERID)]
        public virtual string Register1TPEncoderID { get; set; }

        public virtual string Register1CurrentRead { get; set; }

        public virtual string Register1DeviceCategory { get; set; }

        // Register2
        [View(Display.REGISTER_2_DIALS)]
        public virtual string RegisterTwoDials { get; set; }

        public virtual string Register2UnitOfMeasure { get; set; }
        public virtual ServiceInstallationReadType Register2ReadType { get; set; }

        [View(Display.REGISTER_2RF_MIU)]
        public virtual string Register2RFMIU { get; set; }

        public virtual string Register2Size { get; set; }

        [View(Display.REGISTER_2_ENCODERID)]
        public virtual string Register2TPEncoderID { get; set; }

        public virtual string Register2CurrentRead { get; set; }
        public virtual string Register2DeviceCategory { get; set; }

        //Activity 1
        public virtual ServiceInstallationFirstActivity Activity1 { get; set; }
        public virtual ServiceInstallationSecondActivity Activity2 { get; set; }
        public virtual ServiceInstallationThirdActivity Activity3 { get; set; }

        public virtual ServiceInstallationWorkType AdditionalWorkNeeded { get; set; }
        public virtual SAPWorkOrderPurpose Purpose { get; set; }
        public virtual ServiceInstallationPosition ServiceFound { get; set; }

        public virtual ServiceInstallationPosition ServiceLeft { get; set; }

        // Has an SAPCode and CodeGroup (NO - C-A-ALL3,C01, YES - C-A-ALL3,C02
        public virtual bool OperatedPointOfControl { get; set; }

        [View(Display.SERVICE_INSTALLATION_REASON)]
        public virtual ServiceInstallationReason ServiceInstallationReason { get; set; }

        [View(Display.MIU_INSTALL_REASON)]
        public virtual MiuInstallReasonCode MiuInstallReason { get; set; }

        public virtual string MeterLocationInformation { get; set; }
        public virtual string SAPErrorCode { get; set; }

        #endregion

        #region Logical Properties

        //public virtual OperatingCenter OperatingCenter => WorkOrder.OperatingCenter;

        #endregion

        #endregion
    }
}
