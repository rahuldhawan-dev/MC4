using MapCall.Common.Model.Entities;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ContractorsSecureFormTokenRepository : SecureFormTokenRepositoryBase<ContractorsSecureFormToken,
        ContractorsSecureFormDynamicValue>
    {
        public ContractorsSecureFormTokenRepository(ISession session, IContainer container,
            IDateTimeProvider dateTimeProvider) : base(session, container, dateTimeProvider) { }
    }
}
