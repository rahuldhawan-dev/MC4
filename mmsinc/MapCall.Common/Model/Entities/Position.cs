using System;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Position : IEntity
    {
        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual string Category { get; set; }
        public virtual string PositionDescription { get; set; }
        public virtual bool? Essential { get; set; }
        public virtual string OpCode { get; set; }

        #endregion

        #region References

        public virtual EmergencyResponsePriority EmergencyResponsePriority { get; set; }
        public virtual Local Local { get; set; }
        public virtual LicenseType LicenseRequirementAttainment { get; set; }

        #endregion

        #region Logical Properties

        public virtual string Description => ToString();

        #endregion

        #endregion

        #region Constructors

        public Position() { }

        // only exists for a repository method
        public Position(int id)
        {
            Id = id;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(OpCode))
            {
                sb.Append(OpCode);
                if (Local != null && Local.OperatingCenter != null)
                {
                    sb.AppendFormat("/{0}", Local.OperatingCenter.OperatingCenterCode);
                }
            }
            else if (Local != null && Local.OperatingCenter != null)
            {
                sb.Append(Local.OperatingCenter.OperatingCenterCode);
            }

            sb.AppendFormat(sb.Length > 0 ? "-{0}" : "{0}", PositionDescription);
            sb.AppendFormat(" [{0}]", Id);

            return sb.ToString();
        }

        #endregion
    }
}
