#load "./scripts/cake/mmsinc.cake"
#load "./scripts/cake/migrations.cake"
#load "./scripts/cake/data-import.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .IsDependentOn("MMSINC:Clean");

Task("Restore-NuGet-Packages")
    .IsDependentOn("MMSINC:Restore-NuGet-Packages");

Task("Build")
    .IsDependentOn("MMSINC:Build");

Task("Run-Unit-Tests")
    .IsDependentOn("MMSINC:Run-Unit-Tests");

Task("Data-Refresh")
    .IsDependentOn("Data-Import")
    .IsDependentOn("Migrate");

Task("Sonar")
    .IsDependentOn("MMSINC:Sonar");

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("MMSINC:Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
