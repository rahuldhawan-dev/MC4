using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonServiceLocator;
using StructureMap;

namespace MMSINC.Utilities.StructureMap
{
    public class StructureMapServiceLocator : ServiceLocatorImplBase, IDisposable
    {
        #region Constants

        private const string StructuremapNestedContainerKey = "Structuremap.Nested.Container";

        #endregion

        #region Properties

        public IContainer Container { get; set; }

        private HttpContextBase HttpContext
        {
            get
            {
                var ctx = Container.TryGetInstance<HttpContextBase>();
                return ctx ?? new HttpContextWrapper(System.Web.HttpContext.Current);
            }
        }

        public IContainer CurrentNestedContainer
        {
            get => (IContainer)HttpContext.Items[StructuremapNestedContainerKey];
            set => HttpContext.Items[StructuremapNestedContainerKey] = value;
        }

        #endregion

        #region Constructors

        public StructureMapServiceLocator(IContainer container)
        {
            Container = container;
        }

        #endregion

        #region Private Methods

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (CurrentNestedContainer ?? Container).GetAllInstances(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            var container = (CurrentNestedContainer ?? Container);

            if (string.IsNullOrEmpty(key))
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                    ? container.TryGetInstance(serviceType)
                    : container.GetInstance(serviceType);
            }

            return container.GetInstance(serviceType, key);
        }

        #endregion

        #region Exposed Methods

        public void Dispose()
        {
            CurrentNestedContainer?.Dispose();

            Container.Dispose();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return DoGetAllInstances(serviceType);
        }

        public void DisposeNestedContainer()
        {
            CurrentNestedContainer?.Dispose();
        }

        public void CreateNestedContainer()
        {
            if (CurrentNestedContainer != null) return;
            CurrentNestedContainer = Container.GetNestedContainer(nameof(StructureMapServiceLocator));
        }

        #endregion
    }
}
