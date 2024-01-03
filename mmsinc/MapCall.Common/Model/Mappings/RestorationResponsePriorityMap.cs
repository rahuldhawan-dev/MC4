using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RestorationResponsePriorityMap : EntityLookupMap<RestorationResponsePriority>
    {
        #region Constants

        public const string TABLE_NAME = "RestorationResponsePriorities";

        #endregion

        #region Properties

        protected override string IdName
        {
            get { return "RestorationResponsePriorityID"; }
        }

        #endregion

        #region Constructors

        public RestorationResponsePriorityMap()
        {
            Table(TABLE_NAME);
        }

        #endregion
    }
}
