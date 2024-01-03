using System;
using System.Collections;
using System.Linq;
using NHibernate.Transform;

namespace MMSINC.Data.NHibernate
{
    public class LambdaTransformer<TSource, TDestination> : IResultTransformer
    {
        #region Private Members

        private readonly Func<TSource, TDestination> _transform;

        #endregion

        #region Constructors

        public LambdaTransformer(Func<TSource, TDestination> transform)
        {
            _transform = transform;
        }

        #endregion

        #region Exposed Methods

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            return new RootEntityResultTransformer()
               .TransformTuple(tuple, aliases);
        }

        public IList TransformList(IList collection)
        {
            return (from object item in collection select _transform((TSource)item)).ToList();
        }

        #endregion
    }
}
