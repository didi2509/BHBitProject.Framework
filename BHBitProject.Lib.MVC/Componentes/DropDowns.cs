//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;


//namespace TaskManagerNET.UTILS.EXTENSIONS
//{
//    public static class DropDowns
//    {

//        #region Propriedades

//        public const String SELECIONE = "-- Select --";

//        #endregion

//        /// <summary>
//        /// Retorna um SelectList já configurado
//        /// </summary>
//        /// <param name="list"></param>
//        /// <param name="enumerable"></param>
//        /// <param name="campoValor"></param>
//        /// <param name="campoTexto"></param>
//        /// <param name="valorSelecionado"></param>
//        /// <returns></returns>
//        public static IEnumerable<SelectListItem> Novo(System.Collections.IEnumerable enumerable, string campoValor, string campoTexto, Object valorSelecionado = null, bool IncluirSELECIONE = true)
//        {
//            if (enumerable == null)
//                return new SelectList(new List<Char>());

//            SelectList lista = valorSelecionado == null ? new SelectList(enumerable, campoValor, campoTexto) : new SelectList(enumerable, campoValor, campoTexto, valorSelecionado);

//            return IncluirSELECIONE ? lista.Selecione() : lista;

//        }



//        /// <summary>
//        /// Adiciona o conteudo Selecione a uma lista
//        /// </summary>
//        /// <param name="lista"></param>
//        /// <returns></returns>
//        public static IEnumerable<SelectListItem> Selecione(this SelectList lista)
//        {
//            return (new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = SELECIONE, Value = string.Empty } }).Union(lista);
//        }
//    }
//}
