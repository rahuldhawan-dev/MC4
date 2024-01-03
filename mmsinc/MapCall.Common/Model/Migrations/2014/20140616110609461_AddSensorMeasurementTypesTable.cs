using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140616110609461), Tags("Production")]
    public class AddSensorMeasurementTypesTable : Migration
    {
        #region Consts

        public const int MAX_DESCRIPTION_LENGTH = 50;
        private const string TABLE_NAME = "SensorMeasurementTypes";

        #endregion

        private void UpdateSensor(string desc, string val)
        {
            Execute.WithConnection((conn, tran) => {
                var cmd = conn.CreateCommand();
                cmd.Transaction = tran;

                cmd.CommandText = @"declare @unit int
set @unit = (select top 1 [Id] from [SensorMeasurementTypes] where [Description] = @Description)
update [Sensors] set [SensorMeasurementTypeId] = @unit where [EngUnits] = @Value";

                var descParam = cmd.CreateParameter();
                descParam.ParameterName = "Description";
                descParam.Value = desc;
                cmd.Parameters.Add(descParam);

                var valParam = cmd.CreateParameter();
                valParam.ParameterName = "Value";
                valParam.Value = val;
                cmd.Parameters.Add(valParam);
                cmd.ExecuteNonQuery();
            });
        }

        private void CreateRow(string desc, params string[] values)
        {
            Insert.IntoTable(TABLE_NAME).Row(new {Description = desc});

            UpdateSensor(desc, desc);
            foreach (var val in values)
            {
                UpdateSensor(desc, val);
            }
        }

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(MAX_DESCRIPTION_LENGTH).NotNullable().Unique();

            Alter.Table("Sensors")
                 .AddColumn("SensorMeasurementTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_Sensors_SensorMeasurementTypes_SensorMeasurementTypeId", TABLE_NAME, "Id");

            CreateRow("Amps");
            CreateRow("Degrees F", "deg. f");
            CreateRow("GPM", "Gal");
            CreateRow("kW");
            CreateRow("kWh");
            CreateRow("MGD", "M.D.G", "M.D.G.");
            CreateRow("PSI");
            CreateRow("Pulses");
            CreateRow("Seconds", "sec");
            CreateRow("Volts");
            CreateRow("Watts");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Sensors_SensorMeasurementTypes_SensorMeasurementTypeId").OnTable("Sensors");
            Delete.Column("SensorMeasurementTypeId").FromTable("Sensors");
            Delete.Table("SensorMeasurementTypes");
        }
    }
}
