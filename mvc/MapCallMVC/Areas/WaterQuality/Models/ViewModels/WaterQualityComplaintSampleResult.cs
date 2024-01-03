using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    public class WaterQualityComplaintSampleResultViewModel : ViewModel<WaterQualityComplaintSampleResult>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(WaterConstituent)), EntityMap]
        public virtual int? WaterConstituent { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? SampleDate { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string SampleValue { get; set; }

        [StringLength(25)]
        public virtual string UnitOfMeasure { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string AnalysisPerformedBy { get; set; }

        #endregion

        #region Constructors

        public WaterQualityComplaintSampleResultViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateWaterQualityComplaintSampleResult : WaterQualityComplaintSampleResultViewModel
    {
        #region Constructors

		public CreateWaterQualityComplaintSampleResult(IContainer container) : base(container) {}

        #endregion
	}

    public class EditWaterQualityComplaintSampleResult : WaterQualityComplaintSampleResultViewModel
    {
        #region Constructors

		public EditWaterQualityComplaintSampleResult(IContainer container) : base(container) {}

        #endregion
	}
}