using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class GradientViewModel : ViewModel<Gradient>
    {
        #region Properties

        [Required]
        public virtual string Description { get; set; }

        #endregion

        #region Constructors

        public GradientViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateGradient : GradientViewModel
    {
        #region Constructors

        public CreateGradient(IContainer container) : base(container) { }

        #endregion

        public void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EditGradient : GradientViewModel
    {
        #region Constructors

        public EditGradient(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchGradient : SearchSet<Gradient>
    {
        public virtual string Description { get; set; }
    }
}
