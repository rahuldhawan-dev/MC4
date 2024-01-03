using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchTaskGroupCategory : SearchSet<TaskGroupCategory>
    {
        #region Properties

        [View(TaskGroupCategory.Display.CATEGORY_TYPE)]
        public string Type { get; set; }
        
        public bool? IsActive { get; set; }

        #endregion
    }
}
