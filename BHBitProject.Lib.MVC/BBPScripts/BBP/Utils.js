//Prefixo utilizado para criar as chaves no localStorage
var prefixoLocalStorage = "Hanitta_";
//Tempo padrão para os FadeOuts do sistem
var TempoFadeOutPadrao = 4000;






(function (BBP) {


    BBP.Utils = {

        HabilitarCampo: function (seletor, habilitar) {
            /// <summary>Habilita / desabilita um campo</summary>
            habilitar = typeof habilitar == 'undefined' ? true : habilitar;

            if (habilitar)
                $(seletor).removeAttr('readonly').removeClass('desabilitado');
            else
                $(seletor).attr('readonly', 'readonly').addClass('desabilitado');
        }


        //Local Storage **************************************************************************************


  , setLS: function (chave, valor) {

      /// <summary>Criação - Diogo de Freitas -  Seta o valor de uma chave no localStorage</summary>
      notificarErro(
      function () {

          if ((!ObjetoNulo(chave)) && (!ObjetoNulo(valor))) {

              localStorage.setItem(prefixoLocalStorage + chave, valor);
          }
      }, "setLS");
  }

    , getLS: function (chave, removerItem) {

        /// <summary>Criação - Diogo de Freitas -  Obtém o valor de um campo no local storage</summary>
        /// <param name="chave" type="String">Chave no local storage</param>
        /// <param name="removerItem" type="Boolean">Indica se após a execução do método o valor deve ser removido do local storage</param>

        return notificarErro(
        function () {

            chave = prefixoLocalStorage + chave;

            if (localStorage[chave] != null) {

                removerItem = ObjetoNulo(removerItem) ? false : removerItem;
                retornarFalseCasoNul = ObjetoNulo(removerItem) ? true : removerItem;

                var localStorageValor = localStorage.getItem(chave);

                if (removerItem) {
                    localStorage.removeItem(chave);
                }

                return localStorageValor;
            }
            else return null;
        }, "getLS");
    }

    , ClearAllLocalStorage: function () {
        /// <summary>Limpa todo o conteúdo armazenado no localStorage do Browser</summary>
        localStorage.clear();
    }

        //Local Storage **************************************************************************************




        //AJAX ***********************************************************************************************

  , RedirecionarParaErro: function (codigoErro) {
      /// <summary>Criação - Diogo de Freitas -  Redireciona o usuário para a página de erro</summary>
      location.href = absolutePath + 'Home/ErrorStatus/' + codigoErro;
  }






        //UTILITARIOS  ***************************************************************************************

    , rolarScroll: function (elementId) {
        /// <summary>Id do elemento que será utilizado como ancora(Não passar com o parâmetro '#')</summary>

        if (elementId == "topo_pagina") {
            $('html, body').animate({
                scrollTop: 0
            }, 1000);
            return;
        }

        $('html, body').animate({
            scrollTop: $("#" + elementId).offset().top - 37
        }, 1000);
    }

   , getURLParameter: function (name) {
       ///<summary>Obtem o parametro da url via get</summary>

       return decodeURI((RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]);
   }



     , notificarErro: function (metodo, nomeMetodo, silenciarErro) {

         /// <summary>Criação - Diogo de Freitas -  Notifica erros em métodos</summary>

         try {
             if ((metodo != null) && (typeof metodo == 'function'))
                 return metodo.call();
         }
         catch (erro) {

             silenciarErro = ObjetoNulo(silenciarErro) ? false : silenciarErro;

             if (!silenciarErro) {
                 ModalMensagem('Erro\n\n' + erro.message + '\n\nno método:\n' + nomeMetodo + "\n\n" + metodo.toString())
             }
         }
     }


    , JEsconder: function (seletor) {
        /// <summary>Esconde um elemento</summary>
        $(seletor).hide();
    }

    , JMostrar: function (seletor) {
        /// <summary>Exibe um elemento</summary>
        $(seletor).show();
    }

        /*JQuery*/
    , JEsconderSlide: function (seletor, velocidade) {
        /// <summary>Esconde um elemento</summary>

        if (ObjetoNulo(velocidade))
            $(seletor).slideUp();
        else
            $(seletor).slideUp(velocidade);
    }

    , JMostrarSlide: function (seletor) {
        /// <summary>Exibe um elemento</summary>
        $(seletor).slideDown();
    }

    , JToogle: function (seletor, slideToogle) {

        slideToogle = typeof slideToogle == 'undefined' ? false : slideToogle;

        /// <summary>Efetua um toogle no elemento</summary>

        if (slideToogle)
            $(seletor).slideToggle();
        else
            $(seletor).toggle();

    }

    , SetarErro: function (erro, pai, exibirBotaoFechar) {

        /// <summary>Criação - Diogo de Freitas -  Dispara um erro na tela</summary>
        /// <param name="erro" type="String">Descrição do erro</param>
        /// <param name="pai" type="String">Id do elemento pai no qual o erro irá ser gerado</param>
        /// <param name="exibirBotaoFechar" type="Boolean">Se true o erro será exibido junto ao botão de fechar e não terá fadeout, se não informado o padrão será false e o alerto é exibido no modo fadeout sem o botão fechar</param>

        //Se o pai vier nulo
        pai = typeof pai == 'undefined' ? 'containerErrosAutoCity' : pai;


        exibirBotaoFechar = typeof exibirBotaoFechar == 'undefined' || exibirBotaoFechar == null ? false : exibirBotaoFechar;

        divErro = null;

        //Se o objeto pai nao existir, um outro pai será definido
        if (!document.getElementById(pai))
            pai = 'containerErrosAutoCity';

        //Limpando o conteúdo interno de um pai
        document.getElementById(pai).innerHTML = '';

        //Se a div de erro nao existir então a mesma será criada
        if (!document.getElementById('AutoCitysetarErroFunction')) {

            var divErro = document.createElement('div');
            divErro.setAttribute('id', 'AutoCitysetarErroFunction');
            divErro.setAttribute('class', 'alert alert-error');

            var conteudoSpan = '<span id="AutoCitysetarErroSpan">' + erro + '</span>';
            divErro.innerHTML = exibirBotaoFechar ? '<button data-dismiss="alert" class="close"></button>' + conteudoSpan : conteudoSpan;

            document.getElementById(pai).appendChild(divErro);

        }
        else //Setando erros futuros, depois da criação da div de erros
            document.getElementById('AutoCitysetarErroSpan').innerHTML = erro;

        if (!exibirBotaoFechar) {

            //Resetar o controle
            if ($("#" + pai).is(':animated'))
                $("#" + pai).stop().animate({ opacity: '100' });

            $("#" + pai).show().fadeOut(TempoFadeOutPadrao);
        }
    }

        , IniciarPreenchimento: function (ids, preenchimentoUnico) {

            /// <summary>Coloca o tempo nas datas</summary>
            var tempo = 1;
            var idCamposData = ids.split(',');
            var itens = idCamposData.length;

            preenchimentoUnico = typeof preenchimentoUnico == 'undefined' ? true : preenchimentoUnico;

            if (preenchimentoUnico) {

                for (var i = 0; i < itens; i++)
                    E(idCamposData[i]).value = DataAtual();
            }
            else {
                intervalPreenchimentoData = setInterval(function () {

                    for (var i = 0; i < itens; i++)
                        E(idCamposData[i]).value = DataAtual();

                }, tempo);
            }
        }

     , retiraMascaraCpfCnpj: function (valor) {
         if (typeof valor === "undefined")
             return;

         if (valor != null && valor != "")
             return valor.replace(/[^0-9]/g, "");
     }


    , Habilitar: function (id, habilitar) {

        /// <summary>Controla o status Disabled de um componente</summary>

        if (habilitar)
            E(id).removeAttribute('disabled');
        else
            E(id).setAttribute('disabled', true);

    }

    , Redirecionar: function (url, utilizarAbsolutePath, novaAba) {

        /// <summary>Redireciona a url</summary>

        utilizarAbsolutePath = typeof utilizarAbsolutePath == 'undefined' ? true : utilizarAbsolutePath;

        novaAba = typeof novaAba == 'undefined' ? false : novaAba;

        var caminhoRedirecionamento = utilizarAbsolutePath ? absolutePath + url : url;

        if (!novaAba) {
            location.href = caminhoRedirecionamento; //utilizarAbsolutePath ? absolutePath + url : url;
        }
        else {
            window.open(caminhoRedirecionamento, "_blank");
        }

    }

    , DetalhesErro: function (objetoErro, exibirAlerta) {
        /// <summary>Exibe os detalhes de erro de um método(Usar dentro do método Catch(e))</summary> 
        /// <param name="objetoErro" type="Objeto">Objeto do erro</param> 
        /// <param name="exibirAlerta" type="Booleano">Exibe um alert com a descrição do erro</param> 

        var errorDetail = "DETALHAMENTO DO ERRO\n\n" + "Arquivo - " + objetoErro.fileName + "\n Linha - " + objetoErro.lineNumber + "\n" + objetoErro.toString();


        if (exibirAlerta)
            alert(errorDetail);
        else
            console.log(errorDetail);

    }

    , DetalhesObjeto: function (objeto) {
        /// <summary>Exibe informações do objeto utilizando reflection</summary>

        var info = "";

        for (var prop in objeto)
            info += prop + " : " + objeto[prop] + "<br />";

        ModalMensagem(info);
    }

    , LimparCampos: function (obj) {
        /// <summary>Limpa todos os inputs e selects dentro do contexto de um objeto</summary>

        var filhos = obj.children.length;

        for (var i = 0; i < filhos; i++) {
            try {
                if ((obj.children[i].children != null) && (obj.children[i].children.length > 0))
                    LimparCampos(obj.children[i]);

                if (typeof obj.children[i].type != 'undefined') {

                    var tipo = obj.children[i].type.toString().Igualar();

                    if (tipo == 'text'.Igualar())
                        obj.children[i].value = "";
                    else if ((tipo == 'checkbox'.Igualar()) || (tipo == 'radio'.Igualar()))
                        obj.children[i].checked = false;

                }
                else if (obj.children[i].tagName.Igualar() == 'select'.Igualar()) {
                    E(obj.children[i].id).options[0].selected = true;
                }
            } catch (erro) {
                continue;
            }
        }
    }

    , RetornarValorRadioSelecionado: function (contexto, tipoContexto) {
        /// <summary>Retorna o valor do RadioButton selecionado</summary> 
        /// <param name="contexto" type="String">Container onde os radio button's se encontram</param> 
        /// <param name="tipoContexto" type="String">Se é do tipo class ou id</param> 

        var valorRadioSelecionado = "";

        switch (tipoContexto) {
            case "id":
                $(":radio", $("#" + contexto)).each(function (i, item) {
                    if (item.checked)
                        valorRadioSelecionado = item.value;
                });
                break;

            case "class":
                $(":radio", $("." + contexto)).each(function (i, item) {
                    if (item.checked)
                        valorRadioSelecionado = item.value;
                });
                break;
            default:
                $(":radio").each(function (i, item) {
                    if (item.checked)
                        valorRadioSelecionado = item.value;
                });
        }

        return valorRadioSelecionado;
    }



    , removerAcentos: function (newStringComAcento) {
        var string = newStringComAcento;
        var mapaAcentosHex = {
            a: /[\xE0-\xE6]/g,
            e: /[\xE8-\xEB]/g,
            i: /[\xEC-\xEF]/g,
            o: /[\xF2-\xF6]/g,
            u: /[\xF9-\xFC]/g,
            c: /\xE7/g,
            n: /\xF1/g
        };

        for (var letra in mapaAcentosHex) {
            var expressaoRegular = mapaAcentosHex[letra];
            string = string.replace(expressaoRegular, letra);
        }

        return string;
    }

    , RemoverElemento: function (array, elemento) {
        var itens = array.length;

        for (var i = 0; i < itens; i++) {
            if (array[i] == elemento) {
                array.pop(i);
                break;
            }
        }

    }

    , ArraySplit: function (array, separador) {
        var itens = array.length;
        var retorno = "";
        if (ObjetoNulo(separador)) separador = ",";

        for (var i = 0; i < itens; i++)
            retorno += i == 0 ? array[i] : separador + array[i];

        return retorno;
    }

        // Modifica o CSS do campo caso ele seja INVÁLIDO ou SUCESSO.
    , InputValidadoCSS: function (idCampo, sucesso, texto) {
        var campos = idCampo.split(',');
        var itens = campos.length;
        texto = texto == null ? '' : texto;
        for (var i = 0; i < itens; i++) {

            var input = $('#' + campos[i]);

            if (sucesso) {
                $(input).removeClass("input-validation-error");
                $(input).addClass("valid");

                $(input).next("span").removeClass("input-validation-error-text field-validation-error");
                $(input).next("span").empty().text(texto);

                $("form").unbind('submit').submit()


            } else {
                $(input).removeClass("valid");
                $(input).addClass("input-validation-error");

                $(input).next("span").addClass("input-validation-error-text field-validation-error");
                $(input).next("span").empty().text(texto);

                $("form").submit(function (event) {
                    event.preventDefault();
                });

                $(input).focus();
            }
        }
    }

    , CamposEnable: function (id, enable) {
        /// <summary>habilita/desabilita um drop down</summary>

        var split = id.split(',');
        var itens = split.length;

        for (var i = 0; i < itens; i++)
            $('#' + split[i]).attr('disabled', !enable);
    }

    , CamposEnableClasse: function (seletor, enable) {
        /// <summary>habilita/desabilita um drop down</summary>

        $(seletor).attr('disabled', !enable);
    }

    , CamposReadonlyClasse: function (seletor, readonly) {
        /// <summary>habilita/desabilita um drop down</summary>

        if (readonly)
            $(seletor).attr('readonly', 'readonly');
        else
            $(seletor).removeAttr('readonly');
    }

    , CampoReadonly: function (id, readonly) {
        /// <summary>habilita/desabilita um drop down</summary>

        if (readonly)
            $('#' + id).attr('readonly', 'readonly');
        else
            $('#' + id).removeAttr('readonly');
    }



    , OpenLoader: function () {
        $('#divLoader').show();
    }


    , AutoOpenCloseLoader: function (time, method) {

        /// <summary>Dispara o loader durante um tempo específico</summary>

        if (time < 100) time = 100;

        var tempo = 0;

        $('#divLoader').show();

        var intervalo = setInterval(function () {
            tempo += 100;

            if (tempo > time) {
                CloseLoader();

                if (typeof method == 'function' && method != null)
                    method.call(null);

                clearInterval(intervalo);
            }

        }, 100);

    }
    , CloseLoader: function () {
        $('#divLoader').hide();
    }

    };

})(BBP);
