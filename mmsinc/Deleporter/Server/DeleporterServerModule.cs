using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Web;
using DeleporterCore.Configuration;

namespace DeleporterCore.Server
{
    public class DeleporterServerModule : IHttpModule
    {
        private IChannel _remotingChannel;
        private static int _hasCreatedChannel = 0;

        public void Init(HttpApplication app)
        {
            var badAssemblies = new List<Assembly>();
            if (WasCompiledInDebugMode(app, badAssemblies))
            {
                if (Interlocked.Exchange(ref _hasCreatedChannel, 1) == 0)
                {
                    // Start listening for connections
                    RemotingConfiguration.RegisterWellKnownServiceType(
                        typeof(DeleporterService),
                        NetworkConfiguration.ServiceName,
                        WellKnownObjectMode.Singleton);
                    _remotingChannel = NetworkConfiguration.CreateChannel();
                    ChannelServices.RegisterChannel(_remotingChannel, false);
                }
            }
            else
                throw new InvalidOperationException(
                    String.Format(
                        "You should not enable Deleporter on production web servers. As a security precaution, Deleporter won't run if your ASP.NET application was compiled in Release mode. You need to remove DeleporterServerModule from your Web.config file. If you need to bypass this, the only way is to edit the Deleporter source code and remove this check.{0}The following assemblies violated the rule:{1}",
                        Environment.NewLine,
                        String.Join(Environment.NewLine, badAssemblies)));
        }

        #region Checking for debug mode

        private static bool WasCompiledInDebugMode(object value, IList<Assembly> badAssemblies)
        {
            // In case the app class is auto-generated from a Global.asax file, check its base classes too, going down until we hit ASP.NET itself
            var assembliesToCheck = GetInheritanceChain(value.GetType()).Select(x => x.Assembly)
                                                                        .TakeWhile(x =>
                                                                             x != typeof(HttpApplication).Assembly)
                                                                        .Distinct();
            foreach (
                var assembly in
                assembliesToCheck.Where(
                    a => !AssemblyWasCompiledInDebugMode(a)))
            {
                badAssemblies.Add(assembly);
            }

            return !badAssemblies.Any();
        }

        private static bool AssemblyWasCompiledInDebugMode(Assembly assembly)
        {
            var debuggableAttrib = assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().SingleOrDefault();
            return debuggableAttrib != null && debuggableAttrib.IsJITTrackingEnabled;
        }

        private static IEnumerable<Type> GetInheritanceChain(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }

        #endregion

        public void Dispose()
        {
            if (_remotingChannel != null)
                ChannelServices.UnregisterChannel(_remotingChannel);
        }
    }
}
