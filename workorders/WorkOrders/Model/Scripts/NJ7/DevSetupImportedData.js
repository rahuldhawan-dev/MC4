/*
 * DevSetupImportedData.js by Jason Duncan
 * 2009-04-02
 *
 * Merges all of the necessary sql scripts for dropping and restoring the tables
 * in the WorkOrdersTest database down to one, specifically for use with their
 * imported access data.  Rather than running the script to create the sample
 * data, an SSIS package gets run to import WorkOrders, Markouts, and
 * Restorations from [WorkOrdersMerged] to [WorkOrdersTest].
 */

var OUTPUT_FILE = 'DevSetupImportedData.sql';

var SSIS_PACKAGE = 'ImportDataForDevelopment.dtsx';

var SCRIPT_FILES = {
  DROP: '01 DropDatabase.sql',
  EXISTING_TABLES: '02 CreateExistingTables.sql',
  SITE_DATA: '03 ImportSiteData.sql',
  CREATE: '04 CreateDatabase.sql',
  STATIC_DATA: '05 CreateStaticData.sql'
};

// used to specify the order in which the script files should be read and
// appended to the output script:
var SCRIPT_ARR = [
  SCRIPT_FILES.DROP,
  SCRIPT_FILES.EXISTING_TABLES,
  SCRIPT_FILES.SITE_DATA,
  SCRIPT_FILES.CREATE,
  SCRIPT_FILES.STATIC_DATA
];

var MISC_CLEANUP = '000 MISC.sql';

var DATABASE_NAME = 'WorkOrdersTest';

var FSO = new ActiveXObject('Scripting.FileSystemObject');
var SHELL = new ActiveXObject('WScript.Shell');

// delete the output file, if it already exists:
if (FSO.FileExists(OUTPUT_FILE))
  FSO.DeleteFile(OUTPUT_FILE);

// create the file and append the use statement:
var outFile = FSO.CreateTextFile(OUTPUT_FILE);

outFile.WriteLine('USE [' + DATABASE_NAME + ']');
outFile.WriteLine('GO');

// open each input file, and write them to the output file:
var inFile;

for (var i = 0, len = SCRIPT_ARR.length; i < len; ++i) {
  var file = SCRIPT_ARR[i];

  // 1 for read-only
  inFile = FSO.OpenTextFile(file, 1);

  outFile.WriteLine('RAISERROR (N\'SCRIPT FILE ' + file + ' PROCESSING...\', 10, 1) WITH NOWAIT;');
  outFile.WriteLine(inFile.ReadAll());
}

WScript.Echo('Finished concatenating SQL script files.  Now processing script "' + OUTPUT_FILE + '".');

SHELL.Run('osql -E -i ' + OUTPUT_FILE, 10, true);

WScript.Echo('Finished processing sql script.  Now running SSIS package to import data from WorkOrdersMerged to WorkOrdersTest.');

SHELL.Run('dtexec /f ' + SSIS_PACKAGE, 10, true);

WScript.Echo('Running miscellaneous cleanup on imported data.');

outFile.Close();

FSO.DeleteFile(OUTPUT_FILE);

outFile = FSO.CreateTextFile(OUTPUT_FILE);

inFile = FSO.OpenTextFile(MISC_CLEANUP, 1);

outFile.WriteLine('USE [' + DATABASE_NAME + ']');
outFile.WriteLine('GO');
outFile.WriteLine('RAISERROR (N\'SCRIPT FILE ' + MISC_CLEANUP + ' PROCESSING...\', 10, 1) WITH NOWAIT;');
outFile.WriteLine(inFile.ReadAll());

SHELL.Run('osql -E -i ' + OUTPUT_FILE, 10, true);

WScript.Echo('Finished importing data.');
