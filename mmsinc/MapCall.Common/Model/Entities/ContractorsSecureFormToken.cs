using System;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class
        ContractorsSecureFormToken : SecureFormTokenBase<ContractorsSecureFormToken, ContractorsSecureFormDynamicValue>
    {
        #region Constants

        public const int EXPIRATION_MINUTES = 60;

        #endregion

        #region Properties

        public override int ExpirationMinutes => EXPIRATION_MINUTES;

        #endregion
    }
}
