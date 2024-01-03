using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Notification : IEntity // This is not an entity. -Ross 3/23/2020
    {
        public int Id { get; set; }
        public RoleModules RoleModule { get; set; }
        public string NotificationPurpose { get; set; }
        public int OperatingCenterId { get; set; }
        public string EntityType { get; set; }
    }
}
