# MapCall Scheduler

Windows Service for running time-based tasks for MapCall such as daily notifications, dataimport from ftp/email, and more.

[![](http://hsyplsvns001:8111/app/rest/builds/buildType:(id:MapCallScheduler_BuildTest)/statusIcon)](http://hsyplsvns001:8111/viewType.html?buildTypeId=MapCallScheduler_BuildTest&guest=1)

## Developing

Scheduled jobs consist of a {prefix}Job.cs file within the MapCallScheduler.Core\Jobs\ folder. Within each job file there is a specified interval to run the job in the form of an attribute above the class name. 

Example attributes include: 
[Minutely(15), Immediate]
[Daily, StartAt(3, 30)]
[Daily]
[Hourly, Immediate]

### MakeChanges

Any job which processes files or emails and then deletes them when finished should accept a "makeChanges" boolean attribute in its configuration element in app.config, which should default to false.  When this attribute is set to false, files/emails/whatever will not be deleted after processing.

### Local Run

For local development/testing, set MapCallScheduler.Console as the startup project inv Visual Studio.

To run a single job locally, right-click the MapCallScheduler.Console project in Visual Studio, click Properties -> Debug, and enter "-n -j \<JobName>" into the "Command line arguments" box, where \<JobName> is the name of the job class without the word "Job" at the end.

### Local Run for Missed Production Emails

Restore the latest database locally. DataImport.bat from MVC or MapCall

Change the console\app.config AllEmailsGoTo value to your email address

Set the console Command Line Arguments to "-n -j MapCallNotifier"

You will need to use a .AddDays(-N) to each of these methods.

InterconnectionRepository - GetInterconnectionsThatHaveContractsExpiringIn30DaysThatHaveNotHadNotificationsSent

EnvironmentalPermitExpirationBase - GetDateOfConcern

EmployeeRepository - GetNow()

ArcFlashStudyExpiresIn1Year - GetData()

Run once to test the numbers that should be sent. If any were missed, after confirming you received the emails, remove the AllEmailsGoTo value from console\app.config and run again to send live.

Once done, undo all your local changes - git checkout .

You'll have to point at the Production DB directly to do this, and if it's behind because of a pending release, you'll have to look at the last released scheduler and checkout the local projects to the revision/hash that was released. E.g. http://hsyplsvns001.amwater.net:8111/viewLog.html?buildId=88415&tab=buildChangesDiv&buildTypeId=MapCallScheduler_Release

### Deployment - Staging

Cake is used for deployment. Staging will copy the files to our nonprod server - mapcallnp02.amwaternp.net. The command to do this is as follows and allows for a configuration - QA, QA2, QA3, Training. There is no load balancing
involved with this service.

build --target="Deploy" --configuration="Training"

This will build and copy the files to the nonprod server, stop the running service, delete, then install the 
service and start it. If anything goes wrong, pay close attention to the output to see which step may have 
failed. This process makes the assumption that the service is installed and running on the server.

The configuration file is not part of this solution, for each configuration there is a config file in the c:\web
folder on the mapcallnp02.amwater.net server. If you need to adjust the jobs running before deployment, change
the file there (c\web\MapCallScheduler.Service.QA3.exe.config), if you need to change it after, change it in the scheduler folder (e.g. c\web\scheduler-[qa | qa2 | qa3 | training]\MapCallScheduler.Service.exe.config) 
and then stop/restart the service.

### Deployment Troubleshooting

If you're running into issues with deployment, check the log at c\web\logs\scheduler-[qa | qa2 | qa3 | training]\mapcall-scheduler-log.txt and look at the end of the file for any errors

### New Environments / New Developer setup
Staging to QA depends on the Microsoft.TextTemplating.Targets file. When you install your build tools this will be an optional selection found in the installer under Individual Components/Code tools. You will not be able to stage the scheduler without this file installed.
