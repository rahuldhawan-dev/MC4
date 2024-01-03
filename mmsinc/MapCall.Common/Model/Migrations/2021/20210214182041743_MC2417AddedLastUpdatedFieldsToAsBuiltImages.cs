using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210214182041743), Tags("Production")]
    public class MC2417AddedLastUpdatedFieldsToAsBuiltImages : Migration
    {
        public override void Down()
        {
            Delete.ForeignKeyColumn("AsBuiltImages", "LastUpdatedById", "tblPermissions", "RecId");
            Delete.Column("LastUpdated").FromTable("AsBuiltImages");
        }

        public override void Up()
        {
            Alter.Table("AsBuiltImages").AddForeignKeyColumn("LastUpdatedById", "tblPermissions", "RecId")
                 .AddColumn("LastUpdated").AsDateTime().Nullable();
        }
    }
}
