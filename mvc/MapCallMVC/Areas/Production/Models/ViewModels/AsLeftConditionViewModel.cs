using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AsLeftConditionViewModel : EntityLookupViewModel<AsLeftCondition>
    {
        #region Constructors

        public AsLeftConditionViewModel(IContainer container) : base(container) { }

        #endregion
    }
}