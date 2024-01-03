using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class BappTeamIdeaViewModel : ViewModel<BappTeamIdea>
    {
        #region Properties

        [DoesNotAutoMap("Only used for cascading.")]
        [DropDown, EntityMustExist(typeof(OperatingCenter)), Required]
        public int? OperatingCenter { get; set; }
        [DropDown("BappTeam", "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(BappTeam)), Required]
        public int? BappTeam { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(Employee)), Required]
        public int? Contact { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(SafetyImplementationCategory)), Required]
        public int? SafetyImplementationCategory { get; set; }
        [Required, Multiline]
        public string Description { get; set; }

        #endregion

        #region Constructors

        public BappTeamIdeaViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void Map(BappTeamIdea entity)
        {
            base.Map(entity);

            OperatingCenter = entity.BappTeam != null ? entity.BappTeam.OperatingCenter.Id : (int?)null;
        }

        #endregion
    }

    public class CreateBappTeamIdea : BappTeamIdeaViewModel
    {
        #region Constructors

        public CreateBappTeamIdea(IContainer container) : base(container) {}

        #endregion
    }

    public class EditBappTeamIdea : BappTeamIdeaViewModel
    {
        #region Constructors

        public EditBappTeamIdea(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchBappTeamIdea : SearchSet<BappTeamIdea>
    {
        #region Properties

        [DropDown, SearchAlias("BappTeam", "t", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }
        [DropDown("BappTeam", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        public int? BappTeam { get; set; }
        [DropDown, EntityMustExist(typeof(Employee))]
        public int? Contact { get; set; }
        [DropDown, EntityMustExist(typeof(SafetyImplementationCategory))]
        public int? SafetyImplementationCategory { get; set; }
        public DateRange CreatedAt { get; set; }

        #endregion
    }
}