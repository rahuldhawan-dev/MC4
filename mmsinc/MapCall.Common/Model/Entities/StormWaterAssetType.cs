using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StormWaterAssetType : IEntity
    {
        #region Consts

        public struct StringLengths
        {
            public const int DESCRIPTION_MAX_LENGTH = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }

        #endregion
    }
}
