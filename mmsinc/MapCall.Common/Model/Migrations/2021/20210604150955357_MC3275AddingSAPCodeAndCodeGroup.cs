using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210604150955357), Tags("Production")]
    public class MC3275AddingSAPCodeAndCodeGroup : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrderPurposes")
                 .AddColumn("SapCode").AsString().Nullable()
                 .AddColumn("CodeGroup").AsString().Nullable();
            Execute.Sql(@"IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Customer')
                            INSERT INTO WorkOrderPurposes values('Customer', '0', 'I01', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Compliance')
                            INSERT INTO WorkOrderPurposes values('Compliance', '1', 'I04', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Safety')
                            INSERT INTO WorkOrderPurposes values('Safety', '1', 'I03', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Leak Detection')
                            INSERT INTO WorkOrderPurposes values('Leak Detection', '1', 'I07', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Revenue 150-500')
                            INSERT INTO WorkOrderPurposes values('Revenue 150-500', '0', 'I08', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Revenue 500-1000')
                            INSERT INTO WorkOrderPurposes values('Revenue 500-1000', '0', 'I09', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Revenue >1000')
                            INSERT INTO WorkOrderPurposes values('Revenue >1000', '0', 'I10', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Damaged/Billable')
                            INSERT INTO WorkOrderPurposes values('Damaged/Billable', '1', 'I11', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Estimates')
                            INSERT INTO WorkOrderPurposes values('Estimates', '0', 'I12', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Water Quality')
                            INSERT INTO WorkOrderPurposes values('Water Quality', '1', 'I13', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Asset Record Control')
                            INSERT INTO WorkOrderPurposes values('Asset Record Control', '1', 'I14', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Seasonal')
                            INSERT INTO WorkOrderPurposes values('Seasonal', '1', 'I06', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Demolition')
                            INSERT INTO WorkOrderPurposes values('Demolition', '0', 'I15', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'BPU')
                            INSERT INTO WorkOrderPurposes values('BPU', '0', 'I04', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Natural Disaster')
                            INSERT INTO WorkOrderPurposes values('Natural Disaster', '1', 'I03', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Construction Project')
                            INSERT INTO WorkOrderPurposes values('Construction Project', '1', 'DV02', 'N-D-PUR2');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Equip Reliability')
                            INSERT INTO WorkOrderPurposes values('Equip Reliability', '1', 'I02', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Regulatory')
                            INSERT INTO WorkOrderPurposes values('Regulatory', '1', 'I05', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Locate')
                            INSERT INTO WorkOrderPurposes values('Locate', '0', 'I16', 'N-D-PUR1');
                          IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes where [Description] = 'Clean Out')
                            INSERT INTO WorkOrderPurposes values('Clean Out', '0', 'I17', 'N-D-PUR1');");
            Execute.Sql(@"UPDATE WorkOrderPurposes SET SapCode = 'I01', CodeGroup = 'N-D-PUR1' WHERE Description = 'Customer';
                            UPDATE WorkOrderPurposes SET SapCode = 'I04', CodeGroup = 'N-D-PUR1' WHERE Description = 'Compliance';
                            UPDATE WorkOrderPurposes SET SapCode = 'I03', CodeGroup = 'N-D-PUR1' WHERE Description = 'Safety';
                            UPDATE WorkOrderPurposes SET SapCode = 'I07', CodeGroup = 'N-D-PUR1' WHERE Description = 'Leak Detection';
                            UPDATE WorkOrderPurposes SET SapCode = 'I08', CodeGroup = 'N-D-PUR1' WHERE Description = 'Revenue 150-500';
                            UPDATE WorkOrderPurposes SET SapCode = 'I09', CodeGroup = 'N-D-PUR1' WHERE Description = 'Revenue 500-1000';
                            UPDATE WorkOrderPurposes SET SapCode = 'I10', CodeGroup = 'N-D-PUR1' WHERE Description = 'Revenue >1000';
                            UPDATE WorkOrderPurposes SET SapCode = 'I11', CodeGroup = 'N-D-PUR1' WHERE Description = 'Damaged/Billable';
                            UPDATE WorkOrderPurposes SET SapCode = 'I12', CodeGroup = 'N-D-PUR1' WHERE Description = 'Estimates';
                            UPDATE WorkOrderPurposes SET SapCode = 'I13', CodeGroup = 'N-D-PUR1' WHERE Description = 'Water Quality';
                            UPDATE WorkOrderPurposes SET SapCode = 'I14', CodeGroup = 'N-D-PUR1' WHERE Description = 'Asset Record Control';
                            UPDATE WorkOrderPurposes SET SapCode = 'I06', CodeGroup = 'N-D-PUR1' WHERE Description = 'Seasonal';
                            UPDATE WorkOrderPurposes SET SapCode = 'I15', CodeGroup = 'N-D-PUR1' WHERE Description = 'Demolition';
                            UPDATE WorkOrderPurposes SET SapCode = 'I04', CodeGroup = 'N-D-PUR1' WHERE Description = 'BPU';
                            UPDATE WorkOrderPurposes SET SapCode = 'I03', CodeGroup = 'N-D-PUR1' WHERE Description = 'Natural Disaster';
                            UPDATE WorkOrderPurposes SET SapCode = 'DV02', CodeGroup = 'N-D-PUR2' WHERE Description = 'Construction Project';
                            UPDATE WorkOrderPurposes SET SapCode = 'I02', CodeGroup = 'N-D-PUR1' WHERE Description = 'Equip Reliability';
                            UPDATE WorkOrderPurposes SET SapCode = 'I05', CodeGroup = 'N-D-PUR1' WHERE Description = 'Regulatory';
                            UPDATE WorkOrderPurposes SET SapCode = 'I16', CodeGroup = 'N-D-PUR1' WHERE Description = 'Locate';
                            UPDATE WorkOrderPurposes SET SapCode = 'I17', CodeGroup = 'N-D-PUR1' WHERE Description = 'Clean Out';");
        }

        public override void Down()
        {
            Delete.Column("SapCode").FromTable("WorkOrderPurposes");
            Delete.Column("CodeGroup").FromTable("WorkOrderPurposes");
        }
    }
}

