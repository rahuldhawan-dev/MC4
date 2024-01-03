using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving WorkDescription objects from persistence.
    /// </summary>
    public class WorkDescriptionRepository : WorkOrdersRepository<WorkDescription>
    {
        #region Constants

        public static readonly int[] MAIN_BREAKS_AND_SERVICE_LINES = new [] {
            57, // service line repair
            74, // water main break repair
            80, // water main break replace
            82, // sewer main break repair
            83, // sewer main break replace
            103 // service line repair
        };

        public static readonly int[] SERVICE_LINE_RENEWALS = new[] {
            59, // service line renewal
            193, // service line renewal - storm restoration
            295 // service line renewal lead
        };

        public static readonly int[] SERVICE_LINE_INSTALLATIONS = new[] {
            56, // service line installation
        };

        public static readonly int[] MAIN_BREAKS = new [] {
            WorkDescription.WATER_MAIN_BREAK_REPAIR_ID,
            WorkDescription.WATER_MAIN_BREAK_REPLACE_ID
        };

        public static readonly int[] NEW_SERVICE_INSTALLATION = new[] {
            56, 35, 37, 87
        };

        public static readonly int[] CURB_PIT = new[] {
            5, // curb box repair
            9, // ball/curb stop repair
            14, // excavate meter box/setter
            38, // interior setting repair
            47, // meter box/setter installation
            50, // meter box adjustment/resetter
            56, // service line installation
            59, // service line renewal
            66, // service line/valve box repair
            81, // meter box/setter replace
            103, // service line repair
            126, // service relocation
            132, // curb box replace
            133, // service line/valve box replace
        };

        public static readonly int[] SEWER_OVERFLOW = new[] {
            95,  // sewer main overflow
            128  // sewer service overflow
        };

        public static readonly int[] ASSET_COMPLETION = {
            26, // hydrant installation
            30, // hydrant replacement
            31, // hydrant retirement
            34, // valve blow off installation
            71, // valve replacement
            72, // valve retirement
            93, // sewer opening replace
            94, // sewer opening installation
            118, // valve installation
            119, // valve blow off replacement
            122, // valve blow off retirement
            125, // hydrant relocation
        };

        public const int
            SERVICE_LINE_RENEWAL = 59,
            SERVICE_LINE_RETIRE = 60,
            FIRE_SERVICE_INSTALLATION = 35,
            SEWER_LATERAL_INSTALLATION = 87,
            SEWER_LATERAL_REPLACE = 89,
            SERVICE_LINE_INSTALLATION = 56,
            SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL = 327,
            INSTALL_METER = 37,
            WATER_SERVICE_RENEWAL_CUST_SIDE = 222,
            SERVICE_LINE_RENEWAL_LEAD = 295,
            SERVICE_LINE_RETIRE_LEAD = 298,
            WATER_SERVICE_RENEWAL_CUST_SIDE_LEAD = 307,
            SERVICE_LINE_RETIRE_NO_PREMISE = 313,
            SERVICE_LINE_RETIRE_LEAD_NO_PREMISE = 314;

        public static readonly int[] SERVICE_APPROVAL_WORK_DESCRIPTIONS = {
            SERVICE_LINE_RENEWAL, SERVICE_LINE_RETIRE, FIRE_SERVICE_INSTALLATION,
            SERVICE_LINE_INSTALLATION, SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL, SEWER_LATERAL_INSTALLATION,
            SEWER_LATERAL_REPLACE, WATER_SERVICE_RENEWAL_CUST_SIDE,
            SERVICE_LINE_RENEWAL_LEAD, SERVICE_LINE_RETIRE_LEAD, WATER_SERVICE_RENEWAL_CUST_SIDE_LEAD,
            SERVICE_LINE_RETIRE_NO_PREMISE, SERVICE_LINE_RETIRE_LEAD_NO_PREMISE
        };

        public static readonly int[] NEW_SERVICE_INSTALLATIONS = {
            SERVICE_LINE_INSTALLATION, FIRE_SERVICE_INSTALLATION, INSTALL_METER
        };

        #endregion

        #region Exposed Methods

        public static IEnumerable<WorkDescription> SelectByAssetType(int assetTypeID)
        {
            return
                (from d in DataTable
                 where d.AssetTypeID == assetTypeID
                 orderby d.Description
                 select d);
        }

        public static IEnumerable<WorkDescription> SelectByAssetType(int assetTypeID, bool input, bool revisit)
        {
            return (from d in DataTable
                    where d.IsActive &&
                        d.AssetTypeID == assetTypeID &&
                          ((input) ? d.EditOnly == false : true) &&
                          d.Revisit == revisit
                    orderby d.Description
                    select d);
        }

        public static IEnumerable<WorkDescription> SelectByAssetType(AssetType type)
        {
            return SelectByAssetType(type.AssetTypeID);
        }

        public static IEnumerable<WorkDescription> SelectByAssetType(AssetType type, bool input, bool revisit)
        {
            return SelectByAssetType(type.AssetTypeID, input, revisit);
        }

        public static IEnumerable<WorkDescription> SelectAllSorted()
        {
            return (from d in DataTable orderby d.AssetType.Description, d.Description select d);
        }

        public static IEnumerable<WorkDescription> SelectMainBreakAndServiceLineDescriptions()
        {
            return (from d in DataTable
                    where
                        MAIN_BREAKS_AND_SERVICE_LINES.Contains(
                        d.WorkDescriptionID)
             
                    select d);
        }

        public static IEnumerable<WorkDescription> SelectMainBreakDescriptions()
        {
            return (from d in DataTable
                    where MAIN_BREAKS.Contains(d.WorkDescriptionID)
                    select d);
        }

        #endregion
    }
}
