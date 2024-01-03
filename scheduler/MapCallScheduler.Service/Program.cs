using System;
using System.Net;
using System.ServiceProcess;
using log4net;
using StructureMap;

namespace MapCallScheduler
{
    public static class Program
    {
        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(Program)); }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            IContainer container;
            try
            {
                container = new Container(new DependencyRegistry());
            }
            catch (Exception e)
            {
                Log.Error("Configuration Error", e);
                throw;
            }

            MapCallScheduler scheduler;

            try
            {
                scheduler = container.GetInstance<MapCallScheduler>();
            }
            catch (Exception e)
            {
                Log.Error("Error Initializing Service", e);
                throw;
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServiceBase.Run(new ServiceBase[] {scheduler});
        }
    }
}
