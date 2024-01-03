using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeasurementPointEquipmentType : IEntity
    {
        #region Constants
        
        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             CATEGORY = 1;
        }
        
        #endregion
        
        public virtual int Id { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Min { get; set; }
        public virtual decimal Max { get; set; }
        public virtual string Category { get; set; }
        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
