using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateLockoutFormAnswer: LockoutFormAnswerViewModel
    {
        public CreateLockoutFormAnswer(IContainer container) : base(container) { }

        #region Properties

        public int? LockoutForm { get; set; }

        #endregion
    }
}