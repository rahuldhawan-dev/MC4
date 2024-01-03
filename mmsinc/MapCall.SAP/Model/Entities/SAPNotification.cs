using System;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPNotification
    {
        #region Properties

        #region Logical properties

        public virtual string MapCallPriority
        {
            get
            {
                switch (Priority)
                {
                    case "1: Emergency 1-2 Hrs":
                    case "2: Emergency 24 Hrs.":
                        return "Emergency";
                    case "3: One Business Day":
                    case "4: Within 5 Days":
                    case "5: Within 15 Days":
                        return "High Priority";
                    case "6: Within 30 Days":
                    case "7: Within 90 Days":
                    case "8: Within 160 Days":
                        return "Routine";
                    default:
                        return string.Empty;
                }
            }
        }

        public virtual string Purpose
        {
            get
            {
                switch (CodingGroupCodeDescription)
                {
                    case "Customer (Complaint/Request)":
                        return "Customer";
                    case "Safety":
                        return "Safety";
                    case "AW Compliance":
                        return "Compliance";
                    case "Equipment Reliability":
                    case "Regulatory":
                    case "Seasonal":
                    case "Leak Detection":
                    case "Revenue $150-$500":
                    case "Revenue $501-$1000":
                    case "Revenue >$1000":
                    case "Damaged/Billable":
                    case "Estimates":
                    case "Water Quality":
                    case "Asset Record Control":
                    case "Demolition":
                    case "Locate":
                    case "Clean Out":
                        return CodingGroupCodeDescription;
                    case "Developer - Install Service":
                        return "Construction Project";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        #region WebService Request Properties

        public virtual string Remarks { get; set; }
        public virtual string PlanningPlant { get; set; }
        public virtual string DateCreatedFrom { get; set; }
        public virtual string DateCreatedTo { get; set; }
        public virtual string CreateWorkOrderNotificationNumber { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string NotificationType { get; set; }
        public virtual string AssetType { get; set; }
        public virtual string NotificationTypeText { get; set; }
        public virtual string SAPNotificationNumber { get; set; }
        public virtual string NotificationShortText { get; set; }
        public virtual string NotificationLongText { get; set; }
        public virtual string SpecialInstructions { get; set; }
        public virtual string ReportedBy { get; set; }
        public virtual string AccountNumberOfCustomer { get; set; }
        public virtual string Telephone { get; set; }
        public virtual string FunctionalLocation { get; set; }
        public virtual string Installation { get; set; }
        public virtual string Locality { get; set; }
        public virtual string LocalityDescription { get; set; }
        public virtual string Equipment { get; set; }
        public virtual string Premise { get; set; }
        public virtual string CodingGroupCodeDescription { get; set; }
        public virtual string DateCreated { get; set; }
        public virtual string TimeCreated { get; set; }
        public virtual string Priority { get; set; }
        public virtual string AddressNumber { get; set; }
        public virtual string House { get; set; }
        public virtual string Street1 { get; set; }
        public virtual string Street2 { get; set; }
        public virtual string Street5 { get; set; }
        public virtual string City { get; set; }
        public virtual string OtherCity { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string CityPostalCode { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }
        public virtual string SAPErrorCode { get; set; }
        public virtual string UserStatus { get; set; }
        public virtual string SystemStatus { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string PurposeCodingGroup { get; set; }
        public virtual string PurposeCodingCode { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public SAPNotification() { }

        public SAPNotification(ReceiveNotificationNotifications receiveNotificationNotifications)
        {
            NotificationType = receiveNotificationNotifications.NotificationType;
            AssetType = receiveNotificationNotifications.AssetType;
            NotificationTypeText = receiveNotificationNotifications.NotificationDescription;
            SAPNotificationNumber = receiveNotificationNotifications.SAPNotificationNo?.TrimStart('0');
            NotificationShortText = receiveNotificationNotifications.ShortText;
            NotificationLongText = receiveNotificationNotifications.LongText;
            //ReportedBy = (receiveNotificationNotifications.CustomerName !="" && receiveNotificationNotifications.CustomerName != null)?  receiveNotificationNotifications.CustomerName : receiveNotificationNotifications.ReportedBy;
            ReportedBy = receiveNotificationNotifications.ReportedBy;
            CustomerName = receiveNotificationNotifications.CustomerName;
            PlanningPlant = receiveNotificationNotifications.PlanningPlant;
            AccountNumberOfCustomer = receiveNotificationNotifications.CustomerName;
            Telephone = receiveNotificationNotifications.Telephone;
            FunctionalLocation = receiveNotificationNotifications.FunctionalLOC;
            Equipment = receiveNotificationNotifications.Equipment;
            Premise = receiveNotificationNotifications.Premise;
            CodingGroupCodeDescription = receiveNotificationNotifications.CodingGroupCodeDescription;
            PurposeCodingCode = receiveNotificationNotifications.SAPCode;
            PurposeCodingGroup = receiveNotificationNotifications.SAPCodingGroup;
            DateCreated = receiveNotificationNotifications.DateCreated;
            TimeCreated = receiveNotificationNotifications.TimeCreated;
            Priority = receiveNotificationNotifications.Priority;
            AddressNumber = receiveNotificationNotifications.Address;
            House = receiveNotificationNotifications.House;
            Street1 = receiveNotificationNotifications.Street1;
            Street2 = receiveNotificationNotifications.Street2;
            Street5 = receiveNotificationNotifications.Street5;
            City = receiveNotificationNotifications.City;
            OtherCity = receiveNotificationNotifications.OtherCity;
            State = receiveNotificationNotifications.State;
            Country = receiveNotificationNotifications.Country;
            CityPostalCode = receiveNotificationNotifications.PostalCode;
            Latitude = receiveNotificationNotifications.Latitude;
            Longitude = receiveNotificationNotifications.Longitude;
            UserStatus = receiveNotificationNotifications.UserStatus;
            SystemStatus = receiveNotificationNotifications.SystemStatus;
            SAPErrorCode = "Successful";
            Installation = receiveNotificationNotifications.Installation;
            Locality = receiveNotificationNotifications.Locality;
            LocalityDescription = receiveNotificationNotifications.LocalityDescription;
            SpecialInstructions = receiveNotificationNotifications.CriticalNotes; // field is called CriticalNotes on SAP side
        }

        #endregion
    }
}
