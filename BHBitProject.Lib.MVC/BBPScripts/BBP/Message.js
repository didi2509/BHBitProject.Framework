(function (BBP) {


    BBP.Message = {


        EsconderModal: function (velocidadeMaxima) {

            /// <summary>Esconde o modal</summary>

            velocidadeMaxima = ObjetoNulo(velocidadeMaxima) ? null : velocidadeMaxima;


            JEsconder("#divModalFundo", velocidadeMaxima);

            //Escondendo os botões e o rodapé
            JEsconder('#modalBtn1');
            JEsconder('#modalBtn2');
            JEsconder('#modalBtn3');
            JEsconder('#modalRodape');

            JEsconder('#divModalHeader');

            JEsconder('#divModalFundo');

            //Limpando o corpo
            $('#pConteudoCorpoModal').html('');
        }

   , ModalMensagem: function (mensagem, tituloModal, clickBotaoSair, textoBotaoSair) {

       /// <summary>Exibe o modal apenas com o botão Ok e o texto</summary>
       textoBotaoSair = ObjetoNulo(textoBotaoSair) ? "Ok" : textoBotaoSair;
       tituloModal = ObjetoNulo(tituloModal) || tituloModal == "" ? "Warning" : tituloModal;

       ExibirModal({
           titulo: tituloModal
           , conteudo: mensagem, botoes: {
               btn1: {

                   conteudo: textoBotaoSair
                        , click: function () {

                            if (clickBotaoSair != null)
                                clickBotaoSair.call();

                            EsconderModal();
                        }
               }
           }
       }, false);
   }

    , ExibirModal: function (configuracoesObjetoJson, esconderModalNoClick) {

        /// <summary>Exibe o modal</summary>
        /// <param name="configuracoesObjetoJson" type="ObjectJson">Objeto json para a montagem do modal, descrição:
        /// configuracoesObjetoJson.conteudo - Conteúdo do corpo do modal
        /// configuracoesObjetoJson.botoes - Objeto Json contendo até 3 botões 
        /// {
        /// configuracoesObjetoJson.botoes.conteudo - conteúdo do botao
        /// configuracoesObjetoJson.botoes.click - evento click do botão
        /// }
        ///</param>


        //Esconde o modal antes de mostrar, caso o mesmo já esteja presente
        EsconderModal();

        //Identifica se o modal deverá ser escondido no clique de qualquer botão
        esconderModalNoClick = typeof esconderModalNoClick == 'undefined' ? false : esconderModalNoClick;
        configuracoesObjetoJson.titulo = typeof configuracoesObjetoJson.titulo == 'undefined' ? "Warning" : configuracoesObjetoJson.titulo;
        configuracoesObjetoJson.fechar = typeof configuracoesObjetoJson.fechar == 'undefined' ? false : configuracoesObjetoJson.fechar;
        configuracoesObjetoJson.append = typeof configuracoesObjetoJson.append == 'undefined' ? false : configuracoesObjetoJson.append;
        configuracoesObjetoJson.exibirCabecalho = typeof configuracoesObjetoJson.exibirCabecalho == 'undefined' ? true : configuracoesObjetoJson.exibirCabecalho;
        configuracoesObjetoJson.appendChild = typeof configuracoesObjetoJson.appendChild == 'undefined' ? false : configuracoesObjetoJson.appendChild;

        //Exibindo o fundo do modal com todo seu conteúdo interno
        //$("#divModal").modal("show");

        //Exibindo o fundo do modal com todo seu conteúdo interno
        JMostrar("#divModalFundo");

        if (configuracoesObjetoJson.exibirCabecalho)
            $('#divModalHeader').show();

        //Setando o Conteudo do modal
        if (configuracoesObjetoJson.appendChild)
            E('pConteudoCorpoModal').appendChild(configuracoesObjetoJson.conteudo);
        else if (configuracoesObjetoJson.append)
            $('#pConteudoCorpoModal').append(configuracoesObjetoJson.conteudo);
        else
            $('#pConteudoCorpoModal').html(configuracoesObjetoJson.conteudo);

        if (!configuracoesObjetoJson.fechar)
            $("#btnClose", ".modal-header").hide();

        $("#divModalHeaderTitulo").html(configuracoesObjetoJson.titulo);

        if (!ObjetoNulo(configuracoesObjetoJson.botoes)) {

            //Exibe o rodapé
            JMostrar('#modalRodape');

            //Criando os botões caso necessário

            //Botão 1
            if (!ObjetoNulo(configuracoesObjetoJson.botoes.btn1))
                CriarBotao(configuracoesObjetoJson.botoes, 1, esconderModalNoClick);

            //Botão 2
            if (!ObjetoNulo(configuracoesObjetoJson.botoes.btn2))
                CriarBotao(configuracoesObjetoJson.botoes, 2, esconderModalNoClick);

            //Botão 3
            if (!ObjetoNulo(configuracoesObjetoJson.botoes.btn3))
                CriarBotao(configuracoesObjetoJson.botoes, 3, esconderModalNoClick);
        }

    }





    , CriarBotao: function (objetoJson, idBotao, esconderModalNoClick) {

        /// <summary>Cria um botão no modal</summary>

        var botao = null;

        switch (idBotao) {

            case 1: botao = objetoJson.btn1; break;
            case 2: botao = objetoJson.btn2; break;
            case 3: botao = objetoJson.btn3; break;
            default: botao = objetoJson.btn1; idBotao = 1; break;
        }

        //Exibindo o botão
        $('#modalBtn' + idBotao).show();

        //Texto do botão
        $('#modalBtn' + idBotao).attr({ value: ObjetoNulo(botao.conteudo) ? "Ok" : botao.conteudo });

        $('#modalBtn' + idBotao).unbind('click');

        //Click do botão
        $('#modalBtn' + idBotao).click(function () {

            //Se o click não estiver nulo ele o chama, caso contrário o botão irá fechar o modal
            if (!ObjetoNulo(botao.click)) {
                botao.click.call();

                if (esconderModalNoClick) {
                    EsconderModal(100);
                }
            }
            else EsconderModal(100);
        });
    }

    , ModalConfirmacao: function (mensagem, configuracoes, esconderModalNoClick) {

        configuracoes.conteudoSim = typeof configuracoes.conteudoSim == 'undefined' ? "Sim" : configuracoes.conteudoSim;
        configuracoes.clickSim = typeof configuracoes.clickSim == 'undefined' ? EsconderModal : configuracoes.clickSim;
        configuracoes.conteudoNao = typeof configuracoes.conteudoNao == 'undefined' ? "Não" : configuracoes.conteudoNao;
        configuracoes.clickNao = typeof configuracoes.clickNao == 'undefined' ? EsconderModal : configuracoes.clickNao;

        /// <summary>Cancelar a edição / Nova incorporação</summary>
        ExibirModal({
            conteudo: mensagem
       , botoes: {

           btn1: {
               conteudo: configuracoes.conteudoSim
                , click: configuracoes.clickSim
           }
           , btn2:
               {
                   conteudo: configuracoes.conteudoNao
                   , click: configuracoes.clickNao
               }
       }
        }, esconderModalNoClick);
    }
        , FecharModalEstatico: function (objetoFechar) {
            /// <summary>Botão fechar dos modals gerados através do helper</summary>

            $(objetoFechar).parent().parent().parent().parent().hide();
        }


    }

})(BBP);
