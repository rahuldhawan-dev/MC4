using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191029143552875), Tags("Production")]
    public class ExtendSecureFormValueFieldForMC1745 : Migration
    {
        public override void Up()
        {
            Alter.Table("SecureFormDynamicValues").AlterColumn("XmlValue").AsCustom("nvarchar(MAX)");
        }

        public override void Down() { }
    }
}
