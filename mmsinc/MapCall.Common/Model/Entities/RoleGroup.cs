using MMSINC.Data;
using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RoleGroup : IEntity
    {
        #region Consts

        public struct StringLengths
        {
            public const int NAME = 200;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<RoleGroupRole> Roles { get; set; }
        public virtual IList<User> Users { get; set; }

        #endregion

        #region Constructor

        public RoleGroup()
        {
            Roles = new List<RoleGroupRole>();
            Users = new List<User>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
