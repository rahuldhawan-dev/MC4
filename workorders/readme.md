# Work Orders

## Running the site locally

- Set the start up project to `LINQTo271`
- In `LINQTo271` project properties, set the start url to: `http://localhost:4932/Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceView.aspx`
- Copy web.config.base over web.config
- Update the `sites` node in the `.\.vs\LINQTo271\config\applicationhost.config` file with this:

```xml
<sites>
    <site name="WebSite1" id="1" serverAutoStart="true">
        <application path="/">
            <virtualDirectory path="/" physicalPath="%IIS_SITES_HOME%\WebSite1" />
        </application>
        <bindings>
            <binding protocol="http" bindingInformation=":8080:localhost" />
        </bindings>
    </site>
    <site name="LINQTo271-Site" id="2">
        <application path="/" applicationPool="Clr4ClassicAppPool">
        <virtualDirectory path="/" physicalPath="C:\Solutions\mapcall-monorepo\mapcall\MapCall" />
        <virtualDirectory path="/modules" physicalPath="C:\Solutions\mapcall-monorepo\workorders" />
        </application>
        <application path="/modules/WorkOrders" applicationPool="Clr4ClassicAppPool">
        <virtualDirectory path="/" physicalPath="C:\Solutions\mapcall-monorepo\workorders\LINQTo271" />
        </application>
        <bindings>
            <binding protocol="http" bindingInformation="*:4932:localhost" />
        </bindings>
    </site>
    <siteDefaults>
        <logFile logFormat="W3C" directory="%IIS_USER_HOME%\Logs" />
        <traceFailedRequestsLogging directory="%IIS_USER_HOME%\TraceLogFiles" enabled="true" maxLogFileSizeKB="1024" />
    </siteDefaults>
    <applicationDefaults applicationPool="Clr4IntegratedAppPool" />
    <virtualDirectoryDefaults allowSubDirConfig="true" />
</sites>
```

- Run the following query in your local MapCall database for your account:

```sql
declare @email varchar(50) = '[someone@amwater.com]';

update tblPermissions
   set IsAdministrator = 1
     , IsUserAdministrator = 1
     , IsProductionAdministrator = 1
     , HasAccess = 1
     , IsSiteAdministrator = 1
 where email = @email
```

## Common Errors / Resolutions

Error: Access Denied - Logon Information was not found.

Resolution:

- Close IISExpress from system tray
- Open MapCall project, run project
- Run 271 project again after that
