using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;
namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class EditApcFormAnswer : CreateApcFormAnswer
    {
        #region Constructors

        public EditApcFormAnswer(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(ApcInspectionItem))]
        public int? ApcInspectionItem { get; set; }

        #endregion
    }
}