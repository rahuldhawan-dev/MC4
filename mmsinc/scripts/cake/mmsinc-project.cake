// mmsinc-project.cake
//
// Helper that creates tasks for projects which consume mmsinc libraries.

#load "./mmsinc.cake"

using static System.IO.Path;

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

public class MMSINCProjectSettings : Cake.Core.Tooling.ToolSettings
{
    private string _configuration;

    public string SolutionFile { get; set; }
    public string Configuration
    {
        get => _configuration;
        set
        {
            switch(value.ToUpper())
            {
                case "HS":
                    _configuration = "QA";
                    break;
                case "TD":
                    _configuration = "QA2";
                    break;
                case "PROD":
                    _configuration = "QA3";
                    break;
                case "WQ":
                    _configuration = "QA4";
                    break;
                default:
                    _configuration = value;
                    break;
            }
        }
    }
    public string Platform { get; set; }
    public string PlatformConfig => Combine(Platform, Configuration);
    public Action PreBuild { get; set; }
    public string ProjectName { get; set; }
    public string SonarProjectKey { get; set; }
    public string SonarLogin { get; set; }
    public bool NoSonar { get; set; }
    public string UnitTestsPattern { get; set; }
}

public abstract class MMSINCProjectBase<TSettings>
    where TSettings : MMSINCProjectSettings
{
    public ICakeContext Context { get; protected set; }
    public Func<string, CakeTaskBuilder> Task { get; protected set; }
    public Func<string, CakeReport> RunTarget { get; protected set; }
    public TSettings Settings { get; protected set; }

    public MMSINCProjectBase(ICakeContext context, Func<string, CakeTaskBuilder> taskFn,
                             Func<string, CakeReport> runTargetFn, TSettings settings)
    {
        Context = context;
        Task = taskFn;
        RunTarget = runTargetFn;
        Settings = settings;
    }

    protected virtual void ProcessSettings()
    {
        Settings.Configuration = Context.Argument<string>("configuration", Settings.Configuration ?? "Debug");
        Settings.Platform = Context.Argument<string>("platform", Settings.Platform ?? "x64");
        Settings.SonarLogin = Context.Argument<string>("SonarLogin", Context.EnvironmentVariable<string>("SonarLogin", null));
        Settings.NoSonar = Context.Argument<bool>("NoSonar", Context.EnvironmentVariable<bool>("NO_SONAR", false));
    }

    protected virtual MSBuildSettings ConfigureBuild(MSBuildSettings settings) =>
        settings;

    protected virtual void DefineTasks()
    {
        var workingDirectory = System.IO.Directory.GetCurrentDirectory();

        Task("Clean")
            .Description($"Empty all bin/ and obj/ directories inside '{workingDirectory}'.")
            .Does(() =>
        {
            var path = Combine(workingDirectory, "**");
            Context.CleanDirectories(Combine(path, "bin", "x64"));
            Context.CleanDirectories(Combine(path, "obj", "x64"));
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
            .Does(() =>
        {
            Settings.PreBuild?.Invoke();

            if(Context.IsRunningOnWindows())
            {
                // Use MSBuild
                Context.MSBuild(Settings.SolutionFile, settings => {
                    ConfigureBuild(settings);
                    settings.SetConfiguration(Settings.Configuration);
                });
            }
            else
            {
                // Use XBuild
                Context.XBuild(Settings.SolutionFile, settings =>
                    settings.SetConfiguration(Settings.Configuration));
            }
        });

        var testDllPattern = "*Tests.dll";

        Task("Run-Unit-Tests")
            .Description($"Run MSTests in every dll inside '{workingDirectory}' matching '{testDllPattern}'.")
            .IsDependentOn("Build")
            .Does(() =>
        {
            var tests = Settings.UnitTestsPattern ??
                Combine(workingDirectory, "**", "bin", Settings.PlatformConfig, testDllPattern);
            var testResultsFile = Combine(GetTempPath(), "testResults.trx");
            var coverageResultsFile = Combine(GetTempPath(), "coverageResults.dcvr");

            Context.Information(
                $"Running unit tests from assemblies matching pattern: {tests}");

            try
            {
                // if DotCoverCover is not wrapped in a try/finally block the run will
                // error out on any test failures and TeamCity won't be able to list any
                // of the information
                Context.DotCoverCover(tool => {
                    tool.VSTest(tests, new VSTestSettings {
                        ArgumentCustomization = arg => arg
                            .Append($"/logger:trx;LogFileName={testResultsFile}"),
                        EnableCodeCoverage = true,
                        TestCaseFilter = Context.Argument<string>("testCaseFilter", (string)null)
                    });
                    }, new FilePath(coverageResultsFile),
                       new DotCoverCoverSettings {
                        TargetWorkingDir = workingDirectory,
                        WorkingDirectory = workingDirectory
                    }
                   .WithFilter("+:*")
                   .WithFilter("-:*Test"));
            }
            finally
            {
                if (Context.TeamCity().IsRunningOnTeamCity)
                {
                    Context.TeamCity().ImportData("mstest", testResultsFile);
                    Context.TeamCity().ImportDotCoverCoverage(coverageResultsFile,
                                                              Context.MakeAbsolute(Context.Directory("./tools/JetBrains.dotCover.CommandLineTools.2019.3.2/tools")));
                }
                else
                {
                    var coverageResultsHtml = new FilePath(coverageResultsFile).ChangeExtension("html");
                    Context.Information($"Writing coverage report to {coverageResultsHtml}");
                    Context.DotCoverReport(coverageResultsFile,
                           coverageResultsHtml,
                           new DotCoverReportSettings {
                               ReportType = DotCoverReportType.HTML
                           });
                }
            }
        });

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
            .IsDependentOn("SonarBegin")
            .IsDependentOn("Build")
            .IsDependentOn("SonarEnd");

        Task("Default")
            .Description("Clean, restore NuGet packages, build, and run unit tests.")
            .IsDependentOn("Run-Unit-Tests");
    }

    public void Run()
    {
        ProcessSettings();

        DefineTasks();

        Context.Information($"Working directory: {System.IO.Directory.GetCurrentDirectory()}");
        Context.Information($"Configuration: {Settings.Configuration}");
        Context.Information($"Platform: {Settings.Platform}");

        RunTarget(Context.Argument("target", "Default"));
    }
}

public class MMSINCProjectImpl : MMSINCProjectBase<MMSINCProjectSettings>
{
    public MMSINCProjectImpl(ICakeContext context, Func<string, CakeTaskBuilder> taskFn,
                             Func<string, CakeReport> runTargetFn, MMSINCProjectSettings settings) :
        base(context, taskFn, runTargetFn, settings) {}
}

Action<MMSINCProjectSettings> MMSINCProject = settings => {
    new MMSINCProjectImpl(Context, Task, RunTarget, settings).Run();
};
