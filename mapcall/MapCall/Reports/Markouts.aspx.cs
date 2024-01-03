using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Controls;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;

namespace MapCall.Reports
{
    public partial class Markouts : ReportPageBase
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.WorkManagement; }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods

        private void ApplyFilter(IFilterBuilder builder)
        {
            builder.AddExpression(new FilterBuilderExpression(@"
                (wo.[DateCompleted] IS NULL) 
                AND 
	                (
		                (	
			                (wo.[MarkoutRequirementID] <> 1) 
			                AND 
			                (
				                (
					                (SELECT COUNT(*) FROM   [Markouts] AS [t1] WHERE (DATEADD(HOUR, -DATEPART(HOUR, [t1].[ExpirationDate]), DATEADD(MINUTE, -DATEPART(MINUTE, [t1].[ExpirationDate]), DATEADD(SECOND, -DATEPART(SECOND, [t1].[ExpirationDate]), DATEADD(MILLISECOND, -DATEPART(MILLISECOND, [t1].[ExpirationDate]), [t1].[ExpirationDate])))) >= 
                                    convert(varchar, getDate(), 101) + ' 12:00:000AM'
					                ) AND ([t1].[WorkOrderID] = wo.[WorkOrderID]))) = 0)) OR ((wo.[StreetOpeningPermitRequired] = 1) AND (((
                    SELECT COUNT(*)
                    FROM   [StreetOpeningPermits] AS [t2]
                    WHERE ([t2].[DateIssued] IS NOT NULL) AND (DATEADD(HOUR, -DATEPART(HOUR, [t2].[ExpirationDate]), DATEADD(MINUTE, -DATEPART(MINUTE, [t2].[ExpirationDate]), DATEADD(SECOND, -DATEPART(SECOND, [t2].[ExpirationDate]), DATEADD(MILLISECOND, -DATEPART(MILLISECOND, [t2].[ExpirationDate]), [t2].[ExpirationDate])))) >= 
                convert(varchar, getDate(), 101) + ' 12:00:000AM'
                    ) AND ([t2].[WorkOrderID] = wo.[WorkOrderID])
                    )) = 0))) AND (wo.[MarkoutToBeCalled] IS NOT NULL) AND (DATEADD(HOUR, -DATEPART(HOUR, wo.[MarkoutToBeCalled]), DATEADD(MINUTE, 
                    -DATEPART(MINUTE, wo.[MarkoutToBeCalled]), DATEADD(SECOND, -DATEPART(SECOND, wo.[MarkoutToBeCalled]), DATEADD(MILLISECOND, 
                    -DATEPART(MILLISECOND, wo.[MarkoutToBeCalled]), wo.[MarkoutToBeCalled])))) = 
                convert(varchar, getDate(), 101) + ' 12:00:000AM'
	                ) "));
        }


        protected override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            base.AddExpressionsToFilterBuilder(builder);
            opCntrField.FilterExpression(builder);
            ApplyFilter(builder);
        }
        #endregion
    }
}