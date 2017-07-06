using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBP.MVC.Core
{
    public class AJAXResponse
    {
        public string mensagem { get; set; }
        public object dados { get; set; }


        public static JsonResult Create(string mensagem = "", object data = null)
        {

            AJAXResponse response = new AJAXResponse() { mensagem = mensagem, dados = data };
            return new JsonResult() { Data = response };
        }
    }
}