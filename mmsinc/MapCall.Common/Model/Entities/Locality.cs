using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Locality : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }

        // This is here to return the code back to SAP for the Locality value in the API - MC-2272
        public override string ToString()
        {
            return Code;
        }
    }
}
