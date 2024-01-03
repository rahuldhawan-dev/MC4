using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RoadwayImprovementNotificationStreet : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual RoadwayImprovementNotification RoadwayImprovementNotification { get; set; }
        public virtual Street Street { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual string StartPoint { get; set; }
        public virtual string Terminus { get; set; }
        public virtual MainSize MainSize { get; set; }
        public virtual MainType MainType { get; set; }

        [DisplayName("Street Status")]
        public virtual RoadwayImprovementNotificationStreetStatus RoadwayImprovementNotificationStreetStatus
        {
            get;
            set;
        }

        public virtual int? MainBreakActivity { get; set; }

        [DisplayName("# of Services to be Replaced")]
        public virtual int? NumberOfServicesToBeReplaced { get; set; }

        public virtual int? OpenWorkOrders { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? MoratoriumEndDate { get; set; }

        [Multiline]
        public virtual string Notes { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class RoadwayImprovementNotificationStreetStatus : EntityLookup { }

    [Serializable]
    public class MainSize : EntityLookup { }
}
