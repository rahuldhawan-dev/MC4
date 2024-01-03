using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Text.RegularExpressions;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;

namespace MMSINC.Testing.Linq
{
    [TestClass]
    public abstract class LinqUnitTestClass<TUnitType> where TUnitType : class, new()
    {
        #region Constants

        private const string VARCHAR_LENGTH_REGEX = @"char\((\d+)\)";

        #endregion

        #region Private Members

        private Table<TUnitType> _dataTable;

        protected HttpSimulator _simulator;

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Intended to provide an object of type T which as already
        /// been created, validated, and saved to the database.  This
        /// method may be overridden to add any further requirements
        /// to the object before or after it is saved.  Remember that
        /// when overriding, you must guarantee that the object gets
        /// persisted, because this class currently has no way of
        /// verifying.
        /// </summary>
        /// <returns>
        /// An instantiated and persistant object of type T.
        /// </returns>
        protected abstract TUnitType GetValidObjectFromDatabase();

        /// <summary>
        /// Intented to provide an object of type T which meets
        /// all validation requirements.  This method may be
        /// overridden to add any further values to the object
        /// to make it valid.  This method is called in
        /// GetValidObjectFromDatabase() when a persisted
        /// object does not already exist of the specific
        /// type.
        /// </summary>
        /// <returns>
        /// A "valid" object of type T.
        /// </returns>
        protected virtual TUnitType GetValidObject()
        {
            return new TUnitType();
        }

        /// <summary>
        /// Intended to provide a uniform way to delete an
        /// instantiated and saved object used in a test.
        /// This method may be overridden in the inheriting
        /// class, and should be if GetValidObject() was
        /// overridden, to clean up any extra dependant
        /// objects.
        /// </summary>
        /// <param name="entity">
        /// An object of type T to be deleted from the
        /// database.
        /// </param>
        protected abstract void DeleteObject(TUnitType entity);

        #endregion

        #region Private Methods

        protected int GetLengthFromVarChar(string attributeString)
        {
            // VarChar(10), VarChar(10) NULL / NOT NULL
            var rgx = new Regex(VARCHAR_LENGTH_REGEX, RegexOptions.IgnoreCase);
            var extractedValue = rgx.Match(attributeString).Groups[1].Value;
            return Int32.Parse(extractedValue);
        }

        private static void SetProperty(PropertyInfo propertyInfo, object value)
        {
            var t = new TUnitType();
            propertyInfo.SetValue(t, value, new object[] { });
        }

        private string GetLinqDbType(PropertyInfo propertyInfo)
        {
            object[] customAttributes = propertyInfo.GetCustomAttributes(true);

            foreach (var customAttribute in customAttributes)
            {
                if (customAttribute is ColumnAttribute)
                {
                    return ((ColumnAttribute)customAttribute).DbType;
                }
            }

            return null;

            //if (!(customAttributes[0] is System.Data.Linq.Mapping.ColumnAttribute)) continue;
            //var x =
            //    ((System.Data.Linq.Mapping.ColumnAttribute)propertyInfo.GetCustomAttributes(true)[0]).DbType;
            //return x;
        }

        //[TestMethod] // When called in inherited classes.
        //TODO: Optmize this to only go through valid LinqDBTypes, i.e. Not Text.
        public virtual void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            var propertyInfos = typeof(TUnitType).GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType != typeof(String)) continue;
                var x = GetLinqDbType(propertyInfo);
                if (x != null)
                {
                    if (!x.ToUpper().Contains("TEXT"))
                    {
                        MyAssert.Throws(
                            () =>
                                SetProperty(propertyInfo,
                                    new String('X',
                                        GetLengthFromVarChar(x) + 1)),
                            typeof(DomainLogicException),
                            String.Format(
                                "{0} Property Allows values that are too long. {1}",
                                propertyInfo.Name, GetLengthFromVarChar(x)));
                    }
                }
            }
        }

        public void TestCannotSaveWithNullProperty(IRepository<TUnitType> repository, string property)
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                var propertyInfo = target.GetType().GetProperty(property);
                propertyInfo.SetValue(target, null, new object[] { });

                MyAssert.Throws(() => repository.InsertNewEntity(target),
                    typeof(InvalidOperationException));
            }
        }

        ///*TODO: Implement method below to Test NonNull Properties are saved with Null.
        /// This would be nice. Problem is that you have to allow the property to be
        /// null. So you need to capture the OnValidate call which typically throws
        /// the exception. You would need to make the TUnitType where clause include
        /// something that exposes this method. and create a new SetProperty method
        /// that calls it.
        //[TestMethod] // When called in inherited classes.
        //public virtual void TestAllNonNullPropertiesThrowExceptionWhenNotSet()
        //{
        //    var propertyInfos = typeof (TUnitType).GetProperties();
        //    foreach(var propertyInfo in propertyInfos)
        //    {
        //        var x = GetLinqDbType(propertyInfo);
        //        if(x.ToUpper().EndsWith("NOT NULL"))
        //        {
        //            MyAssert.Throws(() => { SetProperty(propertyInfo, null);  },
        //                            typeof (NoNullAllowedException),
        //                            String.Format("{0} propertyInfo Allowed Null when NOT NULL", propertyInfo.Name));
        //        }
        //    }
        //}

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public virtual void WorkOrdersModelTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public virtual void WorkOrdersModelTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion
    }
}
