#load "../mmsinc/scripts/cake/mapcall-web-project.cake"

using static System.IO.Path;
using static System.IO.Directory;

var solution = "MapCallMVC.sln";

MapCallWebProject(new MapCallWebProjectSettings {
    SolutionFile = solution,
    SonarProjectKey = "mapcall-mvc",
    WebSitePath = "MapCallMvc",
    ProjectName = "MVC",
    UnitTestsPattern =
        Combine(
            GetCurrentDirectory(),
            "MapCallMVC.Tests",
            "**",
            "MapCallMVC.Tests.dll")
});
