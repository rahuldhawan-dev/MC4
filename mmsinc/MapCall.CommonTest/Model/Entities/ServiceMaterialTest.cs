using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ServiceMaterialTest : InMemoryDatabaseTest<ServiceMaterial>
    {
        [TestMethod]
        public void Test_CustomerCompanyEPACodeOverridenOrDefault_ReturnsStateEPACode_IfStateOverrideExists()
        {
            var state = GetEntityFactory<State>().Create();
            var epaCodeLead = GetEntityFactory<EPACode>().Create(new {
                Description = "LEAD"
            });
            var epaCodeNotLead = GetEntityFactory<EPACode>().Create(new {
                Description = "NOT LEAD"
            });

            var target = GetEntityFactory<ServiceMaterial>().Create();
            
            target.CustomerEPACode = epaCodeLead;
            target.CompanyEPACode = epaCodeNotLead;
            
            var epaCodeOverride = GetEntityFactory<ServiceMaterialEPACodeOverride>().Create(new {
                State = state,
                ServiceMaterial = target, 
                CustomerEPACode = epaCodeNotLead,
                CompanyEPACode = epaCodeLead
            });

            Session.Flush();
            Session.Clear();

            target = Session.Load<ServiceMaterial>(target.Id);
            
            Assert.AreEqual(epaCodeOverride.CustomerEPACode.Description, target.CustomerEPACodeOverridenOrDefault(state).Description);
            Assert.AreEqual(epaCodeOverride.CompanyEPACode.Description, target.CompanyEPACodeOverridenOrDefault(state).Description);
        }
        
        
        [TestMethod]
        public void Test_CustomerCompanyEPACodeOverridenOrDefault_ReturnsDefaultEPACode_IfStateOverrideDoesNotExists()
        {
            var state1 = GetEntityFactory<State>().Create();
            var state2 = GetEntityFactory<State>().Create();
            var epaCodeLead = GetEntityFactory<EPACode>().Create(new {
                Description = "LEAD"
            });
            var epaCodeNotLead = GetEntityFactory<EPACode>().Create(new {
                Description = "NOT LEAD"
            });

            var target = GetEntityFactory<ServiceMaterial>().Create();
            
            target.CustomerEPACode = epaCodeLead;
            target.CompanyEPACode = epaCodeNotLead;
            
            var epaCodeOverride = GetEntityFactory<ServiceMaterialEPACodeOverride>().Create(new {
                State = state1,
                ServiceMaterial = target, 
                CustomerEPACode = epaCodeNotLead,
                CompanyEPACode = epaCodeLead
            });

            Session.Flush();
            Session.Clear();

            target = Session.Load<ServiceMaterial>(target.Id);
            
            Assert.AreEqual(epaCodeLead.Description, target.CustomerEPACodeOverridenOrDefault(state2).Description);
            Assert.AreEqual(epaCodeNotLead.Description, target.CompanyEPACodeOverridenOrDefault(state2).Description);
        }
    }
}
