#load "../mmsinc/scripts/cake/mapcall-web-project.cake"

using static System.IO.Path;

var solution = "LINQTo271.sln";

MapCallWebProject(new MapCallWebProjectSettings {
    SolutionFile = solution,
    WebSitePath = "LINQTo271",
    ProjectName = "WorkOrders",
    SonarProjectKey = "mapcall-workorders",
    PreBuild = () => {
        var siteDir = Combine(".", "LINQto271");
        var webConfig = Combine(siteDir, "web.config");

        if (!FileExists(webConfig))
        {
            CopyFile(Combine(siteDir, "web.config.base"), webConfig);
        }
    }
});
