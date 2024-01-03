using System;
using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving AccountingType objects from persistence.
    /// </summary>
    public class AccountingTypeRepository : WorkOrdersRepository<AccountingType>
    {
        #region Constants

        public struct Indices
        {
            public const short CAPITAL = 1,
                               OANDM = 2,
                               RETIREMENT = 3;
        }

        public struct Descriptions
        {
            public const string CAPITAL = "Capital",
                                OANDM = "O&M",
                                RETIREMENT = "Retirement";
        }

        #endregion

        #region Pirate Static Members
        // 20080919 by Jason Duncan -- Yarr

        private static AccountingType _capital,
                                      _oandm,
                                      _retirement;
        #endregion

        #region Static Properties

        public static AccountingType Capital
        {
            get
            {
                _capital = RetrieveAndAttach(Indices.CAPITAL);
                return _capital;
            }
        }

        public static AccountingType OAndM
        {
            get
            {
                _oandm = RetrieveAndAttach(Indices.OANDM);
                return _oandm;
            }
        }

        public static AccountingType Retirement
        {
            get
            {
                _retirement = RetrieveAndAttach(Indices.RETIREMENT);
                return _retirement;
            }
        }

        #endregion

        #region Pirate Static Methods

        private static AccountingType RetrieveAndAttach(int index)
        {
            var type = GetEntity(index);
            if (!DataTable.Contains(type))
                DataTable.Attach(type);
            return type;
        }

        #endregion

        #region Exposed Static Methods

        public static AccountingTypeEnum GetEnumerationValue(AccountingType type)
        {
            switch (type.AccountingTypeID)
            {
                case Indices.CAPITAL:
                    return AccountingTypeEnum.Capital;
                case Indices.OANDM:
                    return AccountingTypeEnum.OAndM;
                case Indices.RETIREMENT:
                    return AccountingTypeEnum.Retirement;
                default:
                    throw new Exception("Invalid AccountingTypeID Specified");
            }
        }
        #endregion
    }
}