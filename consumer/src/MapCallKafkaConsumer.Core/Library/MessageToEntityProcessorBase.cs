using log4net;
using MMSINC.Data;
using Newtonsoft.Json;
using System;

namespace MapCallKafkaConsumer.Library
{
    public abstract class MessageToEntityProcessorBase<TMessage, TEntity> : IMessageToEntityProcessor<TMessage, TEntity> 
        where TEntity : IEntity
    {
        #region Private Members

        protected readonly IUnitOfWorkFactory _unitOfWorkFactory;
        protected readonly ILog _logger;

        #endregion

        #region Constructors

        protected MessageToEntityProcessorBase(ILog logger, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _logger = logger;
        }

        #endregion

        #region Exposed Methods

        public virtual void Process(string message)
        {
            try
            {
                _logger.Info($"Processing message: {message}");

                var hydratedMessage = HydrateMessage(message);

                using (var unitOfWork = _unitOfWorkFactory.Build())
                {
                    var entity = RetrieveEntity(unitOfWork, hydratedMessage);

                    if (entity == null)
                    {
                        _logger.Info($"{GetType().Name} could not find a matching entity for message: {message}");
                        return;
                    }

                    MapMessageToEntity(hydratedMessage, entity);

                    unitOfWork.GetRepository<TEntity>()
                              .Save(entity);
                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{GetType().Name} could not process process message: {message}", ex);
                throw;
            }
        }

        public virtual TMessage HydrateMessage(string message)
        {
            return JsonConvert.DeserializeObject<TMessage>(message);
        }

        public abstract TEntity RetrieveEntity(IUnitOfWork unitOfWork, TMessage message);

        public abstract TEntity MapMessageToEntity(TMessage message, TEntity entity);

        #endregion
    }
}
