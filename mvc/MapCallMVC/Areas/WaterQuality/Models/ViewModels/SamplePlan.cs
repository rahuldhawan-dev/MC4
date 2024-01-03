using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    public class SamplePlanViewModel : ViewModel<SamplePlan>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupply)), Required]
        public int? PWSID { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(Employee)), Required]
        public int? ContactPerson { get; set; }

        public bool Cws { get; set; }
        public bool Ntnc { get; set; }

        [StringLength(50)]
        public virtual string Name { get; set; }

        [Required]
        public virtual DateTime? MonitoringPeriodFrom { get; set; }
        [Required]
        public virtual DateTime? MonitoringPeriodTo { get; set; }

        public virtual bool Standard { get; set; }
        public virtual bool Reduced { get; set; }

        [Required]
        public virtual int? MinimumSamplesRequired { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string NameOfCertifiedLaboratory { get; set; }

        [Required]
        public virtual bool? SameAsPreviousPeriod { get; set; }

        [Required]
        public virtual bool? AllSamplesTier1 { get; set; }

        [Required]
        public virtual bool? Tier2Sites { get; set; }

        [Required]
        public virtual bool? Tier3Sites { get; set; }

        [Required]
        public virtual bool? Tier1SitesVerified { get; set; }

        [Required]
        public virtual bool? LeadServiceLines { get; set; }

        [Required]
        public virtual bool? LeadLinesVerified { get; set; }

        [Required]
        public virtual bool? FiftyPercent { get; set; }

        [Multiline]
        public virtual string Comments { get; set; }

        #endregion

        #region Constructors

        public SamplePlanViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateSamplePlan : SamplePlanViewModel
    {
        #region Constructors

		public CreateSamplePlan(IContainer container) : base(container) {}

        #endregion
	}

    public class EditSamplePlan : SamplePlanViewModel
    {
        #region Constructors

		public EditSamplePlan(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchSamplePlan : SearchSet<SamplePlan>
    {
        #region Properties

        [DropDown]
        public int? PWSID { get; set; }

        [DisplayName("System Type NTNC")]
        public bool? Ntnc { get; set; }

        [DisplayName("Reduced Monitoring")]
        public bool? Reduced { get; set; }

        public DateRange MonitoringPeriodFrom { get; set; }
        public DateRange MonitoringPeriodTo { get; set; }

        public bool? HasActiveSampleSites { get; set; }

        #endregion
    }
}