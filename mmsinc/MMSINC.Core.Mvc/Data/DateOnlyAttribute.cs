using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Utilities;

namespace MMSINC.Data
{
    public class DateOnlyAttribute : DisplayFormatAttribute
    {
        public DateOnlyAttribute()
        {
            DataFormatString = CommonStringFormats.DATE;
            ApplyFormatInEditMode = true;
        }
    }
}
