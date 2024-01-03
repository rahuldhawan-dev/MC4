#load "../mmsinc/scripts/cake/mapcall-web-project.cake"

var solution = "MapCallApi.sln";

MapCallWebProject(new MapCallWebProjectSettings {
    SolutionFile = solution,
    SonarProjectKey = "mapcall-api",
    ProjectName = "API",
    WebSitePath = "MapCallApi"
});
