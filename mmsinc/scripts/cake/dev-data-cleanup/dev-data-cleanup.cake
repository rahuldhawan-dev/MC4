#tool nuget:?package=7-Zip.CommandLine&version=18.1.0
#addin nuget:?package=Cake.7zip&version=0.7.0
#addin nuget:?package=Cake.SqlServer&version=2.2.0
#addin nuget:?package=Cake.FileHelpers&version=3.3.0
#addin nuget:?package=Cake.ArgumentBinder&version=0.6.0

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using Cake.ArgumentBinder;
using System.String;

using static System.IO.Path;

public class CleanupSqlServiceConfig 
{
    [StringArgument("db-host", DefaultValue = "localhost", Description = "Database host to use")]
    public string DbHost { get; set; }
    [StringArgument("db-user", Description = "Database username, if not provided integrated security will be used")]
    public string DbUser { get; set; }
    [StringArgument("db-password", Description = "Database password, if not provided integrated security will be used")]
    public string DbPassword { get; set; }
    [StringArgument("db-name", DefaultValue = "MapCallDev", Description = "Name of the database to use")]
    public string Database { get; set; }
}

public class CleanupSqlService
{
    public const string STARTUP_COMMANDS = @"
        SET ANSI_DEFAULTS ON; 
        SET QUOTED_IDENTIFIER ON;
        SET CURSOR_CLOSE_ON_COMMIT OFF;
        SET IMPLICIT_TRANSACTIONS OFF;
        SET TEXTSIZE 2147483647;
        SET CONCAT_NULL_YIELDS_NULL ON;";
     public const string PERMISSIONS_CLEANUP = @"
        UPDATE tblPermissions set FullName = Replace(FullName, LastName, '');
        update tblPermissions set hasaccess = 1 where username in ('mcuser', 'mcdistro');
        UPDATE tblPermissions set FullName = LEFT(rtrim(ltrim(FullName)) + ' ' + rtrim(ltrim(LastName)), 25);
        update tblpermissions set hasaccess = 0;
        update tblPermissions set hasaccess = 1 where IsSiteAdministrator = 1;
        update tblPermissions set hasaccess = 1 where username in ('bowlbyjs', 'wigganme', 'ryanural', 'kahncr');
        update tblPermissions set hasaccess = 1 where username in ('mcuser', 'mcdistro');
        update tblPermissions set IsSiteAdministrator = 1 where username in ('santryt','smithv3','cannonj1','FRANKLSP','WALKERL3','KUMARN2','STEWARM', 'papines', 'CHHETRS', 'SANCHEA', 'DHAWANR', 'REDDYA5');
        update tblPermissions set hasaccess = 1 where username like 'NJ%test%';";
    public const int DEFAULT_COUNT_TO_KEEP = 1000;
    public const string FINAL_DEV_DB_NAME = "MapCallDev";
    private readonly ICakeContext _context;
    private readonly CleanupSqlServiceConfig _config;
    private readonly string _connectionString;

    public CleanupSqlService(ICakeContext context, CleanupSqlServiceConfig config)
    {
        _config = config;
        _context = context;
        _context.SetSqlCommandTimeout(57600); // 16 hours
        if(!string.IsNullOrWhiteSpace(_config.DbUser) && !string.IsNullOrWhiteSpace(_config.DbPassword)){
            _connectionString = "Data Source= " + _config.DbHost +";uid=" + _config.DbUser + ";password=" + _config.DbPassword + ";Initial Catalog=" + _config.Database + ";Integrated Security=false";
        }
        else{
            _connectionString = "Data Source= " + _config.DbHost + ";Initial Catalog=" + _config.Database + ";Integrated Security=true";
        } 
    }

    public void PostRestoreSteps()
    {
        SetStartUpCommands();
        AddMigrationsUserandRoles();
        CleanupPermissions();
        FlushEmails();
        RandomizePhoneNumbers();
        RandomizeFields(new String[]{"'FirstName'", "'First_Name'"});
        RandomizeFields(new String[]{"'Last_Name'", "'LastName'", "'FullName'"});
        DataClean();
        DropExistingAndRenameNewToExistingDatabase();
    }

    private void SetStartUpCommands()
    {
        _context.Information("Executing Startup Commands");
        _context.ExecuteSqlCommand(_connectionString, STARTUP_COMMANDS);
    }

    private void CleanupPermissions()
    {
        _context.Information("Cleaning up Permissions");
        _context.ExecuteSqlCommand(_connectionString, PERMISSIONS_CLEANUP); 
    }

    private void AddMigrationsUserandRoles()
    {
        _context.Information("Adding Migrations user and neccessary roles");
        _context.ExecuteSqlCommand(_connectionString, @"IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'migrations')
    CREATE USER [migrations] FOR LOGIN [migrations] WITH DEFAULT_SCHEMA=[dbo];");

    _context.ExecuteSqlCommand(_connectionString, @"ALTER ROLE [db_ddladmin] ADD MEMBER [migrations];
    ALTER ROLE [db_datareader] ADD MEMBER [migrations];
    ALTER ROLE [db_datawriter] ADD MEMBER [migrations];
    GRANT EXECUTE TO [migrations];");

    }

    private void DataClean()
    {
        _context.Information("Deleting Notification configs");
        _context.ExecuteSqlCommand(_connectionString, "DELETE FROM NotificationConfigurationsNotificationPurposes;");
        _context.ExecuteSqlCommand(_connectionString, "DELETE FROM NotificationConfigurations;");
        _context.Information("Updating SCWO FSR, CustomerName, ContactNumber");
        _context.Information("Fix MapIds for QA");
        _context.ExecuteSqlCommand(_connectionString, "update OperatingCenters set MapId ='0b39fbc4f5694257817b4b584e58c822', ArcMobileMapId = '6b964837e5bd4ffd9211d8013124b0ce';");
        _context.Information("Deleting COVID issues");
        _context.ExecuteSqlCommand(_connectionString, "DELETE FROM CovidIssues");
        _context.Information("Truncating cookies");
        _context.ExecuteSqlCommand(_connectionString, "TRUNCATE TABLE ASPXCookieTable;");
        _context.Information("Customer Cleanup");
        _context.ExecuteSqlCommand(_connectionString, @"UPDATE Customer SET CustName = 'Lester Freamon', CustContactName = 'Rhonda Pearlman', CustPhone = '7328675309', CustFax = null, CustContactEmail = null, CustLoginName = '';
            UPDATE CustomerSurveys SET CustomerName = 'William Bunk Moreland', Address = '123 Easy St.';
            UPDATE PositionGroups SET GroupCode = 'A' + cast(Id as varchar)
            UPDATE CriticalCustomers SET CustomerName = 'Baltimore Police Dept', ContactName = 'James McNulty', ContactPhoneNumber = '7328675309';
            UPDATE DriversLicenses SET LicenseNumber = 'A12341234501014', IssuedDate = '01/01/' + cast(Year(IssuedDate) as varchar), RenewalDate = '01/01/' + cast(Year(IssuedDate) + 5 as varchar);
            UPDATE tblEmployee SET EmergencyContactName = 'Thomas Houck', EmergencyContactPhone = '2018675309', EmailAddress = 'z@z.com',Drivers_License = null,[Address] = null,City = null,[State] = null,Phone_Cellular = null,Phone_Home = null,Phone_Work = null,Purchase_Card_Number = null;
            UPDATE MeterChangeOuts SET CustomerName = 'Shakima Greggs', ServicePhone = '7328675309', ServicePhone2 = '7328675309';
            UPDATE Services SET Name = 'William A. Rawls';
            UPDATE SampleSites SET CommonSiteName = 'Common Site Name';
            UPDATE WaterQualityComplaints SET CustomerName = 'Cedric Daniels', HomePhoneNumber = '7328675309';
			UPDATE Incidents SET QuestionWhatHappened = 'An incident occurred',MedicalProviderName = null,MedicalProviderPhone = null,SupervisorEmployeeId = null,IncidentSummary = 'An incident occurred',AnyImmediateCorrectiveActionsApplied = 'n/a',ICRResults = 'n./a',ClaimsCarrierId = null;
			DELETE FROM DocumentLink WHERE DataTypeID in (92, 173); -- fmla, hepp;
			UPDATE DriversLicenses SET LicenseNumber = 'A12341234512345';
			UPDATE GeneralLiabilityClaims SET PhoneNumber = null,Address = null, Email = null, DriverName = null, DriverPhone = null, OtherDriver = null, OtherDriverAddress = null,OtherDriverPhone = null, VehicleVIN = null, LicenseNumber = null;
            UPDATE WorkOrders SET CustomerName = 'Ellis Carver', PhoneNumber = '7328675309', SecondaryPhoneNumber = '9088675309'");
        CleanAllBut("ShortCycleCustomerMaterials");
        CleanAllBut("OneCallTickets");
        CleanAllBut("AuditLogEntries");
        CleanAllBut("UserViewed");
        CleanAllBut("AuthenticationLogs");
        // CleanAllButWithForeignKey("Premises", "Services", "PremiseId");
        CleanAllBut("TapImages", "TapImageId");
        CleanAllButWithForeignKey("OneCallMarkoutTickets", "OneCallMarkoutResponses", "OneCallMarkoutTicketId");
        CleanAllBut("CloudSeerAuthenticationLogs");
        CleanAllBut("ContractorsSecureFormDynamicValues");
        CleanAllBut("ContractorsSecureFormTokens");
        CleanAllBut("Readings", "ReadingId");

        _context.ExecuteSqlCommand(_connectionString, @"DBCC SHRINKFILE(2, 1, TRUNCATEONLY);");
        ShrinkWithEmptyFile();
    }

    // more shrinking, the quicker ones though
    private void PostShrink()
    {
        _context.Information("SHRINK THE LOG FILE AGAIN");
        _context.ExecuteSqlCommand(_connectionString, "DBCC SHRINKFILE(N'MCProd_Log' , 0, TRUNCATEONLY);");

        _context.Information("SHRINK THE ORIGINAL DB FILE NOW THAT IT'S EMPTY");
        _context.ExecuteSqlCommand(_connectionString, "DBCC SHRINKFILE(N'MCProd_Data' , 5);");

        _context.Information("SHRINK THE LOG FILE AGAIN");
        _context.ExecuteSqlCommand(_connectionString, "DBCC SHRINKFILE(N'MCProd_Log' , 0, TRUNCATEONLY);");
    }

    private void CleanAllBut(string tableName, string orderByColumn = "Id", int toKeep = DEFAULT_COUNT_TO_KEEP)
    {
        _context.Information($"Deleting all but top {tableName}");
        _context.ExecuteSqlCommand(_connectionString, 
        $@"SELECT TOP {toKeep} * INTO #{tableName} FROM {tableName} ORDER BY {orderByColumn} DESC;
        DROP TABLE {tableName};
        SELECT TOP {toKeep} * INTO {tableName} FROM #{tableName};
        DROP TABLE #{tableName};");
    }

    private void CleanAllButWithForeignKey(string tableName, string foreignTable, string foreignColumn, int toKeep = DEFAULT_COUNT_TO_KEEP)
    {
        _context.Information($"Deleting all but top {tableName} & {foreignTable}");
        _context.ExecuteSqlCommand(_connectionString, $@"DELETE FROM {foreignTable} WHERE {foreignColumn} NOT IN (SELECT TOP {toKeep} {foreignColumn} FROM {tableName} tmp WHERE {foreignTable}.{foreignColumn} = tmp.Id ORDER BY Id DESC);
        SELECT TOP {toKeep} * INTO #{tableName} FROM {tableName} ORDER BY Id DESC;
        SELECT TOP {toKeep} * INTO #{foreignTable} FROM {foreignTable} ORDER BY Id DESC;
        DROP TABLE {foreignTable};
        DROP TABLE {tableName};
        SELECT TOP {toKeep} * INTO {tableName} FROM #{tableName};
        SELECT TOP {toKeep} * INTO {foreignTable} FROM #{foreignTable};
        DROP TABLE #{foreignTable};
        DROP TABLE #{tableName};");
    }

    // we need to add a second file to the primary data file group
    // this will allow us to shrink the existing with empty file 
    // that puts all the data in this new file, then we will shrink the orignal file
    private void ShrinkWithEmptyFile()
    {
        _context.Information($"{DateTime.Now}: Adding secondary file \"Another{_config.Database}\" to primary data group");
        
        // the conditional statement here is neccessary because if the file already exists we cannot call it the same thing
        _context.ExecuteSqlCommand(_connectionString, $@"if exists (select 1 from {FINAL_DEV_DB_NAME}.sys.database_files where [name] = 'Another{_config.Database}')
    ALTER DATABASE [{_config.Database}]
        ADD FILE(
            NAME = N'Another{_config.Database}1',
        FILENAME = N'D:\rdsdbdata\DATA\Another{_config.Database}1.ndf',
        SIZE = 20000000KB , FILEGROWTH = 10 %)
        TO
        FILEGROUP[PRIMARY]
else
    ALTER DATABASE [{_config.Database}]
        ADD FILE(
            NAME = N'Another{_config.Database}',
        FILENAME = N'D:\rdsdbdata\DATA\Another{_config.Database}.ndf',
        SIZE = 20000000KB , FILEGROWTH = 10 %)
        TO
        FILEGROUP[PRIMARY]");

        _context.Information("SHRINK THE DATA FILE USING EMPTYFILE");
        
        // we need to catch here and not break on this error, always fails expectedly with:
        // Cannot move all contents of file "MCProd_Data" to other places to complete the emptyfile operation.
        try {
            _context.ExecuteSqlCommand(_connectionString, "DBCC SHRINKFILE(N'MCProd_Data' , EMPTYFILE);");
        }
        catch (SqlException ex) {
            _context.Information($"Caught SQL Exception {ex.Message}");
        }

        PostShrink();
    }

    private void ShrinkFile(int from, int to, int step)
    {
        _context.Information("Shrinking the data file");
        for(int i = from; i >= to; i = i - step)
        {
            var script = $"DBCC SHRINKFILE(1, {i});";
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                _context.Information($"step:{step}, i:{i}, {DateTime.Now}");
                connection.Open();
                SqlCommand command = new SqlCommand(script, connection);
                command.CommandTimeout = 4200;
                command.ExecuteNonQuery();
            }
        }

        // Once we are done here want to truncate again 
        _context.Information("Truncating data & log again");
        _context.ExecuteSqlCommand(_connectionString, "DBCC SHRINKFILE(2, 1, TRUNCATEONLY);");
    }

    private void FlushEmails()
    {
     _context.Information("Updating emails");   
     var script = "SELECT O.Name as TableName, C.Name as ColumnName FROM SysColumns C JOIN sysObjects o on o.Id = c.Id where C.Name = 'Email' AND O.Name <> 'ContractorUsers'";
     var updateScript = "";

       using (SqlConnection connection = new SqlConnection(_connectionString))
       {
           connection.Open();
           SqlCommand command = new SqlCommand(script, connection);
           command.CommandTimeout = 3600;
           SqlDataReader reader = command.ExecuteReader();
           while(reader.Read()){
               updateScript = updateScript + $"UPDATE {reader[0]} SET email = 'x@x.com'; ";
           }
       }

       _context.ExecuteSqlCommand(_connectionString, updateScript);
    }

    private void RandomizeFields(String[] columns)
    {
        var stringColumns = String.Join(",", columns);
        _context.Information("Randomize fields " + stringColumns);
        var script = $"SELECT c.Name as ColumnName, o.Name as TableName, c.Length FROM sysobjects o JOIN syscolumns c on c.Id = o.Id WHERE c.Name in ({stringColumns}) AND o.xtype = 'U'";
        var createTempTable = $"CREATE TABLE #tmpValues(name varchar(max));";
        var dropTempTable = "DROP TABLE #tmpValues;";
        var updateScript = "";
        var insertScript = "";
        var RandomizeTheThings = "";

        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(script, connection);
            command.CommandTimeout = 3600;
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read()){
                insertScript = insertScript + $"INSERT INTO #tmpValues SELECT distinct {reader[0]} FROM {reader[1]} WHERE isNull({reader[0]}, '') <> '';"; 
                updateScript = updateScript + $"UPDATE {reader[1]} SET {reader[0]} = (SELECT TOP 1 name from #tmpValues WHERE Len(name) <= {reader[2]} AND Len(name) = Len({reader[0]}) order by newID());";
            }
        }
        // Had to combine everything together because for some reason the temp table wasn't persisting
        RandomizeTheThings = createTempTable + insertScript + updateScript + dropTempTable;
        _context.ExecuteSqlCommand(_connectionString, RandomizeTheThings);
    }

     private void RandomizePhoneNumbers()
     {
         _context.Information("Randomize phone numbers");
         var script = "SELECT c.Name as ColumnName, o.Name as TableName, c.max_length as [length] FROM sys.objects o JOIN sys.columns c on c.object_id = o.object_id JOIN sys.types t on t.user_type_id = c.user_type_id WHERE o.type='U' AND (c.Name like '%phone%' or c.Name like '%cell%' or c.Name like '%mobile%') AND c.Name not like '%cancell%' AND c.max_length > 1 AND c.Name not like '%Id' AND t.system_type_id <> 62 order by 2";
         var updateScript = "";

         using(SqlConnection connection = new SqlConnection(_connectionString))
         {
             connection.Open();
             SqlCommand command = new SqlCommand(script, connection);
             command.CommandTimeout = 3600;
             SqlDataReader reader = command.ExecuteReader();

             while(reader.Read()){
                 updateScript = updateScript + "UPDATE [" + reader[1] + "] " + $" SET {reader[0]} = REPLACE(REPLACE(REPLACE(REPLACE( REPLACE( REPLACE( REPLACE( REPLACE( REPLACE( REPLACE(cast({reader[0]} as varchar(max)),'0', ABS(CHECKSUM(NEWID()) % 10)), '1', ABS(CHECKSUM(NEWID()) % 10)), '2', ABS(CHECKSUM(NEWID()) % 10)), '3', ABS(CHECKSUM(NEWID()) % 10)), '4', ABS(CHECKSUM(NEWID()) % 10)), '5', ABS(CHECKSUM(NEWID()) % 10)), '6', ABS(CHECKSUM(NEWID()) % 10)), '7', ABS(CHECKSUM(NEWID()) % 10)), '8', ABS(CHECKSUM(NEWID()) % 10)), '9', ABS(CHECKSUM(NEWID()) % 10))";
             }
         }
         _context.ExecuteSqlCommand(_connectionString, updateScript);
     }

    private void DropExistingAndRenameNewToExistingDatabase()
    {
        if(_config.DbHost.Contains("awapps.com"))
        {
            _context.Information("Renaming " + _config.Database);
            _context.ExecuteSqlCommand(_connectionString, $@"
                use master
                if exists(select 1 from sysdatabases where name = '{FINAL_DEV_DB_NAME}')
                begin
                    ALTER DATABASE [{FINAL_DEV_DB_NAME}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE [{FINAL_DEV_DB_NAME}]
                end
                ALTER DATABASE {_config.Database} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                exec rdsadmin.dbo.rds_modify_db_name N'{_config.Database}', N'{FINAL_DEV_DB_NAME}'
                ALTER DATABASE {FINAL_DEV_DB_NAME} SET MULTI_USER;");
        }
    }
}

Task("Dev-Data-Cleanup")
    .DescriptionFromArguments<CleanupSqlServiceConfig>($"Cleaning data for local developer environments")
    .Does((context) => {
        CleanupSqlServiceConfig config = CreateFromArguments<CleanupSqlServiceConfig>();
        context.Information("Using database name: " + config.Database);

        var cleanupSqlService = new CleanupSqlService(context, config);
        try
        {
            context.Information("Starting Dev Data Clean");
            cleanupSqlService.PostRestoreSteps();
        }
        catch(Exception e)
        {
            context.Information(e.ToString());
        }
    });
