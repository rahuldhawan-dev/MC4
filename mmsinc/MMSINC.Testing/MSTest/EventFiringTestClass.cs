using System;
using System.Diagnostics;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;

namespace MMSINC.Testing.MSTest
{
    /// <summary>
    /// Represents the base functionality for a TestClass
    /// that needs to fire events on its target object.
    /// Mocking support via RhinoMocks is included.
    /// </summary>
    public abstract class EventFiringTestClass
    {
        #region Private Members

        /// <summary>
        /// MockRepository object used throughout this class;
        /// </summary>
        protected MockRepository _mocks;

        /// <summary>
        /// Boolean variable used to determine if a function
        /// was called (i.e. an event has been fired).
        /// </summary>
        protected bool _called;

        /// <summary>
        /// EventHandler method useful to attach to a target
        /// and ensure that said method was called (using
        /// the called field).
        /// </summary>
        protected EventHandler _testableHandler;

        protected IContainer _container;

        #endregion

        #region Initialization And Cleanup

        [TestInitialize]
        public virtual void EventFiringTestClassInitialize()
        {
            _mocks = new MockRepository();
            _called = false;
            _testableHandler = (sender, e) => _called = true;
            _container = new Container();
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public virtual void EventFiringTestClassCleanup()
        {
            _mocks.VerifyAll();
            _container.Dispose();
        }

        #endregion

        #region Helper Methods

        [DebuggerStepThrough]
        protected static object[] GetEventArgArray()
        {
            return GetEventArgArray(null, null);
        }

        [DebuggerStepThrough]
        protected static object[] GetEventArgArray(object sender)
        {
            return GetEventArgArray(sender, null);
        }

        [DebuggerStepThrough]
        protected static object[] GetEventArgArray(EventArgs e)
        {
            return GetEventArgArray(null, e);
        }

        [DebuggerStepThrough]
        protected static object[] GetEventArgArray(object sender, EventArgs e)
        {
            return new[] {sender, e};
        }

        [DebuggerStepThrough]
        protected static void InvokeEventByName(object obj, string eventName)
        {
            InvokeEventByName(obj, eventName, GetEventArgArray());
        }

        [DebuggerStepThrough]
        protected static void InvokeEventByName(object obj, string eventName, params object[] eventArgsArray)
        {
            var type = obj.GetType();
            var mi = type.GetMethod(eventName,
                BindingFlags.Instance |
                BindingFlags.NonPublic);

            if (mi == null)
                throw new ArgumentException(
                    String.Format(
                        "Object type {0} does not appear to implement member function {1}.",
                        type, eventName), "eventName");

            mi.Invoke(obj, eventArgsArray);
        }

        #endregion
    }
}
