using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBP.Util.Extensions.ValidateExtensions
{
    public static class ValidateExtensions
    {

        public static bool IsEmpty(this int number)
        {

            return number == 0;
        }

        public static bool IsEmpty(this int? number)
        {
            return number.HasValue ? number.Value.IsEmpty() : false;
        }



    }
}
