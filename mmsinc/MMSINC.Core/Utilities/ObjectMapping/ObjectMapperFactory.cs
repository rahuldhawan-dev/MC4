using System;
using StructureMap;

namespace MMSINC.Utilities.ObjectMapping
{
    public interface IObjectMapperFactory
    {
        #region Abstract Methods

        IObjectMapper Build(Type primary, Type secondary);

        #endregion
    }

    public class ObjectMapperFactory : IObjectMapperFactory
    {
        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public ObjectMapperFactory(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public IObjectMapper Build(Type primary, Type secondary)
        {
            return new AutoObjectMapper(_container, primary, secondary);
        }

        #endregion
    }
}
