(function (BBP) {


    BBP.JSON = {

        MontarJSON: function (seletor) {
            /// <summary>Retorna um objeto json de acordo com o seletor pai estabelecido</summary> 

            var jsonRetorno = {};
            $('input,select,textarea, div.valueJSON', $(seletor)).each(function () {

                try {
                    //Propriedade a ser criada no JSON 
                    var propriedadeJson = '';
                    //Propriedade valor 
                    var nameJSON = $(this).attr('data-name-JSON');
                    //Se a propriedade nao existir irá buscar o ID 
                    if (typeof nameJSON != 'undefined' && nameJSON != null && nameJSON != '') {
                        propriedadeJson = nameJSON;
                    }
                    else {
                        var id = $(this).attr('id');
                        //Se o ID não existir irá buscar o name 
                        if (typeof id != 'undefined' && id != null && id != '') {
                            propriedadeJson = id;
                        }
                        else {
                            //Se o name n existir a buca não será efetuada 
                            var name = $(this).attr('name');
                            if (typeof name != 'undefined' && name != null && name != '')
                                propriedadeJson = name;
                        }
                    }
                    var valor = "";
                    //Se o objeto tiver a classe valueJSON, o valor será o HTML interno, se não será pego a propriedade val 

                    if ($(this).hasClass('valueJSON')) {
                        valor = $(this).html() != 'undefined' ? $(this).html() : "";
                    }
                    else {
                        valor = $(this).val() != 'undefined' ? $(this).val() : "";
                        if ($(this).attr('type') == 'checkbox') {
                            valor = $(this).is(':checked').toString();
                        }
                    }
                    //Setando a propriedade 
                    // if (typeof String.prototype.replaceAll != 'undefined')
                    //   jsonRetorno[propriedadeJson.replaceAll('.', '_')] = valor;
                    //else
                    jsonRetorno[propriedadeJson] = valor;
                } catch (e) {
                    console.log(e.message);
                }
            });
            return jsonRetorno;
        }
    , MontarJSONListaCSharp: function (seletor) {
        /// <summary>Retorna um objeto json de acordo com o seletor pai estabelecido</summary> 
        /// <param name="seletor" type="String">Nome da classe a ser varrida</param> 
        /// <param name="nomePropriedade" type="String">Nome da propriedade de saída do objeto JSON</param> 
        var vetorJSON = [];
        $(seletor).each(function () {
            vetorJSON.push(MontarJSON($(this)));
        });
        return JSON.stringify(vetorJSON);
    }
    , MontarJSONLista: function (seletor, nomePropriedade) {
        /// <summary>Retorna um objeto json de acordo com o seletor pai estabelecido</summary> 
        /// <param name="seletor" type="String">Nome da classe a ser varrida</param> 
        /// <param name="nomePropriedade" type="String">Nome da propriedade de saída do objeto JSON</param> 
        nomePropriedade = typeof nomePropriedade == 'undefined' ? 'objeto' : nomePropriedade;
        var jsonRetornoLista = {};
        var contador = 0;
        $(seletor).each(function () {
            contador++;
            jsonRetornoLista[nomePropriedade + (contador.toString())] = MontarJSON($(this));
        });
        return jsonRetornoLista;
    }

    , MontarJSONBasic: function (seletor) {

        /// <summary>Monta um objeto JSON dos campos através de seu ID</summary>

        var jsonRetorno = {};

        $('input,select', seletor).each(function () {

            var elementoCorrente = $(this);

            var valorCorrente = elementoCorrente.prop('data-JSONHTML') ? elementoCorrente.html() : elementoCorrente.val();

            if (elementoCorrente.prop('data-JSONNome'))
                jsonRetorno[elementoCorrente.prop('data-JSONNome')] = valorCorrente;
            else
                jsonRetorno[elementoCorrente.prop('id')] = valorCorrente;

        });

        return jsonRetorno;
    }


    }

})(BBP);
