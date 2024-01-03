using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SmartCoverAlertSmartCoverAlertType : IEntity
    {
        #region Table Properties

        public virtual int Id { get; set; }

        public virtual SmartCoverAlert SmartCoverAlert { get; set; }

        public virtual SmartCoverAlertType SmartCoverAlertType { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return SmartCoverAlertType.Description;
        }

        #endregion
    }
}
