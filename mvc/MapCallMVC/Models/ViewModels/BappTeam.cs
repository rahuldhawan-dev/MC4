using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class BappTeamViewModel : ViewModel<BappTeam>
    {
        [Required]
        public string Description { get; set; }
        [Required, EntityMustExist(typeof(OperatingCenter)), EntityMap, DropDown]
        public int? OperatingCenter { get; set; }

        public BappTeamViewModel(IContainer container) : base(container) {}
    }

    public class CreateBappTeam : BappTeamViewModel
    {
        public CreateBappTeam(IContainer container) : base(container) {}
    }

    public class EditBappTeam : BappTeamViewModel
    {
        public EditBappTeam(IContainer container) : base(container) {}
    }

    // What is this class for? There aren't any properties and there isn't a Search action.
    public class SearchBappTeam : SearchSet<BappTeam> { }
}