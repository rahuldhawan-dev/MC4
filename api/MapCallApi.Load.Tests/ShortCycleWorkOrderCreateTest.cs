using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

namespace MapCallApi.Load.Tests
{
    public class ShortCycleWorkOrderCreateTest : WebTest
    {
        public ShortCycleWorkOrderCreateTest()
        {
            Proxy = "default";
        }

        /// <summary>
        /// Generate the body for the post. Json data representing a pre-dispatch work order.
        /// Need to figure out how to randomize the values.
        /// </summary>
        /// <returns></returns>
        private StringHttpBody GetBody()
        {
            return new StringHttpBody {
                ContentType = "application/json",
                InsertByteOrderMark = false,
                BodyString = "{" +
                             "\"WorkOrder\":\"0005130" + Context.WebTestIteration.ToString().PadLeft(4, '0') + "\"," +
                             "\"DueDate\":\"20180504\"," +
                             "\"DueTime\":\"131300\"," +
                             "\"Equipment\":" +
                             "[" +
                             " {\"EquipmentId\":\"000000000051834919\", \"ProcessingIndicator\":\"X\"}, {\"EquipmentID\":\"000000000051834918\"}" +
                             "]," +
                             "\"FunctionalLocation\":6001841541," +
                             "\"NotificationNumber\":313518958," +
                             "\"Priority\":\"6: Within 30 Days\"," +
                             "\"BackReportingType\":32," +
                             "\"CompanyCode\":1015," +
                             "\"Premise\":9050075945," +
                             "\"Status\":\"Dispatched\"," +
                             "\"WBSElement\":\"E15-1420-153006\"," +
                             "\"PlanningPlant\":\"D401\"," +
                             "\"MatCode\":\"CKM\"," +
                             "\"MATCodeDescription\":\"Check MeterVerif Serial #&Read\"," +
                             "\"OperationText\":\"Check Meter, Verify Serial #, Read\"," +
                             "\"FSRId\":60001272," +
                             "\"FSRName\":\"Moustachio Featherbottoms\"," +
                             "\"CustomerNumber\":1101748652," +
                             "\"ServiceType\":\"WT\"," +
                             "\"MeterSerialNumber\":\"000000000085424082\"," +
                             "\"ManufacturerSerialNumber\":\"0085424082\"," +
                             "\"IsCustomerEnrolledForEmail\":false," +
                             "\"AssignmentStart\":20180323150700," +
                             "\"AssignmentEnd\":20180323152500," +
                             "\"EarlyStartDate\":20180323152500," +
                             "\"OrderType\":\"0032\"," +
                             "\"OperationId\":\"0010\"," +
                             "\"WorkCenter\":\"FSR\"," +
                             "\"CustomerAccount\":210020509469," +
                             "\"NormalDuration\":18," +
                             "\"NormalDurationUnit\":\"MIN\"," +
                             "\"PlannerGroup\":\"010\", " +
                             "\"PhoneAhead\": \"true\", " +
                             "\"CustomerAtHome\":\"false\"," +
                             "\"SecurityThreats\":" +
                             "[" +
                             "{\"PoliceEscort\":\"No\", \"SecurityThreat\":\"iGnoRe\" }, " +
                             "{\"PoliceEscort\":\"Yes\", \"SecurityThreat\":\"there is a security threat\" }" +
                             "]" +
                             "}"
            };
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            WebTestRequest request1 = new WebTestRequest("http://localhost:55667/ShortCycleWorkOrder/CreateOrUpdate") {
                    Method = "POST",
                    Encoding = Encoding.GetEncoding("utf-8"),
                    Body = GetBody()
                };
            request1.Headers.Add(new WebTestRequestHeader("Authorization", "Basic cnlzdHJvYTE6cGFzc3dvcmQjMQ=="));

            if (Context.ValidationLevel >= ValidationLevel.High)
            {
                ValidationRuleFindText validationRule1 = new ValidationRuleFindText {
                    FindText = "\"Success\":true,\"Id\"",
                    IgnoreCase = false,
                    UseRegularExpression = false,
                    PassIfTextFound = true
                };
                request1.ValidateResponse += validationRule1.Validate;
            }
            yield return request1;
            request1 = null;
        }
    }
}
