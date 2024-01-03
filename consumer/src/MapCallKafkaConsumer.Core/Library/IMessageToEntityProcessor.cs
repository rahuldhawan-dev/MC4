using MMSINC.Data;

namespace MapCallKafkaConsumer.Library
{
    /// <summary>
    /// Defines the things that can happen when attempting to map a message to an entity and persist it to MapCall.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message after it is hydrated from a string value.</typeparam>
    /// <typeparam name="TEntity">The type of the entity which the hydrated message will be mapped to.</typeparam>
    public interface IMessageToEntityProcessor<TMessage, TEntity>
    {
        /// <summary>
        /// Processes the specified message into an entity and saves it to MapCall.
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        void Process(string message);

        /// <summary>
        /// Hydrates the message to the given type.
        /// </summary>
        /// <param name="message">The message to be hydrated.</param>
        /// <returns>A hydrated instance of that message.</returns>
        TMessage HydrateMessage(string message);

        /// <summary>
        /// Retrieves an entity, if it exists, from MapCall.
        /// </summary>
        /// <param name="unitOfWork">The unit of work that ... does the work.</param>
        /// <param name="message">The message to be used as part of the criteria for retrieving the entity.</param>
        /// <returns></returns>
        TEntity RetrieveEntity(IUnitOfWork unitOfWork, TMessage message);

        /// <summary>
        /// Maps the message onto an entity.
        /// </summary>
        /// <param name="message">The message to be mapped onto the given entity.</param>
        /// <param name="entity">The entity to be mapped onto.</param>
        /// <returns>The entity with the newly mapped values from the message.</returns>
        TEntity MapMessageToEntity(TMessage message, TEntity entity);
    }
}
