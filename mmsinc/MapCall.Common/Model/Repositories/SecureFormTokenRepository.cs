using MMSINC.Metadata;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class SecureFormTokenRepository : SecureFormTokenRepositoryBase<SecureFormToken, SecureFormDynamicValue>
    {
        public SecureFormTokenRepository(ISession session, IContainer container, IDateTimeProvider dateTimeProvider) :
            base(session, container, dateTimeProvider) { }
    }
}
