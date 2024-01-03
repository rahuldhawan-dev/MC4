using StructureMap;

namespace MMSINC.Utilities.ActiveMQ
{
    public abstract class ActiveMqServiceFactoryBase<TService> : IActiveMQServiceFactory
        where TService : ActiveMQServiceBase
    {
        #region Private Members

        protected readonly IContainer _container;

        #endregion

        #region Constructors

        public ActiveMqServiceFactoryBase(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public IActiveMQService Build(IActiveMQConfiguration config)
        {
            return _container.With(config).GetInstance<TService>();
        }

        #endregion
    }
}
