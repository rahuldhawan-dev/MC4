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
    public class ContractorInsurance : IEntityWithCreationTimeTracking, IThingWithNotes, IThingWithDocuments
    {
        #region Constants
        
        public struct Display
        {
            public const string CONTRACTOR_INSURANCE_MINUMUM_REQUIREMENT = "Coverage";
        }

        public struct StringLengths
        {
            public const int INSURANCE_PROVIDER = 50,
                             POLICY_NUMBER = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual Contractor Contractor { get; set; }

        [StringLength(StringLengths.INSURANCE_PROVIDER)]
        public virtual string InsuranceProvider { get; set; }

        public virtual bool? MeetsCurrentContractualLimits { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime EffectiveDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime TerminationDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [View(Display.CONTRACTOR_INSURANCE_MINUMUM_REQUIREMENT)]
        public virtual ContractorInsuranceMinimumRequirement ContractorInsuranceMinimumRequirement { get; set; }

        [StringLength(StringLengths.POLICY_NUMBER)]
        public virtual string PolicyNumber { get; set; }

        #region Notes

        public virtual IList<Note<ContractorInsurance>> Notes { get; set; } = new List<Note<ContractorInsurance>>();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(ContractorInsurance);

        #endregion

        #region Documents

        public virtual IList<Document<ContractorInsurance>> Documents { get; set; } = new List<Document<ContractorInsurance>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #endregion
    }
}
