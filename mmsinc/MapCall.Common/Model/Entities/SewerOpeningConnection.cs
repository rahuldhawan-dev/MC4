using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOpeningConnection : IEntityWithCreationTracking<User>, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual bool? IsInlet { get; set; }
        public virtual SewerOpening DownstreamOpening { get; set; }
        public virtual SewerOpening UpstreamOpening { get; set; }
        public virtual PipeMaterial SewerPipeMaterial { get; set; }
        public virtual decimal? Size { get; set; }
        public virtual decimal? Invert { get; set; }
        public virtual SewerTerminationType SewerTerminationType { get; set; }
        public virtual int? Route { get; set; }
        public virtual int? Stop { get; set; }
        public virtual int? InspectionFrequency { get; set; }
        public virtual RecurringFrequencyUnit InspectionFrequencyUnit { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual User CreatedBy { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public class SewerTerminationType : EntityLookup { }
}
