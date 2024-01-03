using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AssetUpload : IEntityWithCreationTracking<User>
    {
        public virtual int Id { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string FileName { get; set; }
        public virtual Guid FileGuid { get; set; }
        public virtual AssetUploadStatus Status { get; set; }

        [Multiline]
        public virtual string ErrorText { get; set; }
    }
}
