using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ValveDSICReportItemMap : ClassMap<ValveDSICReportItem>
    {
        public const string
            SQL_MATCHES_WBS_PATTERN = @"
                CASE WHEN (
    		    COALESCE((SELECT TOP 1 wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc), WorkOrderNumber) like 'R__-__A_.__-P-____' OR
	        	COALESCE((SELECT TOP 1 wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc), WorkOrderNumber) like 'R__-__B_.__-P-____' OR
		        COALESCE((SELECT TOP 1 wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc), WorkOrderNumber) like 'R__-__C_.__-P-____' OR
		        COALESCE((SELECT TOP 1 wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc), WorkOrderNumber) like 'R__-__F_.__-P-____' OR
		        COALESCE((SELECT TOP 1 wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc), WorkOrderNumber) like 'R__-__H_.__-P-____')
		        THEN 1 ELSE 0 END",
            SQLITE_MATCHES_WBS_PATTERN = @"
                CASE WHEN (
    		    COALESCE((SELECT wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc LIMIT 1), WorkOrderNumber)  like 'R__-__A_.__-P-____' OR
	        	COALESCE((SELECT wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc LIMIT 1), WorkOrderNumber) like 'R__-__B_.__-P-____' OR
		        COALESCE((SELECT wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc LIMIT 1), WorkOrderNumber) like 'R__-__C_.__-P-____' OR
		        COALESCE((SELECT wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc LIMIT 1), WorkOrderNumber) like 'R__-__F_.__-P-____' OR
		        COALESCE((SELECT wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc LIMIT 1), WorkOrderNumber) like 'R__-__H_.__-P-____')
		        THEN 1 ELSE 0 END";

        public ValveDSICReportItemMap()
        {
            Table(nameof(Valve) + "s");

            Id(x => x.Id);

            Map(x => x.StreetNumber).Nullable().Length(Valve.StringLengths.STREET_NUMBER);
            Map(x => x.ValveNumber).Not.Nullable().Length(Valve.StringLengths.VALVE_NUMBER);
            Map(x => x.SAPEquipmentId).Nullable();
            Map(x => x.DateInstalled).Nullable();
            Map(x => x.WBSNumber)
               .DbSpecificFormula(
                    "COALESCE((SELECT TOP 1 wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc), WorkOrderNumber)",
                    "COALESCE((SELECT wo.AccountCharged from WorkOrders wo where wo.ValveId = Id and wo.WorkDescriptionID = 30 ORDER BY wo.WorkOrderID Desc LIMIT 1), WorkOrderNumber)");
            Map(x => x.MatchesWBSPattern)
               .DbSpecificFormula(SQL_MATCHES_WBS_PATTERN, SQLITE_MATCHES_WBS_PATTERN);
            Map(x => x.IsInstalled)
               .Formula("CASE WHEN (DateInstalled IS NOT NULL) THEN 1 ELSE 0 END");

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Town).Column("Town").Not.Nullable();
            References(x => x.Street).Nullable();
            References(x => x.Coordinate).Nullable();
        }
    }
}
