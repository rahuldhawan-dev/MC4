using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Interface;
using MMSINC.Utilities;
using MMSINC.Utilities.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using Rhino.Mocks;
using CoreObjectExtensions = MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions;

namespace MMSINC.Testing.MSTest.TestExtensions
{
    public static class MyAssert // ow!
    {
        #region Constants

        public struct BaseFailureMessages
        {
            public const string EXCEPTION_NOT_THROWN =
                "Exception was not thrown when expected.";

            public const string EXCEPTION_TYPE_MISMATCH =
                "Specific expected exception type was not encountered.  ";
        }

        public struct FailureMessageFormatStrings
        {
            public const string EXCEPTION_TYPE_MISMATCH =
                "Expected: {0}; Got {1}.(more)\r\nDetail:{2}\r\nMessage:{3}";

            public const string EXCEPTION_THROWN =
                "Exception of type {0} was encountered.  {1}";
        }

        public static readonly TimeSpan DEFAULT_ARE_CLOSE_THRESHOLD = new TimeSpan(0, 0, 30);

        #endregion

        #region Assertions

        #region Assert.IsInstanceOfType

        [DebuggerStepThrough]
        public static void IsInstanceOfType<TType>(object value)
        {
            IsInstanceOfType<TType>(value, null);
        }

        [DebuggerStepThrough]
        public static void IsInstanceOfType<TType>(object value, string message)
        {
            IsInstanceOfType<TType>(value, message, null);
        }

        [DebuggerStepThrough, StringFormatMethod("message")]
        public static void IsInstanceOfType<TType>(object value, string message, params object[] parameters)
        {
            Assert.IsInstanceOfType(value, typeof(TType), message, parameters);
        }

        #endregion

        #region Assert.Throws

        [DebuggerStepThrough]
        public static void Throws<TException>(Action fn)
            where TException : Exception
        {
            Throws(fn, typeof(TException));
        }

        [DebuggerStepThrough]
        public static void Throws<TException>(Func<object> fn)
            where TException : Exception
        {
            Throws(fn, typeof(TException));
        }

        [DebuggerStepThrough]
        public static void Throws<TException>(Action fn, string message)
            where TException : Exception
        {
            Throws(fn, typeof(TException), message);
        }

        /// <summary>
        /// Useful for testing that property accessors throw exceptions
        /// </summary>
        [DebuggerStepThrough]
        public static void Throws(Func<object> fn, Type expectedExceptionType, string message = null)
        {
            Throws((Action)(() => fn()), expectedExceptionType, message);
        }

        /// <summary>
        /// Runs the specified delegate, expecting an exception to be thrown.
        /// Causes the test to fail if an exception of any type is not thrown.
        /// </summary>
        /// <param name="fn">
        ///     Action delegate which takes no arguments, expected to throw an
        ///     exception.
        /// </param>
        /// <param name="message">
        ///     Message to display if the test fails.
        /// </param>
        [DebuggerStepThrough]
        public static void Throws(Action fn, string message = null)
        {
            Throws(fn, typeof(Exception), message);
        }

        /// <summary>
        /// Runs the specified delegate, expecting an exception of a specific
        /// type to be thrown.  Causes the test to fail if an exception of the
        /// specified type is not thrown.
        /// </summary>
        /// <param name="fn">
        /// Action delegate which takes no arguments, expected to throw an
        /// exception of the specified type.
        /// </param>
        /// <param name="expectedExceptionType">
        /// Type representing the specific exception type expected.
        /// </param>
        /// <param name="message">
        /// Message to display if the test fails.
        /// </param>
        [DebuggerStepThrough]
        public static void Throws(Action fn, Type expectedExceptionType, string message = null)
        {
            var blThrown = false;
            var blTypeMatched = false;
            Exception ex = null;
            try
            {
                fn();
            }
            catch (TargetInvocationException tie)
            {
                blThrown = true;
                if (expectedExceptionType.IsInstanceOfType(tie))
                {
                    blTypeMatched = true;
                    ex = tie;
                }
                else if (tie.InnerException != null && expectedExceptionType.IsInstanceOfType(tie.InnerException))
                {
                    blTypeMatched = true;
                    ex = tie.InnerException;
                }
                else
                    ex = tie;
            }
            catch (Exception e)
            {
                blThrown = true;
                blTypeMatched = expectedExceptionType.IsInstanceOfType(e);
                ex = e;
            }

            if (!blThrown)
                throw new AssertFailedException(BaseFailureMessages.EXCEPTION_NOT_THROWN +
                                                ((message == null) ? String.Empty : "  " + message));
            // TODO: Revamp how this message is being assembled
            // "Message:" should not show if the string message argument wasn't supplied.
            // this will also need to be tested.
            if (!blTypeMatched)
                throw new AssertFailedException(BaseFailureMessages.EXCEPTION_TYPE_MISMATCH +
                                                String.Format(FailureMessageFormatStrings.EXCEPTION_TYPE_MISMATCH,
                                                    expectedExceptionType.Name,
                                                    ex.GetType().Name,
                                                    ex.Message,
                                                    ((message == null) ? String.Empty : "  " + message)
                                                )
                );
        }

        #endregion

        #region Assert.ThrowsWithMessage

        [DebuggerStepThrough]
        public static void ThrowsWithMessage<TException>(Action fn, string expectedMessage, string message = null)
            where TException : Exception
        {
            ThrowsWithMessage(fn, typeof(TException), expectedMessage, message);
        }

        /// <summary>
        /// Useful for testing that property accessors throw exceptions
        /// </summary>
        [DebuggerStepThrough]
        public static void ThrowsWithMessage(Func<object> fn, Type expectedExceptionType, string expectedMessage,
            string message = null)
        {
            ThrowsWithMessage((Action)(() => fn()), expectedExceptionType, expectedMessage, message);
        }

        /// <summary>
        /// Runs the specified delegate, expecting an exception to be thrown.
        /// Causes the test to fail if an exception of any type is not thrown.
        /// </summary>
        /// <param name="fn">
        ///     Action delegate which takes no arguments, expected to throw an
        ///     exception.
        /// </param>
        /// <param name="expectedMessage">
        /// The expected error message.
        /// </param>
        /// <param name="message">
        ///     Message to display if the test fails.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowsWithMessage(Action fn, string expectedMessage, string message = null)
        {
            ThrowsWithMessage(fn, typeof(Exception), expectedMessage, message);
        }

        /// <summary>
        /// Runs the specified delegate, expecting an exception of a specific
        /// type to be thrown.  Causes the test to fail if an exception of the
        /// specified type is not thrown.
        /// </summary>
        /// <param name="fn">
        /// Action delegate which takes no arguments, expected to throw an
        /// exception of the specified type.
        /// </param>
        /// <param name="expectedExceptionType">
        /// Type representing the specific exception type expected.
        /// </param>
        /// <param name="expectedMessage">
        /// The expected error message.
        /// </param>
        /// <param name="message">
        /// Message to display if the test fails.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowsWithMessage(Action fn, Type expectedExceptionType, string expectedMessage,
            string message = null)
        {
            var blThrown = false;
            var blTypeMatched = false;
            var blMessageMatched = false;
            Exception ex = null;
            try
            {
                fn();
            }
            catch (TargetInvocationException tie)
            {
                blThrown = true;
                if (expectedExceptionType.IsInstanceOfType(tie))
                {
                    blTypeMatched = true;
                    ex = tie;
                }
                else if (tie.InnerException != null && expectedExceptionType.IsInstanceOfType(tie.InnerException))
                {
                    blTypeMatched = true;
                    ex = tie.InnerException;
                }
                else
                {
                    ex = tie;
                }

                blMessageMatched = ex.Message == expectedMessage;
            }
            catch (Exception e)
            {
                blThrown = true;
                blTypeMatched = expectedExceptionType.IsInstanceOfType(e);
                blMessageMatched = e.Message == expectedMessage;
                ex = e;
            }

            if (!blThrown)
                throw new AssertFailedException(BaseFailureMessages.EXCEPTION_NOT_THROWN +
                                                ((message == null) ? String.Empty : "  " + message));
            // TODO: Revamp how this message is being assembled
            // "Message:" should not show if the string message argument wasn't supplied.
            // this will also need to be tested.
            if (!blTypeMatched)
                throw new AssertFailedException(BaseFailureMessages.EXCEPTION_TYPE_MISMATCH +
                                                String.Format(FailureMessageFormatStrings.EXCEPTION_TYPE_MISMATCH,
                                                    expectedExceptionType.Name,
                                                    ex.GetType().Name,
                                                    ex.Message,
                                                    ((message == null) ? String.Empty : "  " + message)
                                                )
                );

            if (!blMessageMatched)
            {
                throw new AssertFailedException(
                    $"Exception was not thrown with the message \"{expectedMessage}\". Exception was thrown with message \"{ex.Message}\".");
            }
        }

        #endregion

        #region Assert.DoesNotThrow

        // TODO:
        // These should probably be split.  Passing a Type argument means
        // you're looking for an Exception of a specific Type to not be thrown,
        // so any old Exception should not necessarily fail the test.

        /// <summary>
        /// TODO: Add Documentation.
        /// </summary>
        /// <param name="fn"></param>
        [DebuggerStepThrough]
        public static void DoesNotThrow(Action fn)
        {
            DoesNotThrow(fn, typeof(Exception));
        }

        [DebuggerStepThrough]
        public static void DoesNotThrow(Func<object> fn, string message = null, params object[] args)
        {
            DoesNotThrow((Action)(() => fn()), message, args);
        }

        [DebuggerStepThrough]
        public static void DoesNotThrow<TException>(Action fn)
            where TException : Exception
        {
            DoesNotThrow(fn, typeof(TException));
        }

        /// <summary>
        /// TODO: Add Documentation.
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="expectedExceptionType"></param>
        [DebuggerStepThrough]
        public static void DoesNotThrow(Action fn, Type expectedExceptionType)
        {
            DoesNotThrow(fn, expectedExceptionType, null);
        }

        /// <summary>
        /// TODO: Add Documentation.
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="message"></param>
        [DebuggerStepThrough, StringFormatMethod("message")]
        public static void DoesNotThrow(Action fn, string message, params object[] args)
        {
            DoesNotThrow(fn, typeof(Exception), message, args);
        }

        [DebuggerStepThrough, StringFormatMethod("message")]
        public static void DoesNotThrow<TException>(Action fn, string message, params object[] args)
        {
            DoesNotThrow(fn, typeof(TException), message, args);
        }

        /// <summary>
        /// TODO: Add Documentation.
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="expectedExceptionType"></param>
        /// <param name="message"></param>
        [DebuggerStepThrough, StringFormatMethod("message")]
        public static void DoesNotThrow(Action fn, Type expectedExceptionType, string message, params object[] args)
        {
            var blThrown = false;
            var blTypeMatched = false;
            Exception ex = null;
            try
            {
                fn();
            }
            catch (Exception e)
            {
                blThrown = true;
                blTypeMatched = expectedExceptionType.IsInstanceOfType(e);
                ex = e;
            }

            if (blThrown)
            {
                var sb = new StringBuilder();
                sb.AppendFormat(FailureMessageFormatStrings.EXCEPTION_THROWN,
                    ex.GetType(), ex.Message);
                if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                    sb.AppendFormat("\r\nInnerException: {0}: {1}",
                        ex.InnerException.GetType(), ex.InnerException.Message);
                if (!String.IsNullOrEmpty(message))
                    sb.AppendFormat("\n" + message, args);
                throw new AssertFailedException(sb.ToString());
            }
        }

        #endregion

        #region Assert.IsGreaterThan<T> and Assert.IsLessThan<T>

        [DebuggerStepThrough]
        public static void IsGreaterThanOrEqualTo<T>(T greater, T less, string message) where T : IComparable<T>
        {
            if (less.CompareTo(greater) > 0)
                throw new AssertFailedException(message);
        }

        [DebuggerStepThrough]
        public static void IsGreaterThanOrEqualTo<T>(T greater, T less) where T : IComparable<T>
        {
            IsGreaterThanOrEqualTo(greater, less,
                String.Format("Value {0} was found to be less than value {1}.", greater, less));
        }

        [DebuggerStepThrough]
        public static void IsGreaterThan<T>(T greater, T less, string message) where T : IComparable<T>
        {
            if (less.CompareTo(greater) >= 0)
                throw new AssertFailedException(message);
        }

        //TODO: Function is confusing when used.
        [DebuggerStepThrough]
        public static void IsGreaterThan<T>(T greater, T less) where T : IComparable<T>
        {
            IsGreaterThan(greater, less,
                String.Format("Value {0} was found to be less than or equal to value {1}.", greater, less));
        }

        [DebuggerStepThrough]
        public static void IsLessThan<T>(T less, T greater, string message) where T : IComparable<T>
        {
            IsGreaterThan(greater, less, message);
        }

        //TODO: Function is confusing when used.
        [DebuggerStepThrough]
        public static void IsLessThan<T>(T less, T greater) where T : IComparable<T>
        {
            IsLessThan(less, greater,
                String.Format("Value {0} was found to be greater than or equal to value {1}.", less, greater));
        }

        #endregion

        #region Assert.IsNotNullButInstanceOfType

        [DebuggerStepThrough]
        public static void IsNotNullButInstanceOfType(object target, Type ofType)
        {
            IsNotNullButInstanceOfType(target, ofType, null);
        }

        [DebuggerStepThrough]
        public static void IsNotNullButInstanceOfType(object target, Type ofType, string message)
        {
            message = (message == null) ? "" : "  " + message;
            Assert.IsNotNull(target, "Object found to be null when not expected." + message);
            Assert.IsInstanceOfType(target, ofType, message);
        }

        #endregion

        #region Assert.AreClose

        /// <summary>
        /// Default span of 1 second
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        [DebuggerStepThrough]
        public static void AreClose(DateTime left, DateTime right)
        {
            AreClose(left, right, DEFAULT_ARE_CLOSE_THRESHOLD);
        }

        [DebuggerStepThrough]
        public static void AreClose(DateTime left, DateTime right, TimeSpan threshold)
        {
            AreClose(left, right, threshold,
                String.Format(
                    "Left DateTime value {0} was not found to be within {1} of right DateTime value {2}.",
                    left, threshold, right));
        }

        [DebuggerStepThrough]
        public static void AreClose(DateTime left, DateTime right, TimeSpan threshold, string message)
        {
            if (left == right)
                return;

            var fail = (left > right)
                ? (left.Subtract(right) > threshold)
                : (right.Subtract(left) > threshold);

            if (fail)
            {
                throw new AssertFailedException(message);
            }
        }

        #endregion

        #region Assert.MethodHasAttribute<T>

        #region Does

        /// <summary>
        /// Asserts that the specified method has the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// Type of attribute that the specified method is expected to have.
        /// </typeparam>
        /// <param name="obj">
        /// Object on which to search for the method by name.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodHasAttribute<TAttribute>(object obj, string methodName)
        {
            return MethodHasAttribute(obj, typeof(TAttribute), methodName);
        }

        /// <summary>
        /// Asserts that the specified method has the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// Type of attribute that the specified method is expected to have.
        /// </typeparam>
        /// <param name="obj">
        /// Object on which to search for the method by name.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <param name="types">
        /// Params array of types representing the arguments the specified
        /// method should accept, in order.  If no type arguments are
        /// specified, the method will be assumed to take no arguments.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodHasAttribute<TAttribute>(object obj, string methodName, params Type[] types)
            where TAttribute : Attribute
        {
            return MethodHasAttribute(obj, typeof(TAttribute), methodName, types);
        }

        /// <summary>
        /// Asserts that the specified method has the specified attribute.
        /// </summary>
        /// <param name="obj">
        /// Object on which to search for the method by name.
        /// </param>
        /// <param name="attributeType">
        /// Type object representing the attribute that the specified method is
        /// expected to have.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <param name="types">
        /// Params array of types representing the arguments the specified
        /// method should accept, in order.  If no type arguments are
        /// specified, the method will be assumed to take no arguments.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodHasAttribute(object obj, Type attributeType, string methodName,
            Type[] types = null)
        {
            var objType = obj.GetType();
            types = types ?? new Type[0];
            var method = objType.GetMethod(methodName, types);

            if (null == method)
            {
                Assert.Fail(
                    "Could not find a public method '{0}' on the class '{1}' with the arguments '{2}'.",
                    methodName, objType.Name, types.TypeArrayToString());
            }

            return MethodHasAttribute(method, attributeType, methodName, objType);
        }

        /// <summary>
        /// Asserts that the specified method has the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// Type of attribute that the specified method is expected to have.
        /// </typeparam>
        /// <param name="method">
        /// MethodInfo object representing the method to be tested.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <param name="objType">
        /// Optional Type object representing the class that implements the
        /// method being tested.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodHasAttribute<TAttribute>(MethodInfo method, string methodName = null,
            Type objType = null)
            where TAttribute : Attribute
        {
            return MethodHasAttribute(method, typeof(TAttribute), methodName, objType);
        }

        /// <summary>
        /// Asserts that the specified method has the specified attribute.
        /// </summary>
        /// <param name="method">
        /// MethodInfo object representing the method to be tested.
        /// </param>
        /// <param name="attributeType">
        /// Type of attribute that the specified method is expected to have.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <param name="objType">
        /// Optional Type object representing the class that implements the
        /// method being tested.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodHasAttribute(MethodInfo method, Type attributeType, string methodName = null,
            Type objType = null)
        {
            methodName = methodName ?? method.Name;
            objType = objType ?? method.DeclaringType;

            if (!method.HasAttribute(attributeType))
            {
                Assert.Fail("Method '{0}' on class '{1}' did not have the '{2}' as expected.", methodName, objType,
                    attributeType);
            }

            return method;
        }

        /// <summary>
        /// Asserts that the specified method has the specified attribute.
        /// </summary>
        /// <param name="method">
        /// MethodInfo object representing the method to be tested.
        /// </param>
        /// <param name="valueMatcher">
        /// Func that checks if a property on the attribute matches a specific value.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <param name="objType">
        /// Optional Type object representing the class that implements the
        /// method being tested.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodHasAttribute<TAttribute>(MethodInfo method, Func<TAttribute, bool> valueMatcher,
            string methodName = null, Type objType = null)
            where TAttribute : Attribute
        {
            // Called to do the regular checking that the attribute exists.
            MethodHasAttribute(method, typeof(TAttribute), methodName, objType);
            methodName = methodName ?? method.Name;
            objType = objType ?? method.DeclaringType;

            var attr = method.GetCustomAttributes<TAttribute>().Single();
            if (!valueMatcher(attr))
            {
                Assert.Fail("Method '{0}' on class '{1}' has an '{2}' as expected but not with the correct values.",
                    methodName, objType, typeof(TAttribute));
            }

            return method;
        }

        #endregion

        #region Does Not

        /// <summary>
        /// Asserts that the specified method does not have the specified
        /// attribute.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// Type of attribute that the specified method is not expected to
        /// have.
        /// </typeparam>
        /// <param name="obj">
        /// Object on which to search for the method by name.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodDoesNotHaveAttribute<TAttribute>(object obj, string methodName)
        {
            return MethodDoesNotHaveAttribute(obj, typeof(TAttribute), methodName);
        }

        /// <summary>
        /// Asserts that the specified method does not have the specified
        /// attribute.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// Type of attribute that the specified method is not expected to
        /// have.
        /// </typeparam>
        /// <param name="obj">
        /// Object on which to search for the method by name.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <param name="types">
        /// Params array of types representing the arguments the specified
        /// method should accept, in order.  If no type arguments are
        /// specified, the method will be assumed to take no arguments.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodDoesNotHaveAttribute<TAttribute>(object obj, string methodName,
            params Type[] types)
            where TAttribute : Attribute
        {
            return MethodDoesNotHaveAttribute(obj, typeof(TAttribute), methodName, types);
        }

        /// <summary>
        /// Asserts that the specified method does not have the specified
        /// attribute.
        /// </summary>
        /// <param name="attributeType">
        /// Type of attribute that the specified method is not expected to
        /// have.
        /// </param>
        /// <param name="obj">
        /// Object on which to search for the method by name.
        /// </param>
        /// <param name="methodName">
        /// Name of the method to test.
        /// </param>
        /// <param name="types">
        /// Params array of types representing the arguments the specified
        /// method should accept, in order.  If no type arguments are
        /// specified, the method will be assumed to take no arguments.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodDoesNotHaveAttribute(object obj, Type attributeType, string methodName,
            Type[] types = null)
        {
            var objType = obj.GetType();
            var method = types == null ? objType.GetMethod(methodName) : objType.GetMethod(methodName, types);

            if (null == method)
            {
                Assert.Fail(
                    "Could not find a public method '{0}' on the class '{1}' with the arguments '{2}'.",
                    methodName, objType.Name, types.TypeArrayToString());
            }

            return MethodDoesNotHaveAttribute(method, attributeType, methodName, objType);
        }

        /// <summary>
        /// Asserts that the specified method does not have the specified
        /// attribute.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// Type of attribute that the specified method is not expected to
        /// have.
        /// </typeparam>
        /// <param name="method">
        /// MethodInfo object representing the method to be tested.
        /// </param>
        /// <param name="objType">
        /// Optional Type object representing the class that implements the
        /// method being tested.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodDoesNotHaveAttribute<TAttribute>(MethodInfo method, string methodName = null,
            Type objType = null)
            where TAttribute : Attribute
        {
            return MethodDoesNotHaveAttribute(method, typeof(TAttribute), methodName, objType);
        }

        /// <summary>
        /// Asserts that the specified method does not have the specified
        /// attribute.
        /// </summary>
        /// <param name="attributeType">
        /// Type of attribute that the specified method is not expected to
        /// have.
        /// </param>
        /// <param name="method">
        /// MethodInfo object representing the method to be tested.
        /// </param>
        /// <param name="objType">
        /// Optional Type object representing the class that implements the
        /// method being tested.
        /// </param>
        /// <returns>
        /// A MethodInfo object representing the method being tested, so that
        /// further assertions can be made/chained.
        /// </returns>
        [DebuggerStepThrough]
        public static MethodInfo MethodDoesNotHaveAttribute(MethodInfo method, Type attributeType,
            string methodName = null, Type objType = null)
        {
            methodName = methodName ?? method.Name;
            objType = objType ?? method.DeclaringType;
            if (method.HasAttribute(attributeType))
            {
                Assert.Fail("Method '{0}' on class '{1}' was found to have the '{2}' attribute unexpectedly",
                    methodName,
                    objType, attributeType);
            }

            return method;
        }

        #endregion

        #endregion

        #region Assert.StaticMethodHasAttribute<T>

        public static MethodInfo StaticMethodHasAttribute<TClass, TAttribute>(string methodName)
            where TClass : class
            where TAttribute : Attribute
        {
            var method = typeof(TClass).GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Static);

            // (MyAssert.)
            MethodHasAttribute<TAttribute>(method);

            return method;
        }

        #endregion

        #region Assert.ClassHasAttribute<T>

        [DebuggerStepThrough]
        public static void ClassHasAttribute<TAttribute>(object obj)
            where TAttribute : Attribute
        {
            ClassHasAttribute<TAttribute>(obj.GetType());
        }

        [DebuggerStepThrough]
        public static void ClassHasAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            if (!type.HasAttribute<TAttribute>())
            {
                Assert.Fail("Class '{0}' was not found to have the '{1}' as expected.", type, typeof(TAttribute));
            }
        }

        [DebuggerStepThrough]
        public static void ClassHasAttribute<TClass, TAttribute>()
            where TClass : class
            where TAttribute : Attribute
        {
            ClassHasAttribute<TAttribute>(typeof(TClass));
        }

        #endregion

        #region Assert.ClassDoesNotHaveAttribute<T>

        [DebuggerStepThrough]
        public static void ClassDoesNotHaveAttribute<TAttribute>(object obj)
            where TAttribute : Attribute
        {
            ClassDoesNotHaveAttribute<TAttribute>(obj.GetType());
        }

        [DebuggerStepThrough]
        public static void ClassDoesNotHaveAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            if (type.HasAttribute<TAttribute>())
            {
                Assert.Fail("Class '{0}' was found to have the attribute '{1}' but wasn't expected to.", type,
                    typeof(TAttribute));
            }
        }

        [DebuggerStepThrough]
        public static void ClassDoesNotHaveAttribute<TClass, TAttribute>()
            where TClass : class
            where TAttribute : Attribute
        {
            ClassDoesNotHaveAttribute<TAttribute>(typeof(TClass));
        }

        #endregion

        #region Assert.Matches

        [DebuggerStepThrough]
        public static void Matches(Regex rgx, string value, string message = null)
        {
            message = String.IsNullOrWhiteSpace(message) ? String.Empty : message + " ";
            if (!rgx.IsMatch(value))
            {
                Assert.Fail(message + "String value '{0}' does not match Regex '{1}' as expected.", value, rgx);
            }
        }

        #endregion

        #region Assert.Contains

        [StringFormatMethod("message")]
        public static void Contains(string subString, string wholeString, string message, params object[] args)
        {
            ContainsAssertion(subString, wholeString,
                String.Format(message, args));
        }

        public static void Contains(string subString, string wholeString)
        {
            Contains(subString, wholeString,
                "String '{0}' does not contain the substring '{1}'.",
                wholeString, subString);
        }

        [StringFormatMethod("message")]
        public static void DoesNotContain(string subString, string wholeString, string message, params object[] args)
        {
            ContainsAssertion(subString, wholeString,
                String.Format(message, args), false);
        }

        public static void DoesNotContain(string subString, string wholeString)
        {
            DoesNotContain(subString, wholeString,
                "String '{0}' unexpectedly contains the substring '{1}'.",
                wholeString, subString);
        }

        private static void ContainsAssertion(string subString, string wholeString, string message,
            bool shouldContain = true)
        {
            if (String.IsNullOrWhiteSpace(wholeString))
            {
                if (shouldContain)
                {
                    Assert.Fail(message);
                }

                return;
            }

            if (wholeString.Contains(subString) != shouldContain)
            {
                Assert.Fail(message);
            }
        }

        #endregion

        #region Assert.Contains

        public static void Contains<TObj>(IEnumerable coll, TObj obj, string message = null)
        {
            Contains((IEnumerable<TObj>)coll, o => o.Equals(obj),
                message ?? $"Item '{obj}' was not found within the collection as expected.");
        }

        public static void Contains<TObj>(IEnumerable<TObj> coll, TObj obj, string message = null)
        {
            Contains(coll, o => o.Equals(obj),
                message ?? $"Item '{obj}' was not found within the collection as expected.");
        }

        public static void Contains<TObj>(IEnumerable<TObj> coll, Func<TObj, bool> predicate, string message = null)
        {
            foreach (var item in coll)
            {
                if (predicate(item))
                {
                    return;
                }
            }

            Assert.Fail(message ?? $"Item matching given predicate {predicate} was not found.");
        }

        public static void DoesNotContain<TObj>(IEnumerable coll, TObj obj, string message = null)
        {
            DoesNotContain((IEnumerable<TObj>)coll, o => o.Equals(obj),
                message ?? $"Item '{obj}' was unexpectedly found within the collection.");
        }

        public static void DoesNotContain<TObj>(IEnumerable<TObj> coll, TObj obj, string message = null)
        {
            DoesNotContain(coll, o => o.Equals(obj),
                message ?? $"Item '{obj}' was unexpectedly found within the collection.");
        }

        public static void DoesNotContain<TObj>(IEnumerable<TObj> coll, Func<TObj, bool> predicate,
            string message = null)
        {
            foreach (var item in coll)
            {
                if (predicate(item))
                {
                    Assert.Fail(message ??
                                $"Item matching given predicate {predicate} was unexpectedly found within the collection.");
                }
            }
        }

        #endregion

        #region Assert.IsEmpty

        public static void IsEmpty(IEnumerable coll, string message = null)
        {
            if (coll.Any())
            {
                Assert.Fail(message ?? "Collection was expected to be empty but instead had some items.");
            }
        }

        public static void IsEmpty<TObj>(IEnumerable<TObj> coll, string message = null)
        {
            if (coll.Any())
            {
                Assert.Fail(message ?? $"Collection was expected to be empty but instead had {coll.Count()} items.");
            }
        }

        #endregion

        #region Assert.AreEqual(IEnumerable<T1>, IEnumerable<T2>, Func<T1, T2, bool>)

        public static void AreEqual<T1, T2>(IEnumerable<T1> expected, IEnumerable<T2> actual,
            Func<T1, T2, bool> comparison = null)
        {
            comparison = comparison ?? ((e, a) => e.Equals(a));
            var expectedArr = expected.ToArray();
            var actualArr = actual.ToArray();

            if (expectedArr.Length != actualArr.Length)
            {
                Assert.Fail(
                    "The lengths of the two collections did not match.  Expected had {0} items, actual had {1}.",
                    expectedArr.Length, actualArr.Length);
            }

            for (var i = 0; i < expectedArr.Length; ++i)
            {
                if (!comparison(expectedArr[i], actualArr[i]))
                {
                    Assert.Fail(
                        "Items at index {0} did not match.  Expected '{1}', actual '{2}'",
                        i, expectedArr[i], actualArr[i]);
                }
            }
        }

        #endregion

        #region Assert.AreEqual<T>(T, T, Func<T, T, bool>)

        public static void AreEqual<T>(T expected, T actual, Func<T, T, bool> fn)
        {
            if (!fn(expected, actual))
            {
                Assert.Fail(
                    "Items of type {0} expected to be equal given the presented predicate were not found to be equal by way of the presented predicate.",
                    typeof(T));
            }
        }

        #endregion

        #region Assert.AreEqual(IEnumerable<T1>, IEnumerable<T2>, Func<T1, T2, bool>)

        public static void AreNotEqual<T1, T2>(IEnumerable<T1> expected, IEnumerable<T2> actual,
            Func<T1, T2, bool> comparison = null)
        {
            comparison = comparison ?? ((e, a) => e.Equals(a));
            var expectedArr = expected.ToArray();
            var actualArr = actual.ToArray();

            if (expectedArr.Length == actualArr.Length)
            {
                for (var i = 0; i < expectedArr.Length; ++i)
                {
                    if (!comparison(expectedArr[i], actualArr[i]))
                    {
                        return;
                    }
                }

                Assert.Fail("Both collections contain the same values in the same order.");
            }
        }

        #endregion

        #region Assert.CausesIncrease(Action, Func<int>)

        public static void CausesIncrease(Action toTest, Func<int> expectedToIncrease, int expectedIncrease = 1)
        {
            var original = expectedToIncrease();
            toTest();
            var actualIncrease = expectedToIncrease() - original;
            if (actualIncrease != expectedIncrease)
            {
                Assert.Fail(
                    "Action did not cause the expected increase of {0}, actual increase was {1}.",
                    expectedIncrease, actualIncrease);
            }
        }

        public static void DoesNotCauseIncrease(Action toTest, Func<int> notExpectedToIncrease)
        {
            var original = notExpectedToIncrease();
            toTest();
            var actualIncrease = notExpectedToIncrease() - original;
            if (actualIncrease != 0)
            {
                Assert.Fail("Action caused an unexpected increase of {0}.", actualIncrease);
            }
        }

        #endregion

        #region Assert.CausesDecrease

        [DebuggerStepThrough]
        public static void CausesDecrease(Action toTest, Func<int> expectedToDecrease, int expectedDecrease = 1)
        {
            var original = expectedToDecrease();
            toTest();
            var actualDecrease = original - expectedToDecrease();
            if (actualDecrease != expectedDecrease)
            {
                Assert.Fail(
                    "Action did not cause the expected decrease of {0}, actual decrease was {1}.",
                    expectedDecrease, actualDecrease);
            }
        }

        [DebuggerStepThrough]
        public static void DoesNotCauseDecrease(Action toTest, Func<int> notExpectedToDecrease)
        {
            var original = notExpectedToDecrease();
            toTest();
            var actualDecrease = original - notExpectedToDecrease();
            if (actualDecrease != 0)
            {
                Assert.Fail("Action caused an unexpected decrease of {0}.", actualDecrease);
            }
        }

        #endregion

        #region Assert.CollectionsAreSimilar

        /// <summary>
        /// This is similar to MyAssert.AreEqual, but not the same thing. This only asserts that the two collections
        /// passed to it have the same items, but not necessarily in the same order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="bothCanBeEmpty">Set to true if both collections can be empty. Defaults to false.</param>
        public static void CollectionsAreSimilar<T>(IEnumerable<T> expected, IEnumerable<T> actual,
            bool bothCanBeEmpty = false)
        {
            // Same instances so no reason to do the rest of the test.
            if (expected == actual && bothCanBeEmpty)
            {
                return;
            }

            // ReSharper disable PossibleMultipleEnumeration
            var expectedCount = expected.Count();
            var actualCount = actual.Count();

            if (!bothCanBeEmpty && (expectedCount == 0 && actualCount == 0))
            {
                Assert.Fail(
                    "At least one collection must have at least one item in it. Expected collection count: {0}, Actual collection count: {1}",
                    expectedCount, actualCount);
            }

            Assert.AreEqual(expectedCount, actualCount,
                "Counts must be equal. Expected collection count: {0}, Actual collection count: {1}",
                expectedCount, actualCount);

            if (expected.Except(actual).Any())
            {
                var expectedNotFound = string.Join(", ", expected.Except(actual));
                var actualNotFound = string.Join(", ", actual.Except(expected));

                Assert.Fail(
                    "Differences in the collections were found. Expected collection: [{0}],   Actual collection: [{1}]",
                    expectedNotFound, actualNotFound);
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        #endregion

        #region Assert.CanGetAndSet

        public static void CanGetAndSetProperty<TObj, TMember>(TObj obj, Expression<Func<TMember>> exp,
            params TMember[] values)
        {
            CoreObjectExtensions.ThrowIfNull((object)obj, "obj");
            CoreObjectExtensions.ThrowIfNull(exp, "exp");
            if (!values.Any())
            {
                throw new ArgumentException(
                    "The values parameter can not be empty, otherwise there wouldn't be anything to test");
            }

            var member = (PropertyInfo)Expressions.GetMember(exp);
            var getter = member.GetGetMethod();
            var setter = member.GetSetMethod();

            var setterArray = new object[1];
            foreach (var curTestVal in values)
            {
                setterArray[0] = curTestVal;
                setter.Invoke(obj, setterArray);
                Assert.AreEqual(curTestVal, getter.Invoke(obj, null));
            }
        }

        /// <summary>
        /// Bool overload, no need for sending in test values. It will always check both true and false.
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exp"></param>
        public static void CanGetAndSetProperty<TObj>(TObj obj, Expression<Func<bool>> exp)
        {
            CanGetAndSetProperty(obj, exp, new[] {true, false});
        }

        #endregion

        #region Assert.StringsAreEqual(string, string)

        public static void StringsAreEqual(string expected, string actual)
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (AssertFailedException e)
            {
                int i, line = 1;
                for (i = 0; i < expected.Length; ++i)
                {
                    if (expected[i] != actual[i])
                    {
                        break;
                    }

                    if (expected[i] == '\n')
                    {
                        line++;
                    }
                }

                if (i >= expected.Length)
                {
                    i = expected.Length - 1;
                }

                throw new AssertFailedException(String.Format(
                    "Difference at line {0}, index {1}, expected '{2}', actual '{3}'.{4}{5}",
                    line, i, expected[i], actual[i], Environment.NewLine, e.Message));
            }
        }

        #endregion

        #region Assert.HasOne

        public static void HasOne<T>(IEnumerable<T> items)
        {
            HasN<T>(1, items);
        }

        public static void HasOne<T>(IEnumerable items)
        {
            HasN<T>(1, items);
        }

        #endregion

        #region Assert.HasN

        public static void HasN<T>(int expectedCount, IEnumerable<T> items)
        {
            HasN<T>(expectedCount, (IEnumerable)items);
        }

        public static void HasN<T>(int expectedCount, IEnumerable items)
        {
            Assert.IsNotNull(items);
            var result = items.OfType<T>().Count();
            if (result != expectedCount)
            {
                Assert.Fail("Expected {0} {1} but found {2}", expectedCount, typeof(T).Name, result);
            }
        }

        #endregion

        #region Assert.StreamsAreEqual(stream, stream)

        /// <summary>
        /// Asserts that two streams have identical byte arrays.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="result"></param>
        public static void StreamsAreEqual(Stream expected, Stream result)
        {
            Assert.AreEqual(expected.Length, result.Length, "Streams are not of equal length");

            var expectedByte = expected.ReadByte();
            while (expectedByte != -1)
            {
                var resultByte = result.ReadByte();

                if (expectedByte != resultByte)
                {
                    Assert.Fail("Streams have differing content.");
                }

                expectedByte = expected.ReadByte();
            }
        }

        /// <summary>
        /// Asserts that a stream has the same content as the expected byte array.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="result"></param>
        public static void StreamsAreEqual(byte[] expected, Stream result)
        {
            using (var ms = new MemoryStream(expected))
            {
                StreamsAreEqual(ms, result);
            }
        }

        #endregion

        #region Assert.IsMatch

        public static void IsMatch(Regex expected, string actual, string message = null)
        {
            Assert.IsTrue(expected.IsMatch(actual),
                message ?? $"Actual value '{actual}' did not match regex '{expected}' as expected.");
        }

        #endregion

        #region Assert.AreEqual(expected, actual, fn)

        public static void AreEqual<T, TValue>(T expected, T actual, Func<T, TValue> fn)
        {
            Assert.AreEqual(fn(expected), fn(actual));
        }

        #endregion

        [DebuggerStepThrough]
        public static void IsNotNullOrWhiteSpace(string str)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(str));
        }

        #endregion
    }

    public static class MockExtensions
    {
        #region Type-Inferred Mocking Methods

        public static MockRepository CreateMock<TContract>(this MockRepository mocks, out TContract obj)
        {
            obj = mocks.CreateMock<TContract>();
            return mocks;
        }

        public static MockRepository DynamicMock<TContract>(this MockRepository mocks, out TContract obj)
            where TContract : class
        {
            obj = mocks.DynamicMock<TContract>();
            return mocks;
        }

        public static MockRepository DynamicMultiMock<TContract>(this MockRepository mocks, out TContract obj,
            params Type[] extraTypes)
        {
            obj = mocks.DynamicMultiMock<TContract>(extraTypes);
            return mocks;
        }

        public static MockRepository Stub<TContract>(this MockRepository mocks, out TContract obj)
        {
            obj = mocks.Stub<TContract>();
            return mocks;
        }

        #endregion
    }

    /// <summary>
    /// For Static Properties
    /// </summary>
    public static class ClassExtensions
    {
        public static object GetHiddenStaticPropertyValueByName(this Type target, string fieldName)
        {
            return
                target.GetProperty(fieldName,
                           BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                      .GetValue(target, null);
        }

        public static void SetHiddenStaticFieldValueByName(this Type target, string fieldName, object value)
        {
            target.GetField(fieldName,
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).SetValue(target,
                value);

            //typeof(CrewRepository).GetField("_securityService",
            //    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).SetValue(
            //    typeof(CrewRepository), _securityService);
        }
    }

    public static class MethodExtensions
    {
        /// <summary>
        /// Asserts that the given method has the specified attribute.
        /// </summary>
        public static MethodInfo MethodHasAttribute<TAttribute>(this MethodInfo method)
            where TAttribute : Attribute
        {
            return MyAssert.MethodHasAttribute<TAttribute>(method);
        }

        /// <summary>
        /// Asserts that the given method has the specified attribute.
        /// </summary>
        public static MethodInfo MethodHasAttribute<TAttribute>(this MethodInfo method,
            Func<TAttribute, bool> valueAccessor)
            where TAttribute : Attribute
        {
            return MyAssert.MethodHasAttribute<TAttribute>(method, valueAccessor);
        }

        /// <summary>
        /// Asserts that the given method does not have the specified attribute.
        /// </summary>
        public static MethodInfo MethodDoesNotHaveAttribute<TAttribute>(this MethodInfo method)
            where TAttribute : Attribute
        {
            return MyAssert.MethodDoesNotHaveAttribute<TAttribute>(method);
        }
    }

    public static class ObjectExtensions
    {
        #region GetFieldValueByName

        public static object GetHiddenFieldValueByName(this object target, string fieldName)
        {
            return
                target
                   .GetType()
                   .GetField(fieldName,
                        BindingFlags.Instance | BindingFlags.NonPublic)
                   .GetValue(target);
        }

        #endregion

        #region SetFieldValueByName

        public static void SetHiddenFieldValueByName(this object target, string fieldName, object value)
        {
            target
               .GetType()
               .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
               .SetValue(target, value);
        }

        #endregion

        #region GetPropertyValueByName

        [Obsolete("Use MMSINC.ClassExtensions.ObjectExtensions.GetPropertyValueByName instead.")]
        public static object GetPropertyValueByName(this object target, string propName)
        {
            return
                target
                   .GetType()
                   .GetProperty(propName,
                        BindingFlags.Instance | BindingFlags.NonPublic)
                   .GetValue(target, null);
        }

        #endregion

        /// <summary>
        /// Useful for testing StructureMap registrations.
        /// </summary>
        /// <typeparam name="TExpected"></typeparam>
        /// <param name="obj"></param>
        public static void ShouldBeOfType<TExpected>(this object obj)
        {
            MyAssert.IsNotNullButInstanceOfType(obj, typeof(TExpected));
        }
    }

    public class MockUser : IUser
    {
        public IIdentity Identity
        {
            get { return null; }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IPermissionsObject CanRead(IModulePermissions perm)
        {
            throw new NotImplementedException();
        }

        public IPermissionsObject CanAdd(IModulePermissions perm)
        {
            throw new NotImplementedException();
        }

        public IPermissionsObject CanEdit(IModulePermissions perm)
        {
            throw new NotImplementedException();
        }

        public IPermissionsObject CanDelete(IModulePermissions perm)
        {
            throw new NotImplementedException();
        }

        public IPermissionsObject CanAdministrate(IModulePermissions perm)
        {
            throw new NotImplementedException();
        }
    }
}
