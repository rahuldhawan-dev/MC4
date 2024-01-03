using System;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPEquipmentClassGeneric
    {
        public virtual string Value { get; set; }
        public virtual string DisplayValue { get; set; }
        public virtual string FieldName { get; set; }
    }
}
