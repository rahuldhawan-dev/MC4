using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class PipeDataLookupValueViewModel : ViewModel<PipeDataLookupValue>
    {
        #region Properties

        [Required]
        public string Description { get; set; }
        [Required]
        public decimal? VariableScore { get; set; }
        [Required]
        public decimal? PriorityWeightedScore { get; set; }
        [Required]
        public bool? IsEnabled { get; set; }
        [Required]
        public bool? IsDefault { get; set; }
        #endregion

        #region Constructors

        public PipeDataLookupValueViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreatePipeDataLookupValue : PipeDataLookupValueViewModel
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(PipeDataLookupType))]
        public int? PipeDataLookupType { get; set; }

        #endregion
        
        #region Constructors

		public CreatePipeDataLookupValue(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override PipeDataLookupValue MapToEntity(PipeDataLookupValue entity)
        {
            if (IsDefault == true)
            {
                var repo = _container.GetInstance<IRepository<PipeDataLookupValue>>();
                var pipeDataLookupValues = repo.Where(x => x.PipeDataLookupType.Id == PipeDataLookupType);
                foreach (var pipeDataLookupValue in pipeDataLookupValues)
                {
                    pipeDataLookupValue.IsDefault = false;
                    repo.Save(pipeDataLookupValue);
                }
            }

            return base.MapToEntity(entity);

        }

        #endregion
    }

    public class EditPipeDataLookupValue : PipeDataLookupValueViewModel
    {
        #region Constructors

		public EditPipeDataLookupValue(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override PipeDataLookupValue MapToEntity(PipeDataLookupValue entity)
        {
            if (IsDefault == true)
            {
                var repo = _container.GetInstance<IRepository<PipeDataLookupValue>>();
                var thisPdlv = repo.Find(Id);
                var pipeDataLookupValues = repo.Where(x => x.PipeDataLookupType.Id == thisPdlv.PipeDataLookupType.Id);
                foreach (var pipeDataLookupValue in pipeDataLookupValues)
                {
                    pipeDataLookupValue.IsDefault = false;
                    repo.Save(pipeDataLookupValue);
                }
            }

            return base.MapToEntity(entity);

        }

        #endregion
    }
}