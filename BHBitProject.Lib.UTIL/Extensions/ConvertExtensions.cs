using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHBitProject.Lib.Util.Extensions.ConvertExtensions
{
    public static class ConvertExtensions
    {

        /// <summary>
        /// Converte uma data? para o formato String caso a mesma não seja nula e lhe permite remover as horas
        /// </summary>
        /// <param name="date"></param>
        /// <param name="removeHour"></param>
        /// <returns></returns>
        public static string GetStringValue(this DateTime? date, bool removeHour = true)
        {
            return date.HasValue ? removeHour ? date.Value.ToString().Substring(0, 10) : date.Value.ToString() : String.Empty;
        }


        /// <summary>
        /// Converte uma data para o formato String caso a mesma não seja nula e lhe permite remover as horas
        /// </summary>
        /// <param name="date"></param>
        /// <param name="removeHour"></param>
        /// <returns></returns>
        public static String GetStringValue(this DateTime date, bool removeHour = true)
        {
            return removeHour ? date.ToString().Substring(0, 10) : date.ToString() ;
        }

     
        
    }
}
