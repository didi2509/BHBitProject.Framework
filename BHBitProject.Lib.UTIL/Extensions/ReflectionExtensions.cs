using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BBP.Util.Extensions.ReflectionExtensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Diogo de Freitas Nunes
        /// Verifica se uma propriedade está contida em uma lista de propriedades
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="propertieName"></param>
        /// <returns></returns>
        public static bool IsHaveProperty(this PropertyInfo[] properties, string propertieName)
        {
            return properties != null ? properties.AsParallel().Where((p) => p.Name == propertieName).Count() > 0 : false;
        }
    }
}
