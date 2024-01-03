using MapCall.Common.Model.Entities;
using MMSINC.Helpers;

namespace MapCallMVC.ClassExtensions
{
    public static class RazorTableExtensions
    {
        public static RazorTable<TEntity> WithWorkOrderRowStyles<TEntity>(this RazorTable<TEntity> rt)
            where TEntity : IHasWorkOrderStatus
        {
            return rt.WithRowBuilder((x, tag) => {
                switch (x.Status)
                {
                    case WorkOrderStatus.Cancelled:
                        tag.AddCssClass("wo-cancelled");
                        break;

                    case WorkOrderStatus.Completed:
                        tag.AddCssClass("wo-completed");
                        break;

                    case WorkOrderStatus.ScheduledPreviously:
                        tag.AddCssClass("wo-scheduled-previously");
                        break;

                    case WorkOrderStatus.ScheduledCurrently:
                        tag.AddCssClass("wo-scheduled-currently");
                        break;

                    case WorkOrderStatus.RequiresSupervisorApproval:
                        tag.AddCssClass("wo-requires-supervisor-approval");
                        break;

                    case WorkOrderStatus.WithCompliance:
                        tag.AddCssClass("asset-not-inspectable");
                        break;
                }
            });
        }
    }
}