using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BBP.Util.Excel
{
    public class ExcelUtil
    {
        #region Exportacao Excel Padrão

        #region MakeHTML

        /// <summary>
        /// Nome da coluna
        /// </summary>
        /// <param name="propriedade"></param>
        /// <returns></returns>
        private static string GetNomeColuna(PropertyInfo propriedade)
        {
            Object[] atributos = propriedade.GetCustomAttributes(true).Where(a => a.GetType() == typeof(ExcelAttributes.ExportCollumAttribute)).ToArray();

            if (atributos != null && atributos.Count() > 0)
            {
                ExcelAttributes.ExportCollumAttribute atributo = atributos[0] as ExcelAttributes.ExportCollumAttribute;

                if (atributo != null && !String.IsNullOrEmpty(atributo.NomeColuna))
                    return atributo.NomeColuna;
            }

            return propriedade.Name;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="tdAttributes"></param>
        /// <param name="trAttributes"></param>
        /// <param name="tableAttributes"></param>
        /// <param name="linhasAIgnorar"></param>
        /// <param name="colunasAIgnorar"></param>
        /// <param name="trHeaderAtributes"></param>
        /// <param name="thHeaderAtributes"></param>
        /// <returns></returns>
        private static string HTMLListTable(List<Object> Lista, string tdAttributes = "", string trAttributes = "", string tableAttributes = "", int[] linhasAIgnorar = null, string trHeaderAtributes = "", string thHeaderAtributes = "")
        {
            if (Lista == null)
                return "";

            bool ExportarTudo = true;

            System.Reflection.PropertyInfo[] info = Lista[0].GetType().GetProperties();

            //Verifica se existem colunas assinadas a serem exportadas
            info.ToList().ForEach(propriedade =>
            {
                if (propriedade.GetCustomAttributes(true).Where(a => a.GetType() == typeof(ExcelAttributes.ExportCollumAttribute)).Count() > 0)
                {
                    ExportarTudo = false;
                    return;
                }
            });

            if (!ExportarTudo)
                info = info.Where(p => p.GetCustomAttributes(true).Where(a => a.GetType() == typeof(ExcelAttributes.ExportCollumAttribute)).Count() > 0).ToArray();

            //Indica se haverá a necessidade de ignorar algumas linhas
            bool ignorarLinhas = (linhasAIgnorar != null);

            //Conteúdo HTML - Abrindo a tabela
            System.Text.StringBuilder sb = new System.Text.StringBuilder("<table  " + tableAttributes + " > ");

            //Quantidade de linhas na tabela
            int linhas = Lista.Count;

            //Quantidade de colunas na tabela
            int colunas = info.Count();

            sb.Append("<tr " + trHeaderAtributes + ">");
            for (int col = 0; col < colunas; col++)
            {
                sb.Append("<th " + thHeaderAtributes + ">");
                sb.Append(GetNomeColuna(info[col]));
                sb.Append("</th>");
            }
            sb.Append("</tr>");

            //Percorrendo as linhas
            for (int i = 0; i < linhas; i++)
            {
                //Pula a linha que for marcada para não aparecer ou a que estiver com o visible = false
                if (((ignorarLinhas) && (FindValueInArray(i, linhasAIgnorar))))
                    continue;

                //Abrindo uma nova linha
                sb.Append("<tr " + trAttributes + " > ");

                //Percorrendo as colunas
                for (int c = 0; c < colunas; c++)
                {
                    //Abrindo uma nova coluna
                    sb.Append("<td " + tdAttributes + " >");

                    //Inserindo o valor da célula na coluna
                    sb.Append(info[c].GetValue(Lista[i], null));

                    //Fechando a coluna aberta
                    sb.Append("</td>");
                }

                //Fechando a linha aberta
                sb.Append("</tr>");
            }

            //Fechando a tabela
            return sb.Append("</table>").ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="tdAttributes"></param>
        /// <param name="trAttributes"></param>
        /// <param name="tableAttributes"></param>
        /// <param name="trHeaderAtributes"></param>
        /// <param name="thHeaderAtributes"></param>
        /// <param name="Properties"></param>
        /// <returns></returns>
        private static string HTMLListTable<Tipo>(List<Tipo> Lista, string tdAttributes = "", string trAttributes = "", string tableAttributes = "", string trHeaderAtributes = "", string thHeaderAtributes = "")
        {
            if (Lista == null)
                return "";

            bool ExportarTudo = true;

            System.Reflection.PropertyInfo[] Properties = Lista[0].GetType().GetProperties();

            //Verifica se existem colunas assinadas a serem exportadas
            Properties.ToList().ForEach(propriedade =>
            {
                if (propriedade.GetCustomAttributes(true).Where(a => a.GetType() == typeof(ExcelAttributes.ExportCollumAttribute)).Count() > 0)
                {
                    ExportarTudo = false;
                    return;
                }
            });

            if (!ExportarTudo)
                Properties = Properties.Where(p => p.GetCustomAttributes(true).Where(a => a.GetType() == typeof(ExcelAttributes.ExportCollumAttribute)).Count() > 0).ToArray();

            //Conteúdo HTML - Abrindo a tabela
            System.Text.StringBuilder sb = new System.Text.StringBuilder(String.Format("<table border='1' {0} > ", tableAttributes));

            //Quantidade de linhas na tabela
            int linhas = Lista.Count;

            //Quantidade de colunas na tabela
            int colunas = Properties.Length;

            sb.Append(String.Format("<thead><tr>"));
            for (int col = 0; col < colunas; col++)
            {
                sb.Append(String.Format("<th {0} " + thHeaderAtributes + ">", trHeaderAtributes));
                sb.Append(GetNomeColuna(Properties[col]));
                sb.Append("</th>");
            }
            sb.Append("</tr></thead><tbody>");

            bool colorir = false;
            string cor = "#eee";

            //Percorrendo as linhas
            for (int i = 0; i < linhas; i++)
            {

                //Colorindo linha sim linha nao
                colorir = !colorir;
                cor = colorir ? "ddd" : "fff";

                //Abrindo uma nova linha
                sb.Append(String.Format("<tr   {0} > ", trAttributes));

                //Percorrendo as colunas
                for (int c = 0; c < colunas; c++)
                {
                    //Abrindo uma nova coluna
                    sb.Append(String.Format("<td {0} style='background-color:#{1};' >", tdAttributes, cor));

                    //Inserindo o valor da célula na coluna
                    sb.Append(Properties[c].GetValue(Lista[i], null));

                    //Fechando a coluna aberta
                    sb.Append("</td>");
                }

                //Fechando a linha aberta
                sb.Append("</tr>");
            }
            //Fechando a tabela
            return sb.Append("</tbody></table>").ToString();
        }


        /// <summary>
        /// Retorna se um valor inteiro existe em um vetor de inteiros
        /// </summary>
        /// <param name="value">Valor a verificar</param>
        /// <param name="Array">Vetor de valores a ser percorrido</param>
        /// <returns></returns>
        private static bool FindValueInArray(int value, int[] Array)
        {
            int tamanhoArray = Array.Length;

            for (int i = 0; i < tamanhoArray; i++)
                if (Array[i] == value)
                    return true;

            return false;
        }


        /// <summary>
        /// Une o HTML de um vetor de strings de forma que fique uma tabela do lado da outra
        /// </summary>
        /// <param name="controls"></param>
        /// <returns></returns>
        private static string UnionHTMLOfComponents(String[] tables)
        {
            int size = tables.Length;
            StringBuilder sb = new StringBuilder("");

            if (size > 0)
            {
                sb.Append("<table><tr>");
                for (int i = 0; i < size; i++)
                    sb.Append("<td>" + tables[i] + "</td>");

                sb.Append("</tr>");
            }
            return sb.ToString();
        }
        #endregion

        #region Export


       
        /// <summary>
        /// Remove os caracteres nao permitidos para nome de arquivos e regra o seu tamanho
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <returns></returns>
        private static String NormalizarNomeArquivo(String nomeArquivo)
        {
            string novoNome = nomeArquivo.Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "");
            return novoNome.Length > 254 ? novoNome.Substring(0, 254) : novoNome;
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="FileName"></param>
        /// <param name="Response"></param>
        /// <param name="tdAttributes"></param>
        /// <param name="trAttributes"></param>
        /// <param name="tableAttributes"></param>
        /// <param name="trHeaderAtributes"></param>
        /// <param name="thHeaderAtributes"></param>
        /// <param name="Properties"></param>
        public static string ExportListToExcel<Tipo>(List<Tipo> Lista, string FileName, ConfiguracaoExportacaoExcel configuracoesExportacao = null)
        {
            if (configuracoesExportacao == null)
                configuracoesExportacao = new ConfiguracaoExportacaoExcel("style=''", "", "style='background-color:#999'", "style='gridHelperExportacaoExcelTrCabecalho'", "style='gridHelperExportacaoExcelTd'");

            return HTMLListTable<Tipo>(Lista, configuracoesExportacao.atributosTD, configuracoesExportacao.atributosTrLinha, configuracoesExportacao.atributosTable, configuracoesExportacao.atributosTrCabecalho, configuracoesExportacao.atributosTH);
        }

        #endregion

        #endregion

        public sealed class ExcelAttributes
        {
            #region Conversao DataTable

            public sealed class TipoConversaoExplicitaDataTableGridAttribute : Attribute
            {
                public enum TipoConversaoExplicita
                {
                    DateTime = 0,
                    Int16,
                    Int32,
                    Int64
                }

                public TipoConversaoExplicita tipoConversaoExplicita { get; set; }
            }

            #endregion

            #region ExportarExcelAtributos

            public sealed class ExportCollumAttribute : Attribute
            {
                public string NomeColuna { get; set; }
            }

            #endregion
        }



        #region Configuracoes Exportacao Excel

        public sealed class ConfiguracaoExportacaoExcel
        {
            /// <summary>
            /// 
            /// </summary>
            public string atributosTable { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string atributosTrLinha { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string atributosTrCabecalho { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string atributosTH { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string atributosTD { get; set; }

            public ConfiguracaoExportacaoExcel(string _atributosTable = "", string _atributosTrLinha = "", string _atributosTrCabecalho = "", string _atributosTH = "style='background-color:#aaa; font-weight:bold;' ", string _atributosTD = "")
            {
                this.atributosTable = _atributosTable;
                this.atributosTrLinha = _atributosTrLinha;
                this.atributosTrCabecalho = _atributosTrCabecalho;
                this.atributosTH = _atributosTH;
                this.atributosTD = _atributosTD;
            }
        }

        #endregion
    }
}
