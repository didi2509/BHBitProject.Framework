


(function (BBP) {


    BBP.Ajax = {

        Init: function ()
        {

            $.ajaxSetup({

                beforeSend: function () {
                    OpenLoader();
                },
                complete: function () {
                    CloseLoader();
                },

                error: function (jqXHR, textStatus, errorThrown) {
                    var codeStatus = parseInt(jqXHR.status);

                    switch (codeStatus) {
                        case 401: RedirecionarParaErro(401); break;
                        case 404: RedirecionarParaErro(404); break;
                        case 407: RedirecionarParaErro(407); break;
                        case 408: RedirecionarParaErro(408); break;
                        case 503: RedirecionarParaErro(503); break;
                        case 500: RedirecionarParaErro(500); break;
                        case 505: RedirecionarParaErro(505); break;
                        default: alert(textStatus);
                    }
                }
            });
        }

       , TratarErrosRequisicao: function (data) {

            /// <summary>Efetua o tratamento dos erros gerados durante uma requisição ajax padronizada</summary>
            try {
                if (!data.Sucesso) {

                    var mensagem = "";

                    //Verificando se existe uma mensagem pai
                    if (!ObjetoNulo(data.Mensagem))
                        mensagem = data.Mensagem;


                    //Verificando as mensagens de validação
                    if (!ObjetoNulo(data.MensagensValidacao)) {

                        var contadorMensagens = data.MensagensValidacao.length;

                        if (!ObjetoNulo(data.Mensagem))
                            mensagem += "<br />";

                        if (contadorMensagens > 0)
                            for (var i = 0; i < contadorMensagens; i++)
                                mensagem += "<br />" + data.MensagensValidacao[i];

                        if (ObjetoNulo(data.Mensagem))
                            mensagem = mensagem.substr(6, mensagem.length - 1);
                    }

                    //Remove a classe de erro de todos os elementos
                    RemoverClasseErrorAuto();

                    //Marcando os campos
                    if (!ObjetoNulo(data.CamposValidacaoSplit))
                        AddClasseError(data.CamposValidacaoSplit);

                    return mensagem;
                }
            } catch (error) {
                alert(error.message);
            }
            return "";
        }


        , Execute: function (url, dados, configuracoes, configuracoesExtra) {

            /// <summary>Criação - Diogo de Freitas -  Efetua uma requisição ajax</summary>
            /// <param name="url" type="String">Url da action</param>
            /// <param name="dados" type="Json">dados a serem enviados</param>
            /// <param name="configuracoes" type="Objeto"> 
            /// ( onSucesso String  passar function(data){ metodo } )
            /// ( tipo String POST ou GET )
            /// ( assincrono Boolean true ou false )
            /// ( onErro String metodo )
            /// ( caminhoAbsoluto String Se passado o método irá ignorar o caminho absoluto padrão )  </param>
            /// ( tratarErros bool Indica se o método irá tratar os erros vindos do objeto Json correspondente a classe ObjetoJson da BLL

            if (typeof configuracoes == 'undefined')
                configuracoes = {};

            //Parâmetros opcionais
            configuracoes.tipo = typeof configuracoes.tipo == 'undefined' ? "POST" : configuracoes.tipo;
            dados = typeof dados == 'undefined' ? null : dados;
            configuracoes.tipoDado = typeof configuracoes.tipoDado == 'undefined' ? "json" : configuracoes.tipoDado;
            configuracoes.tipoConteudo = typeof configuracoes.tipoConteudo == 'undefined' ? "application/json" : configuracoes.tipoConteudo;
            configuracoes.onSucesso = typeof configuracoes.onSucesso == 'undefined' ? null : configuracoes.onSucesso;
            configuracoes.onErro = typeof configuracoes.onErro == 'undefined' ? null : configuracoes.onErro;
            configuracoes.assincrono = typeof configuracoes.assincrono == 'undefined' ? true : configuracoes.assincrono;
            url = typeof configuracoes.caminhoAbsoluto == 'undefined' ? absolutePath + url : configuracoes.caminhoAbsoluto + url;
            configuracoes.stringify = typeof configuracoes.stringify == 'undefined' ? false : configuracoes.stringify;
            configuracoes.tratarErros = typeof configuracoes.tratarErros == 'undefined' ? true : configuracoes.tratarErros;
            configuracoes.removerContentType = typeof configuracoes.removerContentType == 'undefined' ? true : configuracoes.removerContentType;
            configuracoes.removerDataType = typeof configuracoes.removerDataType == 'undefined' ? false : configuracoes.removerDataType;

            var requisicao = {
                url: url
                   , async: configuracoes.assincrono
                   , type: configuracoes.tipo
                //, dataType: configuracoes.tipoDado
                   , data: configuracoes.stringify ? JSON.stringify(dados) : dados
                   , success: function (data) {

                       if (configuracoes.tratarErros) {
                           var mensagem = BBP.Ajax.TratarErrosRequisicao(data);

                           if ((mensagem != "")) {
                               BBP.Message.ModalMensagem(mensagem);

                               if (configuracoes.onErro && typeof configuracoes.onErro == 'function')
                                   configuracoes.onErro.call(undefined, data);
                           }
                           else if (configuracoes.onSucesso)
                               configuracoes.onSucesso.call(undefined, data);
                       }
                       else if (configuracoes.onSucesso)
                           configuracoes.onSucesso.call(undefined, data);

                   }
            };

            if (!configuracoes.removerContentType)
                requisicao.contentType = configuracoes.tipoConteudo;

            if (!configuracoes.removerDataType)
                requisicao.dataType = configuracoes.tipoDado;

            //Copiando os valores extra
            if (configuracoesExtra)
                for (var p in configuracoesExtra)
                    requisicao[p] = configuracoesExtra[p];

            notificarErro(function () {
                $.ajax(requisicao);
            }, "RequisiçãoAjax", false);
        }
    }
})(BBP);
