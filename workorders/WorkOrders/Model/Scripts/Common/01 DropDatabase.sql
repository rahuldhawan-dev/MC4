/*
 * THIS SCRIPT WILL DROP ALL CONSTRAINTS AND TABLES IN ANY DATABASE
 *
 * As such, it's pretty much been commented out and disabled in a number of ways.
 * If you actually need to run it, highlight between the outermost BEGIN and END.
 */

	DECLARE @curstmt nvarchar(200);

	DECLARE constraints CURSOR FOR
		SELECT
			'ALTER TABLE [' + object_name([parent_obj]) + '] DROP CONSTRAINT [' + [name] + '];' + CHAR(13) + CHAR(10)
		FROM
			[sysobjects]
		WHERE
			[xtype] = 'F';

	OPEN constraints;

	FETCH NEXT FROM constraints
	INTO @curstmt;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		exec sp_executesql @curstmt;
		--print @curstmt;

		FETCH NEXT FROM constraints
		INTO @curstmt;
	END

	CLOSE constraints;
	DEALLOCATE constraints;

	DECLARE tables CURSOR FOR
		SELECT 'DROP TABLE [' + [name] + '];' + CHAR(13) + CHAR(10) FROM [sysobjects] WHERE [xtype] = 'u' ORDER BY [crdate] DESC;

	OPEN tables;

	FETCH NEXT FROM tables
	INTO @curstmt;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		exec sp_executesql @curstmt;
		--print @curstmt;

		FETCH NEXT FROM tables
		INTO @curstmt;
	END

	CLOSE tables;
	DEALLOCATE tables;
