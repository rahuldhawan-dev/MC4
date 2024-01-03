#load "../mmsinc/scripts/cake/mapcall-service-project.cake"

var solution = "MapCallActiveMQListener.sln";

MapCallServiceProject(new MapCallServiceProjectSettings {
    Path = "src\\MapCallActiveMQListener",
    SolutionFile = solution,
    SonarProjectKey = "mapcall-activemq-listener",
    ServerNames = "MapCallNP01.amwaternp.net,MapCallNP02.amwaternp.net",
    ServiceName = "MapCallActiveMQListener",
    ReleaseFileName = "listener.tar.bz2"
});
