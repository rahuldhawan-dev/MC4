# mapcall-monorepo

Mono-repository for all of MapCall and its related/supporting projects.

## Working with this project

1. Clone the project to your solutions directory (`c:\solutions`)
2. Try a build to ensure things are working:
   `c:\solutions\mapcall-monorepo\mvc>build --target=Build`

## Cake Scripts

MapCall projects employ [cake](https://cakebuild.net/) scripts for various build
and maintenance tasks. See [here](doc/cake.md) for more information on
MapCall's cake scripts.

## Switching over from individual projects

Moved [here](doc/switching-from-individual-projects.md).

## Running SonarQube Scans Against Everything

Should you find yourself in a position to need to sonar scan everything against a particular branch, say `develop`; first clone this repository to `c:\solutions\mapcall-monorepo`, next get the api token for Sonar from someone who has it (perhaps try Jason, Alex, or Aarti; in their absence DevSecOps team should be able to furnish it), and finally run the following command from the command-line replacing all instances of `<the key>` with the aforementioned token: 

```cmd
c:\solutions\mapcall-monorepo>cd mmsinc && build --target=Sonar --SonarLogin=<the key> && cd ..\mapcall && build --target=Sonar --SonarLogin=<the key> && cd ..\api\ && build --target=Sonar --SonarLogin=<the key> && cd ..\workorders\ && build --target=Sonar --SonarLogin=<the key> && cd ..\contractors\ && build --target=Sonar --SonarLogin=<the key> && cd ..\mvc && build --target=Sonar --SonarLogin=<the key> && cd ..\listener && build --target=Sonar --SonarLogin=<the key> && cd ..\importer && build --target=Sonar --SonarLogin=<the key> && cd ..\scheduler && build --target=Sonar --SonarLogin=<the key>
```
