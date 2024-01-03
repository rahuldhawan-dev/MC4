using System;
using System.Linq.Expressions;
using Moq;

namespace MMSINC.Testing.ClassExtensions
{
    public static class MockExtensions
    {
        #region Exposed Methods
        
        /// <summary>
        /// Setup a result for the given <paramref name="expression"/> that when subsequently called on
        /// <paramref name="mock"/> will return the returned instance of mocked <typeparam name="TReturn" />.
        /// </summary>
        /// <example>
        /// Instead of:
        /// <code>
        /// var parentMock = new Mock&lt;IParentThing&gt;();
        /// var childMock = new Mock&lt;IChildThing&gt;();
        /// parentMock.Setup(x => x.GetChild()).Returns(childMock.Object);
        /// </code>
        /// simply call:
        /// <code>
        /// var parentMock = new Mock&lt;IParentThing&gt;();
        /// var childMock = parentMock.SetupMock(x => x.GetChild());
        /// </code>
        /// </example>
        public static Mock<TReturn> SetupMock<T, TReturn>(this Mock<T> mock, Expression<Func<T, TReturn>> expression)
            where T : class
            where TReturn : class
        {
            var ret = new Mock<TReturn>();

            mock.Setup(expression).Returns(ret.Object);

            return ret;
        }
        
        #endregion
    }
}
