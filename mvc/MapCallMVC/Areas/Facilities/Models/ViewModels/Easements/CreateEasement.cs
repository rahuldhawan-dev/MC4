using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.Easements
{
    public class CreateEasement : EasementViewModel
    {
        #region Constructors

        public CreateEasement(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DropDown, EntityMustExist(typeof(State)), EntityMap, Required]
        public virtual int? State { get; set; }

        [DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = "State", PromptText = "Select a state above"),
         EntityMustExist(typeof(OperatingCenter)), EntityMap, Required]
        public virtual int? OperatingCenter { get; set; }

        #endregion
    }
}