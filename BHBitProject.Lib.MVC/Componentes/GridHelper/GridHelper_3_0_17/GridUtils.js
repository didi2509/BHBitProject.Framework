//Lookup ----------------------------------------------------------------------------------------------
var SelectorReturnField = null;

//Fecha o modal do lookup e chama o metodo responsavel pos setar o retorno   
var SelecionarValorLookup = function (id) {
    $('#divModalLookup').hide();
    SelectorReturnField(id);
};

//Chama a Grid e configura o lookup
var GetLookup = function (url, selectorReturnField) {

    SelectorReturnField = selectorReturnField;

    var grid = new GridHelper({
        posCarregamento: function () {

            $('tr[data-tabela="gridLookup"]').each(function () {

                var td = $('td:last', $(this));
                var id = $(td).html();
                var btn = document.createElement('button');

                btn.innerHTML = "Selecionar";
                $(btn).click(function () {
                    SelecionarValorLookup(id);
                });

                $(td).html('');
                $(td).append(btn);
            });
        }
    });

    grid.GerarGrid('gridLookup', 'divGridLookupContainer', url);
    $('#divModalLookup').show();
}