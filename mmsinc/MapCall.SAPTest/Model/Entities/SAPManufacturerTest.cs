using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities.Users;

namespace SAP.DataTest.Model.Entities
{
    [TestClass()]
    public class SAPManufacturerTest
    {
        #region Private Methods

        public SAPManufacturer SetSAPManufacturerSearchValues()
        {
            var sapManufacturer = new SAPManufacturer();
            sapManufacturer.Class = "";
            sapManufacturer.Manufacturer = "";

            return sapManufacturer;
        }

        #endregion
    }
}
