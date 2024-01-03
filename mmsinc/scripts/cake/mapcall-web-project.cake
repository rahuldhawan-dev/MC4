// mapcall-web-project.cake
//
// Helper that creates tasks for web projects which consume mmsinc
// libraries and the MapCall domain/database, including
// deployment-related functionality.

#load "./mapcall-project.cake"
#load "./web-config-encryption-service.cake"

#addin nuget:?package=DotNet.Xdt&version=2.1.1
#addin nuget:?package=Cake.XdtTransform&version=0.18.1
#tool nuget:?package=NUnit.ConsoleRunner&version=3.5.0
#tool nuget:?package=NUnit.Extension.TeamCityEventListener&version=1.0.7

using static System.IO.Path;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web.Configuration;

public class MapCallWebProjectSettings : MapCallProjectSettings
{
    public string WebSitePath { get; set; }
}

public class MapCallWebProjectBase<TSettings> : MapCallProjectBase<TSettings>
    where TSettings : MapCallWebProjectSettings
{
    public MapCallWebProjectBase(
        ICakeContext context,
        Func<string, CakeTaskBuilder> taskFn,
        Func<string, CakeReport> runTargetFn,
        Action<Func<ISetupContext, MapCallProjectData>> setupFn,
        TSettings settings)
        : base(context, taskFn, runTargetFn, setupFn, settings) {}

    protected override MSBuildSettings ConfigureBuild(MSBuildSettings settings)
        => settings.WithProperty("BuildViewsTooPlease", "true");

    // need to override to provide "WebProjectOutputDir" and change
    // "OutDir" to include bin/
    protected override string BuildToDir(string buildDir)
    {
        if (!Context.DirectoryExists(buildDir))
        {
            Context.Information("Creating build directory '{0}'...", buildDir);
            System.IO.Directory.CreateDirectory(buildDir);
        }

        Context.Information("Building to directory '{0}'...", buildDir);
        if (Context.IsRunningOnWindows())
        {
            // Use MSBuild
            Context.MSBuild(Settings.SolutionFile, settings => {
                settings = settings
                    .WithProperty("WebProjectOutputDir", buildDir)
                    .WithProperty("OutDir", Combine(buildDir, "bin"))
                    .SetConfiguration(Settings.Configuration);

                if (Settings.Configuration == "Release")
                {
                    settings = settings
                        .WithProperty("DebugSymbols", "false")
                        .WithProperty("DebugType", "none");
                }
            });
        }
        else
        {
            // Use XBuild
            Context.XBuild(Settings.SolutionFile, settings => {
                settings = settings
                    .WithProperty("WebProjectOutputDir", buildDir)
                    .WithProperty("OutDir", Combine(buildDir, "bin"))
                    .SetConfiguration(Settings.Configuration);

                if (Settings.Configuration == "Release")
                {
                    settings = settings
                        .WithProperty("DebugSymbols", "false")
                        .WithProperty("DebugType", "none");
                }
            });
        }

        return buildDir;
    }

    private void DoDecrypt()
    {
        var workingDirectory = System.IO.Directory.GetCurrentDirectory();
        var configDirectory = Combine(workingDirectory, Settings.WebSitePath);

        new WebConfigEncryptionService(
            Context,
            Settings.Configuration,
            configDirectory)
            .Decrypt();
    }

    private void DoEncrypt()
    {
        var workingDirectory = System.IO.Directory.GetCurrentDirectory();
        var configDirectory = Combine(workingDirectory, Settings.WebSitePath);

        new WebConfigEncryptionService(
            Context,
            Settings.Configuration,
            configDirectory)
            .Encrypt();
    }

    protected override void DefineTasks()
    {
        base.DefineTasks();
        Task("Run-Functional-Tests")
            .Description(
                "Run NUnit-based functional tests (in a project called " +
                "'RegressionTests')")
            .IsDependentOn("Build")
            .Does(() => {
                var path = System.IO.Directory.GetCurrentDirectory();
                string testFileName;

                // mvc has the project called "RegressionTests", and intranet
                // has it inside a "tests" folder
                (testFileName, path) = path switch {
                    _ when path.EndsWith("\\api") => ("FunctionalTests", path),
                    _ when path.EndsWith("\\intranet")
                        => ("RegressionTests", Combine(path, "tests")),
                    _ => ("RegressionTests", path)
                };
                var testProject = Combine(
                    path,
                    testFileName,
                    "bin",
                    Settings.PlatformConfig,
                    $"{testFileName}.dll");
                var test = Context.HasArgument("test")
                    ? Context.Argument<string>("test")
                    : null;

                if(!System.IO.File.Exists(testProject))
                {
                    throw new FileNotFoundException(
                        $"Test dll does not exist: {testProject}");
                }

                Context.Information("Running testProject '{0}'...", testProject);

                try
                {
                    Context.NUnit3(testProject, new NUnit3Settings {
                        Test = test,
                        NoHeader = true,
                        NoResults = true,
                        TeamCity = Context.TeamCity().IsRunningOnTeamCity
                    });
                }
                catch (Exception) {}
        });

        Task("Build-Release")
            .Description(
                "Build, publish, and package a release in the ./release " +
                "directory.")
            .IsDependentOn("Restore-NuGet-Packages")
            .Does<MapCallProjectData>(data => {
                Settings.PreBuild?.Invoke();

                var buildDir = BuildToDir(Settings.BuildDir);

                Context.Information($"Applying {Settings.Configuration} transform to the web.config file...");

                var workingDirectory = System.IO.Directory.GetCurrentDirectory();
                var configDirectory = Combine(workingDirectory, Settings.WebSitePath);
                Context.XdtTransformConfig(
                    new FilePath(Combine(configDirectory, "web.config")),
                    new FilePath(Combine(
                                     configDirectory,
                                     $"web.{Settings.Configuration}.config")),
                    new FilePath(Combine(buildDir, "web.config")));

                PackageBuild(buildDir);
        });

        Task("Decrypt")
            .Description("Decrypt the specified web.config file")
            .Does(DoDecrypt);

        Task("Encrypt")
            .Description("Encrypt the specified web config file")
            .Does(DoEncrypt);
    }
}

public class MapCallWebProjectImpl
    : MapCallWebProjectBase<MapCallWebProjectSettings>
{
    public MapCallWebProjectImpl(
        ICakeContext context,
        Func<string, CakeTaskBuilder> taskFn,
        Func<string, CakeReport> runTargetFn,
        Action<Func<ISetupContext, MapCallProjectData>> setupFn,
        MapCallWebProjectSettings settings)
        : base(context, taskFn, runTargetFn, setupFn, settings) {}
}

Action<MapCallWebProjectSettings> MapCallWebProject = settings => {
    new MapCallWebProjectImpl(
        Context,
        Task,
        RunTarget,
        Setup<MapCallProjectData>,
        settings).Run();
};
