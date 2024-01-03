using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SystemDeliveryEntryHelpers
    {
        #region Exposed Methods

        /// <summary>
        ///     Unless the current User can Approve or is an Admin, we need to check if today is past the 3rd of the following
        ///     month because users with regular access can't enter reversals after the 3rd of the following month.
        /// </summary>
        /// <param name="today">The current date</param>
        /// <param name="weekOf">The System Delivery Entry weekOf Property</param>
        /// <param name="currentUser"></param>
        /// <returns>bool</returns>
        public static bool IsSystemDeliveryEntryReversable(DateTime today, DateTime weekOf, User currentUser)
        {
            if (currentUser.Roles.Any(x =>
                    x.Module.Value == RoleModules.ProductionSystemDeliveryApprover &&
                    (x.Action.Id == (int)RoleActions.Add || x.Action.Id == (int)RoleActions.UserAdministrator)) ||
                currentUser.IsAdmin)
            {
                return true;
            }

            return today <= weekOf.AddMonths(1) && currentUser.Roles.Any(x =>
                x.Module.Value == RoleModules.ProductionSystemDeliveryEntry && (x.Action.Id == (int)RoleActions.Add ||
                    x.Action.Id == (int)RoleActions.UserAdministrator));
        }

        #endregion
    }
}
