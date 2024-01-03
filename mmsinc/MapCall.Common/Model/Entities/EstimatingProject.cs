using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Utilities;
using StructureMap;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EstimatingProject : IThingWithDocuments, IThingWithNotes, IThingWithOperatingCenter
    {
        #region Fields

        [NonSerialized] private IContainer _container;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [Required, Range(0, 100), DisplayFormat(DataFormatString = "{0}%")]
        public virtual int OverheadPercentage { get; set; }

        [Required, Range(0, 100), DisplayFormat(DataFormatString = "{0}%")]
        public virtual int ContingencyPercentage { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal? LumpSum { get; set; }

        public virtual string WBSNumber { get; set; }

        [Required, StringLength(CreateTablesForBug1774.StringLengths.EstimatingProjects.PROJECT_NUMBER),
         Display(Name = "Preliminary File Number")]
        public virtual string ProjectNumber { get; set; }

        [Required, StringLength(CreateTablesForBug1774.StringLengths.EstimatingProjects.PROJECT_NAME)]
        public virtual string ProjectName { get; set; }

        [Required, StringLength(CreateTablesForBug1774.StringLengths.EstimatingProjects.STREET)]
        public virtual string Street { get; set; }

        [StringLength(MakeDescriptionFieldLongerForBug1968.STRING_LENGTH)]
        public virtual string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime EstimateDate { get; set; }

        public virtual string Remarks { get; set; }

        [StringLength(AddJDEPayrollNumberToEstimatingProjectsForBug2340.LENGTH,
            MinimumLength = AddJDEPayrollNumberToEstimatingProjectsForBug2340.LENGTH)]
        public virtual string JDEPayrollNumber { get; set; }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        public virtual string CreatedBy { get; set; }

        #endregion

        #region References

        [Required]
        public virtual EstimatingProjectType ProjectType { get; set; }

        [Required]
        public virtual Town Town { get; set; }

        [Required]
        public virtual OperatingCenter OperatingCenter { get; set; }

        [Required]
        public virtual Employee Estimator { get; set; }

        public virtual Contractor Contractor { get; set; }

        public virtual IList<EstimatingProjectContractorLaborCost> ContractorLaborCosts { get; set; }
        public virtual IList<EstimatingProjectOtherCost> OtherCosts { get; set; }
        public virtual IList<EstimatingProjectMaterial> Materials { get; set; }
        public virtual IList<EstimatingProjectCompanyLaborCost> CompanyLaborCosts { get; set; }
        public virtual IList<EstimatingProjectPermit> Permits { get; set; }
        public virtual IList<EstimatingProjectDocument> EstimatingProjectDocuments { get; set; }
        public virtual IList<EstimatingProjectNote> EstimatingProjectNotes { get; set; }

        #endregion

        #region Logical Properties

        #region Costs

        #region Project Specific

        public virtual decimal ContingencyPercentageAsDecimal => ContingencyPercentage * 0.01m;

        public virtual decimal OverheadPercentageAsDecimal => OverheadPercentage * 0.01m;

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal TotalOtherCost
        {
            get { return OtherCosts.Sum(c => c.TotalCost); }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal TotalMaterialCost
        {
            get { return Materials.Sum(c => c.TotalCost); }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal TotalCompanyLaborCost
        {
            get { return CompanyLaborCosts.Sum(c => c.TotalCost); }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal TotalContractorLaborCost
        {
            get { return ContractorLaborCosts.Where(c => c.TotalCost.HasValue).Sum(c => c.TotalCost.Value); }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal TotalPermitCost
        {
            get { return Permits.Sum(c => c.TotalCost); }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal EstimatedConstructionCost =>
            TotalMaterialCost + TotalContractorLaborCost + TotalPermitCost + TotalCompanyLaborCost +
            TotalOtherCost;

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal ContingencyCost => EstimatedConstructionCost * ContingencyPercentageAsDecimal;

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal OverheadCost => EstimatedConstructionCost * OverheadPercentageAsDecimal;

        [DisplayFormat(DataFormatString = CommonStringFormats.MONEY)]
        public virtual decimal TotalEstimatedCost =>
            EstimatedConstructionCost + OverheadCost + ContingencyCost + (LumpSum ?? 0);

        #endregion

        #region Asset Specific

        public enum CostType
        {
            Material,
            ContractorLabor,
            CompanyLabor,
            Permit,
            Other
        }

        #endregion

        #endregion

        #region Notes/Documents

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return EstimatingProjectDocuments.Map(td => (IDocumentLink)td); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return EstimatingProjectNotes.Map(n => (INoteLink)n); }
        }

        public virtual string TableName => CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS;

        #endregion

        public virtual bool IsNonFramework => ProjectType.Description == "Non-Framework";

        // Group the materials so we can list totals regardless of asset type.
        public virtual IList<EstimatingProjectMaterial> GroupedMaterials
        {
            get
            {
                return (from m in Materials
                        group m by new {m.Material, Cost = m.MaterialCost}
                        into x
                        select new EstimatingProjectMaterial {
                            MaterialCost = x.Key.Cost,
                            Material = x.Key.Material,
                            Quantity = x.Sum(y => y.Quantity)
                        }).ToList();
            }
        }

        // Group the public virtual IList<EstimatingProjectContractorLaborCost 
        public virtual IList<EstimatingProjectContractorLaborCost> GroupedContractorLaborCosts
        {
            get
            {
                return (from c in ContractorLaborCosts
                        group c by c.ContractorLaborCost
                        into clc
                        select new EstimatingProjectContractorLaborCost {
                            ContractorLaborCost = clc.Key,
                            Quantity = clc.Sum(k => k.Quantity),
                            EstimatingProject = this
                        }).Select(x => {
                    _container.BuildUp(x);
                    return x;
                }).ToList();
                // NOTE: _container.BuildUp needs to be called in order to inject
                // an IDateTimeProvider into each instance. Some properties use that.
            }
        }

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IContainer Container
        {
            set => _container = value;
        }

        #endregion

        #endregion

        #region Constructors

        public EstimatingProject()
        {
            ContractorLaborCosts = new List<EstimatingProjectContractorLaborCost>();
            OtherCosts = new List<EstimatingProjectOtherCost>();
            Materials = new List<EstimatingProjectMaterial>();
            CompanyLaborCosts = new List<EstimatingProjectCompanyLaborCost>();
            Permits = new List<EstimatingProjectPermit>();

            EstimatingProjectDocuments = new List<EstimatingProjectDocument>();
            EstimatingProjectNotes = new List<EstimatingProjectNote>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return String.Format("{0} - {1} - {2}", Id, ProjectNumber, ProjectName);
        }

        #endregion
    }

    [Serializable]
    public class EstimatingProjectDisplayItem : DisplayItem<EstimatingProject>
    {
        [SelectDynamic("OperatingCenterCode")]
        public string OperatingCenter { get; set; }

        [SelectDynamic("ShortName")]
        public string Town { get; set; }

        public string Street { get; set; }

        public string WBSNumber { get; set; }

        public override string Display => $"{OperatingCenter}-{Town}-{Street}-{WBSNumber} [{Id}]";
    }
}
