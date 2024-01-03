using System;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for WorkOrderRequesterTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderRequesterTest : WorkOrdersTestClass<WorkOrderRequester>
    {
        #region Private Methods

        protected override WorkOrderRequester GetValidObject()
        {
            return GetValidWorkOrderRequester();
        }

        protected override WorkOrderRequester GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(WorkOrderRequester entity)
        {
            DeleteWorkOrderRequester(entity);
        }

        #endregion

        #region Exposed Static Methods

        public static WorkOrderRequester GetValidWorkOrderRequester()
        {
            return WorkOrderRequesterRepository.Customer;
        }

        public static void DeleteWorkOrderRequester(WorkOrderRequester entity)
        {
            throw new InvalidOperationException("Cannot delete WorkOrderRequester objects.");
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestToStringMethodReflectsDescriptionProperty()
        {
            var description = "Test Description";
            var target = new WorkOrderRequester {
                Description = description
            };

            Assert.AreEqual(description, target.Description);
            Assert.AreEqual(description, target.ToString());
        }
    }

    public class TestWorkOrderRequesterBuilder : TestDataBuilder<WorkOrderRequester>
    {
        #region Static Properties

        public static WorkOrderRequester CallCenter
        {
            get
            {
                return new WorkOrderRequester {
                    WorkOrderRequesterID =
                        WorkOrderRequesterRepository.Indices.CALL_CENTER,
                    Description =
                        WorkOrderRequesterRepository.Descriptions.CALL_CENTER
                };
            }
        }

        public static WorkOrderRequester Customer
        {
            get
            {
                return new WorkOrderRequester {
                    WorkOrderRequesterID =
                        WorkOrderRequesterRepository.Indices.CUSTOMER,
                    Description =
                        WorkOrderRequesterRepository.Descriptions.CUSTOMER
                };
            }
        }

        public static WorkOrderRequester Employee
        {
            get
            {
                return new WorkOrderRequester {
                    WorkOrderRequesterID =
                        WorkOrderRequesterRepository.Indices.EMPLOYEE,
                    Description =
                        WorkOrderRequesterRepository.Descriptions.EMPLOYEE
                };
            }
        }

        public static WorkOrderRequester LocalGovernment
        {
            get
            {
                return new WorkOrderRequester {
                    WorkOrderRequesterID =
                        WorkOrderRequesterRepository.Indices.LOCAL_GOVERNMENT,
                    Description =
                        WorkOrderRequesterRepository.Descriptions.
                            LOCAL_GOVERNMENT
                };
            }
        }

        #endregion

        #region Exposed Methods

        public override WorkOrderRequester Build()
        {
            return null;
        }

        #endregion
    }
}