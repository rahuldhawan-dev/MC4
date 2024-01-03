using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EditLockoutFormAnswer : LockoutFormAnswerViewModel
    {
        public EditLockoutFormAnswer(IContainer container) : base(container) { }

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(LockoutForm))]
        public int? LockoutForm { get; set; }

        #endregion
    }
}