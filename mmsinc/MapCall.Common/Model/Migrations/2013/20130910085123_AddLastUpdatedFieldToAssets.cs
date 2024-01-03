using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130910085123), Tags("Production")]
    public class AddLastUpdatedFieldToAssets : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string HYDRANTS = "tblNJAWHydrant",
                                VALVES = "tblNJAWValves",
                                SERVICES = "tblNJAWService";
        }

        public struct Columns
        {
            public const string LAST_UPDATED = "LastUpdated";
        }

        public struct UpdateStoredProcedures
        {
            public const string UPDATE_HYDRANT =
                @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_UpDateHydrant]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  StoredProcedure [dbo].[sp_UpDateHydrant]    Script Date: 08/22/2011 13:42:35 ******/
 ALTER PROCEDURE [sp_UpDateHydrant] 
  @ActRet varchar(10),
  @BillInfo varchar(15),
  @BPUKPI varchar(2),
  @BranchLnFt int,
  @BranchLnIn int,
  @Critical varchar(2),
  @CriticalNotes varchar(150),
  @CrossStreet varchar(30),
  @DateInst varchar(10),
  @DateRemoved varchar(10),
  @DEM varchar(3),
  @DepthBuryFt int,
  @DepthBuryIn int,
  @DirOpen varchar(7),
  @Elevation varchar(10),
  @FireD varchar(1),
  @Gradiant varchar(25),
  @ManufacturerID int, 
  @HydrantModelID int, 
  @HydNum varchar(12),
  @HydSize varchar(5),
  @HydSuf int,
  @InspFreq varchar(10),
  @InspFreqUnit varchar(50),
  @Lat varchar(20),
  @LatSize varchar(10),
  @LatValNum varchar(10),
  @Location varchar(150),
  @Lon varchar(20),
  @MapPage varchar(15),
  @OutOfServ varchar(2),
  @RecID int,
  @Remarks varchar(2000),
  @Route varchar(9),
  @SizeOfMain varchar(10),
  @StNum varchar(10),
  @StName varchar(7),
  @Thread varchar(15),
  @TwnSection varchar(30),
  @TypeMain varchar(15),
  @ValLoc varchar(30),
  @WONum varchar(18), 
  @FireDistrictID int, 
  @YearManufactured int,
  @ObjectID int, 
  @HydrantTagStatusID int,
  @PremiseNumber varchar(9),
  @BillingDate varchar(10)
 AS
 
 -- get current abbreviation
 declare @abbreviation varchar(10)
 select @abbreviation = substring(@hydNum, 2, charindex(''-'', @hydNum)-2) 
 
 declare @townID int
 declare @town varchar(50)
 declare @county varchar(50)
 
 -- need these because the database joins on string fields =/
 select @townID = (select town from tblNJAWHydrant where recID = @recID)
 select @town = (select town from Towns where TownID = @townID)
 select @county = (select county from Towns where TownID = @townID)
 
 -- If the Abbreviation is not valid, throw an exception.
 IF (NOT @abbreviation in (
        -- Get Valid Abbreviation Codes for the record''s town
        select ab from Towns where TownID = @townID -- Neptune
        union all
        select distinct abbreviation from TownSections where townID = @townID and isNull(abbreviation, '''') <> ''''
        union all
        select abbreviation as ab from FireDistrict fd inner join FireDistrictsTowns mfd on fd.FireDistrictID = mfd.FireDistrictID where TownID = @townID
        ))
  BEGIN
   SELECT isNull(@abbreviation,'''')
   RAISERROR (N''ERROR: UNABLE TO SAVE THE HYDRANT WITH AN INVALID ABBREVIATION'', 10, 1)
  END
 ELSE  -- Else - Update It
 BEGIN
  Update 
   tblNJAWHydrant 
  Set
   ActRet = @ActRet,
   BillInfo = @BillInfo,
   BPUKPI = @BPUKPI,
   BranchLnFt = @BranchLnFt,
   BranchLnIn = @BranchLnIn,
   Critical = @Critical,
   CriticalNotes = @CriticalNotes,
   CrossStreet = @CrossStreet,
   DateInst = @DateInst,
   DateRemoved = @DateRemoved,
   DEM = @DEM,
   DepthBuryFt = @DepthBuryFt,
   DepthBuryIn = @DepthBuryIn,
   DirOpen = @DirOpen,
   Elevation = @Elevation,
   FireD=@FireD,
   Gradiant = @Gradiant,
   ManufacturerID = @ManufacturerID,
   HydrantModelID = @HydrantModelID,
   HydNum = @HydNum,
   HydSize = @HydSize,
   HydSuf = @HydSuf,
   InspFreq = @InspFreq,
   InspFreqUnit = @InspFreqUnit,
   Lat = @Lat,
   LatSize = @LatSize,
   LatValNum = @LatValNum,
   Location = @Location,
   Lon = @Lon,
   MapPage = @MapPage,
   OutOfServ = @OutOfServ,
   Remarks = @Remarks,
   Route = @Route,
   SizeOfMain = @SizeOfMain,
   StNum = @StNum,
   StName = @StName,
   Thread = @Thread,
   TwnSection = @TwnSection,
   TypeMain = @TypeMain,
   ValLoc = @ValLoc,
   WONum = @WONum, 
   FireDistrictID = @FireDistrictID,
   YearManufactured = @YearManufactured, 
   ObjectID = @ObjectID, 
   HydrantTagStatusID = @HydrantTagStatusID, 
   PremiseNumber = @PremiseNumber,
   BillingDate = case when @BillingDate = '''' then null else @BillingDate end,
   LastUpdated = GetDate() 
  WHERE 
   RecID = @RecID
  SELECT 1
 END' 
END";

            public const string UPDATE_VALVE =
                @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_UpDateValve]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/* Fix up the db for these tables */
/****** Object:  StoredProcedure [dbo].[sp_UpDateValve]    Script Date: 05/31/2011 11:11:05 ******/
ALTER PROCEDURE [sp_UpDateValve] 
	@Critical varchar(2),
	@CriticalNotes varchar(150),
	@CrossStreet varchar(30),
	@DateInst varchar(10),
	@DateRetired varchar(10),
	@InspFreq varchar(10),
	@InspFreqUnit varchar(50),
	@Lat varchar(15),
	@Lon varchar(15),
	@MapPage varchar(6),
	@NorPos varchar(25),
	@Opens varchar(6),
	@RecID int,
	@Remarks varchar(2000),
	@Route varchar(9),
	@SketchNum varchar(15),
	@StNum varchar(10),
	@StName varchar(7),
	@Traffic varchar(2),
	@Turns varchar(6),
	@TwnSection varchar(30),
	@TypeMain varchar(15),
	@ValCtrl varchar(25),
	@ValLoc varchar(150),
	@ValMake varchar(30),
	@ValNum varchar(15),
	@ValSuf int,
	@ValType varchar(25),
	@ValveSize varchar(10),
	@ValveStatus varchar(10),
	@WONum varchar(18),
	@BillInfo varchar(16),
	@BPUKPI varchar(2), 
	@ValveZone int,
	@ObjectID int
	
AS
DECLARE @abbreviation varchar(10)
SELECT @abbreviation = substring(@valNum, 2, charindex(''-'', @valNum)-2) 

DECLARE @townID int
DECLARE @town varchar(50)
DECLARE @county varchar(50)

-- need these because the database joins on string fields =/
SELECT @townID = (SELECT town FROM tblNJAWValves WHERE recID = @recID)
SELECT @town = (SELECT town FROM Towns WHERE TownID = @townID)
SELECT @county = (SELECT county FROM Towns WHERE TownID = @townID)

-- If the Abbreviation is not valid, throw an exception.
IF (NOT @abbreviation in (
							-- Get Valid Abbreviation Codes for the record''s town
							select ab from Towns where TownID = @townID -- Neptune
							union all
							select distinct Abbreviation from TownSections where townID = @townID and isNull(abbreviation, '''') <> ''''
						 ))
	BEGIN
		SELECT isNull(@abbreviation,'''')
		RAISERROR (N''ERROR: UNABLE TO SAVE THE VALVE WITH AN INVALID ABBREVIATION'', 10, 1)
	END
ELSE  -- Else - Update It
BEGIN
	UPDATE 
		tblNJAWValves 
	SET
		Critical = @Critical,
		CriticalNotes = @CriticalNotes,
		CrossStreet = @CrossStreet,
		DateInst = @DateInst,
		DateRetired = @DateRetired,
		InspFreq = @InspFreq,
		InspFreqUnit = @InspFreqUnit,
		Lat = @Lat,
		Lon = @Lon,
		MapPage = @MapPage,
		NorPos = @NorPos,
		Opens = @Opens,
		Remarks = @Remarks,
		Route = @Route,
		SketchNum = @SketchNum,
		StNum = @StNum,
		StName = @StName,
		Traffic = @Traffic,
		Turns = @Turns,
		TwnSection = @TwnSection,
		TypeMain = @TypeMain,
		ValCtrl = @ValCtrl,
		ValLoc = @ValLoc,
		ValMake = @ValMake,
		ValNum = @ValNum,
		ValSuf = @ValSuf,
		ValType = @ValType,
		ValveSize = @ValveSize,
		ValveStatus = @ValveStatus,
		WONum = @WONum,
		BillInfo = @BillInfo,
		BPUKPI = @BPUKPI, 
		ValveZone = @ValveZone,
		ObjectID = @ObjectID,
		LastUpdated = getDate()
	WHERE 
		RecID = @RecID
	SELECT 1
END' 
END
";
        }

        public struct RestoreStoredProcedures
        {
            public const string UPDATE_HYDRANT =
                @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_UpDateHydrant]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  StoredProcedure [dbo].[sp_UpDateHydrant]    Script Date: 08/22/2011 13:42:35 ******/
 ALTER PROCEDURE [sp_UpDateHydrant] 
  @ActRet varchar(10),
  @BillInfo varchar(15),
  @BPUKPI varchar(2),
  @BranchLnFt int,
  @BranchLnIn int,
  @Critical varchar(2),
  @CriticalNotes varchar(150),
  @CrossStreet varchar(30),
  @DateInst varchar(10),
  @DateRemoved varchar(10),
  @DEM varchar(3),
  @DepthBuryFt int,
  @DepthBuryIn int,
  @DirOpen varchar(7),
  @Elevation varchar(10),
  @FireD varchar(1),
  @Gradiant varchar(25),
  @ManufacturerID int, 
  @HydrantModelID int, 
  @HydNum varchar(12),
  @HydSize varchar(5),
  @HydSuf int,
  @InspFreq varchar(10),
  @InspFreqUnit varchar(50),
  @Lat varchar(20),
  @LatSize varchar(10),
  @LatValNum varchar(10),
  @Location varchar(150),
  @Lon varchar(20),
  @MapPage varchar(15),
  @OutOfServ varchar(2),
  @RecID int,
  @Remarks varchar(2000),
  @Route varchar(9),
  @SizeOfMain varchar(10),
  @StNum varchar(10),
  @StName varchar(7),
  @Thread varchar(15),
  @TwnSection varchar(30),
  @TypeMain varchar(15),
  @ValLoc varchar(30),
  @WONum varchar(18), 
  @FireDistrictID int, 
  @YearManufactured int,
  @ObjectID int, 
  @HydrantTagStatusID int,
  @PremiseNumber varchar(9),
  @BillingDate varchar(10)
 AS
 
 -- get current abbreviation
 declare @abbreviation varchar(10)
 select @abbreviation = substring(@hydNum, 2, charindex(''-'', @hydNum)-2) 
 
 declare @townID int
 declare @town varchar(50)
 declare @county varchar(50)
 
 -- need these because the database joins on string fields =/
 select @townID = (select town from tblNJAWHydrant where recID = @recID)
 select @town = (select town from Towns where TownID = @townID)
 select @county = (select county from Towns where TownID = @townID)
 
 -- If the Abbreviation is not valid, throw an exception.
 IF (NOT @abbreviation in (
        -- Get Valid Abbreviation Codes for the record''s town
        select ab from Towns where TownID = @townID -- Neptune
        union all
        select distinct abbreviation from TownSections where townID = @townID and isNull(abbreviation, '''') <> ''''
        union all
        select abbreviation as ab from FireDistrict fd inner join FireDistrictsTowns mfd on fd.FireDistrictID = mfd.FireDistrictID where TownID = @townID
        ))
  BEGIN
   SELECT isNull(@abbreviation,'''')
   RAISERROR (N''ERROR: UNABLE TO SAVE THE HYDRANT WITH AN INVALID ABBREVIATION'', 10, 1)
  END
 ELSE  -- Else - Update It
 BEGIN
  Update 
   tblNJAWHydrant 
  Set
   ActRet = @ActRet,
   BillInfo = @BillInfo,
   BPUKPI = @BPUKPI,
   BranchLnFt = @BranchLnFt,
   BranchLnIn = @BranchLnIn,
   Critical = @Critical,
   CriticalNotes = @CriticalNotes,
   CrossStreet = @CrossStreet,
   DateInst = @DateInst,
   DateRemoved = @DateRemoved,
   DEM = @DEM,
   DepthBuryFt = @DepthBuryFt,
   DepthBuryIn = @DepthBuryIn,
   DirOpen = @DirOpen,
   Elevation = @Elevation,
   FireD=@FireD,
   Gradiant = @Gradiant,
   ManufacturerID = @ManufacturerID,
   HydrantModelID = @HydrantModelID,
   HydNum = @HydNum,
   HydSize = @HydSize,
   HydSuf = @HydSuf,
   InspFreq = @InspFreq,
   InspFreqUnit = @InspFreqUnit,
   Lat = @Lat,
   LatSize = @LatSize,
   LatValNum = @LatValNum,
   Location = @Location,
   Lon = @Lon,
   MapPage = @MapPage,
   OutOfServ = @OutOfServ,
   Remarks = @Remarks,
   Route = @Route,
   SizeOfMain = @SizeOfMain,
   StNum = @StNum,
   StName = @StName,
   Thread = @Thread,
   TwnSection = @TwnSection,
   TypeMain = @TypeMain,
   ValLoc = @ValLoc,
   WONum = @WONum, 
   FireDistrictID = @FireDistrictID,
   YearManufactured = @YearManufactured, 
   ObjectID = @ObjectID, 
   HydrantTagStatusID = @HydrantTagStatusID, 
   PremiseNumber = @PremiseNumber,
   BillingDate = case when @BillingDate = '''' then null else @BillingDate end
  WHERE 
   RecID = @RecID
  SELECT 1
 END' 
END";

            public const string UPDATE_VALVE =
                @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_UpDateValve]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/* Fix up the db for these tables */
/****** Object:  StoredProcedure [dbo].[sp_UpDateValve]    Script Date: 05/31/2011 11:11:05 ******/
ALTER PROCEDURE [sp_UpDateValve] 
	@Critical varchar(2),
	@CriticalNotes varchar(150),
	@CrossStreet varchar(30),
	@DateInst varchar(10),
	@DateRetired varchar(10),
	@InspFreq varchar(10),
	@InspFreqUnit varchar(50),
	@Lat varchar(15),
	@Lon varchar(15),
	@MapPage varchar(6),
	@NorPos varchar(25),
	@Opens varchar(6),
	@RecID int,
	@Remarks varchar(2000),
	@Route varchar(9),
	@SketchNum varchar(15),
	@StNum varchar(10),
	@StName varchar(7),
	@Traffic varchar(2),
	@Turns varchar(6),
	@TwnSection varchar(30),
	@TypeMain varchar(15),
	@ValCtrl varchar(25),
	@ValLoc varchar(150),
	@ValMake varchar(30),
	@ValNum varchar(15),
	@ValSuf int,
	@ValType varchar(25),
	@ValveSize varchar(10),
	@ValveStatus varchar(10),
	@WONum varchar(18),
	@BillInfo varchar(16),
	@BPUKPI varchar(2), 
	@ValveZone int,
	@ObjectID int
	
AS
DECLARE @abbreviation varchar(10)
SELECT @abbreviation = substring(@valNum, 2, charindex(''-'', @valNum)-2) 

DECLARE @townID int
DECLARE @town varchar(50)
DECLARE @county varchar(50)

-- need these because the database joins on string fields =/
SELECT @townID = (SELECT town FROM tblNJAWValves WHERE recID = @recID)
SELECT @town = (SELECT town FROM Towns WHERE TownID = @townID)
SELECT @county = (SELECT county FROM Towns WHERE TownID = @townID)

-- If the Abbreviation is not valid, throw an exception.
IF (NOT @abbreviation in (
							-- Get Valid Abbreviation Codes for the record''s town
							select ab from Towns where TownID = @townID -- Neptune
							union all
							select distinct Abbreviation from TownSections where townID = @townID and isNull(abbreviation, '''') <> ''''
						 ))
	BEGIN
		SELECT isNull(@abbreviation,'''')
		RAISERROR (N''ERROR: UNABLE TO SAVE THE VALVE WITH AN INVALID ABBREVIATION'', 10, 1)
	END
ELSE  -- Else - Update It
BEGIN
	UPDATE 
		tblNJAWValves 
	SET
		Critical = @Critical,
		CriticalNotes = @CriticalNotes,
		CrossStreet = @CrossStreet,
		DateInst = @DateInst,
		DateRetired = @DateRetired,
		InspFreq = @InspFreq,
		InspFreqUnit = @InspFreqUnit,
		Lat = @Lat,
		Lon = @Lon,
		MapPage = @MapPage,
		NorPos = @NorPos,
		Opens = @Opens,
		Remarks = @Remarks,
		Route = @Route,
		SketchNum = @SketchNum,
		StNum = @StNum,
		StName = @StName,
		Traffic = @Traffic,
		Turns = @Turns,
		TwnSection = @TwnSection,
		TypeMain = @TypeMain,
		ValCtrl = @ValCtrl,
		ValLoc = @ValLoc,
		ValMake = @ValMake,
		ValNum = @ValNum,
		ValSuf = @ValSuf,
		ValType = @ValType,
		ValveSize = @ValveSize,
		ValveStatus = @ValveStatus,
		WONum = @WONum,
		BillInfo = @BillInfo,
		BPUKPI = @BPUKPI, 
		ValveZone = @ValveZone,
		ObjectID = @ObjectID
	WHERE 
		RecID = @RecID
	SELECT 1
END' 
END";
        }

        #endregion

        public override void Up()
        {
            Alter.Table(Tables.HYDRANTS).AddColumn(Columns.LAST_UPDATED).AsDateTime().Nullable();
            Alter.Table(Tables.VALVES).AddColumn(Columns.LAST_UPDATED).AsDateTime().Nullable();
            Alter.Table(Tables.SERVICES).AddColumn(Columns.LAST_UPDATED).AsDateTime().Nullable();

            Execute.Sql(UpdateStoredProcedures.UPDATE_HYDRANT);
            Execute.Sql(UpdateStoredProcedures.UPDATE_VALVE);
        }

        public override void Down()
        {
            Delete.Column(Columns.LAST_UPDATED).FromTable(Tables.HYDRANTS);
            Delete.Column(Columns.LAST_UPDATED).FromTable(Tables.VALVES);
            Delete.Column(Columns.LAST_UPDATED).FromTable(Tables.SERVICES);

            Execute.Sql(RestoreStoredProcedures.UPDATE_HYDRANT);
            Execute.Sql(RestoreStoredProcedures.UPDATE_VALVE);
        }
    }
}
