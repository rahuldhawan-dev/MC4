using System;
using System.Collections.Specialized;
using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public static class Navigation
    {
        #region Constants

        public static readonly NameValueCollection PAGE_STRINGS = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase) {
            {"admin user", "User"},
            {"crew assignment", "CrewAssignment"},
            {"crew assignments calendar", "CrewAssignment/ShowCalendar"},
            {"crew management", "Crew"},
            {"finalization work order", "WorkOrderFinalization"},
            {"finalization", "WorkOrderFinalization"},
            {"forbidden", "Error/Forbidden"},
            {"general work order", "WorkOrderGeneral"},
            {"home", "/"},
            {"login", "Authentication/LogOn"},
            {"planning work order with main", "WorkOrderPlanning"},
            {"planning work order with service", "WorkOrderPlanning"},
            {"planning work order with valve", "WorkOrderPlanning"},
            {"planning work order", "WorkOrderPlanning"},
            {"read only work order", "WorkOrderReadOnly"},
            {"scheduling", "WorkOrderScheduling/Search"},
            {"user index", "User"},
            {"user", "User"},
        };

        #endregion

        #region Event-Driven Functionality

        [BeforeTestRun]
        public static void Begin()
        {
            MMSINC.Testing.SpecFlow.StepDefinitions.Navigation.SetPageStringDictionary(PAGE_STRINGS);
        }

        #endregion
    }
}
