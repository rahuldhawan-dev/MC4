using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class MainConditionRepository : WorkOrdersRepository<MainCondition>
    {
        #region Constants

        public struct Indices
        {
            public const short GOOD = 1,
                               FAIR = 2,
                               POOR = 3;
        }

        public struct Descriptions
        {
            public const string GOOD = "Good",
                                FAIR = "Fair",
                                POOR = "Poor";
        }

        #endregion

        #region Private Static Members

        private static MainCondition _good,
                                     _fair,
                                     _poor;
                                     

        #endregion

        #region Static Properties

        public static MainCondition Good
        {
            get
            {
                _good = RetrieveAndAttach(Indices.GOOD);
                return _good;
            }
        }
        
        public static MainCondition Fair
        {
            get
            {
                _fair = RetrieveAndAttach(Indices.FAIR);
                return _fair;
            }
        }

        public static MainCondition Poor
        {
            get
            {
                _poor = RetrieveAndAttach(Indices.POOR);
                return _poor;
            }
        }

        #endregion

        #region Private Static Methods

        public static MainCondition RetrieveAndAttach(int index)
        {
            var condition = GetEntity(index);
            if (!DataTable.Contains(condition))
                DataTable.Attach(condition);
            return condition;
        }

        public static IEnumerable<MainCondition> GetMainConditions()
        {
            return (from m in DataTable
                    orderby m.Description
                    select m);
        }

        #endregion
    }
}