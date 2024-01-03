using MapCall.Common.Model.Entities;
using MMSINC.Authentication;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorsAuthenticationLogMap : AuthenticationLogMapBase<
        ContractorsAuthenticationLog, ContractorUser>
    {
        protected override string TableName => "ContractorAuthenticationLogs";
    }
}
