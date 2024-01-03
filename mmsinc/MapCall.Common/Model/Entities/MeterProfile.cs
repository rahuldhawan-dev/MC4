using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterProfile : IEntity
    {
        public struct StringLenths
        {
            public const int PROFILE_NAME = 255, TEST_COMMENTS = 1000;
        }

        public virtual int Id { get; set; }
        public virtual MeterType Type { get; set; }
        public virtual MeterSize Size { get; set; }
        public virtual MeterManufacturer Manufacturer { get; set; }
        public virtual MeterDialCount DialCount { get; set; }
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
        public virtual MeterOutput Output { get; set; }

        [StringLength(StringLenths.PROFILE_NAME)]
        public virtual string ProfileName { get; set; }

        public virtual float? EstimatedMaximumFlow { get; set; }
        public virtual decimal? AWWALowerLimitPercentage { get; set; }
        public virtual decimal? AWWAUpperLimitPercentage { get; set; }

        [StringLength(StringLenths.TEST_COMMENTS)]
        public virtual string TestComments { get; set; }

        public virtual int? TestPointsMinimum { get; set; }

        public override string ToString()
        {
            return ProfileName;
        }
    }
}
