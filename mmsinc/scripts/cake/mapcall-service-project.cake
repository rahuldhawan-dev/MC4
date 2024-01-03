// mapcall-service-project.cake
//
// Helper that creates tasks for windows services which consume mmsinc
// libraries and the MapCall domain/database, including
// deployment-related functionality.

#load "./mapcall-non-web-project.cake"

public class MapCallServiceProjectSettings : MapCallNonWebProjectSettings
{
    public string Path { get; set; }
    public string ServerNames { get; set; }
    public string ServiceName { get; set; }
    public string ReleaseFileName { get; set; }
}

public class MapCallServiceProjectBase<TSettings> : MapCallNonWebProjectBase<TSettings>
    where TSettings : MapCallServiceProjectSettings
{
    // constructors
    public MapCallServiceProjectBase(ICakeContext context,
                                 Func<string, CakeTaskBuilder> taskFn,
                                 Func<string, CakeReport> runTargetFn,
                                 Action<Func<ISetupContext, MapCallProjectData>> setupFn,
                                 TSettings settings) :
        base(context, taskFn, runTargetFn, setupFn, settings) {}
}

public class MapCallServiceProjectImpl : MapCallServiceProjectBase<MapCallServiceProjectSettings>
{
    public MapCallServiceProjectImpl(ICakeContext context,
                                 Func<string, CakeTaskBuilder> taskFn,
                                 Func<string, CakeReport> runTargetFn,
                                 Action<Func<ISetupContext, MapCallProjectData>> setupFn,
                                 MapCallServiceProjectSettings settings) :
        base(context, taskFn, runTargetFn, setupFn, settings) {}
}

Action<MapCallServiceProjectSettings> MapCallServiceProject = settings => {
    new MapCallServiceProjectImpl(Context, Task, RunTarget, Setup<MapCallProjectData>, settings).Run();
};
