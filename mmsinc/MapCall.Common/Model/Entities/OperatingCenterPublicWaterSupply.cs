using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OperatingCenterPublicWaterSupply : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        public virtual string Abbreviation { get; set; }

        #endregion

        public override bool Equals(object obj)
        {
            var other = obj as OperatingCenterPublicWaterSupply;

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return OperatingCenter == other.OperatingCenter && PublicWaterSupply == other.PublicWaterSupply;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{OperatingCenter.OperatingCenterCode} - {PublicWaterSupply.Identifier}";
        }
    }
}
