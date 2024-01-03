using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class TaskGroupCategoryViewModel : ViewModel<TaskGroupCategory>
    {
        #region Properties

        [Required]
        [StringLength(TaskGroupCategory.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [Required]
        [StringLength(TaskGroupCategory.StringLengths.TYPE)]
        public string Type { get; set; }

        [Required]
        [StringLength(TaskGroupCategory.StringLengths.ABBREVIATION)]
        public string Abbreviation { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        #endregion

        #region Constructors

        public TaskGroupCategoryViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
