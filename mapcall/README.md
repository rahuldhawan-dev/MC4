# mapcall-mapcall

**Initial Setup**

web.config

Once you have the project cloned, you need to copy MapCall\MapCall\web.config.base to MapCall\MapCall\web.config. This is required to build. 

Development Database

Install the mapcall-data-thingy project. Follow the instructions in that project's readme to set it up if you have not already. Add a database named mapcalldev to your local sql server. 
Once it is installed, from the root of the MapCall project in command prompt, you will need to type: "dataimport" to import the database

Other

In Visual Studio, from the menubar, select Tools -> Options.  In the options window select Projects and Solutions -> Web Projects, and make sure "Use the 64 bit version of IIS Express for web sites and projects" is checked
