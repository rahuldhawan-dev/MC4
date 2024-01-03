#load "../mmsinc/scripts/cake/mapcall-web-project.cake"

var solution = "contractors.sln";

MapCallWebProject(new MapCallWebProjectSettings {
    SolutionFile = solution,
    SonarProjectKey = "mapcall-contractors",
    ProjectName = "Contractors",
    WebSitePath = "Contractors"
});
