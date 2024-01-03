# Cake

[Cake](https://cakebuild.net/) is a tool for creating scripts in .net, with many
plugins specifically related to project builds, testing and maintanence.

## Cake Targets

### Clean

The `Clean` target will delete the contents of all `bin` and `obj` folders found
within the current project directory.  This target uses `MMSINC:Clean` as a
dependency, so `MMSINC:Clean` will be run first.

### Restore-NuGet-Packages

The `Restore-NuGet-Packages` target will restore all relevant packages for the
current solution.  This target uses `Clean` and `MMSINC:Restore-NuGet-Packages`
as dependencies, so those will be run first (in that order).

### Build

The `Build` target will build the current solution in-place.  This target uses
`Restore-NuGet-Packages` as a dependency, so `Restore-NuGet-Packages` (and its
dependencies) will be run first.

#### Options

The `--Configuration` flag will build for different configurations, such as
`Debug`, `Release`, and `QA`.  These will affect what projects get built,
whether or not debugging symbols (.pdb files) are generated, and which
transforms are run on the main project's .config file.

### Run-Unit-Tests

The `Run-Unit-Tests` target will run MSTest unit tests in any test projects
within the current solution.  This target uses `Build` as a dependency, so
`Build` (and its dependencies) will be run first.

This is the default target, so simply running `build` with no `--target` flag
will cause this target to execute.

#### Options

The `--testCaseFilter` flag accepts an expression to run only tests that match,
of the format `<property>Operator<value>[|&<Expression>]` where Operator is one
of =, != or ~ (Operator ~ has 'contains' semantics and is applicable for string
properties like DisplayName). Parenthesis () can be used to group
sub-expressions. Examples: `Priority=1
(FullyQualifiedName~Nightly|Name=MyTestMethod)`.

### Run-Functional-Tests

The `Run-Functional-Tests` target will run NUnit functional tests in any
functional test projects (generally called `RegressionTests`) within the current
solution.  This target uses `Build` as a dependency, so `Build` (and its
dependencies) will be run first.

### Build-Release

The `Build-Release` target will build the solution to a temporary directory, and
then compress it to a .tar.bz2 file in a `release` folder in the root of the
current solution.  This target uses `Restore-NuGet-Packages` as a dependency, so
`Restore-NuGet-Packages` (and its dependencies) will be run first.

#### Options

The `--Configuration` flag will build for different configurations, such as
`Debug`, `Release`, and `QA`.  These will affect what projects get built,
whether or not debugging symbols (.pdb files) are generated, and which
transforms are run on the main project's .config file.

The `--BuildDir` flag (with a valid relative directory path argument) will
override the default functionality of building to a temporary directory,
instead building/publishing to the specified path.

### Data-Import

The `Data-Import` target is used to download and restore development database
backups.

#### Options

The `--new-copy` flag, if set to `true`, will force a download of a new set of
files from the server.

### Migrate/Migrate-Up

The `--Migrate` target is used to run any migrations which have not yet been run
against a database.  `--Migrate` is a shorthand synonym for `--Migrate-Up`.
This target uses `Build` as a dependency, so `Build` (and its dependencies) will
be run first.

#### Options

The `--dbHost` flag (with a valid host/ip address argument) will override the
default functionality of running against `localhost`.

The `--database` flag (with a valid database name argument) will override the
default functionality of running against `MapCallDEV`.

The `--dbUser` flag will allow you to pass in a sql login user when AD
integration does not exist in the SQL Server.

The `--dbPassword` flag will allow you pass in the password for the sql login
user you passed in to the `--dbUser` arguement.

The `--previewOnly` flag (with a boolean argument) will output the actions that
would be performed without performing them.

To run migrations up, use `build -Target Migrate`.  To run migrations against
different hosts and databases, the `dbHost` and `database` flags can be used,
such as
`build --Target="Migrate" --dbHost="HSYNWMAPS002.amwaternp.net" --database="MapCallQA2"`.

### Migrate-Down

The `--Migrate-Down` target will run all migrations down against the specified
dbHost/database.  This target uses `Build` as a dependency, so `Build` (and its
dependencies) will be run first.

#### Options

The `--dbHost` flag (with a valid host/ip address argument) will override the
default functionality of running against `localhost`.

The `--database` flag (with a valid database name argument) will override the
default functionality of running against `MapCallDEV`.

The `--version` flag (with a valid migration version number argument) will
override the default functionality of running all migrations down, instead
reverting back to the specified migration/version number.

### Rollback

The `--Rollback` target will run down the last migration to be run up against
the specified dbHost/database.  This target uses `Build` as a dependency, so
`Build` (and its dependencies) will be run first.

#### Options

The `--dbHost` flag (with a valid host/ip address argument) will override the
default functionality of running against `localhost`.

The `--database` flag (with a valid database name argument) will override the
default functionality of running against `MapCallDEV`.

The `--steps` flag (with an integer argument) will rollback x number of steps.

### Data-Refresh

Convenient shortcut for running [Data-Import](#data-import) and then
[Migrate](#migratemigrate-up) (in that order).  Options for both apply.

### Decrypt

Decrypts web.config files in-place.  If the chosen `--configuration` is "Debug"
(or not specified, thus leaving "Debug" as the default), the project's
web.config file will be decrypted.  For other configurations, the corresponding
web.{configuration}.config file will be decrypted.

This will break if eiher the target config or the current web.config are already
unencrypted.

### Encrypt
Encrypts web.config files in-place.  If the chosen `--configuration` is "Debug"
(or not specified, thus leaving "Debug" as the default), the project's
web.config file will be encrypted.  For other configurations, the corresponding
web.{configuration}.config file will be encrypted.

This will break if the target config is already encrypted.

## CI/CD Setup

TeamCity is configured to use the cake scripts for the various build steps, so
troubleshooting TC builds comes down to simply running the corresponding cake
target locally.

| Team City Build Step | Cake Script Target                      |
|:---------------------|:----------------------------------------|
| Build                | `build --target="Sonar"`                |
| Run Unit Tests       | `build --target="Run-Unit-Tests"`       |
| Run Functional Tests | `build --target="Run-Functional-Tests"` |
