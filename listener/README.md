## Running/Installation

The MapCallActiveMQListener project functions as both a console application and a Windows service. It can be started for debugging in Visual Studio (use Set As Startup Project in Solution Explorer as necessary), called directly from the command line, or installed as a service.

## Deploying to Staging

Cake is used for deployment. Staging will copy the files to the servers defined in ServerNames the build.cake file. The command to do this is as follows and allows for a configuration - QA, QA2, QA3, Training. 

build --target="Deploy" --configuration="QA"

This will build and copy the files to the nonprod server, stop the running service, delete, then install the service and start it. If anything goes wrong, pay close attention to the output to see which step may have failed. This process makes the assumption that the service is installed and running on the server.

The configuration file is not part of this solution, for each configuration there is a config file in the c:\web folder on servers. If you need to make configuration adjustments before deployment, change the file on the servers, if you need to change it after, change it in the listener folder and then stop/restart the service. Changes made in the listener folder will be overwritten during the next deployment by the file in c:\web.