using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class AuditLogEntryTest
    {
        [TestMethod]
        public void TestIsLinkableReturnsFalseForInvalidEntityNames()
        {
            foreach (var entityName in AuditLogEntry.INVALID_ENTITY_NAMES)
            {
                var target = new AuditLogEntry {EntityName = entityName};
                Assert.IsFalse(target.IsLinkable);
            }

            var valid = new AuditLogEntry {EntityName = "Bar"};
            Assert.IsTrue(valid.IsLinkable);
        }

        [TestMethod]
        public void TestIsLinkableReturnsFalseForInvalidAuditEntryTypes()
        {
            foreach (var auditEntryType in AuditLogEntry.INVALID_AUDIT_ENTRY_TYPES)
            {
                var target = new AuditLogEntry {AuditEntryType = auditEntryType};
                Assert.IsFalse(target.IsLinkable);
            }

            var valid = new AuditLogEntry {AuditEntryType = "Foo"};
            Assert.IsTrue(valid.IsLinkable);
        }

        [TestMethod]
        public void TestPropertyEntityTypeReturnsEmptyStringWhenNotEntityType()
        {
            var target = new AuditLogEntry { EntityName = "Service" };

            Assert.AreEqual(string.Empty, target.PropertyEntityType);

            target.FieldName = "UpdatedAt";

            Assert.AreEqual(string.Empty, target.PropertyEntityType);
        }

        [TestMethod]
        public void TestPropertyEntityTypeReturnsEntityType()
        {
            var target = new AuditLogEntry { EntityName = "Service", FieldName = "UpdatedBy" };

            Assert.AreEqual("User", target.PropertyEntityType);
        }
    }
}
