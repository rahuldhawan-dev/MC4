using System;
using System.Collections.Generic;
using StructureMap;

namespace MMSINC.Testing.StructureMap
{
    public class ContainerToContextAdapter : IContext
    {
        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Properties

        public string RequestedName => throw new NotImplementedException();

        public Type ParentType => throw new NotImplementedException();

        public Type RootType => throw new NotImplementedException();

        #endregion

        #region Constructors

        public ContainerToContextAdapter(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public void BuildUp(object target)
        {
            _container.BuildUp(target);
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        public T GetInstance<T>(string name)
        {
            return _container.GetInstance<T>(name);
        }

        public object GetInstance(Type pluginType)
        {
            return _container.GetInstance(pluginType);
        }

        public object GetInstance(Type pluginType, string instanceKey)
        {
            return _container.GetInstance(pluginType, instanceKey);
        }

        public T TryGetInstance<T>() where T : class
        {
            return _container.TryGetInstance<T>();
        }

        public T TryGetInstance<T>(string name) where T : class
        {
            return _container.TryGetInstance<T>(name);
        }

        public object TryGetInstance(Type pluginType)
        {
            return _container.TryGetInstance(pluginType);
        }

        public object TryGetInstance(Type pluginType, string instanceKey)
        {
            return _container.TryGetInstance(pluginType, instanceKey);
        }

        public IEnumerable<T> All<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllInstances<T>()
        {
            return _container.GetAllInstances<T>();
        }

        public IEnumerable<object> GetAllInstances(Type pluginType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
