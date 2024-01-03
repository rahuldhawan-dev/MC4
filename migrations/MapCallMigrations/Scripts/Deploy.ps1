if (!$ConnectionString) {
    $ConnectionString = '{ConnectionString}';
}

.\Migrate.exe --task migrate --connection "$ConnectionString" --provider "SqlServer2016" --assembly "MapCallMigrations.dll" --tag Production
