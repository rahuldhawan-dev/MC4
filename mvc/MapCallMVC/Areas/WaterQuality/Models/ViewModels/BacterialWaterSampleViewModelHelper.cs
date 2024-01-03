using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    /// <summary>
    /// Helper methods for setting common values on different BacterialWaterSample
    /// view models that do not inherit from one another.
    /// </summary>
    internal static class BacterialWaterSampleViewModelHelper
    {
        public static bool? GetIsReadyForLIMS(BacterialWaterSample entity)
        {
            // We only want to set the IsReadyForLIMS value if it's Not Ready/Ready.
            // If it's any other status, the user can not edit it anyway and we don't
            // want to post back any values.

            if (entity.LIMSStatus == null || entity.LIMSStatus.Id == LIMSStatus.Indices.NOT_READY)
            {
                return false;
            }
            else if (entity.LIMSStatus.Id == LIMSStatus.Indices.READY_TO_SEND)
            {
                return true;
            }
            else
            {
                // Leave this null if the user can't change this value.
                return null;
            }
        }

        public static void SetLIMSStatus(IContainer container, BacterialWaterSample entity, bool? isReadyForLIMS)
        {
            // LIMSStatus can only be updated when the current status is Not Ready, Ready to Send, or not set at all.
            if (entity.LIMSStatus != null && entity.LIMSStatus.Id != LIMSStatus.Indices.READY_TO_SEND &&
                entity.LIMSStatus.Id != LIMSStatus.Indices.NOT_READY)
            {
                return;
            }

            if (isReadyForLIMS.HasValue)
            {
                var statusId = isReadyForLIMS.Value ? LIMSStatus.Indices.READY_TO_SEND : LIMSStatus.Indices.NOT_READY;
                entity.LIMSStatus = container.GetInstance<IRepository<LIMSStatus>>().Find(statusId);
            }
        }
    }
}