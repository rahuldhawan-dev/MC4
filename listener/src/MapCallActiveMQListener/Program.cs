using System;
using System.ServiceProcess;
using log4net;
using MapCallActiveMQListener.Ioc;
using StructureMap;

namespace MapCallActiveMQListener
{
    static class Program
    {
        #region Private Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            IContainer container;
            ILog log;

            try
            {
                container = DependencyRegistry.Initialize();
                log = container.GetInstance<ILog>();
            }
            catch (Exception e)
            {
                LogManager.GetLogger(typeof(Program)).Error("Configuration Error", e);
                throw;
            }

            MapCallActiveMQListener listener;

            try
            {
                listener = container.GetInstance<MapCallActiveMQListener>();
            }
            catch (Exception e)
            {
                log.Error("Error Initializing Service", e);
                throw;
            }

            log.Info("Starting Listener");

            if (Environment.UserInteractive)
            {
                listener.Start();
                Console.WriteLine("Press RETURN to exit...");
                Console.ReadLine();
                listener.Stop();
            }
            else
            {
                ServiceBase.Run(new ServiceBase[] {listener});
            }
        }

        #endregion
    }
}
