using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PremiseTypeControllerTest : MapCallMvcControllerTestBase<PremiseTypeController, PremiseType>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IRepository<PremiseType>>(Repository);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/PremiseType/Show/", module, RoleActions.Read);
                a.RequiresRole("~/PremiseType/Index/", module, RoleActions.Read);
                a.RequiresRole("~/PremiseType/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/PremiseType/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/PremiseType/New/", module, RoleActions.Add);
                a.RequiresRole("~/PremiseType/Create/", module, RoleActions.Add);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<PremiseTypeFactory>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditPremiseType, PremiseType>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<PremiseType>(eq.Id).Description);
        }

        #endregion
    }
}
