using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DataTableLayoutProperty : IEntity
    {
        #region Structs

        public struct StringLengths
        {
            public const int PROPERTY_NAME = 100;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string PropertyName { get; set; }
        public virtual DataTableLayout DataTableLayout { get; set; }

        #endregion
    }
}
