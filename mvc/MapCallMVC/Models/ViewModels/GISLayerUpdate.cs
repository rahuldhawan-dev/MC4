using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Utilities;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class GISLayerUpdateViewModel : ViewModel<GISLayerUpdate>
    {
        #region Properties

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? Updated { get; set; }
        [Required]
        public virtual bool? IsActive { get; set; }
        [Required, StringLength(GISLayerUpdateMap.MAP_ID_LENGTH), DisplayName("Map ID")]
        public virtual string MapId { get; set; }

        #endregion

        #region Constructors

        public GISLayerUpdateViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateGISLayerUpdate : GISLayerUpdateViewModel
    {
        #region Constructors

		public CreateGISLayerUpdate(IContainer container) : base(container) {}

        #endregion
    }

    public class EditGISLayerUpdate : GISLayerUpdateViewModel
    {
        #region Constructors

		public EditGISLayerUpdate(IContainer container) : base(container) {}

        #endregion
    }
}