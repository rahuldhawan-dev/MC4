using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200722150303806), Tags("Production")]
    public class MC2372AddColumnForEquipmentType : Migration
    {
        public override void Up()
        {
            Alter.Table("SAPEquipmentTypes")
                 .AddColumn("IsLockoutRequired").AsBoolean().NotNullable().WithDefaultValue(false);

            void lockoutRequired(int sapEquipmentId, int lockOutValue)
            {
                Update.Table("SAPEquipmentTypes ").Set(new {IsLockoutRequired = lockOutValue})
                      .Where(new {Id = sapEquipmentId});
            }

            lockoutRequired(121, 1);
            lockoutRequired(124, 1);
            lockoutRequired(125, 1);
            lockoutRequired(126, 1);
            lockoutRequired(127, 1);
            lockoutRequired(130, 1);
            lockoutRequired(131, 1);
            lockoutRequired(132, 1);
            lockoutRequired(133, 1);
            lockoutRequired(134, 1);
            lockoutRequired(135, 1);
            lockoutRequired(136, 1);
            lockoutRequired(145, 1);
            lockoutRequired(146, 1);
            lockoutRequired(148, 1);
            lockoutRequired(154, 1);
            lockoutRequired(162, 1);
            lockoutRequired(163, 1);
            lockoutRequired(165, 1);
            lockoutRequired(166, 1);
            lockoutRequired(167, 1);
            lockoutRequired(168, 1);
            lockoutRequired(169, 1);
            lockoutRequired(170, 1);
            lockoutRequired(171, 1);
            lockoutRequired(172, 1);
            lockoutRequired(173, 1);
            lockoutRequired(174, 1);
            lockoutRequired(182, 1);
            lockoutRequired(183, 1);
            lockoutRequired(184, 1);
            lockoutRequired(189, 1);
            lockoutRequired(190, 1);
            lockoutRequired(191, 1);
            lockoutRequired(192, 1);
            lockoutRequired(197, 1);
            lockoutRequired(199, 1);
            lockoutRequired(200, 1);
            lockoutRequired(201, 1);
            lockoutRequired(202, 1);
            lockoutRequired(203, 1);
            lockoutRequired(204, 1);
            lockoutRequired(205, 1);
            lockoutRequired(206, 1);
            lockoutRequired(207, 1);
            lockoutRequired(209, 1);
            lockoutRequired(214, 1);
            lockoutRequired(215, 1);
            lockoutRequired(219, 1);
            lockoutRequired(220, 1);
            lockoutRequired(221, 1);
            lockoutRequired(222, 1);
            lockoutRequired(223, 1);
            lockoutRequired(224, 1);
            lockoutRequired(225, 1);
            lockoutRequired(227, 1);
            lockoutRequired(228, 1);
            lockoutRequired(229, 1);
            lockoutRequired(232, 1);
            lockoutRequired(234, 1);
            lockoutRequired(235, 1);
            lockoutRequired(239, 1);
        }

        public override void Down()
        {
            Delete.Column("IsLockoutRequired").FromTable("SAPEquipmentTypes");
        }
    }
}
