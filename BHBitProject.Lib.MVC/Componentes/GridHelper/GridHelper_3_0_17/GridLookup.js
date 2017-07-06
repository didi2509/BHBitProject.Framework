//Lookup
var SelectorReturnField = null;

//Fecha o modal do lookup e chama o metodo responsavel pos setar o retorno   
var SelecionarValorLookup = function (id,nome) {
    $('#divModalLookup').hide();
    SelectorReturnField(id,nome);
};

//Chama a Grid e configura o lookup
var GetLookup = function (url, selectorReturnField) {

    SelectorReturnField = selectorReturnField;

    var grid = new GridHelper({
        posCarregamento: function () {

            $('tr[data-tabela="gridLookup"]').each(function () {
              
                var td = $('td:last', $(this));

                var html = $(td).html();
                if (!html) return false;
             
                var dados = html.split('(d)');
                var id = dados[0];
                var nome =dados[1];
                var btn = document.createElement('button');
              

                btn.innerHTML = "Selecionar";
                btn.setAttribute("class","btn blue");

                $(btn).off('click').click(function () {
                    SelecionarValorLookup.call(this,id, nome);
                });

                $(td).html('');
                $(td).append(btn);
            });
        }
    });

    grid.GerarGrid('gridLookup', 'divGridLookupContainer', url);
    $('#divModalLookup').show();
}