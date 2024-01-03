using System.Reflection;
using System.Web.Hosting;

namespace MapCall.Common.Web
{
    public interface IHostingEnvironment
    {
        void RegisterVirtualPathProvider(VirtualPathProvider provider);
    }

    /// <summary>
    /// Wrapper class for System.Web.Hosting.HostingEnvironment static class. 
    /// </summary>
    public class HostingEnvironmentWrapper : IHostingEnvironment
    {
        public void RegisterVirtualPathProvider(VirtualPathProvider provider)
        {
            //            HostingEnvironment.RegisterVirtualPathProvider(provider);

            var hostingEnvironmentType = typeof(HostingEnvironment);
            var instance =
                (HostingEnvironment)
                hostingEnvironmentType.InvokeMember("_theHostingEnvironment",
                    BindingFlags.NonPublic | BindingFlags.Static |
                    BindingFlags.GetField, null, null, null);

            var method = hostingEnvironmentType.GetMethod("RegisterVirtualPathProviderInternal",
                BindingFlags.NonPublic | BindingFlags.Static);

            if (method == null || instance == null)
                return;

            method.Invoke(instance, new object[] {(VirtualPathProvider)provider});
        }
    }
}
