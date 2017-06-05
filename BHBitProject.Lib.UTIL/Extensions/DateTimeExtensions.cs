using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBP.Util.Extensions.DateTimeExtensions
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// Retorna true se a data informada for maior que a data atual, desconsiderando as horas
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsGreaterThanCurrent(this DateTime date) {

            return date.Date > DateTime.Now.Date;
        }

        public static bool IsEmpty(this DateTime? date)
        {
            return date.HasValue ? date.Value.IsEmpty() : false;
        }
        public static bool IsEmpty(this DateTime data) 
        {
            return data == null || data  == DateTime.MinValue;
        }
    }
}
