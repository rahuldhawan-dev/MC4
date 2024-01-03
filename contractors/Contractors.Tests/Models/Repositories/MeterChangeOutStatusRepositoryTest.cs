using System;
using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Data.Models.ViewModels;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class MeterChangeOutStatusRepositoryTest : ContractorsControllerTestBase<MeterChangeOutStatus, MeterChangeOutStatusRepository>
    {
        #region Tests


        [TestMethod]
        public void TestGetScheduledStatusReturnsStatusWithScheduledDescription()
        {
            var model1 = GetFactory<MeterChangeOutStatusFactory>().Create(new { Description = "Oh"});
            var model2 = GetFactory<MeterChangeOutStatusFactory>().Create(new { Description = "Scheduled"});

            var result = Repository.GetScheduledStatus();
            Assert.AreSame(model2, result);
        }

        #endregion
    }
}