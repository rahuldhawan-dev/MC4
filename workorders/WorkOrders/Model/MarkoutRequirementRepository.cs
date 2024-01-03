using System.Linq;
using MapCall.Common.Utility;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving MarkoutRequirement objects from persistence.
    /// </summary>
    public class MarkoutRequirementRepository : WorkOrdersRepository<MarkoutRequirement>
    {
        #region Constants

        public struct Indices
        {
            public const short NONE = 1,
                               ROUTINE = 2,
                               EMERGENCY = 3;
        }

        public struct Descriptions
        {

            public const string NONE = "None",
                                ROUTINE = "Routine",
                                EMERGENCY = "Emergency";
        }

        #endregion

        #region Private Static Members

        private static MarkoutRequirement _none, _routine, _emergency;
        
        #endregion

        #region Static Properties

        public static MarkoutRequirement None
        {
            get
            {
                _none = RetrieveAndAttach(Indices.NONE);
                return _none;
            }
        }

        public static MarkoutRequirement Routine
        {
            get
            {
                _routine = RetrieveAndAttach(Indices.ROUTINE);
                return _routine;
            }
        }

        public static MarkoutRequirement Emergency
        {
            get
            {
                _emergency = RetrieveAndAttach(Indices.EMERGENCY);
                return _emergency;
            }
        }

        #endregion

        #region Private Static Methods

        private static MarkoutRequirement RetrieveAndAttach(int index)
        {
            var req = GetEntity(index);
            if (!DataTable.Contains(req))
                DataTable.Attach(req);
            return req;
        }

        #endregion

        #region Exposed Static Methods

        public static MarkoutRequirementEnum GetEnumerationValue(MarkoutRequirement requirement)
        {
            switch (requirement.MarkoutRequirementID)
            {
                case Indices.NONE:
                    return MarkoutRequirementEnum.None;
                case Indices.EMERGENCY:
                    return MarkoutRequirementEnum.Emergency;
                default:
                    return MarkoutRequirementEnum.Routine;
            }
        }

        #endregion
    }
}
