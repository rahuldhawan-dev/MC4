using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model
{
    public class AspnetUsersInRoles
    {
        public virtual int Id { get; set; }
        public virtual System.Guid UserId { get; set; }
        public virtual System.Guid RoleId { get; set; }
        public virtual AspnetUsers AspnetUsers { get; set; }
        public virtual AspnetRoles AspnetRoles { get; set; }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as AspnetUsersInRoles;
            if (t == null) return false;
            if (UserId == t.UserId
                && RoleId == t.RoleId)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ UserId.GetHashCode();
            hash = (hash * 397) ^ RoleId.GetHashCode();

            return hash;
        }

        #endregion
    }
}
