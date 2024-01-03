using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class EditBlowOffInspectionTest : BlowOffInspectionViewModelTest<EditBlowOffInspection>
    {
        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsGallonsFlowed()
        {
            _viewModel.GPM = 400;
            _viewModel.MinutesFlowed = 2;
            _entity.GallonsFlowed = null;
            _vmTester.MapToEntity();
            Assert.AreEqual(800, _entity.GallonsFlowed);
        }

        #endregion
    }
}
