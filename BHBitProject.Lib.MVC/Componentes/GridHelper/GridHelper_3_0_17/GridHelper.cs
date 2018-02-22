using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Helpers;
using System.Globalization;
using System.Data;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

/// 12/01/2016
/// Diogo de Freitas Nunes
/// Versão 3.1
namespace System.Web.Helpers
{

    [Serializable]
    public class BaseGridHelper
    {
        #region Propriedades

        /// <summary>
        /// Colunas do grid
        /// </summary>
        public GridColumn[] Colunas { get; set; }

        /// <summary>
        /// Colunas do grid
        /// </summary>
        public GridDetails[] Detalhes { get; set; }

        /// <summary>
        /// Atribus inseridos nas linhas
        /// </summary>
        public String AtributosLinha { get; set; }

        /// <summary>
        /// Identifica se a tabela ja foi gerada
        /// </summary>
        public bool TabelaGerada { get; protected set; }

        /// <summary>
        /// Total de registros que a lista gerada está comportando
        /// </summary>
        public int TotalListaRetorno { get; protected set; }

        /// <summary>
        /// Id da grid a ser gerada, deve ser único em um contexto
        /// </summary>
        public string IdTabela { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool GerarContadores { get; set; }

        /// <summary>
        /// Indica o registro que irá ser o inicio da paginação
        /// </summary>
        public int RegistroInicialPaginacao { get; set; }

        /// <summary>
        /// Indica o registro que será o fim da paginação
        /// </summary>
        public int RegistroFinalPaginacao { get; set; }

        /// <summary>
        /// Indica se o detalhamento deverá ser gerado
        /// </summary>
        public bool GerarDetalhamento { get; set; }

        /// <summary>
        /// Indica as colunas que deverão ser removidas na busca e na elaboração das colunas, você pode separar as mesmas por ,
        /// </summary>
        public String RemoverColunas { get; set; }

        /// <summary>
        /// Indica se é a primeira requisição a grid
        /// </summary>
        public bool PrimeiraRequisicao = true;

        /// <summary>
        /// 
        /// </summary>
        public bool MontarColunasAuto = false;

        /// <summary>
        /// 
        /// </summary>
        public String PastaTemporariaExportacoes { get; set; }

        public bool RestringirColunasPesquisa { get; set; }

        #endregion

        #region construtores

        public BaseGridHelper() { }

        public BaseGridHelper(GridHelper grid)
        {
            if (grid != null)
            {
                this.AtributosLinha = grid.AtributosLinha;
                this.Colunas = grid.Colunas;
                this.Detalhes = grid.Detalhes;
                this.GerarContadores = grid.GerarContadores;
                this.GerarDetalhamento = grid.GerarDetalhamento;
                this.IdTabela = grid.IdTabela;
                this.MontarColunasAuto = grid.MontarColunasAuto;
                this.PastaTemporariaExportacoes = grid.PastaTemporariaExportacoes;
                this.PrimeiraRequisicao = grid.PrimeiraRequisicao;
                this.RegistroFinalPaginacao = grid.RegistroFinalPaginacao;
                this.RegistroInicialPaginacao = grid.RegistroInicialPaginacao;
                this.RemoverColunas = grid.RemoverColunas;
                this.TabelaGerada = grid.TabelaGerada;
                this.TotalListaRetorno = grid.TotalListaRetorno;
            }
        }
        #endregion
    }

    /// <summary>
    /// 12/01/2016
    /// Diogo de Freitas Nunes
    /// Versão 3.1
    /// </summary>
    public sealed class GridHelper : BaseGridHelper
    {
        #region Propriedades

        #endregion

        #region Construtores

        /// <summary>
        /// Efetua o controle da Grid
        /// </summary>
        /// <param name="_TempData">TempDataDictionary utilizado no controller</param>
        /// <param name="_IdTabela">Id da grid a ser gerada, deve ser único em um contexto</param>
        /// <param name="_AtributosLinha">Atributos que todas as TR da grid irão ter</param>
        /// <param name="_RemoverColunas">Colunas a serem removidas da busca e do grid, passe o nome das propriedades da lista</param>
        /// <param name="_GerarDetalhamento">Indica se a linha detalhamento deverá ser gerada na grid</param>
        /// <param name="_Colunas">Colunas a serem geradas</param>
        private GridHelper(String _IdTabela, String _AtributosLinha = "", String _PastaTemporariaExportacoes = "Downloads", String _RemoverColunas = "", bool _GerarDetalhamento = false, bool _MontarColunasAuto = false, bool RestringirColunasPesquisa = true, GridDetails[] _Detalhes = null, params GridColumn[] _Colunas)
        {
            this.GerarDetalhamento = _GerarDetalhamento;
            this.Colunas = _Colunas;
            this.AtributosLinha = _AtributosLinha;
            this.IdTabela = _IdTabela;
            this.RemoverColunas = _RemoverColunas;
            this.MontarColunasAuto = _MontarColunasAuto;
            this.PastaTemporariaExportacoes = _PastaTemporariaExportacoes;
            this.Detalhes = _Detalhes;
            this.RestringirColunasPesquisa = RestringirColunasPesquisa;

        }


        /// <summary>
        /// Cria a grid 
        /// </summary>
        /// <param name="_IdTabela"></param>
        /// <param name="_AtributosLinha"></param>
        /// <param name="_PastaTemporariaExportacoes"></param>
        /// <param name="_RemoverColunas"></param>
        /// <param name="_GerarDetalhamento"></param>
        /// <param name="_MontarColunasAuto"></param>
        /// <param name="RestringirColunasPesquisa"></param>
        /// <param name="_Detalhes"></param>
        /// <param name="_Colunas"></param>
        /// <returns></returns>
        public static IHtmlString Create(String _IdTabela, String _AtributosLinha = "", String _PastaTemporariaExportacoes = "Downloads", String _RemoverColunas = "", bool _GerarDetalhamento = false, bool _MontarColunasAuto = false, bool RestringirColunasPesquisa = true, GridDetails[] _Detalhes = null, params GridColumn[] _Colunas)
        {
            GridHelper grid = new GridHelper(_IdTabela, _AtributosLinha, _PastaTemporariaExportacoes, _RemoverColunas, _GerarDetalhamento, _MontarColunasAuto, RestringirColunasPesquisa, _Detalhes, _Colunas);
            return new HtmlString(String.Format("<div style='display:none' id='gridData_{0}'>{1}</div>", grid.IdTabela, JsonConvert.SerializeObject(new BaseGridHelper(grid))));
        }

        public static GridHelper CreateFromBase(BaseGridHelper baseGrid)
        {
            if (baseGrid != null)
            {

                GridHelper grid = new GridHelper(baseGrid.IdTabela, baseGrid.AtributosLinha, baseGrid.PastaTemporariaExportacoes, baseGrid.RemoverColunas, baseGrid.GerarDetalhamento, baseGrid.MontarColunasAuto, baseGrid.RestringirColunasPesquisa, baseGrid.Detalhes, baseGrid.Colunas);

                grid.GerarContadores = baseGrid.GerarContadores;
                grid.PastaTemporariaExportacoes = baseGrid.PastaTemporariaExportacoes;
                grid.PrimeiraRequisicao = baseGrid.PrimeiraRequisicao;
                grid.RegistroFinalPaginacao = baseGrid.RegistroFinalPaginacao;
                grid.RegistroInicialPaginacao = baseGrid.RegistroInicialPaginacao;
                grid.RemoverColunas = baseGrid.RemoverColunas;
                grid.TabelaGerada = baseGrid.TabelaGerada;
                grid.TotalListaRetorno = baseGrid.TotalListaRetorno;

                return grid;
            }
            else return null;
        }

        #endregion

        #region Montagem do HTML da Grid ************************************************

        #region Montando o Head (Cabecalho da grid)

        private String MakeHead(System.Reflection.PropertyInfo[] propriedades, int quantidadeObjetosLista)
        {
            if (quantidadeObjetosLista == 0)
                return "<thead><tr><th scope=\"col\">Registros</th></tr></thead>";

            StringBuilder sb = new StringBuilder("<thead>");

            //Quantidade de propriedades a serem percorridas
            int quantidadePropriedades = propriedades.Length;
            int i = 0;
            int c = 0;

            sb.Append("<tr>");

            //Exibe a linha na qual se encontra o registro
            if (this.GerarContadores)
                sb.Append(String.Format("<th style='width:40px !Important;' class='{0}gridHelperColunaCabecalho gridHelperColunaCabecalho'>&nbsp;</th>", this.IdTabela));

            if (this.GerarDetalhamento)
                sb.Append(String.Format("<th onclick=\"$('.{0}').toggle('slow');\" class='gridHelperDetalhamento gridHelperDetalhamentoCabecalho{1}' scope=\"col\"></th>", GerarClasseLinha(), this.IdTabela));

            //Adicionando as colunas de acordo com as propriedades
            if (this.MontarColunasAuto)
                for (i = 0; i < quantidadePropriedades; i++)
                    sb.Append(String.Format("<th id='{0}{1}' class='{0}gridHelperColunaCabecalho gridHelperOrdenavel gridHelperColunaCabecalho'  scope=\"col\">{1}</th>", this.IdTabela, propriedades[i].Name));

            int quantidadeColunas = (this.Colunas != null) ? this.Colunas.Length : 0;

            //Adicionando as colunas adicionais
            for (c = 0; c < quantidadeColunas; c++)
                sb.Append(String.Format("<th id='{0}{1}' class='{0}gridHelperColunaCabecalho " + (this.Colunas[c].Ordenavel ? "gridHelperOrdenavel" : "") + " gridHelperColunaCabecalho' style='vertical-align: middle;{2}' scope=\"col\">{3}</th>", this.IdTabela, Colunas[c].NomeColuna, this.Colunas[c].Style, string.IsNullOrEmpty(this.Colunas[c].Title) ? "" : this.Colunas[c].Title + "&nbsp&nbsp&nbsp"));

            sb.Append("</tr>");

            //Monta a primeira linha de pesquisa por colunas
            MontarPesquisaPorColuna(sb, propriedades);
            sb.Append("</thead>");

            return sb.ToString();
        }

        #endregion

        #region Body (Corpo do Grid)

        private string MontarTBody()
        {
            return String.Format("<tbody id='{0}'>", GetIdTBody());
        }

        public string GetIdTBody()
        {
            return String.Format("gridHelperBody{0}", this.IdTabela);
        }

        /// <summary>
        /// Monta uma grid de acordo com os parâmetros passados
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <returns></returns>
        private IHtmlString MakeBody<Tipo>(IEnumerable<Tipo> lista, String RemoveColumns = "") where Tipo : class
        {
            StringBuilder sb = new StringBuilder(MontarTBody());

            if (lista.Count() == 0)
                return new MvcHtmlString(String.Format("<tbody id='gridHelperBody{0}'><tr><td>Não foram encontrados registros</td></tr></tbody>", this.IdTabela));

            Type type = typeof(Tipo) is object ? lista.ToList()[0].GetType() : typeof(Tipo);

            //Variaveis utilizadas no laço
            int linha = 0;
            int quantidadePropriedades = 0;
            int quantidadeColunas = (this.Colunas != null) ? this.Colunas.Length : 0;
            int i = 0;
            int c = 0;
            int registroCorrente = this.RegistroInicialPaginacao;

            //Filtrando as propriedades
            System.Reflection.PropertyInfo[] propriedades = String.IsNullOrEmpty(RemoveColumns) ? type.GetProperties() : type.GetProperties().Where(w => !RemoveColumns.Contains(w.Name.ToUpper())).ToArray();

            lista.AsParallel().ToList().ForEach(curObject =>
            {
                linha++;
                string idLinha = GerarIdLinha(linha);

                quantidadePropriedades = propriedades.Length;

                if (!String.IsNullOrEmpty(this.AtributosLinha))
                    sb.Append(String.Format("<tr data-tabela='{2}' name='tr___{2}___{1}' {0}>", ValueOf(curObject, curObject.GetType().GetProperties(), this.AtributosLinha, null, ""), idLinha, this.IdTabela));
                else
                    sb.Append(String.Format("<tr data-tabela='{1}' name='tr___{1}___{0}'>", idLinha, this.IdTabela));


                //Exibe a linha na qual se encontra o registro
                if (this.GerarContadores)
                    sb.Append(String.Format("<td class='gridHelperContador' name='td___{1}'>{0}</td>", registroCorrente, IdTabela + registroCorrente));

                if (this.GerarDetalhamento)
                    sb.Append(String.Format("<td data-expandido='false' data-detalhamento='{1}' class='gridHelperDetalhamento gridHelperDetalhamentoExpandir gridHelperDetalhamento{0}'><div class='gridHelperDetalhamentoDiv gridHelperDetalhamentoFechado'></div></td>", this.IdTabela, GerarIdLinha(linha)));


                //Adicionando as colunas de acordo com as propriedades
                if (this.MontarColunasAuto)
                    for (i = 0; i < quantidadePropriedades; i++)
                        sb.Append(String.Format("<td>{0}</td>", propriedades[i].GetValue(curObject, null)));

                //Adicionando as colunas adicionais
                for (c = 0; c < quantidadeColunas; c++)
                {
                    GridColumn coluna = this.Colunas[c];
                    string conteudoLinha = ValueOf(curObject, propriedades, coluna.Value, coluna.Tipo, idLinha, coluna.AplicarValueAssemblyName, coluna.AplicarValueNameSpace, coluna.AplicarValueMethodName);
                    sb.Append(String.Format("<td style='{0}'>{1}</td>", ValueOf(curObject, propriedades, coluna.Style, null, idLinha), conteudoLinha));
                }

                sb.Append("</tr>");

                if (this.GerarDetalhamento)
                    MakeDetails(ref sb, curObject, propriedades, linha);

                registroCorrente++;
            });

            sb.Append("</tbody>");

            return new MvcHtmlString(sb.ToString());
        }

        #endregion

        #region Gerando o HTML da grid Head + Body

        /// <summary>
        /// Retorna um vetor de HtmlString, a posição 0 possui TODO o HTML da tabela, a posição 1 possui apenas os registros, futuramente para uma questão de performance
        /// irá ser utilizado hora todo o HTML, hora apenas o corpo
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="lista"></param>
        /// <param name="RemoveColumns"></param>
        /// <param name="strAttributes"></param>
        /// <param name="montarTodas"></param>
        /// <param name="tabelaGerada"></param>
        /// <returns></returns>
        private IHtmlString[] MakeGrid<Tipo>(IEnumerable<Tipo> lista, String RemoveColumns = "", string strAttributes = "", bool tabelaGerada = false) where Tipo : class
        {
            if (lista == null)
            {
                this.TotalListaRetorno = 0;
                return new HtmlString[2] { new HtmlString(""), new HtmlString("") };
            }

            StringBuilder sb = new StringBuilder("");

            //Removendo as colunas solicitadas
            RemoveColumns = String.IsNullOrEmpty(RemoveColumns) ? "" : RemoveColumns.ToUpper();

            System.Reflection.PropertyInfo[] propriedades = null;

            //Filtrando as propriedades
            if (lista.Count() > 0)
                propriedades = String.IsNullOrEmpty(RemoveColumns) ? typeof(Tipo).GetProperties() : typeof(Tipo).GetProperties().Where(w => !RemoveColumns.Contains(w.Name.ToUpper())).ToArray();

            if (this.PrimeiraRequisicao)
            {
                //Iniciando a tabela
                sb.Append(String.Format("<table class='table table-striped table-bordered table-hover gridHelperTabela' id=\"{0}\" {1}>", this.IdTabela, strAttributes));

                //Montando o cabecalho
                string head = MakeHead(propriedades, lista.Count());
                sb.Append(head);
            }

            //Montando o corpo
            IHtmlString body = MakeBody(lista, RemoveColumns);
            sb.Append(body);

            //Finalizando a tabela
            if (this.PrimeiraRequisicao)
                sb.Append("</table>");

            this.PrimeiraRequisicao = false;

            lista = null;

            return new HtmlString[2]{ new MvcHtmlString(sb.ToString().Replace("<tr><td></td></tr>", "")),
                                      MvcHtmlString.Create ((body).ToString().Replace("<tr><td></td></tr>", "").Replace(MontarTBody(),"").Replace("</tbody>",""))
                                    };
        }

        #endregion

        #region Auxliares

        /// <summary>
        /// Gera um id para a linha de acordo com o id fornecido pela tabela
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        private string GerarIdLinha(int linha)
        {
            return IdTabela + linha.ToString();
        }

        /// <summary>
        /// Gera um id para a coluna de acordo com o id fornecido pela tabela
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        private string GerarIdColuna(int coluna)
        {
            return "td" + IdTabela + coluna.ToString();
        }

        /// <summary>
        /// Gera a classe da linha
        /// </summary>
        /// <returns></returns>
        private string GerarClasseLinha()
        {
            return String.Format("{0}Linha", this.IdTabela);
        }

        /// <summary>
        /// Gera o input com o filtro especifico para a coluna
        /// </summary>
        /// <param name="NomePropriedade"></param>
        /// <returns></returns>
        private string GerarFiltroEspecificoColuna(string nomeColuna, bool GerarFiltro)
        {
            return GerarFiltro ? String.Format("<input type='text' id='{0}filtro{1}' class='gridHelperColunaEspecifica gridHelperColunaEspecifica{0}' value='' />", this.IdTabela, nomeColuna) : "";
        }

        #region Montando o Detalhamento dos campos

        /// <summary>
        /// Monta os detalhamentos da propriedade
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="obj"></param>
        /// <param name="propriedades"></param>
        private void MakeDetails<Tipo>(ref StringBuilder sb, Tipo curObject, System.Reflection.PropertyInfo[] propriedades, int linha) where Tipo : class
        {
            if (this.Detalhes != null)
            {
                int quantidadeDetalhes = this.Detalhes.Length;

                //Adicionando as colunas adicionais
                if (quantidadeDetalhes > 0)
                {
                    for (int c = 0; c < quantidadeDetalhes; c++)
                    {
                        GridDetails detalhe = this.Detalhes[c];

                        string idLinha = GerarIdLinha(linha);
                        string valor = ValueOf(curObject, curObject.GetType().GetProperties(), detalhe.Detalhamento, null, idLinha, detalhe.AplicarValueAssemblyName, detalhe.AplicarValueNameSpace, detalhe.AplicarValueMethodName);
                        string addPropriedades = String.IsNullOrEmpty(detalhe.AddProperty) ? String.Empty : ValueOf(curObject, curObject.GetType().GetProperties(), detalhe.AddProperty, null, idLinha, detalhe.AplicarValueAssemblyName, detalhe.AplicarValueNameSpace, detalhe.AplicarValueMethodName);

                        sb.Append(String.Format("<tr class='gridHelperTrDetalhamento {0} {1} {5}' {6} style='display:none;'><td>{2}</td><td colspan='100%' style='{3}'>{4}</td></tr>",
                                GerarClasseLinha(),
                                idLinha,
                                String.IsNullOrEmpty(detalhe.TituloDetalhamento) ? "" : detalhe.TituloDetalhamento,
                                this.Colunas != null && (this.Colunas.Length - 1) > c ? this.Colunas[c].Style : "",
                                valor,
                                String.IsNullOrEmpty(detalhe.AddClass) ? String.Empty : detalhe.AddClass,
                                addPropriedades));
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Retorna o Valor de um campo(VALUE) de uma coluna, substituindo o valor entre chaves {} pelo valor correspondente localizado nas propriedades fornecidas como parâmetro
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propriedades"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static String ValueOf<Tipo>(Tipo obj, System.Reflection.PropertyInfo[] propriedades, string Value, GridColumn.GridColumnType tipoColuna, string idLinha, string AplicarValueAssemblyName = "", string AplicarValueNameSpace = "", string AplicarValueMethodName = "") where Tipo : class
        {
            //Reparo
            Value = Value.Replace("%7B", "{").Replace("%7D", "}");
            propriedades = propriedades.Where(w => Value.ToUpper().Contains("{" + w.Name.ToUpper() + "}")).ToArray();

            //Propriedades do laço
            int p = propriedades.Length;
            int i = 0;
            object valor = null;

            for (i = 0; i < p; i++)
            {
                try
                {
                    valor = null;

                    try
                    {
                        valor = propriedades[i].GetValue(obj, null).ToString();
                    }
                    catch (Exception) { }

                    Value = Value.Replace("{" + propriedades[i].Name + "}", valor == null ? "" : valor.ToString());
                }
                catch { continue; }
            }

            //Se o delegate foi preenchido
            if ((!string.IsNullOrEmpty(AplicarValueAssemblyName)) && (!string.IsNullOrEmpty(AplicarValueNameSpace)))
            {
                Assembly assembly = Assembly.Load(AplicarValueAssemblyName);

                MethodInfo method = assembly.GetType(AplicarValueNameSpace).GetMethod(AplicarValueMethodName);

                if (!method.IsStatic)
                    throw new Exception("Os métodos fornecidos em AplicarValue devem ser estáticos!");

                Value = method.Invoke(null, new Object[1] { Value }).ToString();
            }


            //Se o tipo da coluna é o tipo Input Text
            if ((tipoColuna != null) && (tipoColuna.type == GridColumn.GridColumnType.GridColumnTypeDescription.InputText))
            {
                Value = String.Format("<input type='text' value='{0}' {1} />", Value, tipoColuna.addProperties);
            }

            return Value.Replace("{gridHelperIdLinha}", idLinha);//aplicarFinalValue != null ? aplicarFinalValue(Value.Replace("{gridHelperIdLinha}", idLinha)) : Value.Replace("{gridHelperIdLinha}", idLinha);
        }




        /// <summary>
        /// Gera os inputs para o filtro das colunas
        /// </summary>
        /// <param name="sb"></param>
        private void MontarPesquisaPorColuna(StringBuilder sb, System.Reflection.PropertyInfo[] propriedades)
        {
            if (this.UtilizarBuscaPorFiltroIndividual() || this.MontarColunasAuto)
            {
                sb.Append(String.Format("<tr class='{0}gridHelperColunaFiltro gridHelparLinhaFiltro'>", this.IdTabela));

                if (this.GerarContadores)
                    sb.Append(String.Format("<th style='width:40px !Important;' class='{0}gridHelperColunaCabecalho gridHelperColunaCabecalho'>&nbsp;</th>", this.IdTabela));

                if (this.GerarDetalhamento)
                    sb.Append(String.Format("<td id='filtroVazio{0}' class='{0}gridHelperColunaFiltro gridHelperColunaFiltro'  scope=\"col\"></td>", this.IdTabela));

                int quantidadeColunas = (this.Colunas != null) ? this.Colunas.Length : 0;

                int quantidadePropriedades = propriedades.Length;
                int i = 0;

                //Adicionando as colunas de acordo com as propriedades
                if (this.MontarColunasAuto)
                    for (i = 0; i < quantidadePropriedades; i++)
                        sb.Append(String.Format("<td id='filtro{0}{1}' class='{0}gridHelperColunaFiltro gridHelperColunaFiltro'  scope=\"col\">{2}</td>", this.IdTabela, propriedades[i].Name, this.GerarFiltroEspecificoColuna(propriedades[i].Name, true)));

                //Adicionando as colunas adicionais
                for (int c = 0; c < quantidadeColunas; c++)
                    sb.Append(String.Format("<td id='filtro{0}{1}' class='{0}gridHelperColunaFiltro gridHelperColunaFiltro'  scope=\"col\">{2}</td>", this.IdTabela, Colunas[c].NomeColuna, this.GerarFiltroEspecificoColuna(this.Colunas[c].NomeColuna, this.Colunas[c].GerarFiltro)));

                sb.Append("</tr>");
            }
        }

        /// <summary>
        /// Indica se a Grid irá gerar os inputs de busca individuais
        /// </summary>
        /// <returns></returns>
        private bool UtilizarBuscaPorFiltroIndividual()
        {
            return ((this.Colunas != null) && (this.Colunas.Where(c => c.GerarFiltro).Count() > 0));
        }

        #endregion

        #endregion

        #region Paginacao e Filtros **********************************************

        #region Reflect

        /// <summary>
        /// Monta as colunas de forma automática
        /// </summary>
        public void GerarColunas(Type tipo)
        {
            if (this.Colunas == null || this.Colunas.Length == 0)
            {
                List<GridColumn> colunas = new List<GridColumn>();

                tipo.GetProperties().ToList().ForEach(propriedade =>
                {
                    colunas.Add(new GridColumn(propriedade.Name, propriedade.Name, "{" + propriedade.Name + "}"));
                });

                this.Colunas = colunas.ToArray();
            }
        }

        /// <summary>
        /// Filtra a lista de acordo com o filtro fornecido
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="Lista"></param>
        /// <param name="strFiltro"></param>
        /// <param name="OtimizarPesquisa"></param>
        /// <returns></returns>
        private IEnumerable<Tipo> ListaFiltrada<Tipo>(IEnumerable<Tipo> Lista, string strFiltro, string strNomeColunaFiltrar, bool OtimizarPesquisa = false) where Tipo : class
        {
            //Obtendo o tipo de dados relacionado à lista
            Type tipoLista = typeof(Tipo).GetType();

            //Array com as propriedades do tipo
            System.Reflection.PropertyInfo[] propriedadesTipo = tipoLista.GetProperties();

            if ((propriedadesTipo.Length == 0) || (tipoLista.Name.ToUpper() == "RUNTIMETYPE"))
                propriedadesTipo = Lista.First().GetType().GetProperties();

            //Array com as propriedades a consultar
            System.Reflection.PropertyInfo[] propriedadesConsultar = OtimizarPesquisa ? PropriedadesConsultar(propriedadesTipo, strFiltro) : propriedadesTipo;

            //Filtrando as colunas de acordo com o filtro
            if (this.RestringirColunasPesquisa)
            {
                List<System.Reflection.PropertyInfo> propriedadesAConsultar = new List<PropertyInfo>();
                this.Colunas.ToList().ForEach(coluna =>
                {
                    System.Reflection.PropertyInfo prop = propriedadesConsultar.Where(w => String.Compare(w.Name, coluna.NomeColuna) == 0).FirstOrDefault();
                    if (prop != null)
                        propriedadesAConsultar.Add(prop);
                });
                propriedadesConsultar = propriedadesAConsultar.ToArray();
            }

            //Filtrando as propriedades
            if (String.IsNullOrEmpty(strNomeColunaFiltrar))
            {
                //Filtrando por todas as colunas com exceção das colunas escolhidas a remover
                propriedadesConsultar = String.IsNullOrEmpty(this.RemoverColunas) ? propriedadesConsultar : propriedadesConsultar.Where(w => !this.RemoverColunas.Contains(w.Name)).ToArray();
            }
            else
            {
                //Filtrando pela coluna específica
                propriedadesConsultar = propriedadesConsultar.Where(w => w.Name == strNomeColunaFiltrar.Replace(this.IdTabela + "filtro", "")).ToArray();

            }

            //Setando o filtro
            strFiltro = strFiltro.Trim().ToUpper();

            //Filtrando a lista
            IEnumerable<Tipo> listaSaida = String.IsNullOrEmpty(strFiltro) ? Lista : Lista.AsParallel().Where(s => RetornarLinha(s, strFiltro, propriedadesConsultar)).ToList();


            Lista = null;
            propriedadesTipo = null;
            propriedadesConsultar = null;


            return listaSaida;
        }



        /// <summary>
        /// Verifica se a linha irá ser retornada na consulta
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="tipo"></param>
        /// <param name="strFiltro"></param>
        /// <param name="PropriedadesVerificar"></param>
        /// <returns></returns>
        private bool RetornarLinhaParallel<Tipo>(Tipo tipo, string strFiltro, System.Reflection.PropertyInfo[] PropriedadesVerificar) where Tipo : class
        {

            int quantidadePropriedadesVerificar = PropriedadesVerificar.Length;

            //Propriedades da query
            System.Reflection.PropertyInfo[] PropriedadesQuery = tipo.GetType().GetProperties();

            bool retorno = false;
            object valor = null;
            System.Reflection.PropertyInfo propriedadeQuery = null;
            System.Reflection.PropertyInfo PropriedadeVerificar = null;

            System.Threading.Tasks.Parallel.For(0, quantidadePropriedadesVerificar, i =>
            {
                PropriedadeVerificar = PropriedadesVerificar[i];

                //Obtendo a propriedade a ser verificada
                propriedadeQuery = PropriedadesQuery.Where(w => w.Name == PropriedadeVerificar.Name).FirstOrDefault();

                if (propriedadeQuery != null)
                {

                    //Valor da propriedade
                    valor = propriedadeQuery.GetValue(tipo, null);

                    if (valor != null)

                        if (valor.ToString().strIgualar().Contains(strFiltro.strIgualar()))
                            retorno = true;

                }
            });

            return retorno;
        }


        /// <summary>
        /// Verifica se a linha irá ser retornada na consulta
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="tipo"></param>
        /// <param name="strFiltro"></param>
        /// <param name="PropriedadesVerificar"></param>
        /// <returns></returns>
        private bool RetornarLinha<Tipo>(Tipo tipo, string strFiltro, System.Reflection.PropertyInfo[] PropriedadesVerificar) where Tipo : class
        {

            int quantidadePropriedadesVerificar = PropriedadesVerificar.Length;

            //Propriedades da query
            System.Reflection.PropertyInfo[] PropriedadesQuery = tipo.GetType().GetProperties();

            object valor = null;
            System.Reflection.PropertyInfo propriedadeQuery = null;
            System.Reflection.PropertyInfo PropriedadeVerificar = null;

            for (int i = 0; i < quantidadePropriedadesVerificar; i++)
            {
                PropriedadeVerificar = PropriedadesVerificar[i];

                //Obtendo a propriedade a ser verificada
                propriedadeQuery = PropriedadesQuery.Where(w => w.Name == PropriedadeVerificar.Name).FirstOrDefault();

                if (propriedadeQuery == null)
                    continue;

                //Valor da propriedade
                valor = propriedadeQuery.GetValue(tipo, null);

                if (valor == null)
                    continue;
                else
                {
                    if (!valor.ToString().strIgualar().Contains(strFiltro.strIgualar()))
                        continue;
                    else
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Retorna as propriedades da classe que deverão ser consultadas de acordo com o filtro passado como parâmetro
        /// </summary>
        /// <param name="propriedadesTipo">Propriedades a verificar</param>
        /// <param name="strFiltro">Filtro</param>
        /// <returns></returns>
        private System.Reflection.PropertyInfo[] PropriedadesConsultar(System.Reflection.PropertyInfo[] propriedadesTipo, string strFiltro)
        {
            List<System.Reflection.PropertyInfo> listaDePropriedades = new List<System.Reflection.PropertyInfo>();

            //Quantidade de propriedades a serem verificadas
            int quantidadePropriedades = propriedadesTipo.Length;

            //Identificando o tipo do filtro
            Type tipoFiltro = VerificarTipo(strFiltro);
            int i = 0;

            for (i = 0; i < quantidadePropriedades; i++)
            {  //Veririca se o tipo da propriedade corrente é igual ao tipo do filtro - otimização da consulta

                Type tipoPropriedade = propriedadesTipo[i].PropertyType;

                if (tipoPropriedade.FullName.Contains(RemoverNumeros(tipoFiltro.Name)))
                    listaDePropriedades.Add(propriedadesTipo[i]);
            }
            //Lista com as propriedades a ser retornada
            return listaDePropriedades.ToArray();
        }


        #endregion

        #region Montagem do HTML do GRID

        /// <summary>
        /// Aplica os filtros selecionados em uma lista, gerando uma lista resultado
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="lista"></param>
        /// <param name="filtros"></param>
        /// <returns></returns>
        public IEnumerable<Tipo> AplicarFiltrosGrid<Tipo>(IEnumerable<Tipo> lista, FiltroGrid filtros) where Tipo : class
        {
            IEnumerable<Tipo> listaRetorno = lista;
            try
            {
                //Filtrando a lista pelo campo escolhido
                listaRetorno = String.IsNullOrEmpty(filtros.strFiltro) ? lista : ListaFiltrada<Tipo>(lista, filtros.strFiltro, filtros.strNomeColunaFiltrar, false);

                //Ordenação
                if ((!String.IsNullOrEmpty(filtros.strNomeCampoOrdenar)) && (!String.IsNullOrEmpty(this.IdTabela)))
                {
                    if (filtros.tipoOrdenacao == "0")
                        listaRetorno = listaRetorno.AsParallel().OrderBy(o => o.GetType().GetProperties().Where(w => w.Name == filtros.strNomeCampoOrdenar.Replace(this.IdTabela, "")).FirstOrDefault().GetValue(o, null));
                    else
                        listaRetorno = listaRetorno.AsParallel().OrderByDescending(o => o.GetType().GetProperties().Where(w => w.Name == filtros.strNomeCampoOrdenar.Replace(this.IdTabela, "")).FirstOrDefault().GetValue(o, null));
                }
            }
            catch (Exception) { }
            return listaRetorno;
        }

        public IHtmlString[] GerarHTMLGrid<Tipo>(IEnumerable<Tipo> lista, FiltroGrid filtros, string strAttributes = "") where Tipo : class
        {

            //Armazenando o objeto corrento no TempData
            //this.TempData[filtros.IdTabela] = this;

            //Indica se é a primeira requisição
            this.PrimeiraRequisicao = filtros.PrimeiraRequisicao;

            //Filtrando a lista de acordo com os filtros escolhidos
            IEnumerable<Tipo> listaRetorno = AplicarFiltrosGrid(lista, filtros);

            //Obtendo o total gerado
            this.TotalListaRetorno = filtros.TotalRegistros.HasValue ? filtros.TotalRegistros.Value : (lista == null ? 0 : listaRetorno.Count());

            if (!filtros.TotalRegistros.HasValue)
            {

                //Quantidade de registros por pagina
                int quantidadeRegistrosPagina = 0;

                if (IsInt32(filtros.strQuantidadeRegistros))
                    quantidadeRegistrosPagina = Convert.ToInt32(filtros.strQuantidadeRegistros);

                //Montando a paginação da grid
                int irParaPagina = IsInt32(filtros.strIrParaPagina) ? Convert.ToInt32(filtros.strIrParaPagina) : 1;

                //Quantidade de registros a ignorar
                int pular = (irParaPagina - 1) * quantidadeRegistrosPagina;

                //Registro inicial da paginação
                this.RegistroInicialPaginacao = pular + 1;
                this.RegistroFinalPaginacao = pular + quantidadeRegistrosPagina;

                if ((quantidadeRegistrosPagina > 0) && (TotalListaRetorno > 0))
                    listaRetorno = listaRetorno.Skip(pular).Take(quantidadeRegistrosPagina).ToList();
            }
            //Posição 0 possui todo o HTML, posição 1 possui apenas o <tbody>
            IHtmlString[] HTML;

            //Obtendo o HTML
            HTML = MakeGrid(listaRetorno, this.RemoverColunas, strAttributes, this.TabelaGerada);   //HTML.Replace("<table id=\"sample_2\" class=\"table table-striped table-bordered table-hover dataTable\" aria-describedby=\"sample_2_info\">", "").Replace("</table>", "");

            this.TabelaGerada = true;

            return HTML;
        }




        #endregion

        #region Utils

        /// <summary>
        /// Remove os numeros de uma string
        /// </summary>
        /// <param name="strEntrada"></param>
        /// <returns></returns>
        public string RemoverNumeros(string strEntrada)
        {
            int tamanhoEndtrada = strEntrada.Length;

            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            for (int i = 0; i < tamanhoEndtrada; i++)

                if ((strEntrada[i] != '0') && (strEntrada[i] != '4') && (strEntrada[i] != '7') &&
                     (strEntrada[i] != '1') && (strEntrada[i] != '5') && (strEntrada[i] != '8') &&
                     (strEntrada[i] != '2') && (strEntrada[i] != '6') && (strEntrada[i] != '9') &&
                     (strEntrada[i] != '3')
                   )

                    sb.Append(strEntrada[i]);

            return sb.ToString();
        }

        /// <summary>
        /// Verifica se um valor pode ser um inteiro
        /// </summary>
        /// <param name="strSupostInt"></param>
        /// <returns></returns>
        public bool IsInt32(string strSupostInt)
        {

            int strInt = 0;
            return Int32.TryParse(strSupostInt, out strInt);

        }

        /// <summary>
        /// Verifica se uma string pode ser convertida para o tipo datetime
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <returns></returns>
        public bool IsDateTime(string strDateTime)
        {
            DateTime date = DateTime.Now;
            return DateTime.TryParse(strDateTime, out date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDouble"></param>
        /// <returns></returns>
        public bool IsDouble(string strDouble)
        {
            Double doubleValue = 0;
            return Double.TryParse(strDouble, out doubleValue);
        }


        public Type VerificarTipo(string strTipo)
        {
            if (IsInt32(strTipo))
                return typeof(Int32);
            else if (IsDouble(strTipo))
                return typeof(Double);
            else if (IsDateTime(strTipo))
                return typeof(DateTime);
            else return
                typeof(String);
        }

        #endregion


        #endregion

        #region Exportacao Excel Padrão

        #region MakeHTML

        /// <summary>
        /// Nome da coluna
        /// </summary>
        /// <param name="propriedade"></param>
        /// <returns></returns>
        private static string GetNomeColuna(PropertyInfo propriedade)
        {
            Object[] atributos = propriedade.GetCustomAttributes(true).Where(a => a.GetType() == typeof(GridAttributes.ExportCollumAttribute)).ToArray();

            if (atributos != null && atributos.Count() > 0)
            {
                GridAttributes.ExportCollumAttribute atributo = atributos[0] as GridAttributes.ExportCollumAttribute;

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
                if (propriedade.GetCustomAttributes(true).Where(a => a.GetType() == typeof(GridAttributes.ExportCollumAttribute)).Count() > 0)
                {
                    ExportarTudo = false;
                    return;
                }
            });

            if (!ExportarTudo)
                info = info.Where(p => p.GetCustomAttributes(true).Where(a => a.GetType() == typeof(GridAttributes.ExportCollumAttribute)).Count() > 0).ToArray();

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
                if (propriedade.GetCustomAttributes(true).Where(a => a.GetType() == typeof(GridAttributes.ExportCollumAttribute)).Count() > 0)
                {
                    ExportarTudo = false;
                    return;
                }
            });

            if (!ExportarTudo)
                Properties = Properties.Where(p => p.GetCustomAttributes(true).Where(a => a.GetType() == typeof(GridAttributes.ExportCollumAttribute)).Count() > 0).ToArray();

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
        /// Gera o arquivo CSV no servidor, se o mesmo foi gerado com SUCESSO retorna o caminho no formato HTTP do mesmo, se não retorna uma string vazia (String.Empty)
        /// Cuidado com nomes grandes, o limite é 250 caracteres
        /// </summary>
        /// <param name="conteudoCSV">Todo o conteúdo do CSV (campos separados por ;)</param>
        /// <param name="nomeArquivo">Nome do arquivo no Servidor</param>
        /// <param name="Server">Server Corrente</param>
        /// <param name="Request">Request Corrente</param>
        /// <param name="colocarDataHora">Indica se a data e hora em que o arquivo foi gerado deverá ser colocada</param>
        /// <param name="colocarMilisegundos">Indica se os milisegundos deverão ser colocados</param>
        /// <returns></returns>
        private static String GerarArquivo(string conteudoCSV, string nomeArquivo, HttpServerUtilityBase Server, HttpRequestBase Request, string pastaDownload, bool colocarDataHora = true, bool colocarMilisegundos = true)
        {
            try
            {

                //Adiciona os milisegundos ao nome do arquivo
                string miliSegundos = colocarMilisegundos ? "_" + DateTime.Now.Millisecond.ToString() : "";

                //Adiciona a data/hora em que o arquivo foi gerado
                string dataHora = colocarDataHora ? "_" + DateTime.Now.ToString().Replace(" ", "_").Replace("/", "-").Replace(":", "-") : "";

                //Nome do arquivo
                string strArquivo = NormalizarNomeArquivo(nomeArquivo + dataHora + miliSegundos + ".xls");

                //Caminho completo do arquivo
                string strCaminhoCompleto = Server.MapPath("~/" + pastaDownload + "/" + strArquivo);

                if (!Directory.Exists(Server.MapPath("~/" + pastaDownload)))
                    Directory.CreateDirectory(Server.MapPath("~/" + pastaDownload));

                //Se o arquivo ja existir o mesmo é apagado e criado novamente
                if (System.IO.File.Exists(strCaminhoCompleto))
                {
                    System.IO.File.Delete(strCaminhoCompleto);
                    System.IO.File.Create(strCaminhoCompleto).Close();
                }

                //Grava o conteudo do CSV no arquivo gerado
                System.IO.File.WriteAllText(strCaminhoCompleto, conteudoCSV, Encoding.UTF8);

                //Se o arquivo existir retorna o endereço do mesmo no formato HTTP, se não retorna vazio
                return System.IO.File.Exists(strCaminhoCompleto) ? RaizURL(Request) + pastaDownload + "/" + strArquivo : String.Empty;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

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
        /// Retorna a URL Raiz corrente no formato HTTP
        /// </summary>
        /// <param name="Request">Request corrente</param>
        /// <returns></returns>
        private static String RaizURL(HttpRequestBase Request)
        {
            string URL = Request.Url.AbsoluteUri;

            int contadorBarra = 0;
            int tamanhoURL = URL.Length;

            StringBuilder sbRetorno = new StringBuilder("");

            for (int i = 0; i < tamanhoURL; i++)
            {
                if (URL[i] == '/')
                    contadorBarra++;

                //Se todas as barras ja foram encontradas
                if (contadorBarra == 3)
                    break;

                sbRetorno.Append(URL[i]);
            }

            return String.IsNullOrEmpty(sbRetorno.ToString()) ? String.Empty : sbRetorno.ToString() + "/";
        }

        /// <summary>
        /// Exporta um conteúdo para Excel, seja ele texto ou HTML
        /// </summary>
        /// <param name="FileName">Nome do arquivo (não precisa de colocar a extensão)</param>
        /// <param name="HTMLorTextContent">Conteúdo</param>
        /// <param name="Response">Response utilizado na página</param>
        private static string ExportStringToExcel(string FileName, string HTMLorTextContent, String Path, System.Web.HttpServerUtilityBase Server, System.Web.HttpRequestBase request)
        {
            return GerarArquivo(HTMLorTextContent, "Excel", Server, request, Path);

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
        public static string ExportListToExcel<Tipo>(List<Tipo> Lista, string FileName, String Path, System.Web.HttpServerUtilityBase Server, System.Web.HttpRequestBase request, ConfiguracaoExportacaoExcel configuracoesExportacao = null)
        {
            if (configuracoesExportacao == null)
                configuracoesExportacao = new ConfiguracaoExportacaoExcel("style=''", "", "style='background-color:#999'", "style='gridHelperExportacaoExcelTrCabecalho'", "style='gridHelperExportacaoExcelTd'");

            return ExportStringToExcel(FileName, HTMLListTable<Tipo>(Lista, configuracoesExportacao.atributosTD, configuracoesExportacao.atributosTrLinha, configuracoesExportacao.atributosTable, configuracoesExportacao.atributosTrCabecalho, configuracoesExportacao.atributosTH), Path, Server, request);
        }

        #endregion

        #endregion

    }
    #region Classes Aninhadas



    #region Detalhes Grid

    /// <summary>
    /// Detalhes do Grid
    /// </summary>
    public class GridDetails
    {
        public string TituloDetalhamento { get; set; }
        public string Detalhamento { get; set; }

        public string AddClass { get; set; }
        public string AddProperty { get; set; }

        public string AplicarValueNameSpace { get; set; }
        public string AplicarValueMethodName { get; set; }
        public string AplicarValueAssemblyName { get; set; }

        public GridDetails() { }

        public GridDetails(string tituloDetalhamento, string detalhamento, Func<String, String> aplicarValue = null)
        {
            this.TituloDetalhamento = tituloDetalhamento;
            this.Detalhamento = detalhamento;

            if (aplicarValue != null)
            {
                this.AplicarValueAssemblyName = aplicarValue.Method.ReflectedType.Assembly.GetName().Name;
                this.AplicarValueNameSpace = aplicarValue.Method.ReflectedType.FullName;
                this.AplicarValueMethodName = aplicarValue.Method.Name;
            }
        }

    }

    #endregion

    #region Colunas Grid

    /// <summary>
    /// Colunas do Grid Helper
    /// </summary>
    public sealed class GridColumn
    {
        /// <summary>
        /// Tipo da coluna
        /// </summary>
        public sealed class GridColumnType
        {
            /// <summary>
            /// Descricao do tipo
            /// </summary>
            public enum GridColumnTypeDescription
            {
                Text = 0,
                InputText
            }

            public GridColumnTypeDescription type { get; set; }
            public string addProperties { get; set; }
        }

        /// <summary>
        /// Tipo da coluna
        /// </summary>
        public GridColumnType Tipo { get; set; }

        /// <summary>
        /// Titulo da coluna
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nome da propriedade vinculada a esta coluna
        /// </summary>
        public string NomeColuna { get; set; }

        /// <summary>
        /// Valor da propriedade - Onde você passar o nome da propriedade será substituído pelo valor da mesma
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Monta o detalhamento para o campo
        /// </summary>
        public string Detalhamento { get; set; }

        /// <summary>
        /// Monta o detalhamento para o campo
        /// </summary>
        public string TituloDetalhamento { get; set; }

        /// <summary>
        /// Estilo inline da coluna
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Indica se a coluna deverá possuir um filtro específico
        /// </summary>
        public bool GerarFiltro { get; set; }

        /// <summary>
        /// Indica se a coluna será ordenável
        /// </summary>
        public bool Ordenavel { get; set; }

        /// <summary>
        /// Aplica este método no campo Value caso informado
        /// </summary>
        public string AplicarValueNameSpace { get; set; }
        public string AplicarValueMethodName { get; set; }
        public string AplicarValueAssemblyName { get; set; }

        public GridColumn() { }

        /// <summary>
        /// Construtor Indicado para se gerar a coluna
        /// </summary>
        /// <param name="titulo">Título que irá aparecer para a coluna no Grid</param>
        /// <param name="nomePropriedadeCorrespondente">Nome da propriedade correspondente a coluna, irá servir para filtro e ordenação, colocar o nome correspondente da fonte de dados</param>
        /// <param name="valor">Valor da coluna</param>
        /// <param name="tituloDetalhamento">Se houver detalhamento informar o titulo do mesmo</param>
        /// <param name="detalhamento">Valor do detalhamento</param>
        /// <param name="estiloInLine">Estilo inline para a TD </param>
        /// <param name="gerarFiltro">Indica se o filtro individual deverá ser gerado para esta coluna</param>
        public GridColumn(string titulo, string nomePropriedadeCorrespondente, string valor, string estiloInLine = "", bool gerarFiltro = false, Func<String, String> aplicarValue = null, bool ordenavel = true, GridColumnType tipoColuna = null)
        {
            this.Title = titulo;
            this.NomeColuna = nomePropriedadeCorrespondente;
            this.Value = valor;
            this.Style = estiloInLine;
            this.GerarFiltro = gerarFiltro;
            this.Ordenavel = ordenavel;
            this.Tipo = tipoColuna != null ? tipoColuna : new GridColumnType() { type = GridColumnType.GridColumnTypeDescription.Text };

            if (aplicarValue != null)
            {
                this.AplicarValueAssemblyName = aplicarValue.Method.ReflectedType.Assembly.GetName().Name;
                this.AplicarValueNameSpace = aplicarValue.Method.ReflectedType.FullName;
                this.AplicarValueMethodName = aplicarValue.Method.Name;
            }

        }

        /// <summary>
        /// Cria um novo tipo de coluna do tipo input
        /// </summary>
        /// <param name="propriedadesAdicionais"></param>
        /// <returns></returns>
        public static GridColumnType TipoInput(string propriedadesAdicionais = "")
        {
            return new GridColumnType() { type = GridColumnType.GridColumnTypeDescription.InputText, addProperties = propriedadesAdicionais };
        }

    }

    #endregion

    #endregion

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

namespace System.Web
{
    #region Métodos de extensão

    public sealed class GridAttributes
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


    /// <summary>
    /// Método de extensão para os controllers
    /// </summary>
    public static class ExtensoesGrid
    {
        /// <summary>
        /// Coloca a string em um padrão como para comparação
        /// </summary>
        /// <returns></returns>
        public static String strIgualar(this String strCorrente, bool removerEspacos = false)
        {
            if (String.IsNullOrEmpty(strCorrente))
                return strCorrente;

            return removerEspacos ? strCorrente.Replace(" ", "").strRemoveAccents().ToUpper() : strCorrente.Trim().strRemoveAccents().ToUpper();
        }

        /// <summary>
        /// Remove a acentuação de uma String
        /// </summary>
        /// <param name="text"></param>
        /// <returns>String</returns>
        public static string strRemoveAccents(this string text)
        {
            if (text != null)
            {
                StringBuilder sbReturn = new StringBuilder();
                var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                        sbReturn.Append(letter);
                }
                return sbReturn.ToString();
            }

            return String.Empty;
        }

        /// <summary>
        /// Método responsável por gerar o objeto de retorno JSON que irá efetuar a comunicação com a grid
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="controller"></param>
        /// <param name="filtros"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        [HttpPost]
        public static JsonResult GerarGrid<Tipo>(this System.Web.Mvc.Controller controller, FiltroGrid filtros, IEnumerable<Tipo> lista, ConfiguracaoExportacaoExcel configuracoesExportacao = null, Func<FiltroGrid, IEnumerable<Tipo>> listaPesquisa = null) where Tipo : class
        {
            //Objeto de retorno
            JsonResult json = new JsonResult();

            BaseGridHelper baseGrid = JsonConvert.DeserializeObject<BaseGridHelper>(filtros.baseGrid);
            GridHelper grid = GridHelper.CreateFromBase(baseGrid);

            //Se MontarColunasAuto == true
            grid.GerarColunas(typeof(Tipo));

            if (!String.IsNullOrEmpty(filtros.strFiltro) && listaPesquisa != null)
            {
                filtros.TotalRegistros = null;
                lista = listaPesquisa(filtros);
            }

            grid.GerarContadores = filtros.esconderContadorIrPara.ToLower() == "false";

            //Exportação para Excel, ainda nao está pronto
            if ((filtros.GerarExcel) && (controller.Response != null))
            {
                List<String> listaCampos = new List<string>();
                lista = grid.AplicarFiltrosGrid(lista, filtros);

                typeof(Tipo).GetProperties().ToList().ForEach(prop => listaCampos.Add(prop.Name));
                if (listaCampos.Count == 0)
                    lista.First().GetType().GetProperties().ToList().ForEach(prop => listaCampos.Add(prop.Name));

                string caminho = GridHelper.ExportListToExcel(lista.ToList<Tipo>(), "Exportacao_", grid.PastaTemporariaExportacoes, controller.Server, controller.Request, configuracoesExportacao);

                json.Data = new
                {
                    GerarExcel = true,
                    Caminho = caminho,
                    baseGrid = JsonConvert.SerializeObject(new BaseGridHelper(grid))
                };

                // return new FilePathResult(caminho, "application/vnd.ms-excel");
            }
            else
            {
                //Obtendo o HTML de retorno
                IHtmlString[] HTML = grid.GerarHTMLGrid<Tipo>(lista, filtros, "");

                //Montando o objeto de requisição
                json.Data = new
                {
                    htmlGrid = HTML[0].ToString(),
                    htmlCabecalhoCorpo = HTML[1].ToString(),
                    totalRegistros = grid.TotalListaRetorno,
                    registroInicial = grid.RegistroInicialPaginacao,
                    registroFinal = grid.RegistroFinalPaginacao,
                    idTabela = grid.IdTabela,
                    GerarExcel = false,
                    idTBody = grid.GetIdTBody(),
                    baseGrid = JsonConvert.SerializeObject(new BaseGridHelper(grid))
                };

                grid.PrimeiraRequisicao = false;
            }
            return json;
        }


        /// <summary>
        /// Método responsável por gerar o objeto de retorno JSON que irá efetuar a comunicação com a grid
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="controller"></param>
        /// <param name="filtros"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        [HttpPost]
        public static JsonResult GerarGrid<Tipo>(this System.Web.Mvc.Controller controller, FiltroGrid filtros, DataTable dtLista, ConfiguracaoExportacaoExcel configuracoesExportacao = null) where Tipo : class, new()
        {
            return GerarGrid<Tipo>(controller, filtros, lista: dtLista.ToList<Tipo>(), configuracoesExportacao: configuracoesExportacao);
        }




        /// <summary>
        /// Efetua a conversão de um datatable em uma lita de um tipo informado
        /// </summary>
        /// <typeparam name="Tipo">Tipo no qual se deseja realizar a conversão</typeparam>
        /// <param name="dataTable">DataTable de entrada</param>
        /// <returns>Lista do tipo informado</returns>
        public static List<Tipo> ToList<Tipo>(this System.Data.DataTable dataTable) where Tipo : class, new()
        {
            //Nova lista
            List<Tipo> lista = new List<Tipo>();

            //Tipo
            Type tipo = typeof(Tipo);

            //Obtendo apenas as propriedades do Tipo presentes no DataTable
            System.Reflection.PropertyInfo[] propriedades = tipo.GetProperties().Where(w => PropriedadeExiste(w.Name, dataTable.Columns)).ToArray();

            //Quantidade de registros
            int quantidadePropriedades = propriedades.Count();

            //Quantidade de linhas
            int quantidadeLinhas = dataTable.Rows.Count;

            bool IsPropriedadesConversaoExplicita = false;

            //Verifica se existe ao menos uma propriedade com conversão explícitia
            propriedades.ToList().ForEach(propriedade =>
            {
                if (propriedade.GetCustomAttributes(true).Where(w => w.GetType() == typeof(GridAttributes.TipoConversaoExplicitaDataTableGridAttribute)).Count() > 0)
                {
                    IsPropriedadesConversaoExplicita = true;
                    return;
                }
            });

            //Método para setar o valor do objeto
            Action<Type, System.Reflection.PropertyInfo, Object, DataRow> SetarValorObjetoMethod = null;

            if (IsPropriedadesConversaoExplicita)
                SetarValorObjetoMethod = SetarValorObjetoCustom;
            else
                SetarValorObjetoMethod = SetarValorObjeto;



            //Conversão
            if (dataTable != null && quantidadeLinhas > 0)
            {

                //Varrendo as propriedades
                for (int i = 0; i < quantidadeLinhas; i++)
                {
                    //Novo objeto do tipo informado
                    Tipo objeto = new Tipo();

                    //Obtendo a linha corrente
                    DataRow linha = dataTable.Rows[i];

                    //Varrendo as propriedades
                    for (int p = 0; p < quantidadePropriedades; p++)
                    {
                        try
                        {
                            SetarValorObjetoMethod(tipo, propriedades[p], objeto, linha);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    lista.Add(objeto);
                }
            }

            return lista;
        }

        private static void SetarValorObjeto(Type tipo, System.Reflection.PropertyInfo propriedade, Object objoCorrente, DataRow linha)
        {
            string nomePropriedade = propriedade.Name;
            tipo.GetProperty(nomePropriedade).SetValue(objoCorrente, linha[nomePropriedade], null);
        }

        private static void SetarValorObjetoCustom(Type tipo, System.Reflection.PropertyInfo propriedade, Object objoCorrente, DataRow linha)
        {
            string nomePropriedade = propriedade.Name;
            tipo.GetProperty(nomePropriedade).SetValue(objoCorrente, GetValorObjeto(linha[nomePropriedade], propriedade), null);
        }


        /// <summary>
        /// Obtém o valor do objeto de acordo com o atributo TipoConversaoExplicitaDataTableGridAttribute
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="propriedade"></param>
        /// <returns></returns>
        private static object GetValorObjeto(object objeto, System.Reflection.PropertyInfo propriedade)
        {
            GridAttributes.TipoConversaoExplicitaDataTableGridAttribute tipoConversao = propriedade.GetCustomAttributes(true).Where(w => w.GetType() == typeof(GridAttributes.TipoConversaoExplicitaDataTableGridAttribute)).FirstOrDefault() as GridAttributes.TipoConversaoExplicitaDataTableGridAttribute;

            if (tipoConversao != null)
            {

                switch (tipoConversao.tipoConversaoExplicita)
                {

                    case GridAttributes.TipoConversaoExplicitaDataTableGridAttribute.TipoConversaoExplicita.DateTime:
                        return Convert.ToDateTime(objeto);
                    case GridAttributes.TipoConversaoExplicitaDataTableGridAttribute.TipoConversaoExplicita.Int16:
                        return Convert.ToInt16(objeto);
                    case GridAttributes.TipoConversaoExplicitaDataTableGridAttribute.TipoConversaoExplicita.Int32:
                        return Convert.ToInt32(objeto);
                    case GridAttributes.TipoConversaoExplicitaDataTableGridAttribute.TipoConversaoExplicita.Int64:
                        return Convert.ToInt64(objeto);
                    default: return objeto;
                }

            }

            return objeto;
        }

        /// <summary>
        /// Verifica se o nome de uma propriedade está contida nas colunas de um datatable
        /// </summary>
        /// <param name="nomePropriedade"></param>
        /// <param name="dataColumnCollection"></param>
        /// <returns></returns>
        private static bool PropriedadeExiste(string nomePropriedade, DataColumnCollection dataColumnCollection)
        {
            short quantidadeColuans = (short)dataColumnCollection.Count;

            for (int i = 0; i < quantidadeColuans; i++)
                if (dataColumnCollection[i].ColumnName == nomePropriedade)
                    return true;

            return false;
        }

    }
    #endregion

    #region Filtros Grid

    /// <summary>
    /// Filtros necessários para a grid
    /// </summary>
    public sealed class FiltroGrid
    {
        //Atributos de paginação

        /// <summary>
        /// Quantidade de registros a retornar
        /// </summary>
        public string strQuantidadeRegistros { get; set; }

        /// <summary>
        /// Indica para qual pagina ir
        /// </summary>
        public string strIrParaPagina { get; set; }

        /// <summary>
        /// Filtro utilizado na consulta
        /// </summary>
        public string strFiltro { get; set; }

        /// <summary>
        /// Indica o nome da coluna a ser filtrada, caso o filtro seja por coluna
        /// </summary>
        public string strNomeColunaFiltrar { get; set; }

        /// <summary>
        /// Nome do campo a ser ordenado
        /// </summary>
        public string strNomeCampoOrdenar { get; set; }

        public string esconderContadorIrPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string gerarIrPara { get; set; }

        /// <summary>
        /// 0 para crescente e 1 para decrescente
        /// </summary>
        public string tipoOrdenacao { get; set; }

        /// <summary>
        /// Posiciona a grid na página em que contem o registro, sobrepondo a pagina escolhida
        /// </summary>
        public int irParaRegistro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool GerarExcel { get; set; }

        //Atributos do Grid

        /// <summary>
        /// Id da grid
        /// </summary>
        public string IdTabela { get; set; }

        /// <summary>
        /// Indica se é a primeira requisição
        /// </summary>
        public bool PrimeiraRequisicao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string strListCheckBox { get; set; }

        /// <summary>
        /// Total de registros
        /// </summary>
        public int? TotalRegistros { get; set; }

        public void GerarPaginacaoManual(int tamanho)
        {
            this.TotalRegistros = tamanho;
            this.valorTake = Convert.ToInt32(this.strQuantidadeRegistros);
            this.valorSkip = (Convert.ToInt32(this.strIrParaPagina) - 1) * this.valorTake;

        }

        /// <summary>
        /// 
        /// </summary>
        public int valorSkip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int valorTake { get; set; }


        /// <summary>
        /// Serialização
        /// </summary>
        public string baseGrid { get; set; }
    }

    #endregion

}