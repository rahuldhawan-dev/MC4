using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{
    public class EditEmployeeHeadCount : EmployeeHeadCountViewModel
    {
        #region Constructor

        public EditEmployeeHeadCount(IContainer container) : base(container) { }

        #endregion
    }
}