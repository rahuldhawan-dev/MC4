using StructureMap.Web.Pipeline;

namespace MapCallScheduler.Library.Quartz
{
    public class HybridHttpQuartzLifecycle : HttpLifecycleBase<HttpContextLifecycle, QuartzLifecycle>
    {
    }
}