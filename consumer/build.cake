#load "../mmsinc/scripts/cake/mapcall-service-project.cake"

MapCallServiceProject(new MapCallServiceProjectSettings {
    SolutionFile = "MapCallKafkaConsumer.sln",
    Path = "MapCallKafkaConsumer.Service",
    SonarProjectKey = "mapcall-kafka-consumer",
    ServerNames = "MapCallNP02.amwaternp.net",
    ServiceName = "MapCallKafkaConsumer.Service",
    ProjectName = "Consumer",
    ReleaseFileName = "consumer.tar.bz2"
});
