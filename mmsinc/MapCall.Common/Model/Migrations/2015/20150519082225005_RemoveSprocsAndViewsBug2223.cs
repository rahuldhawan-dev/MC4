using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225005), Tags("Production")]
    public class RemoveSprocsAndViewsBug2223 : Migration
    {
        public override void Up()
        {
            Execute.Sql("DROP VIEW [dbo].[viewNJAWHydValInsp]");
            Execute.Sql("DROP PROCEDURE [dbo].[sp_DelHVInsp]");
            Execute.Sql("DROP PROCEDURE [dbo].[sp_AddBOSch]");
            Execute.Sql("DROP PROCEDURE [dbo].[sp_AddHydSch]");
            Delete.Table("tblNJAWHydValInsp");
        }

        public override void Down()
        {
            Create.Table("tblNJAWHydValInsp")
                  .WithColumn("RecID").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Critical").AsString(2).Nullable()
                  .WithColumn("CrossStreet").AsString(30).Nullable()
                  .WithColumn("HydNum").AsString(20).Nullable()
                  .WithColumn("MapPage").AsString(6).Nullable()
                  .WithColumn("OutOfServ").AsString(2).Nullable()
                  .WithColumn("Route").AsFloat().Nullable()
                  .WithColumn("StName").AsInt32().Nullable()
                  .WithColumn("StNum").AsString(10).Nullable()
                  .WithColumn("Town").AsInt32().Nullable()
                  .ForeignKey("FK_tblNJAWHydValInsp_Towns_Town", "Towns", "TownID")
                  .WithColumn("Traffic").AsString(2).Nullable()
                  .WithColumn("UserName").AsString(15).Nullable()
                  .WithColumn("OpCntr").AsString(50).Nullable();

            Execute.Sql(@"CREATE PROCEDURE [dbo].[sp_DelHVInsp] 

@UserName varchar(15)

AS

delete from tblNJAWHydValInsp
where UserName = @UserName
GO");

            Execute.Sql(@"CREATE PROCEDURE [dbo].[sp_AddHydSch] 

@Critical varchar(2),
@CrossStreet varchar(30),
@HydNum varchar(20),
@MapPage varchar(6),
@OutOfServ varchar(2),
@Route varchar(12),
@StName varchar(7),
@StNum varchar(10),
@Town varchar(5),
@UserName varchar(15), 
@OpCntr varchar(10)

 AS
insert into tblNJAWHydValInsp( Critical, CrossStreet, HydNum, MapPage, OutOfServ, Route, StName, StNum, Town, UserName, OpCntr )
values( @Critical, @CrossStreet, @HydNum, @MapPage, @OutOfServ, @Route, @StName, @StNum, @Town, @UserName, @OpCntr )

GO");

            Execute.Sql(@"CREATE PROCEDURE [dbo].[sp_AddBOSch] 

@Critical varchar(2),
@CrossStreet varchar(30),
@MapPage varchar(6),
@Route varchar(12),
@StName varchar(7),
@StNum varchar(10),
@Town varchar(5),
@Traffic varchar(2),
@ValNum varchar(20),
@UserName varchar(15),
@OpCntr varchar(10)

 AS
insert into tblNJAWHydValInsp( Critical, CrossStreet, HydNum, MapPage, Route, StName, StNum, Town, Traffic, UserName, OpCntr )
values( @Critical, @CrossStreet, @ValNum, @MapPage,  @Route, @StName, @StNum, @Town, @Traffic, @UserName, @OpCntr )

GO
");

            Execute.Sql(@"
CREATE VIEW 
	[dbo].[viewNJAWHydValInsp]
AS
SELECT
  H.Critical, H.CrossStreet, S.FullStName, H.HydNum, 
  H.MapPage, H.OutOfServ, H.RecID, H.Route, 
  H.StNum, T.Town, H.Traffic, H.UserName, H.opCntr
FROM
	Towns T
RIGHT OUTER JOIN
    tblNJAWHydValInsp H 
ON 
	T.TownID = H.Town 
LEFT OUTER JOIN
	Streets S 
ON 
	H.StName = S.StreetID

GO");
        }
    }
}
