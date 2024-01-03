namespace MMSINC.DesignPatterns
{
    public abstract class Builder<TTarget>
        where TTarget : class
    {
        #region Abstract Methods

        /// <summary>
        /// Provides the abstract base method to build a new object of type
        /// TTarget.
        /// </summary>
        /// <returns>
        /// A new object of type TTarget, with any default initialized values.
        /// </returns>
        public abstract TTarget Build();

        #endregion

        #region Operators

        /// <summary>
        /// Implicit operator provided such that a specific Builder
        /// can be cast directly down to an object of type TTarget.
        /// </summary>
        /// <param name="builder">A typed Builder object.</param>
        /// <returns>An object of type TTarget.</returns>
        public static implicit operator TTarget(Builder<TTarget> builder)
        {
            return builder.Build();
        }

        #endregion
    }
}
