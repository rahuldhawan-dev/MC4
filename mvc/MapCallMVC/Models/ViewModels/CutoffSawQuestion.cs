using MMSINC.Data;
using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CutoffSawQuestionViewModel : ViewModel<CutoffSawQuestion>
    {
        #region Properties

        public string Question { get; set; }

        #endregion
        
        #region Constructors

        public CutoffSawQuestionViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void Map(CutoffSawQuestion entity)
        {
            base.Map(entity);
        }

        public override CutoffSawQuestion MapToEntity(CutoffSawQuestion entity)
        {
            return base.MapToEntity(entity);
        }

        #endregion
    }
}