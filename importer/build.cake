#load "../mmsinc/scripts/cake/mapcall-non-web-project.cake"

var solution = "MapCallImporter.sln";

MapCallProject(new MapCallProjectSettings {
    SolutionFile = solution,
    SonarProjectKey = "mapcall-importer",
});
