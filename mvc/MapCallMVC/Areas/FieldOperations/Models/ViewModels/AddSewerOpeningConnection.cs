using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class AddSewerOpeningConnection : ViewModel<SewerOpening>
    {
        #region Properties

        [DoesNotAutoMap]
        [Required, EntityMustExist(typeof(SewerOpening)), ComboBox]
        public virtual int? ConnectedOpening { get; set; }

        [DoesNotAutoMap]
        [DisplayName("Is connected opening downstream?")]
        public virtual bool ConnectedOpeningDownstream { get; set; }

        [DoesNotAutoMap]
        public virtual bool IsInlet { get; set; }

        [DropDown, DoesNotAutoMap]
        public virtual int? SewerPipeMaterial { get; set; }

        [DoesNotAutoMap]
        public virtual decimal? Size { get; set; }
        [DoesNotAutoMap]
        public virtual decimal? Invert { get; set; }

        [DropDown, DoesNotAutoMap]
        public virtual int? SewerTerminationType { get; set; }

        [DoesNotAutoMap("This exists on the parent entity but should not map to it.")]
        public virtual int? Route { get; set; }
        [DoesNotAutoMap("This exists on the parent entity but should not map to it.")]
        public virtual int? Stop { get; set; }

        [DoesNotAutoMap]
        public virtual int? InspectionFrequency { get; set; }

        [DropDown, DoesNotAutoMap]
        public virtual int? InspectionFrequencyUnit { get; set; }

        #endregion

        #region Constructors

        public AddSewerOpeningConnection(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override SewerOpening MapToEntity(SewerOpening entity)
        {
            // DO NOT CALL base.MapToEntity!
            var sewerOpeningConnection = new SewerOpeningConnection {IsInlet = IsInlet};
            sewerOpeningConnection.Size = Size;
            sewerOpeningConnection.Invert = Invert;
            sewerOpeningConnection.Route = Route;
            sewerOpeningConnection.Stop = Stop;
            sewerOpeningConnection.InspectionFrequency = InspectionFrequency;
            if (SewerPipeMaterial.HasValue)
                sewerOpeningConnection.SewerPipeMaterial = _container.GetInstance<IRepository<PipeMaterial>>().Find(SewerPipeMaterial.Value);
            if (InspectionFrequencyUnit.HasValue)
                sewerOpeningConnection.InspectionFrequencyUnit = _container.GetInstance<IRepository<RecurringFrequencyUnit>>().Find(InspectionFrequencyUnit.Value);
            if (SewerTerminationType.HasValue)
                sewerOpeningConnection.SewerTerminationType = _container.GetInstance<IRepository<SewerTerminationType>>().Find(SewerTerminationType.Value);
            
            if (ConnectedOpeningDownstream)
            {
                sewerOpeningConnection.UpstreamOpening = entity;
                sewerOpeningConnection.DownstreamOpening = _container.GetInstance<ISewerOpeningRepository>().Find(ConnectedOpening.Value);
                entity.DownstreamSewerOpeningConnections.Add(sewerOpeningConnection);
            }
            else
            {
                sewerOpeningConnection.UpstreamOpening = _container.GetInstance<ISewerOpeningRepository>().Find(ConnectedOpening.Value);
                sewerOpeningConnection.DownstreamOpening = entity;
                entity.UpstreamSewerOpeningConnections.Add(sewerOpeningConnection);
            }
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateSewerOpeningConnection());
        }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateSewerOpeningConnection()
        {
            if (Id == ConnectedOpening)
            {
                yield return new ValidationResult("Upstream opening and Downstream opening cannot be same.");
            }
            var entity = _container.GetInstance<ISewerOpeningRepository>().Find(Id);
            if (entity.SewerOpeningConnections.Any(x =>
                (x.DownstreamOpening.Id == Id && x.UpstreamOpening.Id == ConnectedOpening) ||
                (x.UpstreamOpening.Id == Id && x.DownstreamOpening.Id == ConnectedOpening)))
            {
                yield return new ValidationResult("Connection with same Upstream opening and Downstream already exists.");
            }
        }

        #endregion
    }
}
