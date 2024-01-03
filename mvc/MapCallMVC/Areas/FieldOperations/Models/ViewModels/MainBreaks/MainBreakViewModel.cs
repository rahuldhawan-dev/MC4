using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.MainBreaks
{
    public class MainBreakViewModel : ViewModel<MainBreak>
    {
        #region Constructors

        public MainBreakViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(ServiceSize)), DropDown]
        public int? ServiceSize { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(MainBreakMaterial)), DropDown]
        public int? MainBreakMaterial { get; set; }
        
        public int? FootageReplaced { get; set; }

        [EntityMap, EntityMustExist(typeof(MainBreakMaterial)), DropDown]
        public int? ReplacedWith { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(MainCondition)), DropDown]
        public int? MainCondition { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(MainFailureType)), DropDown]
        public int? MainFailureType { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(MainBreakSoilCondition)), DropDown]
        public int? MainBreakSoilCondition { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(MainBreakDisinfectionMethod)), DropDown]
        public int? MainBreakDisinfectionMethod { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(MainBreakFlushMethod)), DropDown]
        public int? MainBreakFlushMethod { get; set; }

        [Required]
        public decimal? Depth { get; set; }

        [Required]
        public int? CustomersAffected { get; set; }

        [Required]
        public decimal? ShutdownTime { get; set; }

        [Required, Max(4, ErrorMessage = "Must be less than 4. Must be in steps of .1")]
        public decimal? ChlorineResidual { get; set; }

        [CheckBox]
        public bool? BoilAlertIssued { get; set; }

        #endregion
    }
}
