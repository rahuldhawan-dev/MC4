// mapcall-non-web-project.cake
//
// Helper that creates tasks for non-web projects which consume mmsinc
// libraries and the MapCall domain/database, including release-building
// functionality.

#load "./mapcall-project.cake"

public class MapCallNonWebProjectSettings : MapCallProjectSettings {}

public class MapCallNonWebProjectBase<TSettings> : MapCallProjectBase<TSettings>
    where TSettings : MapCallNonWebProjectSettings
{
    public MapCallNonWebProjectBase(ICakeContext context,
                                    Func<string, CakeTaskBuilder> taskFn,
                                    Func<string, CakeReport> runTargetFn,
                                    Action<Func<ISetupContext, MapCallProjectData>> setupFn,
                                    TSettings settings) :
        base(context, taskFn, runTargetFn, setupFn, settings) {}

    protected override void DefineTasks()
    {
        base.DefineTasks();

        Task("Build-Release")
            .Description("Build, publish, and package a release in the ./release directory.")
            .IsDependentOn("Restore-NuGet-Packages")
            .Does<MapCallProjectData>(data => {
                Settings.PreBuild?.Invoke();

                var buildDir = BuildToDir(Settings.BuildDir);

                PackageBuild(buildDir);
            });
    }
}

public class MapCallNonWebProjectImpl : MapCallNonWebProjectBase<MapCallNonWebProjectSettings>
{
    public MapCallNonWebProjectImpl(ICakeContext context,
                                    Func<string, CakeTaskBuilder> taskFn,
                                    Func<string, CakeReport> runTargetFn,
                                    Action<Func<ISetupContext, MapCallProjectData>> setupFn,
                                    MapCallNonWebProjectSettings settings) :
        base(context, taskFn, runTargetFn, setupFn, settings) {}
}

Action<MapCallNonWebProjectSettings> MapCallNonWebProject = settings => {
    new MapCallNonWebProjectImpl(Context, Task, RunTarget, Setup<MapCallProjectData>, settings).Run();
};
