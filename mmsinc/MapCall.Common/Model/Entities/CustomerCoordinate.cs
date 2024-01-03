using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CustomerCoordinate : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual CustomerLocation CustomerLocation { get; set; }

        [Required]
        public virtual float Latitude { get; set; }

        [Required]
        public virtual float Longitude { get; set; }

        [Required]
        public virtual int Source { get; set; }

        public virtual CoordinateSource CoordinateSource => (CoordinateSource)Source;
        public virtual int? Accuracy { get; set; }
        public virtual CoordinateAccuracy CoordinateAccuracy => (CoordinateAccuracy)Accuracy;
        public virtual bool Verified { get; set; }

        #endregion

        #region Exposed Methods

        public virtual string Describe()
        {
            return CoordinateSource.ToString().ToTitleCase();
        }

        #endregion
    }

    public enum CoordinateSource
    {
        AmericanWater = 1,
        GeocodeFarm = 2,
        DataScienceToolkit = 3,
        MapCall = 4
    }

    public enum CoordinateAccuracy
    {
        VeryAccurate = 1,
        GoodAccuracy = 2,
        Accurate = 3,
        UnknownAccuracy = 4
    }
}
