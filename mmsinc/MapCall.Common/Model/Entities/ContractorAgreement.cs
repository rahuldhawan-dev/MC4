using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ContractorAgreement
        : IEntityWithCreationTimeTracking, IThingWithNotes, IThingWithDocuments
    {
        #region Constants

        public struct StringLengths
        {
            public const int ACCOUNTING = 75,
                             AGREEMENT_OWNER = 50,
                             CONTRACTOR_OWNER_LAST_NAME = 50,
                             CONTRACTOR_OWNER_EMPLOYEE_ID = 50,
                             CREATED_BY = 50,
                             NJAW_CONTRACT_NUMBER = 50,
                             STRATEGIC_SOURCING_PROJECT_TRACKING_NUMBER = 255,
                             SUPPLIER_NAME = 255,
                             TITLE = 50;
        }

        public struct Display
        {
            public const string AMENDMENT_NUMBER = "Amendment #",
                                FUNCTIONAL_AREAS = "Agreement Functional Areas",
                                CONTRACTOR_WORK_CATEGORY_TYPE = "Agreement Category",
                                CONTRACTOR_AGREEMENT_STATUS_TYPE = "Agreement Status",
                                CONTRACTOR_INSURANCE = "Contractor Insurance Policy",
                                CONTRACT_OWNER_EMPLOYEE_ID = "Contract Owner EmployeeId",
                                NJAW_CONTRACT_NUMBER = "Contract #",
                                STRATEGIC_SOURCING_PROJECT_TRACKING_NUMBER = "Stategic Sourcing Project Tracking #",
                                OPERATING_CENTERS = "Operating Centers";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual Contractor Contractor { get; set; }

        public virtual ContractorCompany ContractorCompany { get; set; }

        [View(Display.CONTRACTOR_WORK_CATEGORY_TYPE)]
        public virtual ContractorWorkCategoryType ContractorWorkCategoryType { get; set; }

        [View(Display.CONTRACTOR_AGREEMENT_STATUS_TYPE)]
        public virtual ContractorAgreementStatusType ContractorAgreementStatusType { get; set; }

        [View(Display.CONTRACTOR_INSURANCE)]
        public virtual ContractorInsurance ContractorInsurance { get; set; }

        [StringLength(StringLengths.TITLE)]
        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        [StringLength(StringLengths.AGREEMENT_OWNER)]
        public virtual string AgreementOwner { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime AgreementStartDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime AgreementEndDate { get; set; }

        public virtual decimal EstimatedContractValue { get; set; }

        [StringLength(StringLengths.CREATED_BY)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [StringLength(StringLengths.NJAW_CONTRACT_NUMBER), View(Display.NJAW_CONTRACT_NUMBER)]
        public virtual string NJAWContractNumber { get; set; }

        public virtual bool? Legacy { get; set; }

        [StringLength(StringLengths.SUPPLIER_NAME)]
        public virtual string SupplierName { get; set; }

        public virtual float? AmendmentNumber { get; set; }

        [StringLength(StringLengths.STRATEGIC_SOURCING_PROJECT_TRACKING_NUMBER),
         View(Display.STRATEGIC_SOURCING_PROJECT_TRACKING_NUMBER)]
        public virtual string StrategicSourcingProjectTrackingNumber { get; set; }

        [StringLength(StringLengths.ACCOUNTING)]
        public virtual string Accounting { get; set; }

        public virtual float? EstimatedAnnualSpend { get; set; }

        public virtual float? EstimatedLifetimePayments { get; set; }

        public virtual float? EstimatedLifetimeReceipts { get; set; }

        public virtual DateTime? EstimatedTerminationDate { get; set; }

        [StringLength(StringLengths.CONTRACTOR_OWNER_EMPLOYEE_ID), 
         View(Display.CONTRACT_OWNER_EMPLOYEE_ID)]
        public virtual string ContractOwnerEmployeeId { get; set; }

        [StringLength(StringLengths.CONTRACTOR_OWNER_LAST_NAME)]
        public virtual string ContractOwnerLastName { get; set; }

        [View(Display.OPERATING_CENTERS)]
        public virtual IList<OperatingCenter> OperatingCenters { get; set; } = new List<OperatingCenter>();

        [View(Display.FUNCTIONAL_AREAS)]
        public virtual IList<ContractorFunctionalAreaType> FunctionalAreas { get; set; } = new List<ContractorFunctionalAreaType>();

        #region Notes

        public virtual IList<Note<ContractorAgreement>> Notes { get; set; } = new List<Note<ContractorAgreement>>();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(ContractorAgreement) + "s";

        #endregion

        #region Documents

        public virtual IList<Document<ContractorAgreement>> Documents { get; set; } = new List<Document<ContractorAgreement>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #endregion
    }
}
