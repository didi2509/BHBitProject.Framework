using System;
using System.Globalization;
using System.Text;

using System.Linq;
using System.Security.Cryptography;

namespace BBP.Util.Extensions.StringExtensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Verifica se um objeto do banco é igual ao objeto informado pelo usuário.Ignorando acentuação letras maiúsculas ou minúsculas
        /// </summary>
        /// <param name="objeto">Objeto do banco</param>
        /// <param name="StrToCompare">valor informado pelo usuário</param>
        /// <returns>Booleano</returns>
        public static bool CompareInsensitive(this string objeto, string StrToCompare, bool IsRemoveSpecialCharacters = false)
        {
            if (!StrToCompare.Vazia())
            {
                if (objeto != null && objeto.Vazia())
                {
                    return false;
                }

                if (objeto == null)
                {
                    return false;
                }

                StrToCompare = StrToCompare.Igualar();

                return IsRemoveSpecialCharacters
                       ? (objeto ?? StrToCompare).Igualar().Contains(StrToCompare)
                       : (objeto ?? StrToCompare).Trim().ToUpper().Contains(StrToCompare);
            }

            return false;
        }

        ///// <summary>
        ///// Cria um novo MvcHtmlString a partir de uma string
        ///// </summary>
        ///// <param name="strCorrente"></param>
        ///// <returns></returns>
        //public static MvcHtmlString ToMVCHtmlString(this String strCorrente)
        //{
        //    return MvcHtmlString.Create(strCorrente);
        //}
       

        public static bool IsCPF(this string CPF)
        {

            if (String.IsNullOrEmpty(CPF))
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;

            string digito;

            int soma;

            int resto;

            CPF = CPF.Trim();

            CPF = CPF.Replace(".", "").Replace("-", "");

            if (CPF.Length != 11)
                return false;

            tempCpf = CPF.Substring(0, 9);

            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return CPF.EndsWith(digito);
        }

        /// <summary>
        /// Remove a acentuação de uma String
        /// </summary>
        /// <param name="text"></param>
        /// <returns>String</returns>
        public static string RemoveAccents(this string text)
        {
            if (text != null)
            {
                StringBuilder sbReturn = new StringBuilder();
                var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                        sbReturn.Append(letter);
                }
                return sbReturn.ToString();
            }

            return String.Empty;
        }


        public static string GetMD5(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            input += "!@#4123";

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }

        /// <summary>
        /// Verifica se uma string está vazia (utiliza internamente o método trim)
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static bool Vazia(this String strCorrente)
        {
            return String.IsNullOrEmpty(strCorrente) || (String.IsNullOrWhiteSpace(strCorrente));

        }

        /// <summary>
        /// Coloca a string em um padrão como para comparação
        /// </summary>
        /// <returns></returns>
        public static String Igualar(this String strCorrente, bool removerEspacos = false)
        {
            if (String.IsNullOrEmpty(strCorrente))
                return strCorrente;

            return removerEspacos ? strCorrente.Replace(" ", "").RemoveAccents().ToUpper() : strCorrente.Trim().RemoveAccents().ToUpper();
        }

        /// <summary>
        /// Substitui aspas simples e aspas duplas do word pelas utilizadas normalmente
        /// </summary>
        /// <returns></returns>
        public static String ConverterCaracteresEspeciais(this String strCorrente)
        {
            if (String.IsNullOrEmpty(strCorrente))
                return strCorrente;

            return strCorrente.Replace('“', '\"').Replace('”', '\"').Replace('‘', '\'').Replace('’', '\'');
        }


        /// <summary>
        /// Copia uma quantidade de caractéres estabelecidos como parâmetro a partir da posição 0
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <param name="quantidade">quantidade de caractéres a serem copiados a partir da posição 0</param>
        /// <returns></returns>
        public static String CopiarAte(this String strCorrente, int quantidade)
        {
            if (String.IsNullOrEmpty(strCorrente))
                return strCorrente;

            return strCorrente.Length > quantidade ? strCorrente.Substring(0, quantidade) : strCorrente.Substring(0, strCorrente.Length - 1);
        }

        /// Copia uma quantidade de caractéres estabelecidos como parâmetro a partir da posição 0
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <param name="quantidade">quantidade de caractéres a serem copiados a partir da posição 0</param>
        /// <returns></returns>
        public static String CopiarAteChar(this String strCorrente, char charCopiar)
        {
            if (String.IsNullOrEmpty(strCorrente))
                return strCorrente;

            StringBuilder sb = new StringBuilder();

            int quantidade = strCorrente.Length;

            for (int i = 0; i < quantidade; i++)
            {
                if (strCorrente[i] == charCopiar) break;
                sb.Append(strCorrente[i]);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Remove qualquer caractér não numérico de uma string
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static String ApenasNumeros(this String strCorrente)
        {
            if (String.IsNullOrEmpty(strCorrente)) return "";
            else
            {
                StringBuilder sb = new StringBuilder();
                strCorrente.Where(w => Char.IsNumber(w)).ToList().ForEach(f => sb.Append(f));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Coloca a string em um padrão como para comparação
        /// </summary>
        /// <returns></returns>
        public static String IgualarSemEspaco(this String strCorrente)
        {
            return strCorrente.Replace(" ", "").Trim().RemoveAccents().ToUpper();
        }




        /// <summary>
        /// Limita uma string a quantidade de caractéres informados
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static String Limitar(this String strCorrente, short quantidade)
        {
            if (strCorrente.Vazia())
                return strCorrente;
            else if (strCorrente.Length > quantidade)
            {
                return strCorrente.Substring(0, quantidade);
            }
            else return strCorrente;
        }

        public static String Valor(this String strCorrente)
        {
            return (strCorrente.Vazia() ? String.Empty : strCorrente);
        }

        /// <summary>
        /// String.Format
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <param name="objetos"></param>
        /// <returns></returns>
        public static string Formatar(this String strCorrente, params Object[] objetos)
        {
            return String.Format(strCorrente, objetos);
        }

        public static string FormatarPermissao(this String strCorrente)
        {
            return strCorrente.Replace("|", "").Trim();
        }

        #region Conversoes

        #region Verificacoes

        /// <summary>
        /// Verifica se um objeto pode ser do tipo inteiro
        /// </summary>
        /// <param name="objCorrente"></param>
        /// <returns></returns>
        public static bool IsInt(this string strCorrente)
        {
            if (strCorrente == null) return false;

            int saida;

            return int.TryParse(strCorrente, out saida);
        }

        /// <summary>
        /// Verifica se um objeto pode ser do tipo inteiro
        /// </summary>
        /// <param name="objCorrente"></param>
        /// <returns></returns>
        public static bool IsInt64(this string strCorrente)
        {
            if (strCorrente == null) return false;

            Int64 saida;

            return Int64.TryParse(strCorrente, out saida);
        }


        /// <summary>
        /// Verifica se um objeto pode ser do tipo inteiro
        /// </summary>
        /// <param name="objCorrente"></param>
        /// <returns></returns>
        public static bool IsInt16(this string strCorrente)
        {
            if (strCorrente == null) return false;

            Int16 saida;

            return Int16.TryParse(strCorrente, out saida);
        }

        /// <summary>
        /// Verifica se o objeto corrente pode ser do tipo decimal
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string strCorrente)
        {

            if (strCorrente == null) return false;

            decimal dec;

            return decimal.TryParse(strCorrente, out dec);
        }



        /// <summary>
        /// Verifica se o objeto corrente pode ser do tipo decimal
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static bool IsFloat(this string strCorrente)
        {

            if (strCorrente == null) return false;

            float dec;

            return float.TryParse(strCorrente, out dec);
        }


        /// <summary>
        /// Verifica se o objeto corrente pode ser do tipo decimal
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static bool IsDouble(this string strCorrente)
        {

            if (strCorrente == null) return false;

            double dec;

            return double.TryParse(strCorrente, out dec);
        }


        #endregion

        #region Conversoes

        /// <summary>
        /// Converte um valor para int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int32 toInt32(this string str)
        {

            if (IsInt(str))
                return Int32.Parse(str);
            else
                throw new Exception("Não é possível converter o valor em Int32");

        }


        /// <summary>
        /// Converte um valor para int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int64 toInt64(this string str)
        {

            if (IsInt64(str))
                return Int64.Parse(str);
            else
                throw new Exception("Não é possível converter o valor em Int64");

        }


        /// <summary>
        /// Converte um valor para int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int16 toInt16(this string str)
        {

            if (IsInt16(str))
                return Int16.Parse(str);
            else
                throw new Exception("Não é possível converter o valor em Int16");

        }

        /// <summary>
        /// Converte um valor para int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Decimal toDecimal(this string str)
        {

            if (IsDecimal(str))
                return Decimal.Parse(str);
            else
                throw new Exception("Não é possível converter o valor em Decimal");

        }

        /// <summary>
        /// Converte um valor para int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Double toDouble(this string str)
        {

            if (IsDouble(str))
                return Double.Parse(str);
            else
                throw new Exception("Não é possível converter o valor em Double");

        }


        /// <summary>
        /// Converte um valor para int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float toFloat(this string str)
        {

            if (IsFloat(str))
                return float.Parse(str);
            else
                throw new Exception("Não é possível converter o valor em float");

        }

        #endregion

        #endregion
    }
}
