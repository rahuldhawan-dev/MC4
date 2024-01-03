using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentLink : IEntity
    {
        public struct StringLengths
        {
            public const int PAYMENT_METHOD_URL = 2000;
        }

        public virtual int Id { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual LinkType LinkType { get; set; }
        public virtual string Url { get; set; }
    }
}
