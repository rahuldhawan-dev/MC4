using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetSchemaVersions
    {
        public virtual string Feature { get; set; }
        public virtual string CompatibleSchemaVersion { get; set; }
        public virtual bool IsCurrentVersion { get; set; }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as AspnetSchemaVersions;
            if (t == null) return false;
            if (Feature == t.Feature
                && CompatibleSchemaVersion == t.CompatibleSchemaVersion)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ Feature.GetHashCode();
            hash = (hash * 397) ^ CompatibleSchemaVersion.GetHashCode();

            return hash;
        }

        #endregion
    }
}
