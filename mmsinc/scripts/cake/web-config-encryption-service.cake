using System.Xml;
using static System.IO.Path;

public class WebConfigEncryptionService
{
    #region Constants

    public static readonly string[] SECTIONS = new[] {
        "connectionStrings",
        "appSettings"
    };

    public const string PROVIDER = "customProvider";
    public const string DEV_KEY_CONTAINER = "mcDevKeys";

    #endregion

    #region Properties

    public ICakeContext Context { get; }
    public string Configuration { get; }
    public string ConfigDirectory { get; }
    public bool ConfigIsDebug { get; }

    public string ConfigContainerName
    {
        get
        {
            switch (Configuration.ToUpper())
            {
                case "QA1":
                case "QA2":
                case "QA3":
                case "QA4":
                case "TRAINING":
                    return "mcQaKeys";
                case "RELEASE":
                    return "mcProdKeys";
                default:
                    return DEV_KEY_CONTAINER;
            }
        }
    }

    #endregion

    #region Constructors

    public WebConfigEncryptionService(
        ICakeContext context,
        string configuration,
        string configDirectory)
    {
        Context = context;
        Configuration = configuration;
        ConfigDirectory = configDirectory;
        ConfigIsDebug = Configuration.ToUpper() == "DEBUG";
    }

    #endregion

    #region Private Methods

    private string MaybeBackupCurrentWebConfig()
    {
        if (ConfigIsDebug)
        {
            return null;
        }

        var backupPath = System.IO.Path.GetTempFileName();
        Context.Information(
            $"Backing up current web.config to '{backupPath}'");

        System.IO.File.Copy(
            Combine(ConfigDirectory, "Web.config"), backupPath, true);

        return backupPath;
    }

    private void MaybeReplaceWebConfig(string backupPath)
    {
        if (ConfigIsDebug)
        {
            return;
        }

        Context.Information(
            $"Replacing web.config with backup from '{backupPath}'");

        System.IO.File.Copy(
            backupPath, Combine(ConfigDirectory, "Web.config"), true);
    }

    private (string, XmlDocument) LoadConfigDocument(string configFile)
    {
        var path = Combine(ConfigDirectory, configFile);
        var doc = new XmlDocument();
        doc.Load(path);

        return (path, doc);
    }

    private void MaybeCopyConfigSections(
        string fromConfig,
        string toConfig,
        string keyContainerName,
        bool removeProtectionAttribute = false)
    {
        if (ConfigIsDebug)
        {
            return;
        }

        Context.Information(
            $"Copying relevant config sections from '{fromConfig}' to " +
            $"'{toConfig}'");

        var (fromConfigPath, fromConfigDoc) = LoadConfigDocument(fromConfig);
        var (toConfigPath, toConfigDoc) = LoadConfigDocument(toConfig);

        foreach (var section in SECTIONS)
        {
            var fromNode = fromConfigDoc
                .DocumentElement.SelectSingleNode(section);
            var toNode = toConfigDoc
                .DocumentElement.SelectSingleNode(section);

            if (removeProtectionAttribute)
            {
                Context.Debug(
                    "Removing 'configProtectionProvider' attribute from the " +
                    $"'{section}' element of '{toConfig}' so that encryption " +
                    "can be performed");
                toNode.Attributes.RemoveNamedItem("configProtectionProvider");
            }

            Context.Debug(
                $"Removing all children of the '{section}' element of " +
                $"'{toConfig}'");

            // NOTE: if we just iterate over `toNode.ChildNodes` here, the
            // iterator will become invalid the first time
            // `toNode.RemoveChild(...)` happens, and the foreach will bomb out
            // leaving the job incomplete.  thus we must cast to an enumerable
            // of XmlNode and then project that to a list so we can iterate over
            // all of them
            var childNodes = toNode.ChildNodes.Cast<XmlNode>().ToList();
            foreach (XmlNode child in childNodes)
            {
                toNode.RemoveChild(child);
            }

            Context.Debug(
                $"Copying all children of the '{section}' element of " +
                $"'{fromConfig}' to the same element in '{toConfig}'");

            foreach (XmlNode child in fromNode.ChildNodes)
            {
                toNode.AppendChild(toConfigDoc.ImportNode(child, true));
            }
        }

        var configProviderElement =
            (XmlElement)toConfigDoc.SelectSingleNode(
                "//configProtectedData/providers/add[@name='customProvider']");
        configProviderElement.SetAttribute(
            "keyContainerName",
            ConfigContainerName);

        toConfigDoc.Save(toConfigPath);
    }

    private void EncryptOrDecrypt(
        string action,
        Action<SectionInformation> perform)
    {
        var configPath = Combine(ConfigDirectory, "Web.config");
        var map = new ExeConfigurationFileMap {
            ExeConfigFilename = configPath
        };
        var config = ConfigurationManager
            .OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

        Context.Information($"{action}ing '{configPath}'");

        foreach (var section in SECTIONS)
        {
            perform(config.GetSection(section).SectionInformation);
        }

        config.Save();
    }

    private void ProcessWebConfig(
        string action,
        Action<SectionInformation> perform,
        bool removeFirstProtectionAttribute = false)
    {
        var otherConfigFile = $"Web.{Configuration}.config";
        var webConfigBackupPath =
            MaybeBackupCurrentWebConfig();

        try
        {
            MaybeCopyConfigSections(
                otherConfigFile,
                "Web.config",
                ConfigContainerName,
                removeFirstProtectionAttribute);

            EncryptOrDecrypt(action, perform);

            MaybeCopyConfigSections(
                "Web.config",
                otherConfigFile,
                DEV_KEY_CONTAINER);
        }
        finally
        {
            MaybeReplaceWebConfig(webConfigBackupPath);
        }
    }

    #endregion

    #region Exposed Methods

    public void Encrypt()
    {
        ProcessWebConfig(
            nameof(Encrypt),
            i => i.ProtectSection(PROVIDER),
            true);
    }

    public void Decrypt()
    {
        ProcessWebConfig(
            nameof(Decrypt),
            i => i.UnprotectSection());
    }

    #endregion
}
