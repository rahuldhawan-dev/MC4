using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityFacilityArea : IEntity, IThingWithCoordinate
    {
        #region Private Members

        private FacilityFacilityAreaDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual FacilityArea FacilityArea { get; set; }
        public virtual FacilitySubArea FacilitySubArea { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        
        #endregion

        #region Logical Properties

        [View("Facility Area")]
        public virtual string Display => (_display ?? (_display = new FacilityFacilityAreaDisplayItem {
            FacilityAreaDesc = FacilityArea?.Description,
            FacilitySubAreaDesc = FacilitySubArea?.Description,
        })).Display;

        public virtual MapIcon Icon => Coordinate?.Icon ?? Facility.Coordinate.Icon;

        public virtual decimal? Latitude => Coordinate?.Latitude ?? Facility.Coordinate?.Latitude;

        public virtual decimal? Longitude => Coordinate?.Longitude ?? Facility.Coordinate?.Longitude;

        #endregion
    }
}
