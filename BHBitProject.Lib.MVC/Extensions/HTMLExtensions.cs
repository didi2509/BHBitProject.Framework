using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BHBitProject.Lib.MVC.Extensions
{
    public static class HTMLExtensions
    {
        private static string BBPInitScript = @"
 <script type='text/javascript' src='{0}/BBP.js'></script>
    <script type = 'text/javascript'>
        $(document).ready(function() {
            BBP.init('{0}');
        });
    </script>";

        public static IHtmlString GetBBPScripts(this HtmlHelper html, string BBPPath)
        {
            return new HtmlString(String.Format(BBPInitScript, BBPPath));
        }

        /// <summary>
        /// Retorna o script com a versão mais atual de acordo com a versão do sistema
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ContentVersion(this UrlHelper url, string path)
        {
            return url.Content(path); 
        }
    }
}
