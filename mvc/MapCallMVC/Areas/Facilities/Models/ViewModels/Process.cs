using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    // No need for separate Create/Edit models.
    public class ProcessViewModel : ViewModel<Process>
    {
        #region Properties

        [Required]
        [StringLength(Process.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(ProcessStage))]
        [DropDown]
        public int? ProcessStage { get; set; }

        [Required]
        public decimal? Sequence { get; set; }

        public string ProcessOverview { get; set; }

        #endregion

        #region Constructor

        public ProcessViewModel(IContainer container) : base(container) {}

        #endregion
    }

    // This model's needed so index can have sortable columns.
    public class SearchProcess : SearchSet<Process>
    {
        
    }
}