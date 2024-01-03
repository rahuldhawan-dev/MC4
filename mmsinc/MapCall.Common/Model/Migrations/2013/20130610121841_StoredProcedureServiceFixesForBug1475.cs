using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130610121841), Tags("Production")]
    public class StoredProcedureServiceFixesForBug1475 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string UPDATE_STORED_PROCEDURES =
                @"

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetPendingSvcsByOpCntr]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [spGetPendingSvcsByOpCntr]
	@OpCntr varchar(3),
	@SortOrder varchar(4),
	@DevServD varchar(3)
AS

	IF (@DevServD = '''')
		SET @DevServD = ''NO''

	IF (UPPER(@SortOrder) = ''TOWN'')
		
		SELECT CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SC.Description as CatofService, SV.PurpInstal, SV.SizeofService
		
		FROM  tblNJAWService SV, Streets ST, Towns T, ServiceCategories SC
		WHERE SV.OpCntr = @OpCntr
		AND SV.Town = T.TownID
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SC.ServiceCategoryID = SV.CatOfService
		AND SC.Description <> ''Stub Service''
		AND SC.Description NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY T.Town, ST.FullStName, SV.StNum, SV.ServNum

	ELSE If (UPPER(@SortOrder) = ''DATE'')

		SELECT  CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SC.Description as CatofService, SV.PurpInstal, SV.SizeofService
		
		FROM  tblNJAWService SV, Streets ST, Towns T, ServiceCategories SC
		WHERE SV.OpCntr = @OpCntr
		AND SV.Town = T.TownID
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
        AND SC.ServiceCategoryID = SV.CatOfService		
        AND SC.Description <> ''Stub Service''
		AND SC.Description NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY SV.ContactDate, T.Town, ST.FullStName, SV.StNum, SV.ServNum

	ELSE

		SELECT CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SV.CatofService, SV.PurpInstal, SV.SizeofService
		
		FROM  tblNJAWService SV, Streets ST, Towns T
		WHERE SV.OpCntr = @OpCntr
		AND SV.Town = T.TownID
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SV.CatofService <> ''Stub Service''
		AND SV.CatofService NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY SV.ServNum, T.Town, ST.FullStName, SV.StNum
' 
END;
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetPendingSvcsByTownSort]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*	Returns details on Pending Services
	based on the Town adn OpCntr provided.	*/

ALTER PROCEDURE [spGetPendingSvcsByTownSort]
	@OpCntr varchar(3),
	@Town int,
	@SortOrder varchar(4),
	@DevServD varchar(3)
AS

	IF (@DevServD = '''')
		SET @DevServD = ''NO''

	IF (UPPER(@SortOrder) = ''DATE'')

		SELECT CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SC.Description as CatofService, SV.PurpInstal, SV.SizeofService
	
		FROM  tblNJAWService SV, Streets ST, Towns T, ServiceCategories SC
		WHERE SV.Town = @Town 
		AND T.TownID = SV.Town
		AND SV.OpCntr = @OpCntr
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SC.ServiceCategoryID = SV.CatOfService
		AND SC.Description <> ''Stub Service''
		AND SC.Description NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY SV.ContactDate, T.Town, SV.OpCntr, ST.FullStName, SV.StNum, SV.ServNum

	ELSE

		SELECT CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SC.Description as CatofService, SV.PurpInstal, SV.SizeofService
	
		FROM  tblNJAWService SV, Streets ST, Towns T, ServiceCategories SC
		WHERE SV.Town = @Town
		AND T.TownID = SV.Town
		AND SV.OpCntr = @OpCntr
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SC.ServiceCategoryID = SV.CatOfService
		AND SV.CatofService <> ''Stub Service''
		AND SV.CatofService NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY SV.ServNum, SV.OpCntr, T.Town, ST.FullStName, SV.StNum
' 
END;
";

            public const string ROLLBACK_STORED_PROCEDURES =
                @"

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetPendingSvcsByOpCntr]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [spGetPendingSvcsByOpCntr]
	@OpCntr varchar(3),
	@SortOrder varchar(4),
	@DevServD varchar(3)
AS

	IF (@DevServD = '''')
		SET @DevServD = ''NO''

	IF (UPPER(@SortOrder) = ''TOWN'')
		
		SELECT CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SV.CatofService, SV.PurpInstal, SV.SizeofService
		
		FROM  tblNJAWService SV, Streets ST, Towns T
		WHERE SV.OpCntr = @OpCntr
		AND SV.Town = T.TownID
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SV.CatofService <> ''Stub Service''
		AND SV.CatofService NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY T.Town, ST.FullStName, SV.StNum, SV.ServNum

	ELSE If (UPPER(@SortOrder) = ''DATE'')

		SELECT  CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SV.CatofService, SV.PurpInstal, SV.SizeofService
		
		FROM  tblNJAWService SV, Streets ST, Towns T
		WHERE SV.OpCntr = @OpCntr
		AND SV.Town = T.TownID
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SV.CatofService <> ''Stub Service''
		AND SV.CatofService NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY SV.ContactDate, T.Town, ST.FullStName, SV.StNum, SV.ServNum

	ELSE

		SELECT CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SV.CatofService, SV.PurpInstal, SV.SizeofService
		
		FROM  tblNJAWService SV, Streets ST, Towns T
		WHERE SV.OpCntr = @OpCntr
		AND SV.Town = T.TownID
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SV.CatofService <> ''Stub Service''
		AND SV.CatofService NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY SV.ServNum, T.Town, ST.FullStName, SV.StNum
' 
END;
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetPendingSvcsByTownSort]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*	Returns details on Pending Services
	based on the Town adn OpCntr provided.	*/

ALTER PROCEDURE [spGetPendingSvcsByTownSort]
	@OpCntr varchar(3),
	@Town int,
	@SortOrder varchar(4),
	@DevServD varchar(3)
AS

	IF (@DevServD = '''')
		SET @DevServD = ''NO''

	IF (UPPER(@SortOrder) = ''DATE'')

		SELECT CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SV.CatofService, SV.PurpInstal, SV.SizeofService
	
		FROM  tblNJAWService SV, Streets ST, Towns T
		WHERE SV.Town = @Town 
		AND T.TownID = SV.Town
		AND SV.OpCntr = @OpCntr
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SV.CatofService <> ''Stub Service''
		AND SV.CatofService NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY SV.ContactDate, T.Town, SV.OpCntr, ST.FullStName, SV.StNum, SV.ServNum

	ELSE

		SELECT CAST(SV.ServNum AS int) AS ServNum, SV.RecID,
		SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
		T.Town, SV.ContactDate,  SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
		SV.PermitRcvdDate, SV.DateIssuedtoField, SV.DateInstalled,
		SV.WorkIssuedto, SV.CatofService, SV.PurpInstal, SV.SizeofService
	
		FROM  tblNJAWService SV, Streets ST, Towns T
		WHERE SV.Town = @Town
		AND T.TownID = SV.Town
		AND SV.OpCntr = @OpCntr
		AND SV.InActSrv <> ''ON''
		AND SV.DevServD = @DevServD
		AND SV.DateInstalled = ''1/1/1900''
		AND SV.CatofService <> ''Stub Service''
		AND SV.CatofService NOT LIKE ''%Measurement Only%''
		AND SV.StName = ST.StreetID
		ORDER BY SV.ServNum, SV.OpCntr, T.Town, ST.FullStName, SV.StNum
' 
END;";
        }

        #endregion

        public override void Up()
        {
            Execute.Sql(Sql.UPDATE_STORED_PROCEDURES);
        }

        public override void Down()
        {
            Execute.Sql(Sql.ROLLBACK_STORED_PROCEDURES);
        }
    }
}
