using System;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.PreJobSafetyBriefs
{
    public abstract class CreateProductionPreJobSafetyBriefTestBase<TViewModel>
        : ProductionPreJobSafetyBriefViewModelTestBase<TViewModel>
        where TViewModel : CreateProductionPreJobSafetyBriefBase
    {
        #region Tests

        [TestMethod]
        public void Test_SetDefaults_SetsSafetyBriefDateTimeToNow()
        {
            var expected = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);
            _viewModel.SafetyBriefDateTime = null;

            _viewModel.SetDefaults();

            Assert.AreEqual(expected, _viewModel.SafetyBriefDateTime);
        }

        #endregion
    }
}
