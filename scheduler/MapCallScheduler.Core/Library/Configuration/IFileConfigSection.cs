using System.Configuration;

namespace MapCallScheduler.Library.Configuration
{
    public class FileConfigSection : ConfigurationSection, IFileConfigSection
    {
        public struct Keys
        {
            public const string WORKING_DIRECTORY = "workingDirectory", MAKE_CHANGES = "makeChanges";
        }

        [ConfigurationProperty(Keys.WORKING_DIRECTORY, IsRequired = true)]
        public string WorkingDirectory => this[Keys.WORKING_DIRECTORY].ToString();

        [ConfigurationProperty(Keys.MAKE_CHANGES, IsRequired = false, DefaultValue = false)]
        public bool MakeChanges => (bool)this[Keys.MAKE_CHANGES];

    }

    public interface IFileConfigSection
    {
        string WorkingDirectory { get; }
        bool MakeChanges { get; }
    }
}