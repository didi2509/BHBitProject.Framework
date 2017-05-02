using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHBitProject.Lib.MVC.Components
{
    public static class DropDown
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        /// <param name="dataValueField"></param>
        /// <param name="dataTextField"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static SelectList GetDropDown<T>(this IEnumerable<T> lista, string dataValueField = "Value", string dataTextField = "Text", object selectedValue = null) where T : class, new()
        {
            IEnumerable<dynamic> listaRetorno = new List<dynamic>() { new { Text = " ------ Selecione ------", Value = 0 } }.Concat(lista);

            return selectedValue == null
                ? new SelectList(listaRetorno, dataValueField, dataTextField)
                : new SelectList(listaRetorno, dataValueField, dataTextField, selectedValue);
        }
    }
}