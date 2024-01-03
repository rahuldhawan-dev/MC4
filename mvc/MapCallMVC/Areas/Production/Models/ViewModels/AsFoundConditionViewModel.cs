using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AsFoundConditionViewModel : EntityLookupViewModel<AsFoundCondition> 
    {
        #region Constructors

        public AsFoundConditionViewModel(IContainer container) : base(container) { }

        #endregion
    }
}