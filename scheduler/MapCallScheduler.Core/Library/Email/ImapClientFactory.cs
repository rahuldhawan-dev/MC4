using System.Collections.Generic;
using MapCallScheduler.Library.Configuration;
using StructureMap;
using StructureMap.Pipeline;

namespace MapCallScheduler.Library.Email
{
    public class ImapClientFactory : IImapClientFactory
    {
        #region Constants

        public struct ImapClientArgs
        {
            #region Constants

            public const string SERVER = "hostname",
                PORT = "port",
                USERNAME = "username",
                PASSWORD = "password";

            #endregion
        }

        #endregion

        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public ImapClientFactory(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public IWrappedImapClient Build(IIncomingEmailConfigSection config)
        {
            return _container.GetInstance<IWrappedImapClient>(new ExplicitArguments(new Dictionary<string, object> {
                {ImapClientArgs.SERVER, config.Server},
                {ImapClientArgs.PORT, config.Port},
                {ImapClientArgs.USERNAME, config.Username},
                {ImapClientArgs.PASSWORD, config.Password}
            }));
        }

        #endregion
    }

    /// <summary>
    /// Builds IImapClients using configuration data from IIncomingEmailConfigSections.
    /// </summary>
    public interface IImapClientFactory
    {
        #region Abstract Methods

        IWrappedImapClient Build(IIncomingEmailConfigSection config);

        #endregion
    }
}