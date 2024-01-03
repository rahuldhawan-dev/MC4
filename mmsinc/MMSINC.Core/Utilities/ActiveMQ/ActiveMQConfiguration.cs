using System.Configuration;

namespace MMSINC.Utilities.ActiveMQ
{
    public abstract class ActiveMQConfiguration : IActiveMQConfiguration
    {
        #region Private Members

        protected ActiveMQConfigurationSection _section;

        #endregion

        #region Properties

        public virtual ActiveMQConfigurationSection Section => _section ?? (_section =
            (ActiveMQConfigurationSection)ConfigurationManager
               .GetSection($"{GroupName}/{ActiveMQConfigurationSection.SECTION_NAME}"));

        public string Scheme => Section.Scheme;
        public string Host => Section.Host;
        public int Port => Section.Port;
        public string User => Section.User;
        public string Password => Section.Password;
        public string Url => $"failover:{Scheme}://{Host}:{Port}";

        #endregion

        #region Abstract Properties

        public abstract string GroupName { get; }

        #endregion
    }
}
