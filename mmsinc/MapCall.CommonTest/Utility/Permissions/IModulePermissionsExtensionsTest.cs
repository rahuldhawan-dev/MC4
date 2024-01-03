using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Utilities.Permissions;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Utility.Permissions
{
    /// <summary>
    /// Summary description for IModulePermissionsHelperTest
    /// </summary>
    [TestClass]
    public class IModulePermissionsExtensionsTest
    {
        #region Private Fields

        private Mock<IModulePermissions> _targetMock;
        private string _applicationName, _moduleName;
        private Mock<IModuleRepository> _repositoryMock;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void IModulePermissionsExtensionsTestInitialize()
        {
            _container = new Container();
            _applicationName = "foo";
            _moduleName = "bar";

            _targetMock = new Mock<IModulePermissions>();
            _repositoryMock = new Mock<IModuleRepository>();

            _targetMock
               .Setup(x => x.Application)
               .Returns(_applicationName);
            _targetMock
               .Setup(x => x.Module)
               .Returns(_moduleName);

            _container.Inject(_repositoryMock.Object);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        #endregion

        [TestMethod]
        public void TestToModuleReturnsTheCorrespondingModelObjectBasedOnApplicationAndModuleName()
        {
            var expected = new Module();

            _repositoryMock
               .Setup(
                    x =>
                        x.FindByApplicationAndModuleName(_applicationName,
                            _moduleName))
               .Returns(expected);

            Assert.AreSame(expected, _targetMock.Object.ToModule());

            _targetMock.VerifyAll();
            _repositoryMock.VerifyAll();
        }
    }
}
