using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class TrafficControlTicketTest : MapCallMvcSecuredRepositoryTestBase<TrafficControlTicket,
        TrafficControlTicketRepository, User>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITrafficControlTicketRepository>().Use<TrafficControlTicketRepository>();
        }

        #endregion

        [TestMethod]
        public void TestEstimatedInvoiceAmountReturnsZeroIfBillingPartyHasNoHourlyRate()
        {
            var target = new TrafficControlTicket();

            Assert.AreEqual(0, target.EstimatedInvoiceAmount);

            target.BillingParty = new BillingParty();

            Assert.AreEqual(0, target.EstimatedInvoiceAmount);

            target.BillingParty.EstimatedHourlyRate = 155m;

            Assert.AreEqual(0, target.EstimatedInvoiceAmount);

            target.TotalHours = 10;

            Assert.AreEqual(1550, target.EstimatedInvoiceAmount);
        }

        [TestMethod]
        public void TestInvoiceValidReturnsTrueIfAllFieldsEntered()
        {
            var target = new TrafficControlTicket {
                InvoiceAmount = 10m,
                InvoiceDate = DateTime.Now,
                InvoiceTotalHours = 8.5m,
                InvoiceNumber = "123-A"
            };

            Assert.IsTrue(target.InvoiceValid);

            target.InvoiceAmount = null;
            Assert.IsFalse(target.InvoiceValid);

            target.InvoiceAmount = 10m;
            target.InvoiceDate = null;
            Assert.IsFalse(target.InvoiceValid);

            target.InvoiceDate = DateTime.Now;
            target.InvoiceTotalHours = null;
            Assert.IsFalse(target.InvoiceValid);

            target.InvoiceTotalHours = 8.5m;
            target.InvoiceNumber = string.Empty;
            Assert.IsFalse(target.InvoiceValid);
        }

        [TestMethod]
        public void TestInvoicePercentageErrorReturnsCorrectValue()
        {
            var target = new TrafficControlTicket();
            Assert.IsNull(target.InvoicePercentageError);

            target = new TrafficControlTicket {InvoiceAmount = 55m};
            Assert.IsNull(target.InvoicePercentageError);

            // 65 less
            target = new TrafficControlTicket
                {InvoiceAmount = 325, BillingParty = new BillingParty {EstimatedHourlyRate = 65m}, TotalHours = 4};
            Assert.AreEqual(0.25m, target.InvoicePercentageError);

            // 65 more
            target = new TrafficControlTicket
                {InvoiceAmount = 325, BillingParty = new BillingParty {EstimatedHourlyRate = 97.5m}, TotalHours = 4};
            Assert.AreEqual(0.1666666666666666666666666667m, target.InvoicePercentageError);
        }

        //[TestMethod]
        //public void TestRelatedSAPWorkOrderTicketsBringsBackRelatedSAPWorkOrderTickets()
        //{
        //    var wo = GetEntityFactory<WorkOrder>().Create(new {Purpose = typeof(EntityLookupTestDataFactory<WorkOrderPurpose>)});
        //    GetEntityFactory<TrafficControlTicket>().Create(new { SAPWorkOrderNumber = 1235 });
        //    var target = GetEntityFactory<TrafficControlTicket>().Create(new { SAPWorkOrderNumber = 1235 });

        //    Assert.AreEqual(1, target.RelatedSAPWorkOrderTickets.Count);
        //}

        //[TestMethod]
        //public void TestRelatedWorkOrderTicketsBringsBackRelatedWorkOrderTickets()
        //{
        //    var wo = GetEntityFactory<WorkOrder>().Create(new {Purpose = typeof(EntityLookupTestDataFactory<WorkOrderPurpose>)});
        //    GetEntityFactory<TrafficControlTicket>().Create(new { WorkOrder = wo });
        //    var target = GetEntityFactory<TrafficControlTicket>().Create(new { WorkOrder = wo });

        //    Assert.AreEqual(1, target.RelatedWorkOrderTickets.Count);
        //}

        //[TestMethod]
        //public void TestRelatedTownStreetTicketsBringsBackRelatedRelatedTownStreetTickets()
        //{
        //    var town = GetEntityFactory<Town>().Create();
        //    var street = GetEntityFactory<Street>().Create();
        //    GetEntityFactory<TrafficControlTicket>().Create(new { Town = town, Street = street });
        //    var target = GetEntityFactory<TrafficControlTicket>().Create(new { Town = town, Street = street });

        //    Assert.AreEqual(1, target.RelatedTownStreetTickets.Count);
        //}

        [TestMethod]
        public void TestIsCanceledReturnsTrueWhenIsCanceled()
        {
            var target = new TrafficControlTicket {CanceledAt = DateTime.Now};

            Assert.IsTrue(target.IsCanceled);

            target.CanceledAt = null;

            Assert.IsFalse(target.IsCanceled);
        }

        [TestMethod]
        public void TestIsPaidForReturnsTrueWhenIsPaidFor()
        {
            var target = new TrafficControlTicket {PaymentReceivedAt = DateTime.Now};

            Assert.IsTrue(target.IsPaidFor);

            target.PaymentReceivedAt = null;

            Assert.IsFalse(target.IsPaidFor);

            target.PaidByNJAW = true;

            Assert.IsTrue(target.IsPaidFor);
        }

        [TestMethod]
        public void TestHasChecksForCorrectAmountsReturnsCorrectValue()
        {
            var mtotFee = (12m + 150m) * 0.034m;
            var ticket = new TrafficControlTicket
                {TotalCharged = 162m + mtotFee, ProcessingFee = 12m, InvoiceAmount = 150m, MTOTFee = mtotFee};

            Assert.IsFalse(ticket.HasChecksForCorrectAmount);

            ticket.TrafficcControlTicketChecks.Add(new TrafficControlTicketCheck {Amount = 150});

            Assert.IsTrue(ticket.HasChecksForCorrectAmount);
        }

        [TestMethod]
        public void TestAuthorizeNetInvoiceNumberReturnsZeroFormattedId()
        {
            var target = GetEntityFactory<TrafficControlTicket>().Create();

            Assert.AreEqual(String.Format("{0,8}", target.Id.ToString("D8")), target.AuthorizeNetInvoiceNumber);
        }

        [TestMethod]
        public void TestPayableReturnsCorrectValues()
        {
            var ticket = new TrafficControlTicket();
            Assert.IsFalse(ticket.Payable);

            ticket.InvoiceAmount = 13m;
            ticket.InvoiceDate = DateTime.Now;
            ticket.InvoiceTotalHours = 4;
            ticket.InvoiceNumber = "123111-AS";
            ticket.AccountingCode = "1234";
            Assert.IsTrue(ticket.Payable);

            ticket.CanceledAt = DateTime.Now;
            Assert.IsFalse(ticket.Payable);
            ticket.CanceledAt = null;
            Assert.IsTrue(ticket.Payable);

            ticket.SubmittedAt = DateTime.Now;
            Assert.IsFalse(ticket.Payable);
            ticket.SubmittedAt = null;
            Assert.IsTrue(ticket.Payable);

            ticket.PaymentReceivedAt = DateTime.Now;
            Assert.IsFalse(ticket.Payable);

            ticket.PaymentReceivedAt = null;
            Assert.IsTrue(ticket.Payable);

            ticket.AccountingCode = "";
            Assert.IsFalse(ticket.Payable);
        }

        [TestMethod]
        public void TestCanBeSubmittedReturnsTrueWhenCanBeSubmitted()
        {
            var mtotFee = (12m + 150m) * 0.034m;
            var ticket = new TrafficControlTicket {
                TotalCharged = 162m + mtotFee,
                ProcessingFee = 12m,
                InvoiceAmount = 150m,
                MTOTFee = mtotFee,
                PaymentReceivedAt = DateTime.Now,
                TrackingNumber = "123BAC"
            };
            ticket.TrafficcControlTicketChecks.Add(new TrafficControlTicketCheck {Amount = 150m});
            Assert.IsTrue(ticket.CanBeSubmitted);
        }

        [TestMethod]
        public void Test142CanBeSubmittedReturnsTrueWhenCanBeSubmitted()
        {
            var ticket = new TrafficControlTicket {
                TotalCharged = 343.29m,
                ProcessingFee = 12.00m,
                InvoiceAmount = 320.00m,
                MTOTFee = 11.29m,
                PaymentReceivedAt = DateTime.Now,
                TrackingNumber = "123BAC"
            };
            ticket.TrafficcControlTicketChecks.Add(new TrafficControlTicketCheck {Amount = 320.00m});
            Assert.IsTrue(ticket.CanBeSubmitted);
        }

        [TestMethod]
        public void TestCalculateFeesAddsCorrectFees()
        {
            var merchantTotal = new MerchantTotalFee {Fee = 0.034m, IsCurrent = true};
            var ticket = new TrafficControlTicket {
                InvoiceAmount = 50m,
                MerchantTotalFee = merchantTotal
            };

            ticket.CalculateFees();

            Assert.AreEqual(TrafficControlTicket.PROCESSING_FEE, ticket.ProcessingFee);
            Assert.AreEqual(
                (ticket.InvoiceAmount + ticket.ProcessingFee) * merchantTotal.Fee,
                ticket.MTOTFee);
            Assert.AreEqual(
                ticket.InvoiceAmount +
                TrafficControlTicket.PROCESSING_FEE +
                (ticket.InvoiceAmount + ticket.ProcessingFee) * merchantTotal.Fee,
                ticket.TotalCharged);
        }
    }
}
