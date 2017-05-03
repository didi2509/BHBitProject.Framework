/// <reference path="jquery-2.0.3.min.js" />

/*
****
***
**
*

/// 12/01/2016
/// Diogo de Freitas Nunes
/// Versão 3.1

*
**
***
****
*/

/*Métodos globais e correções*/

var GridHelperMensagem = ModalMensagem;
  
//Cria o método que dispara uma mensagem caso o mesmo não exista
if (typeof GridHelperMensagem == 'undefined' || GridHelperMensagem == null) {
    var GridHelperMensagem = function (mens) {
        alert(mens);
    }
}

/*Javascript - Métodos de Extensão*/
String.prototype.Contem = function (strValorPesquisar, ignorarCase) {


    /// <summary>Diogo de Freitas Nunes - Método de extensão para o tipo String - Retorna true se strValorPesquisar estiver contida na string</summary>

    ignorarCase = typeof ignorarCase == 'undefined' ? false : ignorarCase;

    //Quantidade de caracteres da string atual
    var quantidadeCaracteres = this.length;

    //Casas a andar no valor a pesquisar
    var casas = 0;

    //Indica se a string foi localizada
    var contem = false;

    //Valor original
    var strOriginal = ignorarCase ? this.toUpperCase() : this;

    if (ignorarCase)
        strValorPesquisar = strValorPesquisar.toUpperCase();

    if (strOriginal != "")
        for (var i = 0; i < quantidadeCaracteres; i++) {

            //Se a primeira letra foi encontrada então a partir daí o laço é transferido para o while
            if (strOriginal.charAt(i) == strValorPesquisar.charAt(0)) {

                casas++;
                i++;

                while ((i < quantidadeCaracteres) && (casas < strValorPesquisar.length) && (strOriginal.charAt(i) == strValorPesquisar.charAt(casas))) {
                    //Comparando se as casas seguintes são iguais as casas a comparar
                    i++;
                    casas++;
                }

                //Se a quantidade de casas localizadas for igual a string a se localizar, a mesma está contida na string pesquisada
                if (casas == strValorPesquisar.length) {
                    contem = true;
                    break;
                }
                else
                    casas = 0;
            }
        }
    return contem;
}

String.prototype.ApenasNumeros = function () {

    /// <summary>Diogo de Freitas Nunes - Remove as letras de uma string</summary>

    var quantidadeCaracteres = this.length;
    var retorno = "";

    if (quantidadeCaracteres > 0) {

        for (var i = 0; i < quantidadeCaracteres; i++)
            if ((this.charAt(i) == '0') || (this.charAt(i) == '4') || (this.charAt(i) == '7') ||
                (this.charAt(i) == '1') || (this.charAt(i) == '5') || (this.charAt(i) == '8') ||
                (this.charAt(i) == '2') || (this.charAt(i) == '6') || (this.charAt(i) == '9') ||
                (this.charAt(i) == '3'))
                retorno += this.charAt(i);

    }

    return retorno;
}

var GridHelper = function (_eventos) {

    //MÉTODOS
    this.E = function (id) {
        return document.getElementById(id);
    }

    this.RemoverClasseError = function (campos) {
        /// <summary>Remove a classe de erro dos elemento</summary>
        this.SetarClasseError(campos, false);
    }

    this.AddClasseError = function (campos) {
        /// <summary>Adiciona a classe de erro aos elemento</summary>
        this.SetarClasseError(campos, true);
    }

    this.SetarClasseError = function (campos, erro) {
        /// <summary>Adiciona ou remove a classe de erro dos elementos</summary>

        erro = typeof erro == 'undefined' ? true : erro;

        try {

            var listaCamposObrigatorios = campos.split(",");
            for (var item in listaCamposObrigatorios) {

                if (erro)
                    $("#" + listaCamposObrigatorios[item]).addClass("error");
                else {
                    if ($("#" + listaCamposObrigatorios[item]).is("[type=checkbox]")) {
                        $("#" + listaCamposObrigatorios[item]).closest("div").parent().closest("div").removeClass("error");
                    }

                    if ($("[name =\'" + listaCamposObrigatorios[item] + "\']").is("[type=radio]")) {
                        $("[name =\'" + listaCamposObrigatorios[item] + "\']").closest('div').parent().parent().css({ 'border': "0px" })
                    }

                    $("#" + listaCamposObrigatorios[item]).removeClass("error");
                }
            }
        }
        catch (e) {
            GridHelperMensagem("Erro:\n function MarcaCamposObrigatorios");
        }
    }

    //ObjetoSerialize
    this.objetoSerialize = null;

    //Callbacks da grid
    this.eventos = typeof _eventos == 'undefined' ? null : _eventos;

    //Atributos do grid
    //Indica se foi a primeira requisição
    this.primeiraRequisicao = null;

    //Indica se o usuario utilizou o filtro
    this.utilizouFiltro = false;

    //quantidade de registro setada
    this.quantidadeRegistrosCorrente = 0;

    //Indica que o usuário clicou em uma página
    this.utilizouPaginacaoOuFiltro = false;

    //Configura o mínimo de caractéres para se efetuar uma pesquisa
    this.minimoCaracteresPesquisa = typeof _minimoCaracteresPesquisa == 'undefined' ? 3 : _minimoCaracteresPesquisa;

    //Id da tabela a ser manipulada
    this.idTabela = null;

    //Indica se o botão de pesquisa deverá ser criado
    this.criarBotaoPesquisa = false;

    //Exportação para o excel - ainda nao esta funcionando
    this.GerarExcel = false;

    //Coluna corrente utilizada no filtro
    this.strNomeColunaFiltrar = "";

    //Indica se o filtro de pesquisa em todas as colunas está sendo utilizado
    this.GerarFiltroUniversal = true;

    //Valor do filtro corrente
    this.strFiltroCorrente = "";

    //Método executado no sucesso
    this.onSucesso = null;

    //Nome da coluna corrente utilizada na ordenação
    this.strNomeCampoOrdenar = "";

    //Tipo da ordenacao, 0 Crescente 1 Decrescente
    this.tipoOrdenacao = 0;

    //Sufixo dos componentes
    this.sufixoComponentes = null;

    //sobrepõe a paginação, posiciona a grid em determinado registro
    this.irParaRegistro = 0;

    //Indica se o campo Ir Para deverá ser gerado
    this.gerarIrPara = true;
    this.esconderContadorIrPara = true;

    //Indica se o botão ir para deverá ser gerado
    this.gerarBotaoIrPara = false;

    //label do botao de pesquisar
    this.labelTextoPesquisar = "Search";
    this.labelBotaoPesquisar = "Search";

    //Ids dos containers
    this.idContainerPai = null;
    this.idContainerCabecalho = null;
    this.idContainerGrid = null;
    this.idContainerRodape = null;
    this.idContainerGeral = null;

    //Ids dos componentes
    this.idSelectGridHelperQuantidadeRegistros = null;
    this.idTextGridHelperBusca = null;
    this.idCampoInformacoes = null;
    this.idSpanInicio = null;
    this.idSpanFim = null;
    this.idBotaoIrPara = null;
    this.idSpanTotal = null;
    this.idPaginacaoAnterior = null;
    this.idPaginacaoProximo = null;
    this.idPaginacaoPrimeira = null;
    this.idPaginacaoUltima = null;
    this.idPaginacaoPagina = null;
    this.idPaginacaoContainer = null;
    this.idBotaoGridHelperBusca = null;
    this.idBotaoGridHelperExcel = null;
    this.idIrParaRegistro = null;

    //Classes dos componentes
    this.classeLinkPaginacaoSimples = null;
    this.classeLiPaginacaoSimples = null;

    //Informações sobre a fonte de dados
    this.TotalRegistros = null;
    this.RefazerLinks = null;
    this.forcarPagina = false;
    this.TotalPaginas = 0;
    this.UrlRequisicao = null;
    this.PaginaCorrente = null;
    this.quantidadePadraoRegistros = 0;

    //Indica o filtro utilizado quando os dois filtros estiverem ativos
    this.UtilizandoFiltroGlobal = false;
    this.AutoRedimensionarColunas = false;
    this.PermitirExportarExcel = false;
    this.ParametrosRequisicao = null;
    this.clickFiltro = false;


    this.RemoverComponente = function (id) {

        /// <summary>Remove um componente se o mesmo ja tiver sido criado</summary>

        if (this.E(id) && this.E(this.idContainerPai)) {
            this.E(this.idContainerPai).removeChild(E(id));
        }
    }

    this.RemoverHtmlContainerPaiGrid = function () {

        /// <summary>Remove um componente se o mesmo ja tiver sido criado</summary>

        if (this.E(this.idContainerPai)) {
            this.E(this.idContainerPai).innerHTML = "";
        }
    }


    this.caminhoAbsoluto = typeof absolutePath == 'undefined' ? '' : absolutePath;

    this.GerarGrid = function (_idTabela, _idDivContainerPai, _urlRequisicao, _configuracoesGrid) {

        try {

            //Recriando o container - grid dentro de grid
            if (typeof _configuracoesGrid != 'undefined' && typeof _configuracoesGrid.detalheContainer != 'undefined') {
                //Removendo a div criada
                if (this.E(_idDivContainerPai))
                    $('#' + _idDivContainerPai).remove();

                //Recriando o container
                $('.' + _configuracoesGrid.detalheContainer + ' td:nth-child(2)').append('<div id="' + _idDivContainerPai + '"></div>');
            }

            //Reseta o container pai
            $("#" + _idDivContainerPai).html('');

            if (typeof _configuracoesGrid == 'undefined')
                _configuracoesGrid = {};

            if (typeof _configuracoesGrid.labelTextoPesquisar != 'undefined')
                this.labelTextoPesquisar = _configuracoesGrid.labelTextoPesquisar;

            if (typeof _configuracoesGrid.labelBotaoPesquisar != 'undefined')
                this.labelBotaoPesquisar = _configuracoesGrid.labelBotaoPesquisar;

            this.minimoCaracteresPesquisa = typeof _configuracoesGrid.minimoCaracteresPesquisa == 'undefined' ? 3 : _configuracoesGrid.minimoCaracteresPesquisa;

            //Quantidade default de registros a serem retornados
            this.quantidadePadraoRegistros = typeof _configuracoesGrid.quantidadePadraoRegistros == 'undefined' ? 10 : _configuracoesGrid.quantidadePadraoRegistros;

            //Filtro opcional
            if (typeof _configuracoesGrid.parametrosRequisicao != 'undefined')
                this.ParametrosRequisicao = _configuracoesGrid.parametrosRequisicao;

            //Indica se o irPara deverá ser gerado
            this.gerarIrPara = typeof _configuracoesGrid.gerarIrPara == 'undefined' ? true : _configuracoesGrid.gerarIrPara;
            this.esconderContadorIrPara = typeof _configuracoesGrid.esconderContadorIrPara == 'undefined' ? true : _configuracoesGrid.esconderContadorIrPara;
            this.gerarBotaoIrPara = this.gerarIrPara;

            this.objetoSerialize = typeof _configuracoesGrid.objetoSerialize == 'undefined' ? null : _configuracoesGrid.objetoSerialize;

            //Monta o objeto de requisição
            this.MontarObjetoRequisicao();

            //OnSucesso
            this.onSucesso = typeof _configuracoesGrid.onSucesso == 'undefined' ? null : _configuracoesGrid.onSucesso;

            this.UrlRequisicao = _urlRequisicao;
            this.primeiraRequisicao = true;
            this.idTabela = _idTabela;
            this.GerarExcel = false;

            //Indica se o filtro por todos os campos irá ser gerado
            this.GerarFiltroUniversal = typeof _configuracoesGrid.gerarFiltroUniversal == 'undefined' ? true : _configuracoesGrid.gerarFiltroUniversal;
            this.criarBotaoPesquisa = typeof _configuracoesGrid.criarBotaoPesquisa == 'undefined' ? true : _configuracoesGrid.criarBotaoPesquisa;
            this.PermitirExportarExcel = typeof _configuracoesGrid.exportarExcel == 'undefined' ? false : _configuracoesGrid.exportarExcel;


            //Indica se a grid irá poder ser redimensionada
            this.AutoRedimensionarColunas = typeof _configuracoesGrid.redimensionar == 'undefined' ? false : _configuracoesGrid.redimensionar;

            //Se o filtro nao for gerado o botão também nao irá existir
            if (!this.GerarFiltroUniversal)
                this.criarBotaoPesquisa = false;

            //Sufixo dos componentes
            this.sufixoComponentes = this.idTabela

            //Ids dos containers
            this.idContainerPai = "";
            this.idContainerCabecalho = "gridHelperContainerCabecalho" + this.sufixoComponentes;
            this.idContainerGrid = "gridHelperGrid" + this.sufixoComponentes;
            this.idContainerRodape = 'gridHelperRodape' + this.sufixoComponentes;
            this.idContainerGeral = 'gridHelperGeral' + this.sufixoComponentes;

            //Ids dos componentes
            this.idSelectGridHelperQuantidadeRegistros = "selectGridHelperQuantidadeRegistros" + this.sufixoComponentes;
            this.idBotaoGridHelperBusca = "botaoGridHelperBusca" + this.sufixoComponentes;
            this.idBotaoGridHelperExcel = "botaoGridHelperExcel" + this.sufixoComponentes;
            this.idTextGridHelperBusca = "textGridHelperBusca" + this.sufixoComponentes;
            this.idCampoInformacoes = "gridHelperCampoInformacoes" + this.sufixoComponentes;
            this.idSpanInicio = "spanGridHelperInicio" + this.sufixoComponentes;
            this.idSpanFim = "spanGridHelperFim" + this.sufixoComponentes;
            this.idSpanTotal = "spanGridHelperTotal" + this.sufixoComponentes;
            this.idPaginacaoAnterior = 'paginacaoGridHelperAnterior' + this.sufixoComponentes;
            this.idPaginacaoProximo = 'paginacaoGridHelperProximo' + this.sufixoComponentes;
            this.idPaginacaoPrimeira = 'paginacaoGridHelperPrimeira' + this.sufixoComponentes;
            this.idPaginacaoUltima = 'paginacaoGridHelperUltima' + this.sufixoComponentes;
            this.idPaginacaoPagina = 'paginaGridHelper' + this.sufixoComponentes;
            this.idPaginacaoContainer = 'paginacaoGridHelperContainer' + this.sufixoComponentes;
            this.idIrParaRegistro = 'irParaRegistroGridHelper' + this.sufixoComponentes;
            this.idBotaoIrPara = 'gridHelperBotaoIrPara' + this.sufixoComponentes;
            //Classes dos componentes
            this.classeLinkPaginacaoSimples = "paginacaoGridHelperLinkSimples" + this.sufixoComponentes;
            this.classeLiPaginacaoSimples = "paginacaoGridHelperLiSimples" + this.sufixoComponentes;

            //Informações sobre a fonte de dados
            this.TotalRegistros = 0;
            this.RefazerLinks = true;


            this.idContainerPai = _idDivContainerPai;

            //Cria o loader na página
            this.CriarDivLoader();

            this.CriarContainers(_idDivContainerPai);

            this.RetornarConsulta(1, this.quantidadePadraoRegistros, this.ValorFiltro(), this);

        } catch (err) {

            GridHelperMensagem(err.message);
        }
    }

    //Criação dos containers
    this.CriarContainers = function (idObjetoPai) {

        this.RemoverComponente(this.idContainerGeral);

        var geral = document.createElement('div');
        geral.setAttribute('id', this.idContainerGeral);

        var cabecalho = document.createElement("div");
        cabecalho.setAttribute("id", this.idContainerCabecalho);

        var grid = document.createElement("div");
        grid.setAttribute("id", this.idContainerGrid);

        var rodape = document.createElement("div");
        rodape.setAttribute("id", this.idContainerRodape);
        rodape.setAttribute("class", 'gridHelperRodape');

        //DivGeral
        geral.appendChild(cabecalho);
        geral.appendChild(grid);
        geral.appendChild(rodape);

        //ElementoPai
        this.E(idObjetoPai).appendChild(geral);
    }


    this.CriarIrParaRegistro = function (idObjetoPai) {

        /// <summary>Ir para registro</summary>

        if (this.gerarIrPara) {

            //Criando o container
            var divContainer = document.createElement("div");
            divContainer.setAttribute('class', 'irParaRegistroDiv');

            //Criando o input text
            var textIrParaRegistro = document.createElement('input');
            textIrParaRegistro.setAttribute('type', 'text');
            textIrParaRegistro.setAttribute('class', 'small');
            textIrParaRegistro.setAttribute('id', this.idIrParaRegistro);
            textIrParaRegistro.setAttribute('title', 'go to page');
            textIrParaRegistro.setAttribute('alt', 'go to page');

            //Criando o span
            var spanIrParaRegistro = document.createElement('span');
            spanIrParaRegistro.setAttribute('class', 'irParaRegistroSpan');
            spanIrParaRegistro.innerHTML = "page:";

            //Colocando os componentes nos respectivos lugares
            divContainer.appendChild(spanIrParaRegistro);
            divContainer.appendChild(textIrParaRegistro);

            if (this.gerarBotaoIrPara) {
                var botaoIrPara = document.createElement('input');
                botaoIrPara.setAttribute('type', 'button');
                botaoIrPara.setAttribute('class', 'btn gridHelperBotaoIrPara');
                botaoIrPara.setAttribute('id', this.idBotaoIrPara);
                botaoIrPara.setAttribute('value', 'Go');

                divContainer.appendChild(botaoIrPara);
            }

            this.E(idObjetoPai).appendChild(divContainer);
        }
    }


    this.RecriarContainerDetalhamento = function () {

    }

    /* Select */
    this.CriarOptionsQuantidadeItens = function (objetoSelectPai, ListaOptionsSelectQuantidadeItens) {
        /// <summary>Carrega um select com uma lista de option</summary>
        var itens = ListaOptionsSelectQuantidadeItens.length;

        var option = null;

        for (i = 0; i < itens; i++) {

            option = document.createElement("option");
            option.setAttribute("value", ListaOptionsSelectQuantidadeItens[i]);
            option.innerHTML = ListaOptionsSelectQuantidadeItens[i];

            if (ListaOptionsSelectQuantidadeItens[i] == this.quantidadePadraoRegistros)
                option.setAttribute("selected", true);

            objetoSelectPai.appendChild(option);
        }
    }

    this.CriarSelectQuantidadeItens = function (idObjetoPai) {

        /// <summary>Cria o select que irá exibir a quantidade de itens da grid</summary>

        //Criando o container para o select
        var divContainer = document.createElement('div');

        //Configurando o container
        divContainer.setAttribute("class", "gridHelperQuantidadeRegistros");

        //Criando o select
        var select = document.createElement("select");

        //Configurando o select
        select.setAttribute("id", this.idSelectGridHelperQuantidadeRegistros);
        select.setAttribute("class", "m-wrap small selectQuantidadeRegistrosGridHelper");

        //Carregando os options do select
        var ListaOptions = new Array();
        ListaOptions.push("5");
        ListaOptions.push("10");
        ListaOptions.push("15");
        ListaOptions.push("20");
        ListaOptions.push("30");
        ListaOptions.push("50");
        // ListaOptions.push("Todos");

        this.CriarOptionsQuantidadeItens(select, ListaOptions)


        this.E(idObjetoPai).appendChild(divContainer);

        var span = document.createElement('span');
        span.innerHTML = "Records &nbsp;per page:&nbsp;";

        divContainer.appendChild(span);

        //Jogando o select dentro da div
        divContainer.appendChild(select);
    }

    /*Campo de Busca*/
    this.CriarCampoPesquisar = function (idObjetoPai) {

        /// <summary>Cria o campo de pesquisa para a grid</summary>

        var divContainer = document.createElement('div');
        divContainer.setAttribute('class', 'gridHelperBusca');
        divContainer.setAttribute('id', 'gridHelperBusca' + this.idTabela + 'Pesquisa')

        var span = document.createElement('span');
        span.setAttribute('class', 'dataTables_filter');
        span.innerHTML = this.labelTextoPesquisar + ":&nbsp;";

        divContainer.appendChild(span);

        var inputBusca = document.createElement("input");
        inputBusca.setAttribute('id', this.idTextGridHelperBusca);
        inputBusca.setAttribute('class', 'small');
        inputBusca.setAttribute('type', 'text');
        inputBusca.setAttribute('maxlength', '100');

        //Colocando o input de busca na div
        divContainer.appendChild(inputBusca);

        this.E(idObjetoPai).appendChild(divContainer);

    }

    this.CriarBotaoPesquisar = function (idObjetoPai) {

        /// <summary>Cria o botão de pesquisa para a grid</summary>
        if (this.criarBotaoPesquisa) {

            var divContainer = document.createElement('div');
            divContainer.setAttribute('class', 'gridHelperBotaoBusca');


            var divBotaoBusca = document.createElement("div");
            divBotaoBusca.setAttribute('id', this.idBotaoGridHelperBusca);
            divBotaoBusca.setAttribute('class', 'btn blue gridHelperBtnBuscar');
            divBotaoBusca.innerHTML = this.labelBotaoPesquisar;

            //Colocando o input de busca na div
            divContainer.appendChild(divBotaoBusca);

            this.E(idObjetoPai).appendChild(divContainer);
        }
    }

    //Cria o botao do excel
    this.CriarBotaoExcel = function (idObjetoPai) {

        if (this.PermitirExportarExcel) {

            var divContainer = document.createElement('div');
            divContainer.setAttribute('class', 'gridHelperBotaoExcel');


            var divBotaoExcel = document.createElement("div");
            divBotaoExcel.setAttribute('id', this.idBotaoGridHelperExcel);
            divBotaoExcel.setAttribute('class', 'btn btn-sispat');
            divBotaoExcel.innerHTML = "Exportar";

            //Colocando o input de busca na div
            divContainer.appendChild(divBotaoExcel);

            this.E(idObjetoPai).appendChild(divContainer);
        }

    }

    /*Exibição do retorno da consulta e paginação*/
    this.CriarCampoInformacoes = function (idObjetoPai) {

        /// <summary>Cria a div com as informações da consulta</summary>

        var divContainer = document.createElement('div');
        divContainer.setAttribute('class', 'gridHelperInformacoes');
        divContainer.setAttribute('id', this.idCampoInformacoes);

        var span_1 = document.createElement('span');
        var spanInicio = document.createElement('span');
        var span_3 = document.createElement('span');
        var spanFim = document.createElement('span');
        var span_5 = document.createElement('span');
        var spanTotal = document.createElement('span');

        span_1.setAttribute("class", "dataTables_info esconder");
        spanInicio.setAttribute("class", "dataTables_info");
        span_3.setAttribute("class", "dataTables_info esconder");
        spanFim.setAttribute("class", "dataTables_info esconder");
        span_5.setAttribute("class", "dataTables_info");
        spanTotal.setAttribute("class", "dataTables_info");


        if (!this.esconderContadorIrPara) {
            spanInicio.innerHTML = "(0) ";
            span_5.innerHTML = "de";
        }
        else {
            spanInicio.innerHTML = "";
            span_5.innerHTML = "Total: ";
        }

        span_1.innerHTML = "";

        span_3.innerHTML = "";
        spanFim.innerHTML = "Fim;";

        spanTotal.innerHTML = "Total";

        spanInicio.setAttribute("id", this.idSpanInicio);
        spanFim.setAttribute("id", this.idSpanFim);
        spanTotal.setAttribute("id", this.idSpanTotal);

        divContainer.appendChild(span_1);
        divContainer.appendChild(spanInicio);
        divContainer.appendChild(span_3);
        divContainer.appendChild(spanFim);
        divContainer.appendChild(span_5);
        divContainer.appendChild(spanTotal);


        this.E(idObjetoPai).appendChild(divContainer);
    }

    //Cria a paginação
    this.CriarPaginacao = function (idObjetoPai, totalRegistros, registrosPorPagina, that) {

        /// <summary>Criando a paginação da tela</summary>

        //this.E(this.idContainerRodape).removeChild(this.E(this.idPaginacaoContainer));

        if (document.getElementById(that.idPaginacaoContainer))
            that.E(that.idContainerRodape).removeChild(that.E(that.idPaginacaoContainer));

        var divContainer = document.createElement('div');
        divContainer.setAttribute('class', 'dataTables_paginate paging_bootstrap gridHelperPaginacao');
        divContainer.setAttribute('id', that.idPaginacaoContainer);

        //Criando a ul
        var ulPaginacao = document.createElement('ul');

        //Calculando a quantidade de paginas
        var QuantidadePaginasReal = parseFloat(that.TotalRegistros / that.ItensPorPagina());
        var QuantidadePaginasInt = parseInt(QuantidadePaginasReal);

        var Divisao = parseFloat(QuantidadePaginasReal / QuantidadePaginasInt);
        var TotalQuantidadePaginas = Divisao > 1 ? ++QuantidadePaginasInt : QuantidadePaginasInt;

        var liPagina = null;
        var pagina = 0;
        //Primeira página
        liPrimeiraPagina = document.createElement('li');
        liPrimeiraPagina.innerHTML = that.MontarLinkPaginacao("<i  class='fa fa-backward'></i>", that.idPaginacaoPrimeira, "gridHelperPaginacaoSalto");
        ulPaginacao.appendChild(liPrimeiraPagina);
        //Pagina anterior
        liPagina = document.createElement('li');
        liPagina.innerHTML = that.MontarLinkPaginacao("<i  class='fa fa-chevron-circle-left'></i>", that.idPaginacaoAnterior, "gridHelperPaginacaoSalto");
        ulPaginacao.appendChild(liPagina);
        ulPaginacao.setAttribute('class', 'pagination');

        that.TotalPaginas = TotalQuantidadePaginas;

        if (TotalQuantidadePaginas > 5)
            TotalQuantidadePaginas = 5;

        var paginaInicial = 0;

        if (that.forcarPagina) {

            if (that.PaginaCorrente > 5) {
                paginaInicial = that.PaginaCorrente - 5;
                TotalQuantidadePaginas = that.PaginaCorrente;
            }
        }

        for (var i = paginaInicial; i < TotalQuantidadePaginas; i++) {

            pagina = i + 1;

            liPagina = document.createElement('li');
            liPagina.setAttribute('class', that.classeLiPaginacaoSimples)
            liPagina.innerHTML = that.MontarLinkPaginacao(pagina, that.idPaginacaoPagina + pagina, that.classeLinkPaginacaoSimples);
            ulPaginacao.appendChild(liPagina);

        }

        //Próxima página
        liPagina = document.createElement('li');
        liPagina.innerHTML = that.MontarLinkPaginacao("<i  class='fa fa-chevron-circle-right'></i>", that.idPaginacaoProximo, "gridHelperPaginacaoSalto");
        ulPaginacao.appendChild(liPagina);

        //Última página
        liUltimaPagina = document.createElement('li');
        liUltimaPagina.innerHTML = that.MontarLinkPaginacao("<i  class='fa fa-forward'></i>", that.idPaginacaoUltima, "gridHelperPaginacaoSalto");
        ulPaginacao.appendChild(liUltimaPagina);
        divContainer.appendChild(ulPaginacao);
        that.E(idObjetoPai).appendChild(divContainer);

        //Clicks
        that.clickPrimeira(that);
        that.clickUltima(that, QuantidadePaginasInt);

        that.ClickAnterior(that);
        that.ClickProximo(that);

        //Coloca o click nos links de paginação
        that.ClickLinksPaginacao(that);

        if (that.forcarPagina) {
            $('#' + (that.idPaginacaoPagina + that.PaginaCorrente.toString())).parent().addClass("active");
            that.LocalizarRegistro(that);
            //var seletorTrRegistroCorrente = "[name=td___" + that.idTabela + that.irParaRegistro.toString() + "]";
            //$(seletorTrRegistroCorrente).parent().mouseup();
            //$(seletorTrRegistroCorrente).parent().click();
        }



    }

    this.clickPrimeira = function (that) {
        /// <summary>Direicona para a primeira página</summary>
        $('#' + (that.idPaginacaoPrimeira)).click(function () {
            that.RemoverSelecaoPaginas(that);
            that.RefazerLinks = true;
            that.RetornarConsulta(1, that.ItensPorPagina(), that.ValorFiltro(), that);
        });
    }

    this.PossuiClickPaginacao = function () {
        /// <summary>Identifica se a grid vai possuir o evento de click da paginação</summary>

        return ((typeof this.configuracoes != 'undefined' && this.configuracoes != null)
            && (typeof this.eventos.clickPaginacao != 'undefined')
            && (this.eventos.clickPaginacao != null));

    }


    this.clickUltima = function (that, ultimaPagina) {

        /// <summary>Direciona para a ultima pagina</summary>
        $('#' + (that.idPaginacaoUltima)).click(function () {
            that.RemoverSelecaoPaginas(that);
            that.RefazerLinks = false;

            //Se não houverem mais de 5 páginas não será necessário refazer os links
            if (ultimaPagina > 5) {

                //Primeira página das ultimas 5
                var pagina = ultimaPagina - 4;

                //Refazendo os links
                $('.' + that.classeLinkPaginacaoSimples).each(function () {

                    $(this).html(pagina);
                    $(this).attr({ id: that.idPaginacaoPagina + pagina });
                    pagina++;
                });
            }

            //Disparando o click do ultimo link
            $('#' + (that.idPaginacaoPagina + ultimaPagina)).click();
        });
    }

    //Click do botao anterior
    this.ClickAnterior = function (that) {


        $('#' + (that.idPaginacaoAnterior)).click(function () {

            that.RemoverClasseActive(that);

            //Selecionando a primeira pagina caso nao tenham mais paginas
            if (that.PaginaCorrente == 1)
                $('#' + (that.idPaginacaoPagina + "1").toString()).parent().attr('class', 'active');
            else
                that.RemoverSelecaoPaginas(that);

            var pagina = 0;
            var TrocarPaginas = true;

            $('.' + that.classeLinkPaginacaoSimples).each(function () {

                pagina = $(this).html();

                //Não deixar ir para paginas negativas
                if (pagina == "1")
                    TrocarPaginas = false;

                if (TrocarPaginas) {
                    pagina--;
                    $(this).html(pagina);
                    $(this).attr({ id: that.idPaginacaoPagina + pagina });
                }

                //Selecionando a pagina corrente
                if ($(this).html() == that.PaginaCorrente)
                    $(this).parent().addClass('active');
            });

            //Mudando a página no clique
            if (that.PaginaCorrente > 1)
                $('#' + (that.idPaginacaoPagina + (parseInt(that.PaginaCorrente) - 1).toString())).click();
        });
    }

    this.RemoverSelecaoPaginas = function (that) {

        /// <summary>Remove a seleção da primeira e ultima pagina</summary>
        $('.' + that.classeLinkPaginacaoSimples).each(function () {

            setTimeout(function () {
                $(this).parent().removeClass('active');
            }, 200);
        });
    }

    this.SetarClickDetalhamento = function (metodoAbrir, metodoFechar, autoFechardetalhes) {
        /// <summary>Reseta o click do detalhamento</summary>

        autoFechardetalhes = typeof autoFechardetalhes == 'undefined' ? true : autoFechardetalhes;
        var contextoGrid = this;

        //Remove o click do detalhamento pai
        $('#' + this.idContainerPai).on('mouseover', '.gridHelperDetalhamentoCabecalho' + this.idTabela, function () {
            $(this).attr('onclick', '');
        });

        /// <summary>Remove o click padrão do detalhamento</summary>
        $('#' + this.idContainerPai).on('mouseenter', '.gridHelperDetalhamento' + this.idTabela + ' div', function () {
            $($(this)).unbind('mouseup');
        });


        //Remove o click do detalhamento
        $('#' + this.idContainerPai).unbind('click');

        /// <summary>Seta o clique do detalhamento</summary>
        $('#' + this.idContainerPai).on('click', '.gridHelperDetalhamento' + this.idTabela + ' div', function () {

            var detalheContainerGrid = $(this).parent().attr('data-detalhamento');

            if (autoFechardetalhes)
                contextoGrid.FecharDetalhamentosGrid(detalheContainerGrid);

            contextoGrid.ControlarDetalhamento($(this), metodoAbrir, metodoFechar, detalheContainerGrid);
        });
    }

    this.FecharDetalhamentosGrid = function (detalheAberto) {
        /// <summary>Permite apenas um detalhe ficar aberto na grid um</summary>
        var that = this;

        $('.gridHelperDetalhamento' + this.idTabela + ' div').each(function () {
            var detalheCorrente = $(this).parent().attr('data-detalhamento');

            if (detalheAberto != detalheCorrente) {
                that.RecolherDetalhamento($(this));
            }
        });
    }

    this.RetornarClasseOrdenacao = function () {
        return this.tipoOrdenacao == "0" ? "gridHelperOrdenacaoAsc" : "gridHelperOrdenacaoDesc";
    }

    this.ClickBotaoExcel = function (that) {

        $('#' + that.idBotaoGridHelperExcel).click(function () {

            that.GerarExcel = true;
            that.RetornarConsulta(that.PaginaCorrente, that.ItensPorPagina(), that.ValorFiltro(), that);

        });

    }

    //Click do botao proximo
    this.ClickProximo = function (that) {


        $('#' + (that.idPaginacaoProximo)).click(function () {

            //Selecionando a ultima pagina caso nao tenham mais paginas
            if (that.PaginaCorrente == that.TotalPaginas)
                $('#' + (that.idPaginacaoPagina + (parseInt(that.PaginaCorrente)).toString())).parent().attr('class', 'active');
            else
                that.RemoverSelecaoPaginas(that);

            that.RemoverClasseActive(that);

            var pagina = 0;
            var TrocarPaginas = true;

            //Varrendo as páginas para verificar se a última página ja foi inserida
            $('.' + that.classeLinkPaginacaoSimples).each(function () {

                pagina = $(this).html();

                if (parseInt(pagina) == that.TotalPaginas)
                    TrocarPaginas = false;
            });


            if (TrocarPaginas)
                $('.' + that.classeLinkPaginacaoSimples).each(function () {

                    pagina = $(this).html();

                    pagina++;
                    $(this).html(pagina);
                    $(this).attr({ id: that.idPaginacaoPagina + pagina });

                    //Selecionando a pagina corrente
                    if ($(this).html() == that.PaginaCorrente)
                        $(this).parent().addClass('active');
                });

            //Mudando a página no clique
            if (that.PaginaCorrente < that.TotalPaginas)
                $('#' + (that.idPaginacaoPagina + (parseInt(that.PaginaCorrente) + 1).toString())).click();
        });


    }

    //Clique dos links simples de paginacao
    this.ClickLinksPaginacao = function (that) {


        $('.' + that.classeLinkPaginacaoSimples).each(function () {

            //Seleciona a primeira pagina
            if ($(this).html() == "1") {
                if (!that.forcarPagina)
                    $(this).parent().addClass("active");
            }

            $(this).click(function () {

                if (that.PossuiClickPaginacao())
                    that.eventos.clickPaginacao.call(that, { contexto: that });

                //Remove a seleção da primeira e última página
                that.RemoverSelecaoPaginas(that);

                var irPagina = $(this).html();
                that.utilizouPaginacaoOuFiltro = true;

                that.RefazerLinks = false;

                that.RemoverClasseActive(that);

                //Coloca a classe no link
                if ($(this).html() == irPagina)
                    $(this).parent().addClass("active");

                that.RetornarConsulta(irPagina, that.ItensPorPagina(), that.ValorFiltro(), that);
            });
        });
    }

    //Ordenação, click dos cabeçalhos
    this.ClickOrdenacao = function (that) {
        $('.' + that.idTabela + 'gridHelperColunaCabecalho').click(function () {

            //Se não contiver a classe ordenável, não permite a ordenação desse campo
            if (!$(this).attr('class').toString().Contem("gridHelperOrdenavel"))
                return;

            var idCampoOrdenacao = $(this).attr('id');
            that.utilizouPaginacaoOuFiltro = true;

            //Tipo da ordenação - 0 Crescente, 1 - Decrescente
            if (that.strNomeCampoOrdenar == idCampoOrdenacao) {
                that.tipoOrdenacao = that.tipoOrdenacao == "0" ? "1" : "0"
            }
            else
                that.tipoOrdenacao = "0"

            that.strNomeCampoOrdenar = idCampoOrdenacao;

            //Removendo as setas de ordenação dos outros cabeçalhos do grid
            $('.' + that.idTabela + "gridHelperColunaCabecalho").each(function () {

                $(this).removeClass("gridHelperOrdenacaoAsc");
                $(this).removeClass("gridHelperOrdenacaoDesc");
            });

            //Adicionando a seta de ordenação
            $(this).addClass(that.RetornarClasseOrdenacao());
            that.RefazerLinks = true;

            that.RetornarConsulta(1, that.ItensPorPagina(), that.ValorFiltro(), that);
        });

        try {
            $('#' + that.strNomeCampoOrdenar).addClass(that.RetornarClasseOrdenacao());
        } catch (error) { }
    }

    //KeyUp da pesquisa por campo
    this.EventoPesquisaFiltrosIndividuais = function (that) {

        $('.gridHelperColunaEspecifica' + that.idTabela).keyup(function (e) {

            var code = e.keyCode || e.which;
            if (code == '13') {

                //Retirando o valor do grid de pesquisa global
                if (document.getElementById(that.idTextGridHelperBusca))
                    $('#' + that.idTextGridHelperBusca).val("");

                var valorDigitado = $(this).val();
                that.RemoverValorBuscasIndividuais();
                $(this).val(valorDigitado);

                that.RefazerLinks = true;
                that.UtilizandoFiltroGlobal = false;
                that.strNomeColunaFiltrar = $(this).attr("id");
                that.RetornarConsulta(1, that.ItensPorPagina(), valorDigitado, that);
            }
        });


    }

    this.RemoverValorBuscasIndividuais = function () {
        $('.gridHelperColunaEspecifica' + this.idTabela).val('');
    }

    //Remove a classe active dos LI
    this.RemoverClasseActive = function (that) {

        $('.' + that.classeLiPaginacaoSimples).each(function () {
            $(this).removeClass('active');
        });

    }

    //Monta os links de paginação
    this.MontarLinkPaginacao = function (innerHTML, id, classe) {
        return "<a id='" + id + "' class='" + classe + "' href='javascript:;'>" + innerHTML + "</a>"
    }


    //Controla a div de loader
    this.ControlarDiv = function (mostrar) {
        try {

            if (mostrar)
                $('#globalLoader').show();
            else
                $('#globalLoader').hide();

        } catch (err) { GridHelperMensagem(err.mesage); }
    }

    //Inicia os componentes da grid
    this.IniciarComponentesGrid = function (idObjetoPai) {

        /// <summary>Inicia os componentes da grid</summary>

        this.E(this.idContainerCabecalho).innerHTML = "";
        this.E(this.idContainerRodape).innerHTML = "";



        if (this.GerarFiltroUniversal) {
            this.CriarCampoPesquisar(this.idContainerCabecalho);
            this.CriarBotaoPesquisar('gridHelperBusca' + this.idTabela + 'Pesquisa');
        }


        this.CriarBotaoExcel(this.idContainerRodape);
        this.CriarCampoInformacoes(this.idContainerRodape);
        this.CriarSelectQuantidadeItens(this.idContainerRodape);
        this.CriarIrParaRegistro(this.idContainerRodape);

    }

    /* EVENTOS ********************************************  */

    this.IniciarSelectQuantidadeRegistrosChange = function (that) {

        /// <summary>Evento do select da quantidade de registros</summary>

        $('#' + this.idSelectGridHelperQuantidadeRegistros).change(function () {
            that.RefazerLinks = true;
            that.RetornarConsulta(1, that.ItensPorPagina(), that.ValorFiltro(), that);

        });
    }

    this.IniciarIrParaRegistro = function (that) {
        /// <summary>Vai para registro selecionado</summary>
        $('#' + that.idIrParaRegistro).keypress(function (evento) {

            var keycode = (evento.keyCode ? evento.keyCode : evento.which);
            //Só aciona o evento ao se aperar enter
            if (keycode == '13') {
                var registro = $(this).val();
                $(this).val('');

                that.SetarRegistro(that, registro);
            }
        });

        //$('#' + that.idIrParaRegistro).focusout(function (evento) {
        //    var registro = $(this).val();
        //    $(this).val('');
        //    that.SetarRegistro(that, registro);
        //});

        if (that.gerarBotaoIrPara) {

            $('#' + that.idBotaoIrPara).click(function () {

                var registro = $('#' + that.idIrParaRegistro).val();
                $('#' + that.idIrParaRegistro).val('');
                that.SetarRegistro(that, registro);

            });
        }
    }


    this.SetarRegistro = function (that, registro) {
        /// <summary>Leva ao registro selecionado</summary>
        that.irParaRegistro = registro != null ? registro.toString().ApenasNumeros() : '';

        if ((that.irParaRegistro == null) || (that.irParaRegistro == ""))
            return false;

        //ir para a pagina
        if (that.irParaRegistro.toString().charAt(0) != '0') {

            if (that.irParaRegistro > that.TotalPaginas || that.irParaRegistro < 1) {
                GridHelperMensagem('Página ' + that.irParaRegistro + " não localizada");
                that.irParaRegistro = 0;
                return false;
            }

            that.forcarPagina = true;
            that.RefazerLinks = true;
            that.RetornarConsulta(parseInt(that.irParaRegistro), that.ItensPorPagina(), that.ValorFiltro(), that);

        }
        else {

            this.RemoverClasseError(that.idIrParaRegistro)
            if (that.irParaRegistro < 1) {
                GridHelperMensagem('Favor informar valores inteiros maiores que 0.');
                this.AddClasseError(that.idIrParaRegistro);
                that.irParaRegistro = 0;
            }
            else if (that.irParaRegistro > that.TotalRegistros) {
                this.AddClasseError(that.idIrParaRegistro);
                GridHelperMensagem('Registro ' + that.irParaRegistro + " não localizado.");
                that.irParaRegistro = 0;
            }
            else {
                var irPara = 1;

                quantidadeRegistrosPagina = that.ItensPorPagina();
                quantidadePaginas = parseFloat(that.TotalRegistros) / parseFloat(quantidadeRegistrosPagina);

                inicial = 0;
                final = 0;

                //Incrementando a página caso seja necessário
                if (quantidadePaginas.toString().Contem('.')) {
                    var valorDecimal = quantidadePaginas.toString().split('.');
                    if (parseInt(valorDecimal[1]) > 0)
                        quantidadePaginas++;
                }

                //Localizando a página na qual se encontra o registro
                if (quantidadePaginas != Infinity) {
                    for (var p = 1; p <= quantidadePaginas; p++) {
                        inicial = (p * quantidadeRegistrosPagina) - quantidadeRegistrosPagina;
                        final = p * quantidadeRegistrosPagina;

                        if (that.irParaRegistro >= inicial && that.irParaRegistro <= final) {
                            irPara = p;
                            break;
                        }
                    }
                }
                that.forcarPagina = true;
                that.RefazerLinks = true;
                that.RetornarConsulta(irPara, that.ItensPorPagina(), that.ValorFiltro(), that);
            }
        }
    }

    //Key Up e click dos campos buscar
    this.IniciarCampoPesquisaKeyUp = function (that) {

        /// <summary>Coloca o evento no botão de pesquisa ou campo de pesquisa, dependendo do valor da propriedade criarBotaoPesquisa </summary>

        if (that.criarBotaoPesquisa) {
            $('#' + that.idBotaoGridHelperBusca).click(function () {
                that.clickFiltro = true;
                that.PesquisarTodos(that);
            });

            $('#' + that.idTextGridHelperBusca).keyup(function (e) {
                var code = e.keyCode || e.which;
                if (code == '13')
                    that.PesquisarTodos(that);
            });

        }
        else {
            $('#' + that.idTextGridHelperBusca).keyup(function (e) {
                //var code = e.keyCode || e.which;
                //sif (code == '13')
                that.PesquisarTodos(that);
            });
        }
    }

    //Método que efetua a pesquisa em todos os campos
    this.PesquisarTodos = function (that) {

        that.utilizouFiltro = true;
        that.RemoverValorBuscasIndividuais();
        that.UtilizandoFiltroGlobal = true;
        that.strNomeColunaFiltrar = "";
        that.RefazerLinks = true;
        that.RetornarConsulta(1, that.ItensPorPagina(), that.ValorFiltro(), that);

    }

    this.IniciarEventosObjetos = function (that) {

        that.IniciarSelectQuantidadeRegistrosChange(that);
        that.IniciarCampoPesquisaKeyUp(that);
        that.IniciarIrParaRegistro(that);

    }

    this.ClickDetalhamento = function (that) {
        /// <summary>Cria o evento para os detalhamentos</summary>
        $('.gridHelperDetalhamento' + that.idTabela + ' div').mouseup(function () {
            that.ControlarDetalhamento($(this));
        });
    }

    this.ControlarDetalhamento = function (objDetalhamento, metodoAbrir, metodoFechar, detalheContainerGrid) {
        /// <summary>Exibe / esconde o detalhamento e faz o controle de seu estado</summary>
        var expandido = objDetalhamento.parent().attr('data-expandido') == 'true';

        if (expandido) {
            this.RecolherDetalhamento(objDetalhamento);
            if (typeof metodoFechar != 'undefined' && metodoFechar != null)
                metodoFechar(detalheContainerGrid, objDetalhamento);
        }
        else {
            this.ExpandirDetalhamento(objDetalhamento);
            if (typeof metodoAbrir != 'undefined' && metodoAbrir != null)
                metodoAbrir(detalheContainerGrid, objDetalhamento);
        }
    }

    this.ExpandirDetalhamento = function (objDetalhamento) {

        /// <summary>Expande o detalhamento</summary>

        //controle do estado
        objDetalhamento.parent().attr('data-expandido', 'true');

        //controle daw classes
        objDetalhamento.addClass('gridHelperDetalhamentoAberto');
        objDetalhamento.removeClass('gridHelperDetalhamentoFechado');

        $('.' + objDetalhamento.parent().attr('data-detalhamento')).show();
    }

    this.RecolherDetalhamento = function (objDetalhamento) {

        /// <summary>Recolhe o detalhamento</summary>
        //controle do estado
        objDetalhamento.parent().attr('data-expandido', 'false');

        //controle das classes
        objDetalhamento.addClass('gridHelperDetalhamentoFechado');
        objDetalhamento.removeClass('gridHelperDetalhamentoAberto');


        $('.' + objDetalhamento.parent().attr('data-detalhamento')).hide();
    }

    //Métodos auxiliares

    this.GerarInformacoesRetorno = function (inicio, fim, total) {

        /// <summary>Exibe as nformações referentes a consulta</summary>

        fim = fim > this.TotalRegistros ? this.TotalRegistros : fim;

        //$('#' + this.idSpanInicio).html("&nbsp;" + inicio + "&nbsp;");
        $('#' + this.idSpanFim).html("&nbsp;" + fim + "&nbsp;");
        $('#' + this.idSpanTotal).html("&nbsp;" + total + "&nbsp; Records &nbsp;");
    }

    this.MontarObjetoRequisicao = function () {

        /// <summary>Adiciona objetos a requisição</summary>

        if ((this.ParametrosRequisicao == null) || (typeof this.ParametrosRequisicao == 'undefined')) {

            this.ParametrosRequisicao = {
                'strIrParaPagina': ""
                     , 'strQuantidadeRegistros': ""
                     , 'strFiltro': ""
                     , 'PrimeiraRequisicao': ""
                     , 'IdTabela': ""
                     , 'strNomeCampoOrdenar': ""
                     , 'tipoOrdenacao': ""
                     , 'strNomeColunaFiltrar': ""
                     , 'GerarExcel': ""
                     , 'primeiraRequisicao': ""
                     , 'baseGrid': null
            };
        }
        else {

            this.ParametrosRequisicao.strIrParaPagina = "";
            this.ParametrosRequisicao.strQuantidadeRegistros = "";
            this.ParametrosRequisicao.strFiltro = "";
            this.ParametrosRequisicao.PrimeiraRequisicao = "";
            this.ParametrosRequisicao.IdTabela = "";
            this.ParametrosRequisicao.strNomeCampoOrdenar = "";
            this.ParametrosRequisicao.tipoOrdenacao = "";
            this.ParametrosRequisicao.strNomeColunaFiltrar = "";
            this.ParametrosRequisicao.GerarExcel = "";
            this.ParametrosRequisicao.primeiraRequisicao = "";
            this.ParametrosRequisicao.gerarIrPara = "";
            this.ParametrosRequisicao.esconderContadorIrPara = "";
            this.ParametrosRequisicao.baseGrid = null;
        }

    }

    this.CompletarObjetoRequisicao = function (Pagina, QuantidadeRegistros, strFiltro) {

        /// <summary>Completa os filtros da requisição</summary>

        //Adicionando parâmetros a requisicação
        this.ParametrosRequisicao.strIrParaPagina = Pagina;
        this.ParametrosRequisicao.strQuantidadeRegistros = QuantidadeRegistros.toString();
        this.ParametrosRequisicao.strFiltro = strFiltro;
        this.ParametrosRequisicao.PrimeiraRequisicao = this.primeiraRequisicao;
        this.ParametrosRequisicao.IdTabela = this.idTabela;
        this.ParametrosRequisicao.strNomeCampoOrdenar = this.strNomeCampoOrdenar;
        this.ParametrosRequisicao.tipoOrdenacao = this.tipoOrdenacao;
        this.ParametrosRequisicao.strNomeColunaFiltrar = this.strNomeColunaFiltrar;
        this.ParametrosRequisicao.GerarExcel = this.GerarExcel;
        this.ParametrosRequisicao.primeiraRequisicao = this.primeiraRequisicao;
        this.ParametrosRequisicao.irParaRegistro = this.irParaRegistro;
        this.ParametrosRequisicao.gerarIrPara = this.gerarIrPara;
        this.ParametrosRequisicao.esconderContadorIrPara = this.esconderContadorIrPara;
        this.ParametrosRequisicao.baseGrid = $('#gridData_' + this.idTabela).val();

        try {
            if (this.objetoSerialize != null) {
                this.objetoSerialize = this.objetoSerialize.split('&');
                var obj = {};
                for (var key in this.objetoSerialize) {
                    console.log(this.objetoSerialize[key]);
                    this.ParametrosRequisicao[this.objetoSerialize[key].split("=")[0]] = this.objetoSerialize[key].split("=")[1];
                }
            }
        } catch (erro) { }

    }


    this.PossuiEventos = function () {

        /// <summary>Indica se a grid possui configurações customizadas</summary>
        return !this.isNulo(this.eventos);
    }

    this.PossuiEventoPreCarregamento = function () {

        /// <summary>Indica se a grid possui um evento de load antes a requisição ajax</summary>

        return (
                 (this.PossuiEventos())
            && (!this.isNulo(this.eventos.preCarregamento))
            );
    }

    this.PossuiEventoPosCarregamento = function () {

        /// <summary>Indica se a grid possui um evento de load após a requisição ajax</summary>
        return (
                 (this.PossuiEventos())
            && (!this.isNulo(this.eventos.posCarregamento))
            );
    }

    this.isNulo = function (obj) {
        /// <summary>Indica se um objeto é nulo</summary>
        return typeof obj == 'undefined' || obj == null;
    }

    this.Atualizar = function (MostrarMensagemTotalRegistro) {
        this.RetornarConsulta(this.PaginaCorrente, this.quantidadeRegistrosCorrente, this.strFiltroCorrente, this, MostrarMensagemTotalRegistro);
    }

    this.RetornarConsulta = function (Pagina, QuantidadeRegistros, strFiltro, that, MostrarMensagemTotalRegistro) {

        //Para ser utilizado nas telas em que é necessário atualizar a grid após a adição de um novo registro
        if (this.isNulo(MostrarMensagemTotalRegistro) || MostrarMensagemTotalRegistro) {
            MostrarMensagemTotalRegistro = true;
        }

        /// <summary>Método de pesquisa utilizado no grid</summary>
        try {
            this.RemoverClasseError(this.idTextGridHelperBusca);
            this.RemoverClasseError(this.idIrParaRegistro);
            //Indica se o usuário utilizou filtro ou paginação
            if ((!this.utilizouPaginacaoOuFiltro) && (strFiltro.length > 0) && (this.utilizouFiltro)) {

                this.utilizouFiltro = false;
                this.utilizouPaginacaoOuFiltro = false;

                if (strFiltro.length < this.minimoCaracteresPesquisa) {
                    this.AddClasseError(this.idTextGridHelperBusca);
                    this.clickFiltro = false;
                    GridHelperMensagem('Favor informar no mínimo ' + this.minimoCaracteresPesquisa.toString() + ' caracteres.');
                    return false;
                }
            }

            this.PaginaCorrente = Pagina;
            this.strFiltroCorrente = strFiltro;
            this.quantidadeRegistrosCorrente = QuantidadeRegistros;
            this.utilizouFiltro = false;
            this.utilizouPaginacaoOuFiltro = false;

            //Completa o objeto de requisição
            this.CompletarObjetoRequisicao(Pagina, QuantidadeRegistros, strFiltro);

            if (this.PossuiEventoPreCarregamento())
                this.eventos.preCarregamento.call(null, { contexto: this });

            $.ajax({
                url: this.caminhoAbsoluto + this.UrlRequisicao
                , data: this.ParametrosRequisicao
                , type: "POST"
                , async: true
                , traditional: true
                , beforeSend: function () { that.ExibirDivLoader(true); }
                , complete: function () { that.ExibirDivLoader(false); }
                , success: function (Data) {
                    var mensagem = TratarErrosRequisicao(Data);

                    if ((mensagem != "")) {
                        GridHelperMensagem(mensagem);
                        return false;
                    }

                    //Se for a primeira requisição os componentes deverão ser criados
                    that.baseGrid = Data.baseGrid;

                    //Se for a primeira requisição os componentes deverão ser criados
                    that.TryInsertDataJson(that.baseGrid);

                    //Limpando o erro
                    $('span.' + that.idTabela + 'spanNenhumElementoEncontrado').html('');

                    if (Data.toString().Contem("Algo deu errado na solicitação")) {
                        GridHelperMensagem("Algo deu errado na solicitação");
                        return false;
                    }

                    if (Data.GerarExcel) {

                        var link = document.createElement('a');
                        link.setAttribute('href', Data.Caminho);
                        document.body.appendChild(link);
                        link.click();


                    } else {

                        //Recuperando de volta o id da tabela
                        that.idTabela = Data.idTabela;

                        if (that.eventos && that.eventos.onSucesso)
                            that.eventos.onSucesso.call(null, {
                                TotalRegistros: Data.totalRegistros,
                                MostrarMensagemNaoPossuiRegistros: MostrarMensagemTotalRegistro && Data.totalRegistros < 1
                            });

                        if (Data.totalRegistros == 0) {
                            if (that.E(Data.idTBody)) {

                                //Escondendo paginação e informações
                                $('#' + that.idPaginacaoContainer).hide();
                                $('#' + that.idCampoInformacoes).hide();

                                if (that.clickFiltro) {
                                    that.clickFiltro = true;
                                    that.AddClasseError(that.idTextGridHelperBusca);
                                }
                                $('#' + Data.idTBody).html("<tr><td colspan='100'>" + "Nenhum registro encontrado." + "</td></tr>");
                            }
                            else {

                                $('#' + that.idContainerPai).html("<span class='" + that.idTabela + "spanNenhumElementoEncontrado'>" + "Nenhum registro encontrado." + "</span>");
                            }
                        }
                        else {

                            $('.' + that.idTabela + "spanNenhumElementoEncontrado").html('');

                            //Se for a primeira requisição os componentes deverão ser criados
                            if (that.primeiraRequisicao) {


                                that.IniciarComponentesGrid(that.idContainerPai);

                                that.IniciarEventosObjetos(that);

                                //Carrega o HTML na grid
                                $('#' + that.idContainerGrid).html(Data.htmlGrid);

                                //Gerando os filtros por coluna, caso existam
                                that.EventoPesquisaFiltrosIndividuais(that);

                                //Gera o click das ordenações
                                that.ClickOrdenacao(that);

                                //Gera o click do botao excel
                                that.ClickBotaoExcel(that);

                                //Indica se é a primeira requisição
                                that.primeiraRequisicao = false;

                                //Caso precise redimensionar as colunas
                                if (that.AutoRedimensionarColunas) {
                                    try {
                                        $(function () {
                                            $('#' + that.idTabela).colResizable();
                                        });
                                    } catch (err) { }
                                }
                            }
                            else {
                                //Carrega o HTML na grid
                                $('#' + Data.idTBody).html(Data.htmlCabecalhoCorpo);
                            }

                            //Atribui o click do detalhamento
                            that.ClickDetalhamento(that);

                            //Gera o mouseUp do TR
                            that.GerarMouseUpTr(that);

                            //Total de registros
                            that.TotalRegistros = Data.totalRegistros;

                            //Monta as informações do retorno
                            that.GerarInformacoesRetorno(Data.registroInicial, Data.registroFinal, that.TotalRegistros);

                            //Gerando os links de paginação
                            if (that.RefazerLinks)
                                that.CriarPaginacao(that.idContainerRodape, that.TotalRegistros, that.ItensPorPagina(), that);

                            //Exibindo os containers caso estejam desabilitados
                            $('#' + that.idPaginacaoContainer).show();
                            $('#' + that.idCampoInformacoes).show();



                            if (that.forcarPagina) {
                                that.forcarPagina = false;
                                that.irParaRegistro = 0;
                            }
                            else
                                that.LocalizarRegistro(that);
                        }
                    }
                    that.GerarExcel = false;

                    if (that.PossuiEventoPosCarregamento())
                        that.eventos.posCarregamento.call(null, { contexto: that });
                }
                , error: function (a, b, error) {
                    GridHelperMensagem('erro');
                }
            });
        } catch (err) { GridHelperMensagem(err.message); }

    }

    this.TryInsertDataJson = function (baseGrid) {
        /// <summary>Carrega os dados de estrutura da grid em um Json</summary>
        $('#gridData_' + this.idTabela).val(baseGrid);
    }

    this.GerarMouseUpTr = function (that) {
        /// <summary>Mouse up</summary>
        $('tr', '#' + that.idTabela).each(function () {
            var name = $(this).attr('name');

            if (name != null)
                $(this).click(function (event) {
                    SelectionLineTableName(name, that.gerarIrPara, that.idTabela);
                });
        });
    }

    this.LocalizarRegistro = function (that) {
        /// <summary>Seleciona novamente o registro selecionado pelo usuário</summary>
        var irPara = that.irParaRegistro;
        that.irParaRegistro = 0;

        if ((irPara != "") && (parseInt(irPara) > 0)) {
            var seletorTrRegistroCorrente = "[name=td___" + that.idTabela + irPara + "]";
            var objeto = $(seletorTrRegistroCorrente);
            if (objeto) {
                $(seletorTrRegistroCorrente).parent().click();
            }
        }
    }

    this.VariavelVazia = function (strVariavel) {
        return typeof strVariavel == 'undefined' || strVariavel == "";
    }

    this.ItensPorPagina = function () {
        /// <summary>Retorna a quantidade de itens por pagina selecionado no select</summary>
        var itensPorPagina = $('#' + this.idSelectGridHelperQuantidadeRegistros).val();
        return itensPorPagina = itensPorPagina.toUpperCase() == "TODOS" ? 0 : itensPorPagina;
    }


    this.ValorFiltro = function () {
        /// <summary>Retorna o valor do filtro escolhido</summary>

        return this.UtilizandoFiltroGlobal ? $('#' + this.idTextGridHelperBusca).val() : this.strFiltroCorrente;
    }


    this.CriarDivLoader = function () {

        /// <summary>Cria o loader da página (apenas 1 vez independente de quantas grid existirem)</summary>

        if (!document.getElementById('gridHelperGlobalLoader')) {
            var divModal = document.createElement('div');
            divModal.setAttribute('id', 'gridHelperGlobalLoader');

            var divCarregando = document.createElement('div');

            //Criando a div com o loader
            divCarregando.setAttribute('id', 'gridHelperLoaderGif')

            //Aplicando a div no modal
            divModal.appendChild(divCarregando);

            //Aplicando a div ao corpo da pagina
            document.body.appendChild(divModal);

            /*if (document.addEventListener)
            divModal.addEventListener('click', function () { this.ExibirDivLoader(false); },false);
            else if (document.attachEvent)
            document.attachEvent('onclick', function () { this.ExibirDivLoader(false); });*/

        }


    }

    this.ExibirDivLoader = function (mostrar) {

        /// <summary>Exibe / Oculta o loader da página</summary>

        try {

            if (mostrar)
                $('#gridHelperGlobalLoader').show();
            else
                $('#gridHelperGlobalLoader').hide();

        } catch (err) { GridHelperMensagem(err.mesage); }
    }
}


var SelectionLineTableName = function (lineId, substituirSpan, tableID) {

    /// <summary>Seleciona a linha da tabela</summary>

    tableIDSelector = '#' + tableID;
    var elementoObjectName = $(tableIDSelector + ' [name="' + lineId + '"]');

    if (elementoObjectName.attr('data-tabela') == tableID) {
        $('[data-tabela="' + tableID + '"]').removeClass("GridHelperSelection");
    }

    $(elementoObjectName).addClass("GridHelperSelection");

    substituirSpan = typeof substituirSpan == 'undefined' ? true : substituirSpan;

    //Selecionando o registro corrente
    if (substituirSpan) {
        var idGrid = lineId.split("___");
        var registroSelecionado = $('tr[name="' + lineId + '"] > td').html() + "&nbsp;";
        $(tableIDSelector + ' #spanGridHelperInicio' + idGrid[1]).html(registroSelecionado);
    }
}