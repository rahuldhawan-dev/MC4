using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Library
{
    /// <summary>
    /// Summary description for PermissionsExtensionsTestTest.
    /// </summary>
    [TestClass]
    public class PermissionsExtensionsTestTest : EventFiringTestClass
    {
        #region Private Members

        private IUser _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _target);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Extension Method Tests

        [TestMethod]
        public void TestHasAccessToReturnsTrueIfUserIsInUserRoleForGivenOperatingCenter()
        {
            var opCode = "NJ7";
            var expectedRoleString = opCode + "_Field Services_Work Management_Read";
            var opCntr = new OperatingCenter {
                OpCntr = opCode
            };
            
            using (_mocks.Record())
            {
                SetupResult.For(_target.IsInRole(expectedRoleString))
                    .Return(true);
            }

            using (_mocks.Playback())
            {
                Assert.IsTrue(
                    PermissionsExtensions
                        .HasAccessTo(_target, opCntr));
            }
        }

        [TestMethod]
        public void TestHasAccessToReturnsFalseIfUserIsNotInUserRoleForGivenOperatingCenter()
        {
            var opCode = "NJ7";
            var expectedRoleString = opCode + "_Field Services_Work Management_Read";
            var opCntr = new OperatingCenter
            {
                OpCntr = opCode
            };

            using (_mocks.Record())
            {
                SetupResult.For(_target.IsInRole(expectedRoleString))
                    .Return(false);
            }

            using (_mocks.Playback())
            {
                Assert.IsFalse(
                    PermissionsExtensions
                        .HasAccessTo(_target, opCntr));
            }
        }

        [TestMethod]
        public void TestIsAdminForReturnsTrueIfUserIsInAdminRoleForGivenOperatingCenter()
        {
            var opCode = "NJ7";
            var expectedRoleString = opCode + "_Field Services_Work Management_User Administrator";
            var opCntr = new OperatingCenter {
                OpCntr = opCode
            };
            
            using (_mocks.Record())
            {
                SetupResult.For(_target.IsInRole(expectedRoleString))
                    .Return(true);
            }

            using (_mocks.Playback())
            {
                Assert.IsTrue(
                    PermissionsExtensions
                        .IsAdminFor(_target, opCntr));
            }
        }

        [TestMethod]
        public void TestIsAdminForReturnsFalseIfUserIsNotInAdminRoleForGivenOperatingCenter()
        {
            var opCode = "NJ7";
            var expectedRoleString = opCode + "_Field Services_Work Management_User Administrator";
            var opCntr = new OperatingCenter
            {
                OpCntr = opCode
            };

            using (_mocks.Record())
            {
                SetupResult.For(_target.IsInRole(expectedRoleString))
                    .Return(false);
            }

            using (_mocks.Playback())
            {
                Assert.IsFalse(
                    PermissionsExtensions
                        .IsAdminFor(_target, opCntr));
            }
        }

        #endregion
    }
}
