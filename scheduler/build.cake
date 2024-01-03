#load "../mmsinc/scripts/cake/mapcall-service-project.cake"

using static System.IO.Path;
using static System.IO.Directory;

var solution = "MapCallScheduler.sln";

MapCallServiceProject(new MapCallServiceProjectSettings {
    Path = "MapcallScheduler.Service",
    SolutionFile = solution,
    SonarProjectKey = "mapcall-scheduler",
    ServerNames = "MapCallNP02.amwaternp.net",
    ServiceName = "MapCallScheduler.Service",
    ReleaseFileName = "scheduler.tar.bz2",
    ProjectName = "Scheduler",
    UnitTestsPattern =
        Combine(
            GetCurrentDirectory(),
            "MapCallScheduler.Tests",
            "**",
            "MapCallScheduler.Tests.dll")
});
