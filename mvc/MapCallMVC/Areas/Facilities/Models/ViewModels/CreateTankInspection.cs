using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentNHibernate.Conventions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.Ajax.Utilities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class CreateTankInspection : TankInspectionViewModel
    {
        #region Constructor

        public CreateTankInspection(IContainer container) : base(container) { }

        #endregion

        public void SetValuesFromProductionWorkOrder(ProductionWorkOrder order)
        {
            foreach (var assignment in order.CurrentAssignments)
            {
                if (assignment.DateStarted.HasValue && !assignment.DateEnded.HasValue)
                {
                    TankObservedBy = assignment.AssignedTo.Id;
                    ObservationDate = assignment.DateStarted;
                    break;
                }
            }
            ProductionWorkOrder = order.Id;
            OperatingCenter = order.OperatingCenter?.Id;
            Facility = order.Facility.Id;
            PublicWaterSupply = order.Facility.PublicWaterSupply?.Id;
            Town = order.Facility.Town?.Id;
            ZipCode = order.Facility.ZipCode;
            Coordinate = order.Facility.Coordinate.Id;
            TankAddress = order.Facility.Address;

            if (order.Equipment != null)
            {
                var inspectionCount = order.Equipment.TankInspections.Count;
                Equipment = order.Equipment.Id;
                foreach (var eqCharacteristicField in order.Equipment.Characteristics)
                {
                    if (eqCharacteristicField.Field.DisplayField.Contains("TNK_VOLUME"))
                    {
                        TankCapacity = Convert.ToDecimal(eqCharacteristicField.Value);
                    }
                }
                if (inspectionCount > 0)
                {
                    var lastTankInsp = order.Equipment.TankInspections.LastOrDefault();
                    LastObserved = lastTankInsp.ObservationDate;
                }
            }
        }
    }
}
