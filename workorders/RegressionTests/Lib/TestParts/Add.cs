using MMSINC.Testing.Selenium;
using Selenium;

namespace RegressionTests.Lib.TestParts.Add
{
    public class MaterialsUsed
    {
        #region Constants

        public struct NecessaryIDs
        {
            public static readonly string TXT_QUANTITY =
                "ctl00_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_ctl02_txtQuantity";

            public static readonly string DDL_PART_NUMBER =
                "ctl00_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_ctl02_ddlPartNumber",
                                          DDL_STOCK_LOCATION =
                                              "ctl00_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_ctl02_ddlStockLocation";

            public static readonly string LB_INSERT =
                "ctl00_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_ctl02_lbInsert";

            public static readonly string LBL_DESCRIPTION =
                "ctl00_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_ctl02_lblDescription";

        }

        #endregion

        public static Types.MaterialsUsed WithDefaultValues(ISelenium selenium)
        {
            var material = new Types.MaterialsUsed {
                Quantity = "1",
                PartNumber = "1404544",
                StockLocation = "H&M",
                Description = "Coupling,  1\" TNA X CC"
            };

            selenium.Type(NecessaryIDs.TXT_QUANTITY, material.Quantity);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_PART_NUMBER,
                material.PartNumber);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_STOCK_LOCATION,
                material.StockLocation);
            return material;
        }
    }
}
