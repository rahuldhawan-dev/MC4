using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for RestorationTypeTestTest
    /// </summary>
    [TestClass]
    public class RestorationTypeIntegrationTest : WorkOrdersTestClass<RestorationType>
    {
        #region Constant

        public const int REFERENCE_ID = 1;

        #endregion

        #region Exposed Static Methods

        public static RestorationType GetValidRestorationType()
        {
            return RestorationTypeRepository.GetEntity(REFERENCE_ID);
        }

        #endregion

        #region Private Methods

        protected override RestorationType GetValidObject()
        {
            return new RestorationType();
        }

        protected override RestorationType GetValidObjectFromDatabase()
        {
            var method = GetValidObject();
            RestorationTypeRepository.Insert(method);
            return method;
        }

        protected override void DeleteObject(RestorationType entity)
        {
            RestorationTypeRepository.Delete(entity);
        }

        #endregion
        
        [TestMethod]
        public void TestCreateNewRestorationType()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                Assert.IsNotNull(target);

                MyAssert.DoesNotThrow(
                    () => RestorationTypeRepository.Insert(target));
                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(RestorationType));

                DeleteObject(target);
            }
        }

        /* THIS WILL DUMP ALL THE RestorationTypes AND THEIR ASSOCIATED INITIAL/FINAL RestorationMethods
         * IN .CSV FORMAT:

        [TestMethod]
        public void TestRelationships()
        {
            using (_simulator.SimulateRequest())
            {
                var sb = new System.Text.StringBuilder();
                var types =
                    System.Linq.Enumerable.OrderBy(
                        RestorationTypeRepository.SelectAllAsList(),
                        rt => rt.RestorationTypeID);

                foreach (var type in types)
                {
                    sb.Append(string.Format("{0},", type.RestorationTypeID));
                    sb.Append(string.Format("{0},", type.Description));

                    var initialMethods =
                        (from mt in type.RestorationMethodsRestorationTypes
                         where mt.InitialMethod
                         orderby mt.RestorationMethodID
                         select mt.RestorationMethod);
                    var finalMethods =
                        (from mt in type.RestorationMethodsRestorationTypes
                         where mt.FinalMethod
                         orderby mt.RestorationMethodID
                         select mt.RestorationMethod);

                    sb.AppendFormat("{0},",
                        initialMethods.Aggregate(String.Empty,
                            (baseStr, m) => baseStr + m.RestorationMethodID.ToString() + "; "));

                    sb.AppendFormat("{0},",
                        initialMethods.Aggregate(String.Empty,
                            (baseStr, m) => baseStr + m.Description + "; "));

                    sb.AppendFormat("{0},",
                        finalMethods.Aggregate(String.Empty,
                            (baseStr, m) => baseStr + m.RestorationMethodID.ToString() + "; "));

                    sb.AppendFormat("{0}\n",
                        finalMethods.Aggregate(String.Empty,
                            (baseStr, m) => baseStr + m.Description + "; "));
                }
                System.Diagnostics.Debug.WriteLine(sb.ToString());
            }
        }
        */
    }
}