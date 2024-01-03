using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180321101017585), Tags("Production")]
    public class MC83_AddCodeToServiceMaterials : Migration
    {
        public override void Up()
        {
            Create.Column("Code").OnTable("ServiceMaterials").AsString(2).Nullable();

            Update.Table("ServiceMaterials").Set(new {Code = "AC"}).Where(new {Description = "AC"});
            Update.Table("ServiceMaterials").Set(new {Code = "CL"}).Where(new {Description = "Carlon"});
            Update.Table("ServiceMaterials").Set(new {Code = "CI"}).Where(new {Description = "Cast Iron"});
            Update.Table("ServiceMaterials").Set(new {Code = "C"}).Where(new {Description = "Copper"});
            Update.Table("ServiceMaterials").Set(new {Code = "D"}).Where(new {Description = "Ductile"});
            Update.Table("ServiceMaterials").Set(new {Code = "G"}).Where(new {Description = "Galvanized"});
            Update.Table("ServiceMaterials").Set(new {Code = "L"}).Where(new {Description = "Lead"});
            Update.Table("ServiceMaterials").Set(new {Code = "P"}).Where(new {Description = "Plastic"});
            Update.Table("ServiceMaterials").Set(new {Code = "TR"}).Where(new {Description = "Transite"});
            Update.Table("ServiceMaterials").Set(new {Code = "L"}).Where(new {Description = "Tubeloy"});
            Update.Table("ServiceMaterials").Set(new {Code = "U"}).Where(new {Description = "Unknown"});
            Update.Table("ServiceMaterials").Set(new {Code = "VC"}).Where(new {Description = "Vitrified Clay"});
            Update.Table("ServiceMaterials").Set(new {Code = "WC"}).Where(new {Description = "WICL"});
            Update.Table("ServiceMaterials").Set(new {Code = "U"}).Where(new {Description = "Not Present"});
            Update.Table("ServiceMaterials").Set(new {Code = "L"})
                  .Where(new {Description = "Galvanized with Lead Gooseneck"});
            Update.Table("ServiceMaterials").Set(new {Code = "L"})
                  .Where(new {Description = "Other with Lead Gooseneck"});

            Alter.Table("tblWQSample_Sites")
                 .AddColumn("IsNewSite").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("IsCustomerRequest").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("IsNewSite").FromTable("tblWQSample_Sites");
            Delete.Column("IsCustomerRequest").FromTable("tblWQSample_Sites");
            Delete.Column("Code").FromTable("ServiceMaterials");
        }
    }
}
