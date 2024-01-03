using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PositionGroupCommonNameControllerTest : MapCallMvcControllerTestBase<PositionGroupCommonNameController, PositionGroupCommonName>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IRepository<PositionGroupCommonName>>(_container.GetInstance<RepositoryBase<PositionGroupCommonName>>());
            _container.Inject<IRepository<TrainingModule>>(_container.GetInstance<RepositoryBase<TrainingModule>>());
        }

        #endregion

        #region Tests
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var roles = new Dictionary<RoleModules, RoleActions>();

                Action<RoleActions> changeAction = (act) => {
                    roles[RoleModules.OperationsTrainingModules] = act;
                    roles[RoleModules.FieldServicesDataLookups] = act;
                };

                changeAction(RoleActions.Read);
                a.RequiresRoles("~/PositionGroupCommonName/Index", roles);
                a.RequiresRoles("~/PositionGroupCommonName/Show", roles);

                changeAction(RoleActions.Add);
                a.RequiresRoles("~/PositionGroupCommonName/New", roles);
                a.RequiresRoles("~/PositionGroupCommonName/Create", roles);

                changeAction(RoleActions.Edit);
                a.RequiresRoles("~/PositionGroupCommonName/Edit", roles);
                a.RequiresRoles("~/PositionGroupCommonName/Update", roles);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<PositionGroupCommonName>().Create(new { Description = "Argh!" });
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditPositionGroupCommonName, PositionGroupCommonName>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<PositionGroupCommonName>(eq.Id).Description);
        }

        #endregion

        #endregion
    }
}
