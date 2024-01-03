using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSINC.Testing.ClassExtensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns an enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="butValues"></param>
        /// <returns></returns>
        public static T AnythingBut<T>(params T[] butValues) where T : struct
        {
            var enumVals = Enum.GetValues(typeof(T)).OfType<T>().ToArray();
            if (enumVals.Count() <= 1)
            {
                throw new InvalidOperationException("An enum must have more than one value in order for this to work.");
            }

            return enumVals.Except(butValues).OrderBy(x => Guid.NewGuid()).First();
        }
    }
}
