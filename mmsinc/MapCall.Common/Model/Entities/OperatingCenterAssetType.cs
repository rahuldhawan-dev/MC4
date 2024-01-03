using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OperatingCenterAssetType : IEntity
    {
        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual AssetType AssetType { get; set; }
    }
}
