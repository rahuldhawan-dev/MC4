#load "../mmsinc/scripts/cake/mapcall-non-web-project.cake"

var solution = "MapCallEverything.sln";

public class EverythingProjectImpl : MapCallProjectBase<MapCallProjectSettings>
{
    public EverythingProjectImpl(ICakeContext context,
                              Func<string, CakeTaskBuilder> taskFn,
                              Func<string, CakeReport> runTargetFn,
                              Action<Func<ISetupContext, MapCallProjectData>> setupFn,
                              MapCallProjectSettings settings) :
        base(context, taskFn, runTargetFn, setupFn, settings) {}

    private void DoBuild()
    {
        Settings.PreBuild?.Invoke();

        if(Context.IsRunningOnWindows())
        {
            // Use MSBuild
            Context.MSBuild(Settings.SolutionFile, settings =>
                            settings.SetConfiguration(Settings.Configuration));
        }
        else
        {
            // Use XBuild
            Context.XBuild(Settings.SolutionFile, settings =>
                           settings.SetConfiguration(Settings.Configuration));
        }
    }

    protected override void DefineTasks()
    {
        var workingDirectory = System.IO.Directory.GetCurrentDirectory();
        var checkoutDirectory = Combine(workingDirectory, "..");

        Task("Clean")
            .Description($"Empty all bin/ and obj/ directories inside '{checkoutDirectory}'.")
            .Does(() =>
        {
            var path = Combine(checkoutDirectory, "**");
            Context.CleanDirectories(Combine(path, "bin", "x64"));
            Context.CleanDirectories(Combine(path, "obj"));
        });

        Task("Restore-NuGet-Packages")
            .Description($"Restore NuGet packages for solution file '{Settings.SolutionFile}'.")
            .IsDependentOn("Clean")
            .Does(() =>
        {
            Context.NuGetRestore(Settings.SolutionFile);
        });

        Task("Build")
            .Description($"Build using solution file '{Settings.SolutionFile}'.")
            .IsDependentOn("Restore-NuGet-Packages")
            .Does(DoBuild);

        Task("Build-No-Restore")
            .Description($"Build using solution file '{Settings.SolutionFile}', but without first cleaning or restoring NuGet packages.")
            .Does(DoBuild);

        Task("SonarBegin")
            .Does(() => {
                if (Settings.NoSonar)
                {
                    Context.Information("Skipping SonarQube analysis");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Settings.SonarProjectKey))
                {
                    throw new System.InvalidOperationException("You must set SonarProjectKey for this project.");
                }
                if (string.IsNullOrWhiteSpace(Settings.SonarLogin))
                {
                    throw new System.InvalidOperationException("You must set SonarLogin for this project.");
                }

                Context.SonarBegin(new SonarBeginSettings {
                    Branch = Context.GitBranchCurrent("..").FriendlyName,
                    Key = Settings.SonarProjectKey,
                    Name = Settings.SonarProjectKey,
                    Login = Settings.SonarLogin,
                    Url = "https://sonar.awapps.com",
                    ArgumentCustomization = args => args
                        .Append("/d:sonar.scm.disabled=True")
                });
            });

        Task("SonarEnd")
            .Does(() => {
                if (Settings.NoSonar)
                {
                    Context.Information("Skipping SonarQube analysis");
                    return;
                }

                Context.SonarEnd(new SonarEndSettings {
                    Login = Settings.SonarLogin
                });
            });

        Task("Sonar")
            .IsDependentOn("Restore-NuGet-Packages")
            .IsDependentOn("SonarBegin")
            .IsDependentOn("Build-No-Restore")
            .IsDependentOn("SonarEnd");

        Task("Default")
            .Description("Clean, restore NuGet packages, and build.")
            .IsDependentOn("Build");

        Setup(_ => new MapCallProjectData());
    }
}

Action<MapCallProjectSettings> EverythingProject = settings => {
    new EverythingProjectImpl(Context, Task, RunTarget, Setup<MapCallProjectData>, settings).Run();
};


EverythingProject(new MapCallProjectSettings {
    SolutionFile = solution,
    SonarProjectKey = "mapcall",
});
