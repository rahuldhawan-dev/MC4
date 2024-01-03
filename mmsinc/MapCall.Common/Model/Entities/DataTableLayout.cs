using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DataTableLayout : IEntity
    {
        #region Structs

        public struct StringLengths
        {
            public const int LAYOUT_NAME = 100,
                             AREA = 100,
                             CONTROLLER = 100;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string LayoutName { get; set; }
        public virtual string Area { get; set; }
        public virtual string Controller { get; set; }

        public virtual IList<DataTableLayoutProperty> Properties { get; set; }

        #endregion

        #region Constructor

        public DataTableLayout()
        {
            Properties = new List<DataTableLayoutProperty>();
        }

        #endregion
    }
}
