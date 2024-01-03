using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.CustomerOrder;

namespace MapCall.SAP.Model.Entities
{
    public class SAPCustomerOrder
    {
        #region Properties

        public virtual string SAPErrorCode { get; set; }
        public virtual FRS frs { get; set; }
        public virtual CustomerOrderWorkOrder customerOrderWorkOrder { get; set; }

        #endregion

        #region Exposed Methods

        public CustomerOrder_FSRQuery CustomerOrderRequest(SearchSapCustomerOrder search)
        {
            CustomerOrder_FSRQuery request = new CustomerOrder_FSRQuery();
            request.SourceIdentifier = "MAPCALL";
            request.FSR_WO = new CustomerOrder_FSRQueryFSR_WO();
            request.FSR_WO.FSR_ID = search.FSR_ID;
            request.FSR_WO.WorkOrder = search.WorkOrder;

            return request;
        }

        public SAPCustomerOrder(CustomerOrder_FSRInfoMapCall_ResponseRecord FRSRecorders)
        {
            frs = new FRS();
            frs.WorkOrderNumber = FRSRecorders.WorkOrder;
            frs.OperationNumber = FRSRecorders.OperationNo;
            frs.AssignmentStart = FRSRecorders.AssignmentStart;
            frs.AssignmentFinish = FRSRecorders.AssignmentFinish;
            frs.Priority = FRSRecorders.Priority;
            frs.Status = FRSRecorders.Status;
            frs.AssignedEngineer = FRSRecorders.AssignedEngineer;
            SAPErrorCode = "Successful";
        }

        public SAPCustomerOrder(CustomerOrder_FSRInfoMapCall_ResponseResponse_WorkOrder WorkOrderRecorders)
        {
            customerOrderWorkOrder = new CustomerOrderWorkOrder();

            customerOrderWorkOrder.EquipText = WorkOrderRecorders.EquipText;
            customerOrderWorkOrder.EquipmentID = WorkOrderRecorders.EquipmentID;
            customerOrderWorkOrder.FunctionalLocation = WorkOrderRecorders.FunctionalLocation;
            customerOrderWorkOrder.InstallationDate = WorkOrderRecorders.InstallationDate;
            customerOrderWorkOrder.HouseNumber = WorkOrderRecorders.HouseNumber;
            customerOrderWorkOrder.Street = WorkOrderRecorders.Street;
            customerOrderWorkOrder.Criticality = WorkOrderRecorders.Criticality;
            customerOrderWorkOrder.YearMFG = WorkOrderRecorders.YearMFG;
            customerOrderWorkOrder.Latitude = WorkOrderRecorders.Latitude;
            customerOrderWorkOrder.Longitude = WorkOrderRecorders.Longitude;
            customerOrderWorkOrder.Manufacturer = WorkOrderRecorders.Manufacturer;
            customerOrderWorkOrder.EquipmentLongText = WorkOrderRecorders.EquipLongText;
            customerOrderWorkOrder.SafetyConcernDescr = WorkOrderRecorders.SafetyConcernDescr;
            customerOrderWorkOrder.NotificationNumber = WorkOrderRecorders.NotificationNumber;
            customerOrderWorkOrder.NotificationShortText = WorkOrderRecorders.NotificationShortText;
            customerOrderWorkOrder.NotificationLongText = WorkOrderRecorders.NotificationLongText;
            customerOrderWorkOrder.Priority = WorkOrderRecorders.Priority;
            customerOrderWorkOrder.BackReportingType = WorkOrderRecorders.BackReportingType;
            customerOrderWorkOrder.CreateTime = WorkOrderRecorders.CreateTime;
            customerOrderWorkOrder.CreateDate = WorkOrderRecorders.CreateDate;
            customerOrderWorkOrder.AsFoundCode = WorkOrderRecorders.AsFoundCode;
            customerOrderWorkOrder.CurbBox_Longitude = WorkOrderRecorders.CurbBox_Longitude;
            customerOrderWorkOrder.CurbBox_Latitude = WorkOrderRecorders.CurbBox_Latitude;
            customerOrderWorkOrder.WorkOrderNumber = WorkOrderRecorders.WorkOrder;
            customerOrderWorkOrder.Status = WorkOrderRecorders.Status;
            customerOrderWorkOrder.WBSElement = WorkOrderRecorders.WBSElement;
            customerOrderWorkOrder.PlanningPlant = WorkOrderRecorders.PlanningPlant;
            customerOrderWorkOrder.WorkOrderText = WorkOrderRecorders.WorkOrderText;
            customerOrderWorkOrder.OperationText = WorkOrderRecorders.OperationText;
            customerOrderWorkOrder.Customer = WorkOrderRecorders.Customer;
            customerOrderWorkOrder.CustomerPhone = WorkOrderRecorders.CustomerPhone;
            customerOrderWorkOrder.CustomerAlternatePhone = WorkOrderRecorders.CustomerAlternatePhone;
            customerOrderWorkOrder.CustomerMailingAddress = WorkOrderRecorders.CustomerMailingAddress;
            customerOrderWorkOrder.CrossStreet = WorkOrderRecorders.CrossStreet;
            customerOrderWorkOrder.IsWorkRequired = WorkOrderRecorders.IsWorkRequired;
            customerOrderWorkOrder.NextReplacementYear = WorkOrderRecorders.NextReplacementYear;
            customerOrderWorkOrder.ServiceType = WorkOrderRecorders.ServiceType;
            customerOrderWorkOrder.SerialNumber = WorkOrderRecorders.SerialNumber;
            customerOrderWorkOrder.MFGSerialNum = WorkOrderRecorders.MFGSerialNum;
            customerOrderWorkOrder.IsCustomerEnrolledForEmail = WorkOrderRecorders.IsCustomerEnrolledForEmail;

            if (WorkOrderRecorders.DependencyNode != null && WorkOrderRecorders.DependencyNode.Any())
            {
                var dependencyNode =
                    from d in WorkOrderRecorders.DependencyNode
                    select new DependencyNode {
                        CodeGroup = d.CodeGroup,
                        Code = d.Code,
                        CodeText = d.CodeText,
                        DependencyText = d.DependencyText,
                        ValidDate = d.ValidDate,
                        ExpirationDate = d.ExpirationDate,
                        DependencyID = d.DependencyID
                    };
                customerOrderWorkOrder.DependencyNodes = dependencyNode.ToList();
            }

            if (WorkOrderRecorders.SecurityThreatNode != null && WorkOrderRecorders.SecurityThreatNode.Any())
            {
                var securityThreatNode =
                    from s in WorkOrderRecorders.SecurityThreatNode
                    select new SecurityThreatNode {
                        Threat_Notif_No = s.Threat_Notif_No,
                        Address_security = s.Address_security,
                        ISPendingInvestigation = s.ISPendingInvestigation,
                        ISPoliceEscort = s.ISPoliceEscort,
                        CreationDate = s.CreationDate,
                    };

                customerOrderWorkOrder.SecurityThreatNodes = securityThreatNode.ToList();
            }

            SAPErrorCode = "Successful";
        }

        public SAPCustomerOrder() { }

        #endregion
    }

    public class FRS
    {
        #region Properties

        public virtual string WorkOrderNumber { get; set; }
        public virtual string OperationNumber { get; set; }
        public virtual string AssignmentStart { get; set; }
        public virtual string AssignmentFinish { get; set; }
        public virtual string Priority { get; set; }
        public virtual string Status { get; set; }
        public virtual string AssignedEngineer { get; set; }

        #endregion
    }

    public class CustomerOrderWorkOrder
    {
        #region Properties

        public virtual string EquipText { get; set; }
        public virtual string EquipmentID { get; set; }
        public virtual string FunctionalLocation { get; set; }
        public virtual string InstallationDate { get; set; }
        public virtual string HouseNumber { get; set; }
        public virtual string Street { get; set; }
        public virtual string Criticality { get; set; }
        public virtual string YearMFG { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string EquipmentLongText { get; set; }
        public virtual string SafetyConcernDescr { get; set; }
        public virtual string NotificationNumber { get; set; }
        public virtual string NotificationShortText { get; set; }
        public virtual string NotificationLongText { get; set; }
        public virtual string Priority { get; set; }
        public virtual string BackReportingType { get; set; }
        public virtual string CreateTime { get; set; }
        public virtual string CreateDate { get; set; }
        public virtual string AsFoundCode { get; set; }
        public virtual string CurbBox_Longitude { get; set; }
        public virtual string CurbBox_Latitude { get; set; }
        public virtual string WorkOrderNumber { get; set; }
        public virtual string Status { get; set; }
        public virtual string WBSElement { get; set; }
        public virtual string PlanningPlant { get; set; }
        public virtual string WorkOrderText { get; set; }
        public virtual string OperationText { get; set; }
        public virtual string Customer { get; set; }
        public virtual string CustomerPhone { get; set; }
        public virtual string CustomerAlternatePhone { get; set; }
        public virtual string CustomerMailingAddress { get; set; }
        public virtual string CrossStreet { get; set; }
        public virtual string IsWorkRequired { get; set; }
        public virtual string NextReplacementYear { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string MFGSerialNum { get; set; }
        public virtual string IsCustomerEnrolledForEmail { get; set; }
        public virtual IEnumerable<DependencyNode> DependencyNodes { get; set; }
        public virtual IEnumerable<SecurityThreatNode> SecurityThreatNodes { get; set; }

        #endregion
    }

    public class DependencyNode
    {
        public virtual string CodeGroup { get; set; }
        public virtual string Code { get; set; }
        public virtual string CodeText { get; set; }
        public virtual string DependencyText { get; set; }
        public virtual string ValidDate { get; set; }
        public virtual string ExpirationDate { get; set; }
        public virtual string DependencyID { get; set; }
    }

    public class SecurityThreatNode
    {
        public virtual string Threat_Notif_No { get; set; }
        public virtual string Address_security { get; set; }
        public virtual string ISPendingInvestigation { get; set; }
        public virtual string ISPoliceEscort { get; set; }
        public virtual string CreationDate { get; set; }
    }
}
