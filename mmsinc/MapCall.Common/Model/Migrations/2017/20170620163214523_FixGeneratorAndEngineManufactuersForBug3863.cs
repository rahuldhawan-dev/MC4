using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170620163214523), Tags("Production")]
    public class FixGeneratorAndEngineManufactuersForBug3863 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                @"  if not exists (select 1 from SAPEquipmentManufacturers where Description = 'KOHLER' and SAPEquipmentTypeID = 163) insert into SAPEquipmentManufacturers Values(163, 'KOHLER')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'CATERPILLAR' and SAPEquipmentTypeID = 163) insert into SAPEquipmentManufacturers Values(163, 'CATERPILLAR')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'DELCO AC' and SAPEquipmentTypeID = 163) insert into SAPEquipmentManufacturers Values(163, 'DELCO AC')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'RUDOX' and SAPEquipmentTypeID = 163) insert into SAPEquipmentManufacturers Values(163, 'RUDOX')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'ONAN' and SAPEquipmentTypeID = 163) insert into SAPEquipmentManufacturers Values(163, 'ONAN')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'CONSOLIDATED' and SAPEquipmentTypeID = 163) insert into SAPEquipmentManufacturers Values(163, 'CONSOLIDATED')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'MECON' and SAPEquipmentTypeID = 163) insert into SAPEquipmentManufacturers Values(163, 'MECON')

                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'CATERPILLAR' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'CATERPILLAR')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'CUMMINS' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'CUMMINS')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'DETROIT DIESEL' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'DETROIT DIESEL')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'FORD' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'FORD')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'GM' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'GM')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'JOHN DEERE' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'JOHN DEERE')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'KOHLER' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'KOHLER')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'MITSUBISHI' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'MITSUBISHI')
                            if not exists (select 1 from SAPEquipmentManufacturers where Description = 'VOLVO PENTA' and SAPEquipmentTypeID = 154) insert into SAPEquipmentManufacturers Values(154, 'VOLVO PENTA')");

            Delete.ForeignKey("FK_Generators_EquipmentManufacturers_GeneratorManufacturerID").OnTable("Generators");
            Delete.ForeignKey("FK_Generators_EquipmentManufacturers_EngineManufacturerID").OnTable("Generators");
            Execute.Sql(@"
                        update
	                        Generators
                        set
	                        EngineManufacturerID = (SELECT Id from SAPEquipmentManufacturers sapem where sapem.SAPEquipmentTypeId = 154 and sapem.Description = gman.Description)
                        from
	                        Generators G
                        join
	                        EquipmentManufacturers gman on gman.EquipmentManufacturerID = G.EngineManufacturerID
                        join	
	                        EquipmentTypes gType on gtype.EquipmentTypeID = gman.ManufacturerTypeID
                        ");
            Execute.Sql(" update" +
                        " Generators" +
                        " set GeneratorManufacturerID = (SELECT Id from SAPEquipmentManufacturers sapem where sapem.SAPEquipmentTypeId = 163 and sapem.Description = gman.Description)" +
                        " from" +
                        " Generators G" +
                        " join" +
                        " EquipmentManufacturers gman on gman.EquipmentManufacturerID = G.GeneratorManufacturerID" +
                        " join" +
                        " EquipmentTypes gType on gtype.EquipmentTypeID = gman.ManufacturerTypeID");
            Create.ForeignKey("FK_Generators_SAPEquipmentManufacturers_GeneratorManufacturerID").FromTable("Generators")
                  .ForeignColumn("GeneratorManufacturerID").ToTable("SAPEquipmentManufacturers").PrimaryColumn("Id");
            Create.ForeignKey("FK_Generators_SAPEquipmentManufacturers_EngineManufacturerID").FromTable("Generators")
                  .ForeignColumn("EngineManufacturerID").ToTable("SAPEquipmentManufacturers").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Generators_SAPEquipmentManufacturers_GeneratorManufacturerID").OnTable("Generators");
            Delete.ForeignKey("FK_Generators_SAPEquipmentManufacturers_EngineManufacturerID").OnTable("Generators");

            Execute.Sql("UPDATE Generators SET GeneratorManufacturerID = NULL;" +
                        "UPDATE Generators SET EngineManufacturerID = NULL;");

            Create.ForeignKey("FK_Generators_EquipmentManufacturers_GeneratorManufacturerID").FromTable("Generators")
                  .ForeignColumn("GeneratorManufacturerID").ToTable("EquipmentManufacturers")
                  .PrimaryColumn("EquipmentManufacturerId");
            Create.ForeignKey("FK_Generators_EquipmentManufacturers_EngineManufacturerID").FromTable("Generators")
                  .ForeignColumn("EngineManufacturerID").ToTable("EquipmentManufacturers")
                  .PrimaryColumn("EquipmentManufacturerId");
        }
    }
}
