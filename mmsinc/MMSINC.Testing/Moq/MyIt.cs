using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TechTalk.SpecFlow.Configuration.AppConfig;

namespace MMSINC.Testing.Moq
{
    public static class MyIt
    {
        public static TColl ContainsTheseButNotThose<TColl, TValue>(IEnumerable<TValue> mustContain,
            IEnumerable<TValue> mustNotContain)
            where TColl : IEnumerable<TValue>
        {
            return Match.Create<TColl>(coll => mustContain.All(item => coll.Contains(item)) &&
                                               mustNotContain.All(item => !coll.Contains(item)));
        }

        public static TColl ContainsAll<TColl, TValue>(IEnumerable<TValue> mustContain)
            where TColl : IEnumerable<TValue>
        {
            return ContainsTheseButNotThose<TColl, TValue>(mustContain, new TValue[0]);
        }
    }
}
