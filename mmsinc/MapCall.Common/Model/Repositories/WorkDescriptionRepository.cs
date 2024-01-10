using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class WorkDescriptionRepository : RepositoryBase<WorkDescription>, IWorkDescriptionRepository
    {
        #region Constants

        public static readonly int[] MAIN_BREAKS_AND_SERVICE_LINES = new[] {
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

        public static readonly int[] MAIN_BREAKS = new[] {
            (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR,
            (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE
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
            95, // sewer main overflow
            128 // sewer service overflow
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

        public static readonly int[] NEW_SERVICE_INSTALLATIONS = {
            (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION,
            (int)WorkDescription.Indices.FIRE_SERVICE_INSTALLATION,
            (int)WorkDescription.Indices.INSTALL_METER,
            (int)WorkDescription.Indices.IRRIGATION_INSTALLATION
        };

        public static readonly int[] SERVICE_APPROVAL_WORK_DESCRIPTIONS = {
            (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL, 
            (int)WorkDescription.Indices.SERVICE_LINE_RETIRE, 
            (int)WorkDescription.Indices.FIRE_SERVICE_INSTALLATION,
            (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION, 
            (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL, 
            (int)WorkDescription.Indices.SEWER_LATERAL_INSTALLATION,
            (int)WorkDescription.Indices.SEWER_LATERAL_REPLACE, 
            (int)WorkDescription.Indices.WATER_SERVICE_RENEWAL_CUST_SIDE,
            (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL_LEAD,
            (int)WorkDescription.Indices.SERVICE_LINE_RETIRE_LEAD, 
            (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL_CUST_LEAD,
            (int)WorkDescription.Indices.SERVICE_LINE_RETIRE_LEAD_NO_PREMISE
        };

        #endregion

        #region Constructors

        public WorkDescriptionRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        public IEnumerable<WorkDescription> GetUsedByAssetTypeIds(int[] assetTypeIds)
        {
            WorkDescription workDescription = null;
            var query = Session.QueryOver(() => workDescription)
                               .Where(Restrictions.In(Projections.Property<WorkDescription>(wd => wd.AssetType.Id),
                                    assetTypeIds));

            var subQuery = QueryOver.Of<WorkOrder>()
                                    .Where(wo => wo.WorkDescription.Id == workDescription.Id)
                                    .Select(Projections.Distinct(
                                         Projections.Property<WorkOrder>(wo => wo.WorkDescription.Id)));
            query.WithSubquery.WhereExists(subQuery);
            // Business doesn't want below list of inactive work descriptions to be displayed in any page
            // So excluding them from inactive list
            return query.List().Where(x => !WorkDescription.EXCLUDED_INACTIVE_WORK_DESCRIPTIONS.Contains(x.Id));
        }
    }

    public interface IWorkDescriptionRepository : IRepository<WorkDescription>
    {
        IEnumerable<WorkDescription> GetUsedByAssetTypeIds(int[] assetTypeIds);
    }

    public static class IWorkDescriptionRepositoryExtensions
    {
        public static IQueryable<WorkDescription> GetActiveByAssetTypeId(
            this IRepository<WorkDescription> that,
            int assetTypeId)
        {
            return that.Where(s => s.IsActive && s.AssetType.Id == assetTypeId);
        }
        
        public static IQueryable<WorkDescription> GetActiveByAssetTypeIdForCreate(
            this IRepository<WorkDescription> that,
            int assetTypeId,
            bool revisit)
        {
            return that.GetActiveByAssetTypeId(assetTypeId).Where(s => !s.EditOnly && s.Revisit == revisit);
        }
    }
}
