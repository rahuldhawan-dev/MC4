using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.SAP.TechnicalMasterWS;

namespace MapCall.SAP.Model.Entities
{
    public class SAPTechnicalMasterAccount
    {
        #region Properties

        public virtual string DeviceSerialNumber { get; set; }
        public virtual string DeviceLocation { get; set; }
        public virtual string Installation { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string AccountStatusAfterReview { get; set; }
        public virtual string Customer { get; set; }
        public virtual string Owner { get; set; }
        public virtual string BillingClassificationMapCall { get; set; }
        public virtual string CustomerEmail { get; set; }
        public virtual string Phone { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual string SAPError { get; set; }
        public virtual string MeterSizeSAPCode { get; set; }
        public virtual string InstallationTypeSAP { get; set; }
        public virtual string ServiceSize { get; set; }
        public virtual string SAPEquipmentNumber { get; set; }
        public virtual string MeterSerialNumber { get; set; }
        public virtual string CriticalCareType { get; set; }

        #region Logical properties

        public virtual string InstallationTypeMapCall
        {
            get
            {
                switch (InstallationTypeSAP)
                {
                    case "FPUB":
                        return "Public Fire Service";
                    case "FPVT":
                        return "Private Fire Service";
                    case "IRRG":
                        return "Irrigation";
                    case "WATR":
                        return "Domestic Water";
                    case "WWTR":
                        return "Domestic Wastewater";
                    case "WT":
                        return "Water Service";
                    case "SW":
                        return "Waste Water";
                    case "FS":
                        return "Fire Service";
                    case "BULK":
                        return "Bulk Water";
                    case "BUMA":
                        return "Bulk Water Master";
                    case "DISC":
                        return "Discharge Water";
                    case "DVST":
                        return "Divestiture Service/Sold";
                    case "FLAT":
                        return "Flat Rate";
                    case "FREE":
                        return "Free Water";
                    case "FSDC":
                        return "Fire Service Detector Check";
                    case "GREY":
                        return "Grey Water";
                    case "MB1C":
                        return "Master/Battery w/Primary Charg";
                    case "MBAC":
                        return "Master/Battery w/all Charges";
                    case "NON":
                        return "Not-Potable";
                    case "RUSE":
                        return "Reuse/recycled water";
                    case "TEMP":
                        return "Temporary Hydrant Meter";
                    case "USER":
                        return "User Agreement";
                    case "WWDT":
                        return "Wastewater w/Deduct Service";
                    default:
                        return string.Empty;
                }
            }
        }

        public virtual string MeterSizeMapCall
        {
            get
            {
                switch (MeterSizeSAPCode)
                {
                    case "0001":
                        return "5/8";
                    case "0002":
                        return "3/4";
                    case "0003":
                        return "1";
                    case "0004":
                        return "1 1/2";
                    case "0005":
                        return "2";
                    case "0006":
                        return "3";
                    case "0007":
                        return "4";
                    case "0008":
                        return "6";
                    case "0009":
                        return "8";
                    case "0010":
                        return "10";
                    case "0011":
                        return "12";
                    case "0012":
                        return "1/2";
                    case "0013":
                        return "14";
                    case "0014":
                        return "16";
                    case "0015":
                        return "5";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        #endregion

        #region Exposed Methods

        public SAPTechnicalMasterAccount(TechnicalMaster_AccountDetailsInfoRecord technicalMasterDataResponseRecord)
        {
            DeviceSerialNumber = technicalMasterDataResponseRecord.Device;
            DeviceLocation = technicalMasterDataResponseRecord.DeviceLocation;
            Installation = technicalMasterDataResponseRecord.Installation;
            AccountNumber = technicalMasterDataResponseRecord.AccountNo;
            AccountStatusAfterReview = technicalMasterDataResponseRecord.AccountStatusAfterReview;
            Customer = technicalMasterDataResponseRecord.Customer;
            Owner = technicalMasterDataResponseRecord.Owner;
            BillingClassificationMapCall = technicalMasterDataResponseRecord.BillingClassification;
            CustomerEmail = technicalMasterDataResponseRecord.CustomerEmail;
            Phone = technicalMasterDataResponseRecord.Phone;
            MobilePhone = technicalMasterDataResponseRecord.MobilePhone;
            MeterSizeSAPCode = technicalMasterDataResponseRecord.MeterSize;
            InstallationTypeSAP = technicalMasterDataResponseRecord.InstallationType;
            ServiceSize = technicalMasterDataResponseRecord.ServiceSize;
            SAPEquipmentNumber = technicalMasterDataResponseRecord.Equipment;
            MeterSerialNumber = technicalMasterDataResponseRecord.ManufacturerSerialNo;
            CriticalCareType = technicalMasterDataResponseRecord.CriticalCareType;
            SAPError = "Successful";
        }

        public SAPTechnicalMasterAccount() { }

        #endregion
    }
}
