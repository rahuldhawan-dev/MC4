using System;
using DataAnnotationsExtensions;

namespace MMSINC.Data
{
    public class MaxCurrentYearAttribute : MaxAttribute
    {
        public MaxCurrentYearAttribute() : base(DateTime.Now.Year) { }

        protected MaxCurrentYearAttribute(int max) : base(max) { }
        protected MaxCurrentYearAttribute(double max) : base(max) { }
    }
}
