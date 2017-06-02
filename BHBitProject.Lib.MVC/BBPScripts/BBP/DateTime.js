//SetInterval para o preenchimento dinâmico de datas
var intervalPreenchimentoData = null;

(function (BBP) {


    BBP.DateTime = {



        CompararPrimeiraDataEMaior: function (primeiraData, segundaData) {

            if (primeiraData == "" || segundaData == "")
                return false;

            var partPrimeiraData = primeiraData.split("/");
            var primeiraDataConvertida = new Date(partPrimeiraData[2], partPrimeiraData[1] - 1, partPrimeiraData[0]);
            var partSegundaData = segundaData.split("/");
            var segundaDataConvertida = new Date(partSegundaData[2], partSegundaData[1] - 1, partSegundaData[0]);

            return primeiraDataConvertida > segundaDataConvertida;

        }

, SubtrairDatas: function (dataInicio, dataFim) {
    /// <summary>Retorna a diferença de dias entre duas datas(dataFim - dataInicio)</summary>
    /// <param name="dataInicio" type="String">Data de início</param>
    /// <param name="name" type="dataFim">Data final</param>
    var d1 = new Date(dataInicio.substr(6, 4), dataInicio.substr(3, 2) - 1, dataInicio.substr(0, 2));
    var d2 = new Date(dataFim.substr(6, 4), dataFim.substr(3, 2) - 1, dataFim.substr(0, 2));
    return Math.ceil((d2.getTime() - d1.getTime()) / 1000 / 60 / 60 / 24);
}

, RetornarDataAtual: function () {
    /// <summary>Retorna a data atual</summary>
    var hoje = new Date();
    var dia = hoje.getDate();
    var mes = hoje.getMonth() + 1 //Janeiro é 0
    var ano = hoje.getFullYear();

    if (dia < 10)
        dia = "0" + dia;

    if (mes < 10)
        mes = "0" + mes;

    hoje = dia + "/" + mes + "/" + ano;

    return hoje.toString();
}

, FormatarDataDotNet: function (data) {

    /// <summary>Formata a data vinda do .NET para dd/mm/yyyy hh:mm:ss</summary>
    try {
        data = data.replace('/Date(', '').replace(')/', '');
        var expData = new Date(parseInt(data));
        return FormatarData(expData);
    } catch (erro) {

        return "";
    }
}

, ConverterDataJSON: function (jsonDateString, retornarDataParcial) {

    /// <summary>Converte um retorno Data json em data ddMMyyyy</summary> 
    /// <param name="jsonDateString" type="Objeto">Data JSON</param> 
    /// <param name="retornarDataParcial" type="Booleano">se true retorna apenas a primeira parte da data</param> 

    if (jsonDateString == null || jsonDateString == "")
        return "";

    var data = new Date(parseInt(jsonDateString.replace('/Date(', '')));

    var yyyy = data.getFullYear().toString();
    var MM = (data.getMonth() + 1).toString();
    var dd = data.getDate().toString();

    MM = MM < 10 ? '0' + MM : MM;
    dd = dd < 10 ? '0' + dd : dd;

    if (retornarDataParcial != null && retornarDataParcial == true) {
        var hh = data.getHours().toString();
        var mm = data.getMinutes().toString();
        var ss = data.getSeconds().toString();


        hh = hh < 10 ? '0' + hh : hh;
        mm = mm < 10 ? '0' + mm : mm;
        ss = ss < 10 ? '0' + ss : ss;

        return dd + "/" + MM + "/" + yyyy;
    }

    return dd + "/" + MM + "/" + yyyy + " " + hh + ":" + mm + ":" + ss;
}
 , ValidarDataHora: function (objeto) {
     /// <summary>Valida a hora e os minutos</summary>

     var valores = objeto.value.split(":");
     var hora = valores[0];
     var minuto = valores[1];

     if ((hora > 23) || (minuto > 59)) {
         objeto.value = '';
     }
 }

  , DataAtual : function () {

     /// <summary>Formata a data para dd/mm/yyyy hh:mm:ss</summary>

     try {
         var d = new Date();
         var strData = ("00" + d.getDate()).slice(-2) + "/" +
         ("00" + (d.getMonth() + 1)).slice(-2) + "/" +
         d.getFullYear() + " " +
         ("00" + d.getHours()).slice(-2) + ":" +
         ("00" + d.getMinutes()).slice(-2) + ":" +
         ("00" + d.getSeconds()).slice(-2);

         return strData.toString();
     }
     catch (erro) {
         alert(erro.message);
     }
 }

 , FormatarData : function (d) {

    /// <summary>Formata a data para dd/mm/yyyy hh:mm:ss</summary>

    return ("00" + d.getDate()).slice(-2) + "/" +
    ("00" + (d.getMonth() + 1)).slice(-2) + "/" + d.getFullYear() + " " +
    ("00" + d.getHours()).slice(-2) + ":" +
    ("00" + d.getMinutes()).slice(-2) + ":" +
    ("00" + d.getSeconds()).slice(-2);

}





    };

})(BBP);
