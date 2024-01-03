/* 
	If you like Greg you will read this:

	I can't make a stored procedure because RDS hates me so this is the best I can do.
	the steps are 

	1. Drop your database
	2. Restore your back up with correct DB name and pointed to correct path and file in S3 bucket
	3. Monitor your progress

	On load test of latest prod backup 6/17/20 it took about 15 minutes to restore our prod backup

	You can monitor your progress with the last stored procedure below

	Example:


		exec msdb.dbo.rds_drop_database  N'MapCallQA'

		exec msdb.dbo.rds_restore_database 
			@restore_db_name='MapCallQA', 
			@s3_arn_to_restore_from='arn:aws:s3:::mapcall-va-np-bucket//DataSync/Unprocessed/MCProd_0617123alotofnumbers.bak';

		exec msdb.dbo.rds_task_status @db_name='MapCallQA'

		*** In case you get an error about disk space: 

		Apparently it takes a few minutes for RDS to reclaim disk space, after dropping a database. Ran into this issue on cut over day.

		The SQL Server has 800 gb disk and it said I didn't have enough disk space for the restore at first, if this happens
		drop the DB first, wait like 5 minutes and try again. Good ol Amazon


*/



exec msdb.dbo.rds_drop_database  N'YOURDATABASEHERE' -- This will close any active connections and drop 


exec msdb.dbo.rds_restore_database 
	@restore_db_name='YOURDATABASEHERE', 
	@s3_arn_to_restore_from='arn:aws:s3:::mapcall-va-np-bucket/DataSync/Unprocessed/YOURBACKUPNAMEHERE.bak';



exec msdb.dbo.rds_task_status @db_name='YOURDATABASEHERE'
  