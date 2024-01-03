using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class GasMonitorCalibrationDueTest : BaseNotificationTest
    {
        #region Notification Tests

        [TestMethod]
        public void GasMonitorCalibrationDueNotification()
        {
            var model = new GasMonitor {
                Equipment = new Equipment {
                    //Identifier = "MAD-1",
                    Description = "mad-1",
                    EquipmentModel = new EquipmentModel {
                        Description = "equipment model"
                    },
                    EquipmentManufacturer = new EquipmentManufacturer {
                        Description = "manufacturer", EquipmentType = new EquipmentType {
                            Abbreviation = "MAN", Description = "equip type"
                        }
                    }
                },
                AssignedEmployee = new Employee {FirstName = "Bill", MiddleName = "S.", LastName = "Preston"},
                CalibrationFrequencyDays = 6
            };
            model.MostRecentPassingGasMonitorCalibration = new MostRecentGasMonitorCalibration {
                NextDueDate = new DateTime(2020, 10, 5)
            };

            var template =
                RenderTemplate(
                    "MapCall.Common.Resources.NotificationTemplates.Production.Equipment.GasMonitorCalibrationDue.cshtml",
                    model);

            Assert.AreEqual(@"Equipment ID: --0<br/>
Equipment Description: mad-1<br/>
Manufacturer: manufacturer<br/>
Model #: equipment model<br/>
Owned By: Unknown<br/>
Gas Monitor: <br/>
Next Calibration Date: 10/5/2020 12:00:00 AM<br/>
Assigned Employee: Bill S. Preston<br/>", template);
        }

        #endregion
    }
}
