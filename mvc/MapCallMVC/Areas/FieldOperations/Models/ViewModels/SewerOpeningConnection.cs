using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SewerOpeningConnectionViewModel : ViewModel<SewerOpeningConnection>
    {
        #region Constructors

        public SewerOpeningConnectionViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties
              
        public bool? IsInlet { get; set; }
        [ComboBox, Required, EntityMap, EntityMustExist(typeof(SewerOpening))]
        public int? DownstreamOpening { get; set; }
        [ComboBox, Required, EntityMap, EntityMustExist(typeof(SewerOpening))]
        public int? UpstreamOpening { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PipeMaterial))]
        public int? SewerPipeMaterial { get; set; }
        public decimal? Size { get; set; }
        public decimal? Invert { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(SewerTerminationType))]
        public int? SewerTerminationType { get; set; }
        public int? Route { get; set; }
        public int? Stop { get; set; }
        public int? InspectionFrequency { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? InspectionFrequencyUnit { get; set; }

        #endregion
    }

    public class EditSewerOpeningConnection : SewerOpeningConnectionViewModel
    {
        [DoesNotAutoMap]
        public int OriginalOpeningId { get; set; }

        #region Constructor

        public EditSewerOpeningConnection(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateSewerOpeningConnection : SewerOpeningConnectionViewModel
    {
        #region Constructors

        public CreateSewerOpeningConnection(IContainer container) : base(container) { }

        #endregion
    }
}