using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;

namespace SAP.DataTest.Model.Entities
{
    [TestClass()]
    public class SAPWBSElementTest
    {
        #region Private Methods

        public SAPWBSElement SetWBSElementSearchValues()
        {
            var sapWBSElement = new SAPWBSElement();
            sapWBSElement.AssetCode = "";
            sapWBSElement.PlanningPlant = "";
            sapWBSElement.ProfitCentre = "";
            sapWBSElement.ProfitCentreFieldName = "";
            sapWBSElement.ProjectDefintion = "";
            sapWBSElement.ProjectType = "BL";
            sapWBSElement.SystemStatus = "Complete";
            sapWBSElement.SystemStatus = "";
            sapWBSElement.WBSDescription = "";
            sapWBSElement.WBSNumber = ""; //B12-01-0015
            sapWBSElement.Year = "2017";
            return sapWBSElement;
        }

        public SAPWBSElement SetWBSElementSearchValuesByWSB()
        {
            var sapWBSElement = new SAPWBSElement();
            sapWBSElement.AssetCode = "";
            sapWBSElement.PlanningPlant = "P208";
            sapWBSElement.ProfitCentre = "";
            sapWBSElement.ProfitCentreFieldName = "";
            sapWBSElement.ProjectDefintion = "";
            sapWBSElement.ProjectType = "";
            sapWBSElement.Status = "";
            sapWBSElement.SystemStatus = "";
            sapWBSElement.WBSDescription = "";
            sapWBSElement.WBSNumber = "R18-22G1.17-P-0001";
            sapWBSElement.Year = "";
            return sapWBSElement;
        }

        public SAPWBSElement SetWBSElementSearchNullValues()
        {
            var sapWBSElement = new SAPWBSElement();
            sapWBSElement.AssetCode = "";
            sapWBSElement.PlanningPlant = "";
            sapWBSElement.ProfitCentre = "";
            sapWBSElement.ProfitCentreFieldName = "";
            sapWBSElement.ProjectDefintion = "";
            sapWBSElement.ProjectType = "";
            sapWBSElement.Status = "";
            sapWBSElement.SystemStatus = "";
            sapWBSElement.WBSDescription = "";
            sapWBSElement.WBSNumber = "R18-19Q1.17-P-000*";
            sapWBSElement.Year = "";
            return sapWBSElement;
        }

        #endregion
    }
}
