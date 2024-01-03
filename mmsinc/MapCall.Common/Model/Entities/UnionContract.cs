using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class UnionContract : IThingWithNotes, IThingWithDocuments, IEntityLookup, IThingWithOperatingCenter
    {
        #region Private Members

        private UnionContractDisplayItem _display;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual float? PercentIncreaseYr1 { get; set; }
        public virtual float? PercentIncreaseYr2 { get; set; }
        public virtual float? PercentIncreaseYr3 { get; set; }
        public virtual float? PercentIncreaseYr4 { get; set; }
        public virtual float? PercentIncreaseYr5 { get; set; }
        public virtual float? PercentIncreaseYr6 { get; set; }
        public virtual DateTime? NewContractExpirationDate { get; set; }
        public virtual DateTime? NewContractEffectiveDate { get; set; }

        [StringLength(50)]
        public virtual string TermOfContract { get; set; }

        public virtual DateTime? DateOfMoa { get; set; }
        public virtual string CompanyNegotiatingCommittee { get; set; }
        public virtual string UnionNegotiatingCommittee { get; set; }
        public virtual bool? ContractExtended { get; set; }
        public virtual DateTime? ContractExtensionDate { get; set; }
        public virtual string CompanyKeyObjectivesSummary { get; set; }
        public virtual float? RatificationVoteFor { get; set; }
        public virtual float? RatificationVoteAgainst { get; set; }
        public virtual float? TotalBargainingUnitMembers { get; set; }
        public virtual string Notes { get; set; }
        public virtual bool? Retroactivity { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? EndDate { get; set; }

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Local Local { get; set; }

        // NOTE: Not called Notes because of the other Notes property.
        public virtual IList<Note<UnionContract>> UnionContractNotes { get; set; }
        public virtual IList<Document<UnionContract>> Documents { get; set; }
        public virtual IList<Grievance> Grievances { get; set; }
        public virtual IList<UnionContractProposal> Proposals { get; set; }

        #endregion

        #region Logical Properties

        public virtual string Description => (_display ?? (_display = new UnionContractDisplayItem {
            OperatingCenter = OperatingCenter?.OperatingCenterCode,
            Local = Local?.Description,
            StartDate = StartDate,
            EndDate = EndDate
        })).Display;

        // This is used only for a report pdf.
        public virtual string DescriptionWithoutDates => String.Format("{0} - {1}",
            OperatingCenter == null ? String.Empty : OperatingCenter.OperatingCenterCode,
            Local == null ? String.Empty : Local.Description);

        public virtual string TableName => FixUnionContractsTableAndColumnNamesForBug1702.NEW_TABLE_NAME;

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => UnionContractNotes.Cast<INoteLink>().ToList();

        [Display(Name = "Documents")]
        public virtual int DocumentCount => Documents.Count;

        [Display(Name = "Notes")]
        public virtual int NoteCount => UnionContractNotes.Count;

        [Display(Name = "Grievances")]
        public virtual int GrievanceCount => Grievances.Count;

        [Display(Name = "Active")]
        public virtual int ActiveGrievanceCount
        {
            get { return Grievances.Count(g => g.Status.Description == "Active"); }
        }

        [Display(Name = "Compromise Settlement")]
        public virtual int CompromiseSettlementGrievanceCount
        {
            get { return Grievances.Count(g => g.Status.Description == "Compromise Settlement"); }
        }

        [Display(Name = "For Information Only")]
        public virtual int ForInformationOnlyGrievanceCount
        {
            get { return Grievances.Count(g => g.Status.Description == "For Information Only"); }
        }

        [Display(Name = "Non-Grievable Returned")]
        public virtual int NonGrievableReturnedGrievanceCount
        {
            get
            {
                return
                    Grievances.Count(
                        g =>
                            g.Status.Description == "Non Grievable-Returned" ||
                            g.Status.Description == "Non-Grievable Returned");
            }
        }

        [Display(Name = "Sustained")]
        public virtual int SustainedGrievanceCount
        {
            get { return Grievances.Count(g => g.Status.Description == "Sustained"); }
        }

        [Display(Name = "Sustained Company Position")]
        public virtual int SustainedCompanyPositionGrievanceCount
        {
            get { return Grievances.Count(g => g.Status.Description == "Sustained Company Position"); }
        }

        [Display(Name = "Sustained Union Position")]
        public virtual int SustainedUnionPositionGrievanceCount
        {
            get { return Grievances.Count(g => g.Status.Description == "Sustained Union Position"); }
        }

        [Display(Name = "Withdrawn by Union")]
        public virtual int WithdrawnByUnionGrievanceCount
        {
            get { return Grievances.Count(g => g.Status.Description == "Withdrawn by Union"); }
        }

        [Display(Name = "Active-Arbitration")]
        public virtual int ActiveArbitrationGrievanceCount
        {
            get { return Grievances.Count(g => g.Status.Description == "Active-Arbitration"); }
        }

        #endregion

        #endregion

        #region Constructors

        public UnionContract()
        {
            UnionContractNotes = new List<Note<UnionContract>>();
            Documents = new List<Document<UnionContract>>();
            Grievances = new List<Grievance>();
            Proposals = new List<UnionContractProposal>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class UnionContractDisplayItem : DisplayItem<UnionContract>
    {
        [SelectDynamic("OperatingCenterCode")]
        public string OperatingCenter { get; set; }

        [SelectDynamic("Description")]
        public string Local { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override string Display => $"{OperatingCenter} - {Local} - {StartDate} - {EndDate}";
    }
}
