using MapCall.Common.Views;
using StructureMap;

namespace MapCallMVC.Views
{
    // This class needs to be abstract so we don't have a no-op virtual Execute method. 
    public abstract class ViewBase<T> : MvcViewBase<T>
    {
        protected ViewBase() : base() { }
    }
}