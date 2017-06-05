using System;
using System.Collections.Generic;
using System.Linq;
namespace BBP.Util.Extensions.CollectionExtensions
{
    public static class CollectionExtensions
    {

        /// <summary>
        /// Verifica se uma lista está vazia
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this List<T> lista)
        {
            return (lista == null) || (lista.Count < 1);
        }

        /// <summary>
        /// Verifica se a lista de strings contem ao menos um dos elementos passados como parâmetro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static bool AnyElement(this List<String> lista, params string[] elementos)
        {
            if (lista == null) return false;

            foreach (var elemento in elementos)
            {
                if (lista.Contains(elemento))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///  Retorna o primeiro objeto ou o default, caso nao encontre retorna um novo objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static T FirstOrDefaultOrObject<T>(this IEnumerable<T> enumerable, T returnIfNull)
        {
            var obj = enumerable.FirstOrDefault<T>();
            return obj != null ? obj : returnIfNull;
        }

        /// <summary>
        /// Retorna o primeiro objeto ou o default, caso nao encontre retorna um novo objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static T FirstOrDefaultOrObject<T>(this IQueryable<T> enumerable, T returnIfNull) 
        {
            var obj = enumerable.FirstOrDefault<T>();
            return obj != null ? obj : returnIfNull;
        }

    }
}
