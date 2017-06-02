/*Javascript - Métodos de Extensão*/
String.prototype.replaceAll = function (termoSubstituir, novoTermo) {
    /// <summary>Substitui um termo pelo outro</summary>

    novoTermo = typeof novoTermo == 'undefined' ? '' : novoTermo;
    var expressao = new RegExp(termoSubstituir, 'g');
    return this.replace(expressao, novoTermo).replace(termoSubstituir, novoTermo);
}

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

String.prototype.Remover = function (strRemover) {

    /// <summary>Diogo de Freitas Nunes - Remove os caractéres em uma string</summary>
    return this.replaceAll(strRemover, '');

}

String.prototype.Igualar = function (removerTodosEspacos) {

    /// <summary>Diogo de Freitas Nunes - Padroniza uma string para se efetuar uma comparação</summary>

    removerTodosEspacos = typeof strFormaDeIngresso == 'undefined' ? true : strFormaDeIngresso;

    //EMBORA PARECAM IGUAIS OS CARACTÉRES DO REPLACE DO ESPAÇO ' ' SÃO **DIFERENTES**
    return removerTodosEspacos ? this.replaceAll(' ', '').replaceAll(' ', '').replace(' ','').replace(' ','').toUpperCase().removerAcentos(true) : this.toUpperCase().removerAcentos(true);
}

String.prototype.removerAcentos = function (upper) {

    /// <summary>Remove os acentos</summary>
    upper = typeof upper == 'undefined' ? false : upper;

    var strReplace = this.replaceAll('Á', 'A')
               .replaceAll('É', 'E')
               .replaceAll('Í', 'I')
               .replaceAll('Ó', 'O')
               .replaceAll('Ú', 'U')
               .replaceAll('Â', 'A')
               .replaceAll('Ê', 'E')
               .replaceAll('Î', 'I')
               .replaceAll('Ô', 'O')
               .replaceAll('Û', 'U')
               .replaceAll('Ã', 'A')
               .replaceAll('Õ', 'O')
               .replaceAll('À', 'A')
               .replaceAll('È', 'E')
               .replaceAll('Ì', 'I')
               .replaceAll('Ò', 'O')
               .replaceAll('Ù', 'U')
               .replaceAll('Ç', 'C');

    return upper ? strReplace : strReplace.replaceAll('á', 'a')
               .replaceAll('é', 'e')
               .replaceAll('í', 'i')
               .replaceAll('ó', 'o')
               .replaceAll('ú', 'u')
               .replaceAll('â', 'a')
               .replaceAll('ê', 'e')
               .replaceAll('î', 'i')
               .replaceAll('ô', 'o')
               .replaceAll('û', 'u')
               .replaceAll('ã', 'a')
               .replaceAll('õ', 'o')
               .replaceAll('à', 'a')
               .replaceAll('è', 'e')
               .replaceAll('ì', 'i')
               .replaceAll('ò', 'o')
               .replaceAll('ù', 'u')
               .replaceAll('ç', 'c');

}

String.prototype.val = function (valor) {

    /// <summary>Retorna ou seta o valor de um objeto</summary>

    var str = this;

    if (typeof valor != 'undefined') {
        jQuery('#' + str.Remover('#')).val(valor);
        return valor;
    }
    else
        return jQuery('#' + str.Remover('#')).val();

}

String.prototype.extenso = function (c) {

    /// <summary>Escreve um número por extenso</summary>

    var ex = [
        ["zero", "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove"],
        ["dez", "vinte", "trinta", "quarenta", "cinqüenta", "sessenta", "setenta", "oitenta", "noventa"],
        ["cem", "cento", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos"],
        ["mil", "milhão", "bilhão", "trilhão", "quadrilhão", "quintilhão", "sextilhão", "setilhão", "octilhão", "nonilhão", "decilhão", "undecilhão", "dodecilhão", "tredecilhão", "quatrodecilhão", "quindecilhão", "sedecilhão", "septendecilhão", "octencilhão", "nonencilhão"]
    ];
    var a, n, v, i, n = this.replace(c ? /[^,\d]/g : /\D/g, "").split(","), e = " e ", $ = "real", d = "centavo", sl;
    for (var f = n.length - 1, l, j = -1, r = [], s = [], t = ""; ++j <= f; s = []) {
        j && (n[j] = (("." + n[j]) * 1).toFixed(2).slice(2));
        if (!(a = (v = n[j]).slice((l = v.length) % 3).match(/\d{3}/g), v = l % 3 ? [v.slice(0, l % 3)] : [], v = a ? v.concat(a) : v).length) continue;
        for (a = -1, l = v.length; ++a < l; t = "") {
            if (!(i = v[a] * 1)) continue;
            i % 100 < 20 && (t += ex[0][i % 100]) ||
            i % 100 + 1 && (t += ex[1][(i % 100 / 10 >> 0) - 1] + (i % 10 ? e + ex[0][i % 10] : ""));
            s.push((i < 100 ? t : !(i % 100) ? ex[2][i == 100 ? 0 : i / 100 >> 0] : (ex[2][i / 100 >> 0] + e + t)) +
            ((t = l - a - 2) > -1 ? " " + (i > 1 && t > 0 ? ex[3][t].replace("ão", "ões") : ex[3][t]) : ""));
        }
        a = ((sl = s.length) > 1 ? (a = s.pop(), s.join(" ") + e + a) : s.join("") || ((!j && (n[j + 1] * 1 > 0) || r.length) ? "" : ex[0][0]));
        a && r.push(a + (c ? (" " + (v.join("") * 1 > 1 ? j ? d + "s" : (/0{6,}$/.test(n[0]) ? "de " : "") + $.replace("l", "is") : j ? d : $)) : ""));
    }
    return r.join(e);
}

Object.defineProperty(Object.prototype, "Get", {
    value: function (propriedade) {
        return this[propriedade] != null ? this[propriedade] : "";
    },
    enumerable: false
});

String.prototype.CopiarAte = function (quantidadeCopiar) {
    /// <summary>Copia até uma quantidade de caracteres estabelecida</summary>
    quantidade = this != null ? this.length - 1 : 0;
    var retorno = "";

    for (var i = 0; i <= quantidadeCopiar - 1; i++) {
        if (i > quantidade)
            break;
        else
            retorno = retorno + this.charAt(i);
    }

    return retorno;
}