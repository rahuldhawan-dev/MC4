using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.Areas.SAP.Controllers;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using MapCallMVC.Tests.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing;
using MMSINC.Testing.Utilities;

namespace MapCallMVC.Tests
{
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class AssemblyTest
    {
        #region Constants

        public struct Assemblies
        {
            #region Constants

            public static readonly Assembly MAPCALLMVC = Assembly.GetAssembly(typeof(OperatingCenterController)),
                                            MAPCALL_COMMON = Assembly.GetAssembly(typeof(User)),
                                            MAPCALLMVC_TEST = Assembly.GetAssembly(typeof(AssemblyTest));

            #endregion
        }

        // public static string MIGRATIONS = @"C:\Solutions\Permits\Permits.Data\Models\Migrations\";

        #endregion

        #region Private Members

        // these aren't meant to be used, they're just so that the necessary
        // they come from get copied over.  if you have tests that pass in
        // visual studio but fail in TC and from the command line, you might
        // need these in your project also.
#pragma warning disable 169
        private SQLiteException _doNotUseThisException;
#pragma warning restore 169

        #endregion

        #region Views

        [TestMethod]
        public void TestAllViewFilesAtPathAreContentFilesInTheProject()
        {
            var currentPath = Path.GetFullPath(".");
            var path =
                currentPath
                   .ReplaceRegex(@"\\TestResults\\[^\\]+\\Out", string.Empty)
                   .Replace(@"\everything", @"\mvc")
                + @"\MapCallMVC\MapCallMVC.csproj";

            var tester = new MvcProjectFileTester(path);
            
            tester.AssertAllViewFilesAtPathAreContentFilesInTheProject();
        }
        
        #endregion
        
        #region ViewModels

        [TestMethod]
        public void TestAllViewModelPropertiesHaveMatchingPropertyOnEntity()
        {
            // DO NOT use the ignoredTypes array to include concrete types. 
            // The test gets all sad when EntityLookupViewModel<TEntity> shows up since TEntity isn't actually defined.
            var ignoredTypes = new[] { typeof(EntityLookupViewModel<>) };
            TestLibrary.TestAllViewModelPropertiesHaveMatchingPropertyOnEntity(Assemblies.MAPCALLMVC, ignoredTypes);
        }

        [TestMethod]
        public void TestAllConcreteViewModelsHaveOnlyASingleConstructorWithAContainerArgument()
        {
            var ignoredTypes = new[] {
                typeof(CreateWorkOrder),
                typeof(EditRecurringProject),
                typeof(BidForm),
                typeof(CreateProductionWorkOrder),
                typeof(CreateTrafficControlTicket),
                typeof(CreateService)
            };
            TestLibrary.TestAllConcreteViewModelsHaveOnlyASingleConstructorWithAContainerArgument(Assemblies.MAPCALLMVC, ignoredTypes);
        }

        #endregion

        #region Repositories

        [TestMethod]
        public void TestAllRepositoriesHaveAUnitTest()
        {
            Assert.Inconclusive("This should probably exist in MapCall.CommonTest instead?");
            //TestLibrary.TestAllRepositoriesHaveAUnitTest(
            //    Assemblies.MAPCALL_COMMON,
            //    Assemblies.MAPCALLMVC_TEST,
            //    "MapCallMVC.Tests.Models.Repositories");
        }

        #endregion

        #region Controllers

        [TestMethod]
        public void TestRESTfulCRUDMethods()
        {
            var skipControllers = new Type[] {
                typeof(InterconnectionMeterController),
                typeof(PositionGroupCommonNameTrainingRequirementController),
                typeof(EquipmentPurposeController),
                typeof(EquipmentTypeController),
                typeof(TrainingRecordAttendanceFormController),
                typeof(TrainingRecordController), // including this
                typeof(SewerOpeningConnectionController),
                typeof(FacilityProcessController),
                typeof(SampleSiteLeadCopperCertificationController),
                typeof(SapNotificationController),
                typeof(ServiceController),
                typeof(HydrantController),
                typeof(ValveController),
                typeof(ActionItemController), // Need this because it also doesn't fit the mold

                // these controllers go out to SAP, do not fit the mold for this:
                typeof(SAPCreatePreventiveWorkOrderController),
                typeof(SAPMaintenancePlanController)
            };

            var skippableActions = new Dictionary<Type, string[]>();
            skippableActions[typeof(DocumentController)] = new[] { nameof(DocumentController.New) }; // Can't have a specific http method. See comment on action.
            skippableActions[typeof(DocumentLinkController)] = new[] { nameof(DocumentLinkController.New) }; // Can't have a specific http method. See comment on action.
            skippableActions[typeof(WorkOrderFinalizationController)] = new[] { nameof(WorkOrderFinalizationController.Show) }; // See comment on action.
            skippableActions[typeof(RequisitionController)] = new[] { nameof(RequisitionController.New) }; // See comment on action.

            TestLibrary.TestRESTfulCRUDMethods(Assemblies.MAPCALLMVC,
                typeof(Controller),
                t => !skipControllers.Contains(t),
                skippableActions);
        }

        [TestMethod]
        public void TestAllControllersHaveAUnitTest()
        {
            TestLibrary
               .TestAllControllersHaveAUnitTest(Assemblies.MAPCALLMVC, Assemblies.MAPCALLMVC_TEST,
                    "MapCallMVC.Tests", typeof(Controller));
        }

        [TestMethod]
        public void TestAllControllerTestClassesUseTheControllerTestBaseTClass()
        {
            var skipControllers = new[] {
                typeof(ControllerTestBase<,,>),
                typeof(ControllerTestBase<,,,>),
                typeof(HomeControllerTest)
            };

            TestLibrary
               .TestClassesInAssemblyNamespaceUseTheBaseClass(Assemblies.MAPCALLMVC_TEST,
                    "MapCallMVC.Tests.Controllers", typeof(MapCallMvcControllerTestBase<,,>), skipControllers);
        }

        #endregion
    }
}
