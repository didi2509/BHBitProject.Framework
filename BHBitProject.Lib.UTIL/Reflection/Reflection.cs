using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BBP.Util
{
    public static class Reflection
    {

        #region metodos e funcoes

        #region DTO

        /// <summary>
        /// Gera um arquivo com o conteúdo da classe DTO
        /// </summary>
        /// <typeparam name="Tipo"></typeparam>
        /// <param name="nameSpace"></param>
        /// <param name="excluir"></param>
        /// <param name="caminho"></param>
        /// <param name="sufixo"></param>
        public static void GerarDTO(this Type Tipo, string nameSpace, string excluir = "EntityKey,EntityState", string caminho = @"C:\DTO\", string sufixo = "DTO")
        {
            //Verifica se o diretório não existe, se não existir o mesmo é criado
            if (!Directory.Exists(caminho))
                Directory.CreateDirectory(caminho);

            //Se o dirétorio existir
            if (Directory.Exists(caminho))
            {
                //Nome da classe seguido do sufixo
                string nomeClasse = Tipo.Name + sufixo;

                //StringBuilder com as propriedades
                System.Text.StringBuilder sb = new System.Text.StringBuilder("");

                //Filtrando as propriedades da classe de acordo com os campos passados para a exclusão
                PropertyInfo[] propriedades = String.IsNullOrEmpty(excluir) ? Tipo.GetProperties() : Tipo.GetProperties().Where(w => !excluir.ToUpper().Contains(w.Name.ToUpper())).ToArray();

                //quantidade de propriedades
                int quantidadePropriedades = propriedades.Length;


                //varrendo as propriedades
                for (int i = 0; i < quantidadePropriedades; i++)
                {
                    //Comentário com o nome da propriedade
                    sb.AppendLine(IniciarSumario(propriedades[i].Name));

                    //Obtendo o tipo da mesma
                    String tipoPropriedade = propriedades[i].PropertyType.Name;

                    //Se o tipo for nullable, deverá ser pego o tipo completo e feito um tratamento em cima do mesmo
                    if (tipoPropriedade.ToUpper().Contains("NULLABLE"))
                    {
                        //Obtendo o nome completo sem os caracteres especiais
                        string nomeCompleto = propriedades[i].PropertyType.FullName;

                        //Inserindo um comentário com o tipo original
                        sb.AppendLine("/// Tipo Completo: " + nomeCompleto);

                        //Obtendo a propriedade do tipo nullable de acordo com o seu tipo
                        tipoPropriedade = CopiarTipoNullable(nomeCompleto);
                    }

                    sb.AppendLine(FecharSumario());

                    //Criando a propriedade corrente
                    sb.AppendLine("public " + tipoPropriedade + " " + propriedades[i].Name + " { get; set; }");

                    //Inserindo um espaço de uma propriedade para a outra
                    sb.AppendLine("");
                }

                //Gerando o conteúdo do arquivo
                string conteudo = MontarClasse(nomeClasse, nameSpace, sb.ToString());

                //Gerando o caminho completo do arquivo
                string arquivo = caminho + nomeClasse + ".cs";

                //Criando o arquivo
                File.Create(arquivo).Close();

                //Colocando o conteúdo dentro do arquivo
                using (StreamWriter sw = new StreamWriter(arquivo))
                {
                    sw.Write(conteudo.Replace("System.Int6?", "System.Int16?"));
                }
            }
        }

        private static string IniciarSumario(string itemSumario)
        {
            return String.Format(@"/// <summary>
/// {0}", itemSumario);
        }

        private static string FecharSumario()
        {
            return "/// </summary>";
        }

        /// <summary>
        /// Trata o tipo nullable retornando o seu tipo original
        /// </summary>
        /// <param name="strCopiar"></param>
        /// <returns></returns>
        private static string CopiarTipoNullable(string strCopiar)
        {
            strCopiar = strCopiar.Replace("`", "").Replace("1", "").Replace("System.Nullable[[", "");

            String[] split = strCopiar.Split(',');

            if ((split == null) || split.Length < 1)
                return strCopiar;
            return split[0] + "?";
        }

        /// <summary>
        /// Monta a string com o conteúdo da classe
        /// </summary>
        /// <param name="nomeClasse"></param>
        /// <param name="nameSpace"></param>
        /// <param name="propriedades"></param>
        /// <returns></returns>
        private static string MontarClasse(string nomeClasse, string nameSpace, string propriedades)
        {
            return @"using System;
using System.Web;


namespace " + nameSpace + @"
{
    public class " + nomeClasse + @"
    {

        " + propriedades + @"
    }
}";

        }

        #endregion

        /// <summary>
        /// Faz a cópia dos atributos de dois objetos
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyObject(Object source, Object destination, string notIncludeProperties = "")
        {
            //Propriedade fonte
            PropertyInfo[] propertiesSource = source.GetType().GetProperties();

            //Propriedade destino
            PropertyInfo[] propertiesDestination = destination.GetType().GetProperties();


            //Quantidade de propriedades
            int sourceLength = propertiesSource.Length;
            int destinationLength = propertiesDestination.Length;

            //Varrendo as propriedades da fonte
            for (int s = 0; s < sourceLength; s++)
            {

                //Se a propriedade puder ser lida
                if (propertiesSource[s].CanRead)
                {

                    //Varrendo as propriedades do destino em função da fonte
                    for (int d = 0; d < destinationLength; d++)
                    {
                        //Caso não seja para copiar a propriedade
                        if ((!String.IsNullOrEmpty(notIncludeProperties)) && (notIncludeProperties.ToLower().Contains(propertiesSource[s].Name.ToLower())))
                            break;

                        //Se o nome da propriedade do destino for igual ao nome da propriedade da fonte
                        if (propertiesSource[s].Name == propertiesDestination[d].Name)
                        {
                            //se a propriedade de destino puder ser escrita
                            if (propertiesDestination[d].CanWrite)
                                propertiesDestination[d].SetValue(destination, propertiesSource[s].GetValue(source, null), null);

                            break;
                        }

                    }

                }

            }


        }


        /// <summary>
        /// Verifica se uma propriedade existe em uma lista de propriedades
        /// </summary>
        /// <param name="Properties"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        public static bool ExistProperty(PropertyInfo[] Properties, string Property)
        {
            int length = Properties.Length;

            for (int i = 0; i < length; i++)
                if (Properties[i].Name.ToUpper() == Property)
                    return true;

            return false;
        }

        #region Reflection Validate

        /// <summary>
        /// Faz a validação dos campos da classe de acordo com parâmetros pré-Definidos
        /// </summary>
        /// <param name="obj">objeto a ser validado</param>
        /// <param name="propertiesToInclude">se true as propriedades passadas são adicionadas a validação, se false o método inclui todas as propriedades do objeto menos as passadas como parâmetro</param>
        /// <param name="ErrorSeparator">string separador de erros</param>
        /// <param name="strProperties">propriedades do objeto a serem validadas</param>
        /// <returns></returns>
        public static string Validate(object obj, bool propertiesToInclude = true, string ErrorSeparator = "<br />", params string[] strProperties)
        {

            //MetaData relacionada ao objeto
            PropertyInfo[] properties = obj.GetType().GetProperties();

            if (!propertiesToInclude)
                strProperties = RemoveProperties(properties, strProperties);

            //Lista de erros
            string errors = String.Empty;

            //Quantidade de propriedades a validar
            int sizePropertiesToValidate = strProperties.Length;

            for (int i = 0; i < sizePropertiesToValidate; i++)
            {
                var propertie = properties.Where(p => p.Name.ToUpper().Trim() == strProperties[i].ToString().ToUpper().Trim()).FirstOrDefault();

                if (propertie != null)
                {
                    object value = propertie.GetValue(obj, null);

                    if (value != null)
                    {
                        if (String.IsNullOrEmpty(value.ToString()))
                            errors += ErrorSeparator + "O campo " + propertie.Name + "não pode ser vazio";
                    }
                    else
                        errors += ErrorSeparator + "O campo " + propertie.Name + " não pode ser vazio";

                }

            }

            return errors;

        }

        /// <summary>
        /// Remove propriedades em uma lista
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="strProperties"></param>
        /// <returns></returns>
        private static string[] RemoveProperties(PropertyInfo[] properties, string[] strProperties)
        {
            List<String> listRemove = new List<string>();
            int size = properties.Length;
            int sizeProperties = strProperties.Length;

            bool allProperties = ((strProperties.Length == 1) && String.IsNullOrEmpty(strProperties[0]));

            if (allProperties)
            {
                for (int i = 0; i < size; i++)
                    listRemove.Add(properties[i].Name);
            }
            else
            {
                for (int i = 0; i < size; i++)
                {

                    bool contains = false;

                    for (int a = 0; a < sizeProperties; a++)
                        if (properties[i].Name == strProperties[a])
                        {
                            contains = true;
                            break;
                        }

                    if (!contains)
                        listRemove.Add(properties[i].Name);
                }


            }
            return listRemove.ToArray();
        }
        #endregion
        #endregion

    }
}
