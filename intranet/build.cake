#load "../mmsinc/scripts/cake/mapcall-web-project.cake"

using static System.IO.Path;

var solution = "MapCallIntranet.sln";

MapCallWebProject(new MapCallWebProjectSettings {
    SolutionFile = solution,
    SonarProjectKey = "mapcall-intranet",
    ProjectName = "Intranet",
    WebSitePath = "src/MapCallIntranet",
    UnitTestsPattern =
        Combine("tests", "MapCallIntranet.Tests", "bin", "x64", "Debug", "MapCallIntranet.Tests.dll")
});
