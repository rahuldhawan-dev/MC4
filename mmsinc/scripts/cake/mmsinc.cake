// mmsinc.cake
//
// Arguments, variables, and tasks for the mmsinc project.  These are
// useful to any project which consumes libraries from mmsinc.

#tool nuget:?package=Microsoft.TestPlatform&version=16.2.0
#tool nuget:?package=JetBrains.dotCover.CommandLineTools&version=2019.3.2
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool&version=4.8.0

#addin nuget:?package=Cake.Sonar&version=1.1.25
#addin nuget:?package=Cake.Git&version=1.0.1

using static System.IO.Path;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var configuration = Argument("configuration", "Debug");
var platform = Argument("platform", "x64");

var platformConfig = Combine(platform, configuration);
var sonarLogin = Argument<string>("SonarLogin", EnvironmentVariable<string>("SonarLogin", null));
var noSonar = Argument<bool>("NoSonar", EnvironmentVariable<bool>("NO_SONAR", false));

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var mmsincDir = Combine("..", "mmsinc");
var packageDir = Combine("..", "packages");
var mmsincSolution = Combine(mmsincDir, "MMSINC.sln");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

TaskSetup(c =>
{
    if (TeamCity.IsRunningOnTeamCity)
    {
        var description = c.Task.Description ?? c.Task.Name;

        TeamCity.WriteStartBuildBlock(description);
        TeamCity.WriteStartProgress(description);
    }
});

TaskTeardown(c =>
{
    if (TeamCity.IsRunningOnTeamCity)
    {
        var description = c.Task.Description ?? c.Task.Name;

        TeamCity.WriteEndProgress(description);
        TeamCity.WriteEndBuildBlock(description);
    }
});

Task("MMSINC:Clean")
    .Description($"Empty all bin/ and obj/ directories inside '{mmsincDir}'.")
    .Does(() =>
{
    var path = Combine(mmsincDir, "**");
    CleanDirectories(Combine(path, "bin", "x64"));
    CleanDirectories(Combine(path, "obj", "x64"));
});

Task("MMSINC:Restore-NuGet-Packages")
    .Description($"Restore NuGet packages for solution file '{mmsincSolution}'.")
    .IsDependentOn("MMSINC:Clean")
    .Does(() =>
{
    var projectDirs = new[] {"MMSINC.Core", "MapCall.Common.Testing"};
    NuGetRestore(mmsincSolution);

    // foreach (var dir in projectDirs)
    // {
    //     var projectDir = Combine(mmsincDir, dir, "bin", platformConfig);
    //     // this file doesn't seem to want to get there on its own.
    //     EnsureDirectoryExists(projectDir);
    //     CopyFile(Combine(packageDir, "evohtmltopdf_x64.7.5.0", "lib", "net40", "evointernal.dat"),
    //              Combine(projectDir, "evointernal.dat"));
    // }
});

Task("MMSINC:Build")
    .Description($"Build using solution file '{mmsincSolution}'.")
    .IsDependentOn("MMSINC:Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
        MSBuild(mmsincSolution, new MSBuildSettings {
                Configuration = configuration,
                MaxCpuCount = 0
            });
    }
    else
    {
        XBuild(mmsincSolution, settings =>
            settings.SetConfiguration(configuration));
    }
});

var testDllPattern = "*Test.dll";

Task("MMSINC:Run-Unit-Tests")
    .Description($"Run MSTests in every dll inside '{mmsincDir}' matching '{testDllPattern}'.")
    .IsDependentOn("MMSINC:Build")
    .Does(() =>
{
    var tests = Combine(mmsincDir, "**", "bin", platformConfig, testDllPattern);
    var testResultsFile = Combine(GetTempPath(), "testResults.trx");
    var coverageResultsFile = Combine(GetTempPath(), "coverageResults.dcvr");

    try
    {
        // if DotCoverCover is not wrapped in a try/finally block the run will
        // error out on any test failures and TeamCity won't be able to list any
        // of the information
        DotCoverCover(tool => {
            tool.VSTest(tests, new VSTestSettings {
                ToolPath = Context.Tools.Resolve("vstest.console.exe"),
                TestAdapterPath = Combine(packageDir, "MSTest.TestAdapter.2.2.8"),
                ArgumentCustomization = arg => arg.Append($"/logger:trx;LogFileName={testResultsFile}"),
                EnableCodeCoverage = true,
                TestCaseFilter = Argument("testCaseFilter", (string)null)
            });
        },  new FilePath(coverageResultsFile),
            new DotCoverCoverSettings {
                TargetWorkingDir = mmsincDir,
                WorkingDirectory = mmsincDir
            }
            .WithFilter("+:*")
            .WithFilter("-:*Test"));
    }
    finally
    {
        if (TeamCity.IsRunningOnTeamCity)
        {
            TeamCity.ImportData("mstest", testResultsFile);
            TeamCity.ImportDotCoverCoverage(coverageResultsFile,
                                            MakeAbsolute(Directory("./tools/JetBrains.dotCover.CommandLineTools.2019.3.2/tools")));
        }
        else
        {
            var coverageResultsHtml = new FilePath(coverageResultsFile).ChangeExtension("html");
            Information($"Writing coverage report to {coverageResultsHtml}");
            DotCoverReport(coverageResultsFile,
                           coverageResultsHtml,
                           new DotCoverReportSettings {
                               ReportType = DotCoverReportType.HTML
                           });
        }
    }
});

Task("MMSINC:SonarBegin")
    .Does(() => {
        if (noSonar)
        {
            Information("Skipping sonar analysis");
            return;
        }

        SonarBegin(new SonarBeginSettings {
            Branch = GitBranchCurrent("..").FriendlyName,
            Key = "mapcall-mmsinc",
            Name = "mapcall-mmsinc",
            Login = sonarLogin,
            Url = "https://sonar.awapps.com",
        });
    });

Task("MMSINC:SonarEnd")
    .Does(() => {
        if (noSonar)
        {
            Information("Skipping sonar analysis");
            return;
        }

        SonarEnd(new SonarEndSettings {
            Login = sonarLogin
        });
    });

Task("MMSINC:Sonar")
    .IsDependentOn("MMSINC:SonarBegin")
    .IsDependentOn("MMSINC:Build")
    .IsDependentOn("MMSINC:SonarEnd");
