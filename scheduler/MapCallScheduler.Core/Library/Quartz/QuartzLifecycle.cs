using StructureMap;
using StructureMap.Pipeline;

namespace MapCallScheduler.Library.Quartz
{
    public class QuartzLifecycle : ILifecycle
    {
        #region Properties

        public string Description => "QuartzLifecycle";

        #endregion

        #region Exposed Methods

        public void EjectAll(ILifecycleContext context)
        {
            FindCache(context).DisposeAndClear();
        }

        public IObjectCache FindCache(ILifecycleContext context)
        {
            return context.ContainerCache;
        }

        #endregion
    }
}
