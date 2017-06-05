using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BBP.Test.Selenium
{
    public static class FakeGenerator
    {
        #region [Properties]

        /// <summary>
        /// Randômico para auxilio
        /// </summary>
        private static readonly Random random;

        /// <summary>
        /// Utilizado para gerar textos de forma randômica
        /// </summary>
        private static readonly string loremIspsum;

        /// <summary>
        /// Lista de emails disponíveis
        /// </summary>
        private static readonly string[] mailArray;

        #endregion

        #region [Constructors]

        static FakeGenerator()
        {
            random = new Random();
            loremIspsum = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
            mailArray = new string[] { "hotmail", "gmail", "zipmail", "facebook", "yahoo" };
        }

        #endregion

        #region [Unique]

        /// <summary>
        ///  Gerador de valores randômicos únicos
        /// </summary>
        public static class Unique
        {
            /// <summary>
            /// Gera um novo guid
            /// </summary>
            /// <returns></returns>
            public static string NewGuid() => Guid.NewGuid().ToString();
        }

        #endregion

        #region [Number]

        /// <summary>
        /// Gerador de valores randômicos para número / guid
        /// </summary>
        public static class Number
        {
            /// <summary>
            /// Retorna um valor numérico randômico
            /// </summary>
            /// <param name="minValue"></param>
            /// <param name="maxValue"></param>
            /// <returns></returns>
            public static int NewInteger(int minValue, int maxValue) => random.Next(minValue, maxValue);

            /// <summary>
            /// Retorna um valor numérico randômico, por padrão contempla de minValue ao maior valor disponível para o tipo inteiro
            /// </summary>
            /// <param name="minValue"></param>
            /// <returns></returns>
            public static int NewInteger(int minValue) => NewInteger(minValue, int.MaxValue);

            /// <summary>
            /// Retorna um valor numérico randômico, por padrão contempla de 0 ao maior valor disponível para o tipo inteiro
            /// </summary>
            /// <returns></returns>
            public static int NewInteger() => random.Next(0, int.MaxValue);

            /// <summary>
            /// Retorna um valor numérico randômico, por padrão contempla de 0 ao maior valor disponível para o tipo inteiro
            /// </summary>
            /// <returns></returns>
            public static string NewStringMoney(int minValue = 0, int maxValue = int.MaxValue, char centsSeparator = ',')
            {
                string cents = Number.NewInteger(0, 99).ToString();
                if (cents.Length == 1) cents = "0" + cents;

                return $"{Number.NewInteger(minValue, maxValue)}{centsSeparator}{cents}";
            }

            /// <summary>
            /// Retorna um valor numérico randômico, por padrão contempla de 0 ao maior valor disponível para o tipo inteiro
            /// </summary>
            /// <returns></returns>
            public static decimal NewDecimalMoney()
            {
                return Convert.ToDecimal(NewStringMoney('.'));
            }
        }

        #endregion

        #region [Text]

        /// <summary>
        /// Gerador de valores randômicos para texto
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// Retorna um texto randômico baseado nos dizeres Lorem Ipsum
            /// </summary>
            /// <param name="maxLetters">Quantidade máxima de letras a serem geradas, por padrão o valor é 0 e é gerada a frase padrão do Lorem Ipsum</param>
            /// <returns></returns>
            public static string NewLoremIpsumText(int maxLetters = 0)
            {
                if (maxLetters < 0) return String.Empty;

                if (maxLetters == 0)
                    return loremIspsum;

                string newLoremIpsum = loremIspsum;


                while (maxLetters > newLoremIpsum.Length)
                    newLoremIpsum += String.Format(" {0}", loremIspsum);

                return newLoremIpsum.Substring(0, maxLetters - 1);
            }



            /// <summary>
            /// Gera um texto randômico baseado em letras, números e caractéres especiais
            /// </summary>
            /// <param name="totalLetters">Total de Letras a serem geradas, se informado 0 (que é o padrão) este total será randômico</param>
            /// <param name="isCanContainsSpace">Indica se o texto poderá conter espaços</param>
            /// <param name="isContainsAccentuation">Indica se o texto poderá conter acentuação</param>
            /// <param name="isContainsSpecialCharacters">Indica se o texto poderá conter caractéres especiais</param>
            /// <returns></returns>
            public static string NewRandomText(int totalLetters = 0, bool isCanContainsSpace = true, bool isContainsAccentuation = true, bool isContainsSpecialCharacters = false)
            {
                if (totalLetters < 1) return String.Empty;

                StringBuilder sbInput = new StringBuilder("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

                if (isCanContainsSpace)
                    sbInput.Append(" ");

                if (isContainsAccentuation)
                    sbInput.Append(@"çáéíóúâêîôûãõÇÁÉÍÓÚÂÊÎÔÛÃÕ");

                if (isContainsSpecialCharacters)
                    sbInput.Append(@"~^/\´`[]{}?;:.,<>!@#$%¨&*()_+-=§'""");

                string input = sbInput.ToString();
                short inputLength = (short)input.Length;

                int numberOfLetters = totalLetters == 0 ? Number.NewInteger() : totalLetters;

                string randomText = new string(
                                        Enumerable.Range(0, numberOfLetters)
                                       .Select(x => input[random.Next(0, inputLength)])
                                       .ToArray()
                                       );

                return randomText;
            }
        }

        #endregion

        #region [Date]

        /// <summary>
        /// Gerador de valores randômicos para data
        /// </summary>
        public static class Date
        {
            /// <summary>
            /// Gera uma data randômica baseada nas datas de entrada
            /// </summary>
            /// <param name="minDate">Valor mínimo para a data gerada (se não fornecido o valor assumido é 01/01/1980)</param>
            /// <param name="maxDate">Valor máximo para a data gerada</param>
            /// <returns></returns>
            public static DateTime NewDate(DateTime? minDate = null, DateTime? maxDate = null)
            {
                if (!minDate.HasValue)
                    minDate = new DateTime(1980, 1, 1);

                if (maxDate.HasValue && minDate.Value > maxDate.Value)
                    throw new Exception("A data mímina não pode ser maior que a data máxima");

                DateTime start = minDate.Value;

                int range = 0;

                if (minDate.HasValue && maxDate.HasValue) //Informou as duas datas
                    range = (maxDate.Value.Date - start).Days;
                else if (minDate.HasValue && !maxDate.HasValue)
                    range = Number.NewInteger(1, 10800);  //10800 == Adiociona no máximo 30 anos - Informou apenas a data mínima
                else
                    range = Number.NewInteger(1, 36000);  //36000 == Adiocona no máximo 100 anos - Não informou nenhuma das duas


                DateTime returnDate = start.AddDays(random.Next(range));

                while (
                            (returnDate < minDate)
                         || (maxDate.HasValue && returnDate > maxDate.Value)
                      )
                    returnDate = start.AddDays(random.Next(range));

                return returnDate;
            }
        }
        #endregion

        #region [Person]

        /// <summary>
        /// Gerador de valores randômicos para dados referentes a pessoa física
        /// </summary>
        public static class Person
        {

            /// <summary>
            /// Primeiro nome
            /// </summary>
            public static string FirstName { get { return NFaker.NameBR.FirstName; } }

            public static string LasttName { get { return NFaker.NameBR.LasttName; } }

            public static string Name { get { return NFaker.NameBR.Name; } }

            public static string NameWithPrefix { get { return NFaker.NameBR.NameWithPrefix; } }

            public static string Prefix { get { return NFaker.NameBR.Prefix; } }

            /// <summary>
            /// Gera um CPF randômico
            /// </summary>
            /// <param name="isFormatted">Indica se o valor deverá ser formatado</param>
            /// <returns></returns>
            public static String CPF(bool isFormatted = false)
            {
                int soma = 0, resto = 0;
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                string semente = Number.NewInteger(100000000, 999999999).ToString();

                for (int i = 0; i < 9; i++)
                    soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                semente = semente + resto;
                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

                resto = soma % 11;

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                semente = semente + resto;

                return isFormatted
                    ? semente
                      .Insert(3, ".")
                      .Insert(7, ".")
                      .Insert(11, "-")
                    : semente;
            }

        }
        #endregion

        #region [JuridicPerson]

        /// <summary>
        /// Gerador randômico de dados para pessoa jurídico
        /// </summary>
        public static class JuridicPerson
        {

            public static string CNPJ(bool isFormatted = false)
            {
                int digito1 = 0, digito2 = 0, resto = 0;
                string nDigResult, numerosContatenados;

                #region numeros

                //numeros gerados
                int n1 = Number.NewInteger(0, 10);
                int n2 = Number.NewInteger(0, 10);
                int n3 = Number.NewInteger(0, 10);
                int n4 = Number.NewInteger(0, 10);
                int n5 = Number.NewInteger(0, 10);
                int n6 = Number.NewInteger(0, 10);
                int n7 = Number.NewInteger(0, 10);
                int n8 = Number.NewInteger(0, 10);
                int n9 = Number.NewInteger(0, 10);
                int n10 = Number.NewInteger(0, 10);
                int n11 = Number.NewInteger(0, 10);
                int n12 = Number.NewInteger(0, 10);

                #endregion

                int soma = n12 * 2 + n11 * 3 + n10 * 4 + n9 * 5 + n8 * 6 + n7 * 7 + n6 * 8 + n5 * 9 + n4 * 2 + n3 * 3 + n2 * 4 + n1 * 5;
                int valor = (soma / 11) * 11;
                digito1 = soma - valor;
                //Primeiro resto da divisão por 11.
                resto = (digito1 % 11);
                if (digito1 < 2)
                    digito1 = 0;
                else
                    digito1 = 11 - resto;

                int soma2 = digito1 * 2 + n12 * 3 + n11 * 4 + n10 * 5 + n9 * 6 + n8 * 7 + n7 * 8 + n6 * 9 + n5 * 2 + n4 * 3 + n3 * 4 + n2 * 5 + n1 * 6;

                int valor2 = (soma2 / 11) * 11;

                digito2 = soma2 - valor2;

                //Primeiro resto da divisão por 11.
                resto = (digito2 % 11);
                if (digito2 < 2)
                {
                    digito2 = 0;
                }
                else
                {
                    digito2 = 11 - resto;
                }
                //Conctenando os numeros
                numerosContatenados = n1.ToString() + n2.ToString() + "." + n3.ToString() + n4.ToString() +
                                      n5.ToString() + "." + n6.ToString() + n7.ToString() + n8.ToString() + "/" +
                                      n9.ToString() + n10.ToString() + n11.ToString() + n12.ToString() + "-";

                //Concatenando o primeiro resto com o segundo.
                nDigResult = digito1.ToString() + digito2.ToString();
                string strResult = numerosContatenados + nDigResult;

                return isFormatted ? strResult : strResult.Replace(".", "").Replace("/", "").Replace("-", "");
            }
        }

        #endregion

        #region [Address]

        /// <summary>
        /// Gerador de valores randômicos para endereço
        /// </summary>
        public static class Address
        {
            #region [Address]

            /// <summary>
            /// Cidade
            /// </summary>
            public static string City { get { return NFaker.AddressBR.City; } }

            /// <summary>
            /// Estado
            /// </summary>
            public static string State { get { return NFaker.AddressBR.State; } }


            /// <summary>
            /// Sigla do estadp
            /// </summary>
            public static string StateAbbr { get { return NFaker.AddressBR.StateAbbr; } }

            /// <summary>
            /// Rua
            /// </summary>
            public static string Street { get { return NFaker.AddressBR.Street; } }

            /// <summary>
            /// Prefixo para rua
            /// </summary>
            public static string StreetPrefix { get { return NFaker.AddressBR.StreetPrefix; } }

            /// <summary>
            /// CEP
            /// </summary>
            public static string ZipCode(bool isFormatted = false)
            {
                StringBuilder sbCEP = new StringBuilder();

                for (int i = 0; i < 8; i++)
                    sbCEP.Append(Number.NewInteger(0, 10));

                return isFormatted ? sbCEP.ToString().Insert(5, "-") : sbCEP.ToString();
            }
        }

        #endregion

        #endregion

        #region [GeoLocation]

        /// <summary>
        /// Gerador de valores randômicos para geolocalizacao
        /// </summary>
        public static class GeoLocation
        {
            public static double Latitude { get { return NFaker.Geolocation.Latitude; } }
            public static double Longitude { get { return NFaker.Geolocation.Longitude; } }
        }

        #endregion

        #region [Mail]

        /// <summary>
        /// Gerador de valores randômicos para e-mail
        /// </summary>
        public static class Mail
        {
            public static string Address()
            {
                short mailArrayLength = (short)(mailArray.Length - 1);
                return $"{Person.FirstName}@{mailArray[Number.NewInteger(0, mailArrayLength)]}.com";
            }
        }

        #endregion
    }
}
