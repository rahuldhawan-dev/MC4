#load "../mmsinc/scripts/cake/mapcall-web-project.cake"

var solution = "MapCall.sln";

MapCallWebProject(new MapCallWebProjectSettings {
    SolutionFile = solution,
    WebSitePath = "MapCall",
    ProjectName = "MapCall",
    SonarProjectKey = "mapcall",
    PreBuild = () => {
        var siteDir = Combine(".", "MapCall");
        var webConfig = Combine(siteDir, "web.config");

        if (!FileExists(webConfig))
        {
            CopyFile(Combine(siteDir, "web.config.base"), webConfig);
        }
    }
});
