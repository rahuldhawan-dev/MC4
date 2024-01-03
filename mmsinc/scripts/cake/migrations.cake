// migrations.cake
//
// Arguments, variables, and tasks for running migrations for the
// MapCall domain/database.

#addin nuget:?package=Cake.FLuentMigrator&version=0.4.0

#tool nuget:?package=FluentMigrator.Console&version=3.3.1
#tool nuget:?package=OctopusTools&version=9.1.7

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var dbHost = Argument("dbHost", "localhost");
var database = Argument("database", "mapcalldev");
var dbUser = Argument("dbUser", "");
var dbPassword = Argument("dbPassword", "");
var previewOnly = Argument("preview", false);
var output  = Argument("output", false);
var outputFileName = Argument("outputFileName", "");
var steps = Argument("steps", 0);
var version = Argument("dbVersion", (long)0);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var releaseDir = Combine("..", "release");
var migrationsDir = Combine("..", "migrations");
var migrationsPackageDir = Combine("..", "packages");
var migrationsSolution = Combine(migrationsDir, "Migrations.sln");
var migrationsProjectDir = Combine(migrationsDir, "MapCallMigrations");
var migrationsNuspec = Combine(migrationsProjectDir, "MapCallMigrations.nuspec");
var migrationAssembly = Combine(migrationsProjectDir, "bin", configuration, "net472", "MapCallMigrations.dll");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
var connection = "";

if(dbUser != "" && dbPassword != ""){
    connection = "Data Source= " + dbHost +";uid=" + dbUser + ";password=" + dbPassword + ";Initial Catalog=" + database + ";Integrated Security=false";
}
else{
    connection = "Data Source= " + dbHost + ";Initial Catalog=" + database + ";Integrated Security=true";
}

System.Action<string> migrateTaskFn = task => FluentMigrator(new FluentMigratorSettings {
    Assembly = migrationAssembly,
    Connection = connection,
    PreviewOnly = previewOnly,
    Output = output,
    OutputFileName = outputFileName,
    Provider = "sqlserver",
    Steps = steps,
    Tags = new[] {"Production"},
    Task = task,
    // TODO: this should probably come from the current nuget version,
    //       which I believe cake provides.
    ToolPath = Combine(".", "tools", "FluentMigrator.Console.3.3.1", "net461", "x64", "Migrate.exe"),
    Version = version
});

Task("Migrations:Clean")
    .Description($"Empty all bin/ and obj/ directories inside '{migrationsDir}'.")
    .Does(() =>
{
    var path = Combine(migrationsDir, "**");
    CleanDirectories(Combine(path, "bin", "x64"));
    CleanDirectories(Combine(path, "obj", "x64"));
});

Task("Migrations:Restore-NuGet-Packages")
    .Description($"Restore NuGet packages for solution file '{migrationsSolution}'.")
    .IsDependentOn("Migrations:Clean")
    .Does(() =>
{
    NuGetRestore(migrationsSolution, new NuGetRestoreSettings {
        // need to force this to use the packages directory so we can access the migrator tools when we build
        // the nuget package
        NoCache = true,
        PackagesDirectory = migrationsPackageDir
    });
});

Task("Migrations:Build")
    .Description($"Build using solution file '{migrationsSolution}'.")
    .IsDependentOn("Migrations:Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
        MSBuild(migrationsSolution, new MSBuildSettings {
                Configuration = configuration,
                MaxCpuCount = 0
            });
    }
    else
    {
        XBuild(migrationsSolution, settings =>
            settings.SetConfiguration(configuration));
    }
});

Task("Migrations:Package")
    .Description($"Build a nuget package for the migrations")
    .IsDependentOn("Migrations:Build")
    .Does(() =>
{
    var publishDir = Combine(migrationsProjectDir, "bin", "release");
    var publishSettings = new MSBuildSettings();
    publishSettings.Targets.Add("Publish");
    publishSettings.Properties.Add("OutputPath", new List<string> {
        // MSBuild seems to run with the project dir as the working directory, so we need to send in a higher
        // relative path
        Combine("..", publishDir) + "\\"
    });

    MSBuild(migrationsSolution, publishSettings);

    var preRelease = Context.Argument<string>("preRelease", null)
        ?? Context.GitBranchCurrent("..").FriendlyName.Replace("-", "");
    var now = DateTime.Now;
    var version = $"{now.Year}.{now.Month}.{now.Day}";
    var build = Math.Round((now - now.Date).TotalSeconds);

    var fluentMigratorConsolePackage =
        Combine(migrationsPackageDir, "fluentmigrator.console", "3.3.1", "net461", "x64");

    OctoPack("Migrations", new OctopusPackSettings {
        Include = new[] {
            Combine(fluentMigratorConsolePackage, "*.dll"),
            Combine(fluentMigratorConsolePackage, "*.exe"),
            // MSBuild will append "publish" to the end of the path no matter what we do it seems
            Combine(publishDir, "publish", "*.dll"),
            Combine(migrationsProjectDir, "Scripts", "*.ps1")
        },
        OutFolder = releaseDir,
        Version = $"{now.Year}.{now.Month}.{now.Day}-{preRelease}.{build}"
    });
});

Task("Migrate-Up")
    .Description($"Migrate up using the migrations defined in assembly '{migrationAssembly}'.")
    .IsDependentOn("Migrations:Build")
    .Does(() =>
{
    migrateTaskFn("migrate");
});

Task("Migrate-Down")
    .Description($"Migrate down using the migrations defined in assembly '{migrationAssembly}'.")
    .IsDependentOn("Migrations:Build")
    .Does(() =>
{
    migrateTaskFn("migrate:down");
});

Task("Rollback")
    .Description($"Revert the most recent migration defined in assembly '{migrationAssembly}'.")
    .IsDependentOn("Migrations:Build")
    .Does(() =>
{
    migrateTaskFn("rollback");
});

Task("Migrate")
    .Description($"Migrate up using the migrations defined in assembly '{migrationAssembly}'.")
    .IsDependentOn("Migrate-Up");
