using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class PipeDiameterViewModel : ViewModel<PipeDiameter>
    {
        #region Properties

        [Required]
        public decimal? Diameter { get; set; }

        #endregion

        #region Constructors

        public PipeDiameterViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreatePipeDiameter : PipeDiameterViewModel
    {
        #region Constructors

		public CreatePipeDiameter(IContainer container) : base(container) {}

        #endregion
	}

    public class EditPipeDiameter : PipeDiameterViewModel
    {
        #region Constructors

		public EditPipeDiameter(IContainer container) : base(container) {}

        #endregion
	}
}