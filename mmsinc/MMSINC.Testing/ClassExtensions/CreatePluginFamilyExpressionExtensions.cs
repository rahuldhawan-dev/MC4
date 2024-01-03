using Moq;
using StructureMap.Configuration.DSL.Expressions;

namespace MMSINC.Testing.ClassExtensions
{
    public static class CreatePluginFamilyExpressionExtensions
    {
        #region Exposed Methods

        /// <summary>
        /// Creates a mock object for a given interface, registers it with the container, and returns
        /// the mock instance so it can be used for testing.
        /// </summary>
        /// <typeparam name="TThingy"></typeparam>
        /// <param name="that"></param>
        /// <returns></returns>
        public static Mock<TThingy> Mock<TThingy>(this CreatePluginFamilyExpression<TThingy> that)
            where TThingy : class
        {
            var mock = new Mock<TThingy>();

            that.Use(mock.Object);

            return mock;
        }

        #endregion
    }
}
