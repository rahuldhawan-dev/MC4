using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class ExpressionsTest
    {
        #region Test Class

        public class SomeClass
        {
            public virtual string SomeVirtualProperty { get; set; }
            public string SomeProperty { get; set; }
            public static string SomeStaticProperty { get; set; }

            public static string SomeStaticMethod()
            {
                return null;
            }

            public static string SomeStaticMethodWithArgs(string arg)
            {
                return null;
            }

            public static void SomeStaticVoidWithArgs(string arg) { }
            public static void SomeStaticVoid() { }

            public string SomeInstanceMethod()
            {
                return null;
            }

            public bool SomeOverloadFunction()
            {
                return false;
            }

            public bool SomeOverloadFunction(bool arg)
            {
                return false;
            }

            public virtual bool SomeVirtualMethod()
            {
                return false;
            }

            private static string SomePrivateStaticProperty { get; set; }

            public static MemberInfo GetSomePrivateStaticPropertyMemberInfo()
            {
                return Expressions.GetMember(() => SomePrivateStaticProperty);
            }

            private string SomePrivateProperty { get; set; }

            public MemberInfo GetSomePrivatePropertyMemberInfo()
            {
                return Expressions.GetMember((SomeClass s) => s.SomePrivateProperty);
            }
        }

        public class SomeDerivedClass : SomeClass
        {
            public override string SomeVirtualProperty
            {
                get { return base.SomeVirtualProperty; }
                set { base.SomeVirtualProperty = value; }
            }

            public override bool SomeVirtualMethod()
            {
                return base.SomeVirtualMethod();
            }
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetMemberThrowsForNullExpressionParameter()
        {
            // Test all the overloads.
            MyAssert.Throws<ArgumentNullException>(() => Expressions.GetMember(null));
            MyAssert.Throws<ArgumentNullException>(() => Expressions.GetMember<object, object>(null));
            MyAssert.Throws<ArgumentNullException>(() => Expressions.GetMember<object>(null));
        }

        #region GetMember for properties

        #region Instance

        [TestMethod]
        public void TestGetMemberReturnsMemberInfoForAnInstanceProperty()
        {
            var result = Expressions.GetMember((SomeClass s) => s.SomeProperty);
            Assert.AreEqual("SomeProperty", result.Name);
            Assert.AreSame(typeof(SomeClass), result.ReflectedType);
        }

        [TestMethod]
        public void TestGetMemberReturnsMemberInfoForAnInstancePropertyWhenUsingTheNewKeyword()
        {
            var result = Expressions.GetMember(() => new SomeClass().SomeProperty);
            Assert.AreEqual("SomeProperty", result.Name);
            Assert.AreSame(typeof(SomeClass), result.ReflectedType);
        }

        [TestMethod]
        public void TestGetMemberWorksOnPrivateInstanceProperties()
        {
            var instance = new SomeClass();
            var result = instance.GetSomePrivatePropertyMemberInfo();
            Assert.AreEqual("SomePrivateProperty", result.Name);
        }

        public void TestGetMemberReturnsDerivedInstanceMembersOnDerivedClasses()
        {
            var result = Expressions.GetMember((SomeDerivedClass s) => s.SomeVirtualProperty);
            Assert.AreEqual("SomeVirtualProperty", result.Name);
            Assert.AreSame(typeof(SomeDerivedClass), result.ReflectedType);
        }

        #endregion

        #region Static

        [TestMethod]
        public void TestGetMemberWorksOnPrivateStaticProperties()
        {
            var result = SomeClass.GetSomePrivateStaticPropertyMemberInfo();
            Assert.AreEqual("SomePrivateStaticProperty", result.Name);
        }

        [TestMethod]
        public void TestGetMemberReturnsTheBaseStaticMemberWhenExpressionCallsStaticMemberThroughADerivedType()
        {
            // ReSharper disable AccessToStaticMemberViaDerivedType
            // ReSharper disable should be renamed "ReSharper STFU"
            var result = Expressions.GetMember(() => SomeDerivedClass.SomeStaticProperty);
            // ReSharper restore AccessToStaticMemberViaDerivedType
            Assert.AreEqual("SomeStaticProperty", result.Name);
            Assert.AreSame(typeof(SomeClass), result.ReflectedType);
        }

        #endregion

        #endregion

        #region GetMember for methods

        #region Instance

        [TestMethod]
        public void TestGetMemberReturnsMethodInfoForInstanceMethod()
        {
            var result = (MethodInfo)Expressions.GetMember((SomeClass s) => s.SomeInstanceMethod());
            Assert.AreEqual("SomeInstanceMethod", result.Name);
        }

        [TestMethod]
        public void TestGetMemberReturnsMethodInfoForDerivedInstanceMethod()
        {
            var result = (MethodInfo)Expressions.GetMember((SomeDerivedClass s) => s.SomeVirtualMethod());
            Assert.AreEqual("SomeVirtualMethod", result.Name);
            Assert.AreSame(typeof(SomeDerivedClass), result.ReflectedType);
        }

        [TestMethod]
        public void TestGetMemberReturnsCorrectMethodInfoForInstanceOverloadMethodWithNoParameters()
        {
            var result = (MethodInfo)Expressions.GetMember((SomeClass s) => s.SomeOverloadFunction());
            Assert.AreEqual("SomeOverloadFunction", result.Name);
            Assert.IsFalse(result.GetParameters().Any());
        }

        [TestMethod]
        public void TestGetMemberReturnsCorrectMethodInfoForInstanceOverloadMethodWithParameters()
        {
            var result = (MethodInfo)Expressions.GetMember((SomeClass s) => s.SomeOverloadFunction(true));
            Assert.AreEqual("SomeOverloadFunction", result.Name);
            Assert.IsTrue(result.GetParameters().Count() == 1);
            Assert.AreSame(typeof(bool), result.GetParameters().Single().ParameterType);
        }

        #endregion

        #region Static

        [TestMethod]
        public void TestGetMemberReturnsMethodInfoForStaticMethod()
        {
            var result = (MethodInfo)Expressions.GetMember(() => SomeClass.SomeStaticMethod());
            Assert.AreEqual("SomeStaticMethod", result.Name);
        }

        [TestMethod]
        public void TestGetMemberReturnsMethodInfoForStaticMethodWithParameters()
        {
            var result = (MethodInfo)Expressions.GetMember(() => SomeClass.SomeStaticMethodWithArgs(null));
            Assert.AreEqual("SomeStaticMethodWithArgs", result.Name);
        }

        [TestMethod]
        public void TestGetMemberReturnsMethodInfoForStaticVoidMethodWithParameters()
        {
            var result = (MethodInfo)Expressions.GetMember(() => SomeClass.SomeStaticVoidWithArgs(null));
            Assert.AreEqual("SomeStaticVoidWithArgs", result.Name);
        }

        #endregion

        #endregion

        #region GetSortExpression

        [TestMethod]
        public void TestGetSortExpressionReturnsNullIfNullEmptyWhiteSpace()
        {
            var result = Expressions.GetSortExpression<SomeClass>(null);
            Assert.IsNull(result);

            result = Expressions.GetSortExpression<SomeClass>("   ");
            Assert.IsNull(result);

            result = Expressions.GetSortExpression<SomeClass>(string.Empty);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetSortExpressionReturnsNullIfPropertyDoesNotExist()
        {
            var result = Expressions.GetSortExpression<SomeClass>("CookieCrisp");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetSortExpressionReturnsCorrectSortExpression()
        {
            Func<SomeClass, object> expected = x => x.SomeProperty;
            var actual = Expressions.GetSortExpression<SomeClass>("SomeProperty").Compile();

            var someClass = new SomeClass();

            Assert.AreEqual(expected.Invoke(someClass), actual.Invoke(someClass));
            Assert.AreEqual(expected.Method.ReturnType, actual.Method.ReturnType);
            expected.Method.GetParameters().EachWithIndex(
                (x, i) => Assert.AreEqual(x.GetType(), actual.Method.GetParameters()[i].GetType()));
        }

        #endregion

        #region BuildGetterExpression

        [TestMethod]
        public void TestBuildGetterExpressionDoesNotAllowAccessToPrivateProperties()
        {
            MyAssert.Throws(() => Expressions.BuildGetterExpression<SomeObject>("Internal"));
        }

        [TestMethod]
        public void TestGenericBuildGetterExpressionWithOnlyClassTypeParameterReturns_BOXED_Expression()
        {
            var obj = new SomeObject();
            obj.SimpleProperty = "a value";
            var result = Expressions.BuildGetterExpression<SomeObject>("SimpleProperty");
            Assert.IsInstanceOfType(result, typeof(Expression<Func<SomeObject, object>>));

            var func = result.Compile();
            Assert.AreEqual("a value", func(obj));
        }

        [TestMethod]
        public void TestGenericBuildGetterExpressionWithClassAndPropertyTypeParamReturns_UNBOXED_Expression()
        {
            var obj = new SomeObject();
            obj.SimpleProperty = "a value";
            var result = Expressions.BuildGetterExpression<SomeObject, string>("SimpleProperty");
            Assert.IsInstanceOfType(result, typeof(Expression<Func<SomeObject, string>>));

            var func = result.Compile();
            Assert.AreEqual("a value", func(obj));
        }

        [TestMethod]
        public void TestBuildGetterExpressionWithOnlyClassTypeParameterReturns_UNBOXED_Expression()
        {
            var obj = new SomeObject();
            obj.SimpleProperty = "a value";
            var result = Expressions.BuildGetterExpression(typeof(SomeObject), "SimpleProperty");
            Assert.IsInstanceOfType(result, typeof(Expression<Func<SomeObject, string>>));

            var func = ((Expression<Func<SomeObject, string>>)result).Compile();
            Assert.AreEqual("a value", func(obj));
        }

        [TestMethod]
        public void
            TestBuildGetterExpressionWithClassTypeAndPropertyTypeParameterReturnsExpressionThatCastsPropertyToThatSpecificPropertyType()
        {
            var obj = new SomeObject();
            obj.SimpleProperty = "a value";
            var result = Expressions.BuildGetterExpression(typeof(SomeObject), typeof(object), "SimpleProperty");
            Assert.IsInstanceOfType(result, typeof(Expression<Func<SomeObject, object>>));

            var func = ((Expression<Func<SomeObject, object>>)result).Compile();
            Assert.AreEqual("a value", func(obj));
        }

        [TestMethod]
        public void TestBuildGetterExpressionDoesNestedPropertyAccess()
        {
            var obj = new SomeObject();
            obj.ComplexProperty = new SomeObject {SimpleProperty = "Neato"};
            var result = Expressions.BuildGetterExpression<SomeObject, string>("ComplexProperty.SimpleProperty");
            Assert.IsInstanceOfType(result, typeof(Expression<Func<SomeObject, string>>));

            var func = result.Compile();
            Assert.AreEqual("Neato", func(obj));
        }

        [TestMethod]
        public void TestBuildGetterExpressionWorksWithDynamically()
        {
            var obj = new SomeObject();
            obj.SimpleProperty = "a value";
            dynamic result = Expressions.BuildGetterExpression(typeof(SomeObject), typeof(object), "SimpleProperty");
            var func = result.Compile();
            Assert.AreEqual("a value", func(obj));
        }

        #endregion

        #region BuildSetterExpression

        [TestMethod]
        public void TestBuildSetterExpression()
        {
            var obj = new SomeObject();
            var result = Expressions.BuildSet<SomeObject, string>("SimpleProperty");
            result.Compile()(obj, "i am being set");
            Assert.AreEqual("i am being set", obj.SimpleProperty);
        }

        [TestMethod]
        public void TestBuildSetterExpressionWithoutPropertyTypeParameter()
        {
            var obj = new SomeObject();
            var result = Expressions.BuildSet<SomeObject>("SimpleProperty");
            result.Compile()(obj, "i am being set");
            Assert.AreEqual("i am being set", obj.SimpleProperty);
        }

        [TestMethod]
        public void TestBuildSetterExpressionWithIncorrectPropertyTypeThrowsExceptionOnlyWhenCompiled()
        {
            var obj = new SomeObject();
            var result = Expressions.BuildSet<SomeObject>("SimpleProperty");
            var compiled = result.Compile();
            MyAssert.Throws(() => compiled(obj, 42)); // int is not valid for string prop
        }

        #endregion

        #endregion

        #region Test classes

        private class SomeObject
        {
            public string SimpleProperty { get; set; }

            public SomeObject ComplexProperty { get; set; }

            internal object Internal { get; set; }
        }

        #endregion
    }
}
