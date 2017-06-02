/// <reference path="DropDowns.js" />
/*Utils drop downs*/

var selecioneText = "-- Selecione --";
var selecioneValue = "";

(function (BBP) {

    BBP.Componentes.DropDown =
    {

     AddOption : function (id, text, value) {

            /// <summary>Adiciona um option em um select</summary>
            var select = E(id);
            var itens = select.options.length;
            select.options[itens] = new Option(text, value);
        }

, RetornarOption : function (idSelect, indice) {

    /// <summary>Retorna um option de acordo com a posicao</summary>

    var selectEncontrado = E(idSelect);
    var optionRetorno = null;

    if (selectEncontrado) {
        optionRetorno = selectEncontrado.options[indice];
    }

    return optionRetorno;
}

    ,OptionSelecionado : function (idSelect) {

        /// <summary>Retorna um option selecionado em um select</summary>
        var selectEncontrado = E(idSelect);
        var optionRetorno = "";

        if (selectEncontrado) {
            var quantidadeOptions = selectEncontrado.options.length;

            for (var i = 0; i < quantidadeOptions; i++) {

                if (selectEncontrado.options[i].selected) {
                    optionRetorno = selectEncontrado.options[i];
                    break;
                }
            }
        }
        return optionRetorno;
    }

    , OptionSelecionadoValue : function (idSelect)
    {
        return OptionSelecionado(idSelect).value;
    }

    ,SelecionarOption : function (idSelect, valor) {

        /// <summary>Retorna um option selecionado em um select</summary>
        if ((valor == '0') || (valor == ""))
            return false;

        var selectEncontrado = E(idSelect);
        var optionRetorno = null;
    
        if (selectEncontrado) {
            var quantidadeOptions = selectEncontrado.options.length;
   
            for (var i = 0; i < quantidadeOptions; i++) {

                if (selectEncontrado.options[i].value == valor) {
                    $('option:nth-child(' + (i + 1) + ')', selectEncontrado).attr({ selected: true });
                    selectEncontrado.options[i].setAttribute('selected', 'true');
                    optionRetorno = selectEncontrado.options[i];
                    $('#' + idSelect).val(valor);
                    break;
                }
            }
        }
        return optionRetorno;
    }

   , DropDownEnable : function (id, enable, zerar) {
        /// <summary>habilita/desabilita um drop down</summary>
        zerar = ObjetoNulo(zerar) ? false : zerar;

        var split = id.split(',');
        var itens = split.length;

        if (zerar)
            ZerarDropDown(id);

        for (var i = 0; i < itens; i++)
            $('#' + split[i]).attr('disabled', !enable);

    }



    , SelecionarOptionPorPosicao : function (idSelect, index) {
        /// <summary>Seleciona uma posição no select</summary>

        var selectEncontrado = E(idSelect);

        if (selectEncontrado) {
            if ((selectEncontrado.options.length - 1) >= index)
                $('option:nth-child(' + (index + 1) + ')', selectEncontrado).attr({ selected: true });

        }
    }

    ,SetarPrimeiroValorDropDown : function (id) {
        /// <summary>Seta o primeiro valor do dropdown</summary>
        $('#' + id).select2('val', "");
    }

    ,ZerarDropDown : function (ids) {
        /// <summary>Zera o dropDown</summary>

        var split = ids.split(',');
        var itens = split.length;

        for (var i = 0; i < itens; i++) {

            var id = split[i];

            $('#' + id).html('');

            var option = document.createElement('option');
            option.setAttribute("value", selecioneValue);
            option.innerHTML = selecioneText;

            var select = E(id);
            select.options[0] = option;

            SetarPrimeiroValorDropDown(id);
        }
    }

    ,MontarDropDown : function (id, fonte, valorSelecionado, propriedadeValor, propriedadeNome) {
        /// <summary>Description</summary>
        if (ObjetoNulo(propriedadeValor))
            propriedadeValor = "Value";

        if (ObjetoNulo(propriedadeNome))
            propriedadeNome = "Text";

        //reseta o dropdown
        $('#' + id).html('');

        var select = E(id);

        var itens = fonte.length;

        for (var i = 0; i < itens; i++) {
            var option = document.createElement('option');
            option.setAttribute("value", fonte[i][propriedadeValor]);
            option.innerHTML = fonte[i][propriedadeNome];
     
            if ((fonte[i][propriedadeValor] == "disabled") || (fonte[i][propriedadeValor] == "0"))
                option.setAttribute('disabled', 'true');
            select.options[select.options.length] = option;
        }

        if (ObjetoNulo(valorSelecionado) || valorSelecionado == "0")
            $('#' + id).val('');
        else
            $('#' + id).val(valorSelecionado);

    }

    ,MontarDropDownAno : function (id) {
        /// <summary>Monta o dropdown com os anos dos carros</summary>
        var anoFinal = new Date().getFullYear();

        var source = new Array();

        source.push({ Text: selecioneText, Value: selecioneValue });

        for (var i = anoFinal; i >= 1930; i--)
            source.push({ Value: i, Text: i });

        MontarDropDown(id, source);
    }

    , MontarDropDownCidade:function (siglaUF, idDropDown, selecionarValue) {
        if (siglaUF != "") {
            RequisicaoAjax("Home/DropDownCidades", { id: siglaUF }, {
                assincrono: false,
                onSucesso: function (data) {
                    MontarDropDown(idDropDown, data.Dados, selecionarValue);
                    DropDownEnable(idDropDown, true);
                }
            });
        }
    }


     , SelecionarValorDropdown : function (idDropDown, valor) {
            /// <summary>Muda o valor para selecionado no select</summary>

        $('#' + idDropDown + ' option[value=' + valor + ']').attr('selected', 'selected');
    }



    }
})(BBP);

