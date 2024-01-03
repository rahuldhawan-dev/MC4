using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MapCall.Common.Model.Entities
{
    public class AssetStatusNumberDuplicationValidator
    {
        #region Constants

        public static readonly (int AssetAStatus, int AssetBStatus, bool ExpectedResult)[] TRUTH_TABLE = {
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.ACTIVE, false),
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.INSTALLED, false),
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.PENDING, true),
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.REQUEST_CANCELLATION, false),
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.REQUEST_RETIREMENT, false),
            (AssetStatus.Indices.ACTIVE, AssetStatus.Indices.RETIRED, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.ACTIVE, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.INSTALLED, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.PENDING, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REQUEST_CANCELLATION, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REQUEST_RETIREMENT, true),
            (AssetStatus.Indices.CANCELLED, AssetStatus.Indices.RETIRED, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.ACTIVE, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.INSTALLED, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.PENDING, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.REQUEST_CANCELLATION, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.REQUEST_RETIREMENT, true),
            (AssetStatus.Indices.INACTIVE, AssetStatus.Indices.RETIRED, true),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.ACTIVE, false),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.INSTALLED, false),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.PENDING, true),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.REQUEST_CANCELLATION, true),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.REQUEST_RETIREMENT, true),
            (AssetStatus.Indices.INSTALLED, AssetStatus.Indices.RETIRED, true),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.ACTIVE, true),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.INSTALLED, true),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.PENDING, false),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.REQUEST_CANCELLATION, true),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.REQUEST_RETIREMENT, true),
            (AssetStatus.Indices.PENDING, AssetStatus.Indices.RETIRED, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.ACTIVE, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.INSTALLED, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.PENDING, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.REQUEST_CANCELLATION, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.REQUEST_RETIREMENT, true),
            (AssetStatus.Indices.REMOVED, AssetStatus.Indices.RETIRED, true),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.ACTIVE, false),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.INSTALLED, true),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.PENDING, true),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.REQUEST_CANCELLATION, false),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.REQUEST_RETIREMENT, false),
            (AssetStatus.Indices.REQUEST_CANCELLATION, AssetStatus.Indices.RETIRED, true),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.ACTIVE, false),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.INSTALLED, true),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.PENDING, true),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.REQUEST_CANCELLATION, false),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.REQUEST_RETIREMENT, false),
            (AssetStatus.Indices.REQUEST_RETIREMENT, AssetStatus.Indices.RETIRED, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.ACTIVE, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.CANCELLED, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.INACTIVE, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.INSTALLED, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.PENDING, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.REMOVED, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.REQUEST_CANCELLATION, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.REQUEST_RETIREMENT, true),
            (AssetStatus.Indices.RETIRED, AssetStatus.Indices.RETIRED, true)
        };

        public readonly int[] INACTIVE_STATUSES = {
            AssetStatus.Indices.CANCELLED,
            AssetStatus.Indices.INACTIVE,
            AssetStatus.Indices.REMOVED,
            AssetStatus.Indices.RETIRED
        };

        public readonly int[] ACTIVE_STATUSES = {
            AssetStatus.Indices.ACTIVE,
            AssetStatus.Indices.INSTALLED,
            AssetStatus.Indices.REQUEST_CANCELLATION,
            AssetStatus.Indices.REQUEST_RETIREMENT
        };

        public readonly int[] REQUEST_STATUSES = {
            AssetStatus.Indices.REQUEST_CANCELLATION,
            AssetStatus.Indices.REQUEST_RETIREMENT
        };

        #endregion

        public bool ShouldAllowDuplicate(int assetAStatus, int assetBStatus)
        {
            var both = new[] {assetAStatus, assetBStatus};
            return
                // can't be the same
                (assetAStatus != assetBStatus ||
                 // unless they're both cancelled, inactive, removed, or retired
                 INACTIVE_STATUSES.Contains(assetAStatus)) &&

                // one can't be active while the other is
                !(both.Contains(AssetStatus.Indices.ACTIVE) &&
                  // installed
                  (both.Contains(AssetStatus.Indices.INSTALLED) ||
                   // request cancellation
                   both.Contains(AssetStatus.Indices.REQUEST_CANCELLATION) ||
                   // or request retirement
                   both.Contains(AssetStatus.Indices.REQUEST_RETIREMENT))) &&

                // one can't be request retirement while the other is request cancellation
                !(both.Contains(AssetStatus.Indices.REQUEST_RETIREMENT) &&
                  both.Contains(AssetStatus.Indices.REQUEST_CANCELLATION));
        }

        public bool IsValid<TEntity>(int assetStatus, IQueryable<TEntity> stuff)
            where TEntity : IThingWithAssetStatus
        {
            if (INACTIVE_STATUSES.Contains(assetStatus))
            {
                return true;
            }

            if (assetStatus == AssetStatus.Indices.ACTIVE)
            {
                return !stuff.Any(x => ACTIVE_STATUSES.Contains(x.Status.Id));
            }

            if (REQUEST_STATUSES.Contains(assetStatus))
            {
                return !stuff.Any(x =>
                    REQUEST_STATUSES.MergeWith(new[] {AssetStatus.Indices.ACTIVE}).Contains(x.Status.Id));
            }

            if (ACTIVE_STATUSES.Contains(assetStatus))
            {
                return !stuff.Any(x => new[] {AssetStatus.Indices.ACTIVE, assetStatus}.Contains(x.Status.Id));
            }

            return !stuff.Any(x => x.Status.Id == assetStatus);
        }
    }
}
