using log4net;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.ActiveMQ;

namespace MapCallActiveMQListener.Library
{
    public abstract class MessageProcessorBase : IMessageProcessor
    {
        #region Private Members

        protected readonly IUnitOfWorkFactory _uowFactory;
        protected readonly IDateTimeProvider _dateTimeProvider;
        protected readonly ILog _log;
        
        #endregion

        #region Constructors

        public MessageProcessorBase(IUnitOfWorkFactory uowFactory, IDateTimeProvider dateTimeProvider, ILog log)
        {
            _uowFactory = uowFactory;
            _dateTimeProvider = dateTimeProvider;
            _log = log;
        }

        #endregion

        #region Abstract Methods

        public abstract void Process(IActiveMQService service, IMessage message, string topic);

        #endregion
    }
}
