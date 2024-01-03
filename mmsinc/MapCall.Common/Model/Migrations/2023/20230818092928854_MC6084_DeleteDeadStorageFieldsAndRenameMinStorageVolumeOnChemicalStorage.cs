using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230818092928854), Tags("Production")]
    public class MC6084_DeleteDeadStorageFieldsAndRenameMinStorageVolumeOnChemicalStorage : Migration
    {
        private struct Tables
        {
            public const string CHEMICAL_STORAGE = "ChemicalStorage";
        }

        private struct Columns
        {
            public const string DEAD_STORAGE_QUANTITY_GALLONS = "DeadStorageQuantityGallons",
                                DEAD_STORAGE_QUANTITY_POUNDS = "DeadStorageQuantityPounds",
                                MIN_STORAGE_VOLUME_OLD_NAME = "MinStorageVolumePounds",
                                MIN_STORAGE_VOLUME_NEW_NAME = "MinStorageQuantityPounds";
        }
        public override void Up()
        {
            Rename.Column(Columns.MIN_STORAGE_VOLUME_OLD_NAME).OnTable(Tables.CHEMICAL_STORAGE).To(Columns.MIN_STORAGE_VOLUME_NEW_NAME);
            Delete.Column(Columns.DEAD_STORAGE_QUANTITY_GALLONS).FromTable(Tables.CHEMICAL_STORAGE);
            Delete.Column(Columns.DEAD_STORAGE_QUANTITY_POUNDS).FromTable(Tables.CHEMICAL_STORAGE);
        }

        public override void Down()
        {
            Rename.Column(Columns.MIN_STORAGE_VOLUME_NEW_NAME).OnTable(Tables.CHEMICAL_STORAGE).To(Columns.MIN_STORAGE_VOLUME_OLD_NAME);
            Alter.Table(Tables.CHEMICAL_STORAGE).AddColumn(Columns.DEAD_STORAGE_QUANTITY_GALLONS).AsFloat().Nullable();
            Alter.Table(Tables.CHEMICAL_STORAGE).AddColumn(Columns.DEAD_STORAGE_QUANTITY_POUNDS).AsFloat().Nullable();
        }
    }
}