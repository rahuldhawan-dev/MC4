using StructureMap;

namespace MMSINC.Utilities.ActiveMQ
{
    public class ApacheActiveMQServiceFactory : ActiveMqServiceFactoryBase<ApacheActiveMQService>
    {
        #region Constructors

        public ApacheActiveMQServiceFactory(IContainer container) : base(container) { }

        #endregion
    }
}
