using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170227105224203), Tags("Production")]
    public class Bug3291Facilities : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities")
                 .AddColumn("StreetID").AsInt32().Nullable()
                 .ForeignKey("FK_tblFacilities_Streets_StreetId", "Streets", "StreetID")
                 .AddColumn("CrossStreetID").AsInt32().Nullable()
                 .ForeignKey("FK_tblFacilities_Streets_CrossStreetId", "Streets", "StreetId");

            // Update data
            Execute.Sql(@"
update tblFacilities
	set tblFacilities.StreetID = S.StreetId,
	    tblFacilities.CrossStreetID = CS.StreetID
from 
	tblFacilities F
left join
	streets S on S.FullStName = IsNull(F.StreetPrefix + ' ', '') + isNull(F.StreetName + ' ', '') + isNull(F.StreetSuffix, '') and S.TownID = F.TownID
left join
	streets CS on CS.FullStName = F.NearestCrossStreet and CS.TownID = F.TownID
left join OperatingCenters oc on oc.OperatingCenterId = F.OperatingCenterID
");

            // TODO: Either restore these in rollback or let them sit idle for easier data correction after this releases.
            // Right now these are being deleted to easily see where things are broken in all of the projects.
            //Delete.Column("StreetPrefix").FromTable("tblFacilities");
            //Delete.Column("StreetName").FromTable("tblFacilities");
            //Delete.Column("StreetSuffix").FromTable("tblFacilities");
            //Delete.Column("NearestCrossStreet").FromTable("tblFacilities");
            //Delete.Column("Town").FromTable("tblFacilities");
            //Delete.Index("IDX_tblFacilities_County").OnTable("tblFacilities");
            //Delete.Column("County").FromTable("tblFacilities");
            //Delete.Column("CountyID").FromTable("tblFacilities");
            //Delete.Column("State").FromTable("tblFacilities");
            //Delete.ForeignKey("FK_tblFacilities_States_StateID").OnTable("tblFacilities");
            //Delete.Column("StateID").FromTable("tblFacilities");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblFacilities_Streets_StreetId").OnTable("tblFacilities");
            Delete.ForeignKey("FK_tblFacilities_Streets_CrossStreetId").OnTable("tblFacilities");
            Delete.Column("StreetID").FromTable("tblFacilities");
            Delete.Column("CrossStreetID").FromTable("tblFacilities");
        }
    }
}
