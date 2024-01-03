// mapcall-project.cake
//
// Helper that creates tasks for projects which consume mmsinc libraries
// and the MapCall domain/database.

#load "./mmsinc-project.cake"
#load "./migrations.cake"
#load "./data-import.cake"

#tool nuget:?package=OctopusTools&version=9.1.7

public class MapCallProjectData { }

public class MapCallProjectSettings : MMSINCProjectSettings
{
    public string BuildDir { get; set; }
}

public abstract class MapCallProjectBase<TSettings> : MMSINCProjectBase<TSettings>
    where TSettings : MapCallProjectSettings
{
    public Action<Func<ISetupContext, MapCallProjectData>> Setup { get; }

    public MapCallProjectBase(ICakeContext context,
                              Func<string, CakeTaskBuilder> taskFn,
                              Func<string, CakeReport> runTargetFn,
                              Action<Func<ISetupContext, MapCallProjectData>> setupFn,
                              TSettings settings) :
        base(context, taskFn, runTargetFn, settings)
    {
        Setup = setupFn;
    }

    protected string DetermineBuildDir()
    {
        var buildDir = Context.Argument<string>("buildDir", null);

        if (string.IsNullOrWhiteSpace(buildDir))
        {
            buildDir = Combine(GetTempPath(), Guid.NewGuid().ToString());
        }
        else
        {
            buildDir = GetFullPath(buildDir);
        }

        return buildDir;
    }

    protected virtual string BuildToDir(string buildDir)
    {
        if (!Context.DirectoryExists(buildDir))
        {
            Context.Information("Creating build directory '{0}'...", buildDir);
            System.IO.Directory.CreateDirectory(buildDir);
        }

        Context.Information("Creating build directory '{0}'...", buildDir);
        if (Context.IsRunningOnWindows())
        {
            // Use MSBuild
            Context.MSBuild(Settings.SolutionFile, settings => {
                settings = settings
                    .WithProperty("OutDir", buildDir)
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
                    .WithProperty("OutDir", buildDir)
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

    protected void PackageBuild(string buildDir)
    {
        var preRelease = Context.Argument<string>("preRelease", null)
            ?? Context.GitBranchCurrent("..").FriendlyName.Replace("-", "");
        var now = DateTime.Now;
        var build = Math.Round((now - now.Date).TotalSeconds);

        Context.OctoPack(Settings.ProjectName, new OctopusPackSettings {
            BasePath = buildDir,
            OutFolder = buildDir,
            Version = $"{now.Year}.{now.Month}.{now.Day}-{preRelease}.{build}"
        });
    }

    protected override void ProcessSettings()
    {
        base.ProcessSettings();
        Settings.BuildDir = DetermineBuildDir();
    }

    protected override void DefineTasks()
    {
        base.DefineTasks();

        Task("Data-Refresh")
            .IsDependentOn("Data-Import")
            .IsDependentOn("Migrate");

        Setup(_ => new MapCallProjectData());
    }
}

public class MapCallProjectImpl : MapCallProjectBase<MapCallProjectSettings>
{
    public MapCallProjectImpl(ICakeContext context,
                              Func<string, CakeTaskBuilder> taskFn,
                              Func<string, CakeReport> runTargetFn,
                              Action<Func<ISetupContext, MapCallProjectData>> setupFn,
                              MapCallProjectSettings settings) :
        base(context, taskFn, runTargetFn, setupFn, settings) {}
}

Action<MapCallProjectSettings> MapCallProject = settings => {
    new MapCallProjectImpl(Context, Task, RunTarget, Setup<MapCallProjectData>, settings).Run();
};
