using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BappTeam : EntityLookup, IEntityWithCreationUserTracking<User>
    {
        [Required]
        public virtual OperatingCenter OperatingCenter { get; set; }

        [Required]
        public virtual User CreatedBy { get; set; }
    }
}
