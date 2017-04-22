using System;

namespace BHBitProject.Lib.Util.Extensions.MaskExtensions
{
    public static class MaskExtensions
    {
        /// <summary>
        /// Insere máscara de CNPJ ou de CPF
        /// </summary>
        /// <param name="numCpfCnpj"></param>
        /// <returns>String</returns>
        public static string GetCpfCpnjMask(this string numCpfCnpj)
        {
            if (numCpfCnpj != null && numCpfCnpj != String.Empty)
            {
                //Se for CNPJ
                if (numCpfCnpj.Length == 14)
                {
                    return numCpfCnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
                }
                //Se for CPF
                else if (numCpfCnpj.Length == 11)
                {
                    return numCpfCnpj.Insert(3, ".").Insert(7, ".").Insert(11, "-");
                }
                else
                    return numCpfCnpj;
            }

            return String.Empty;          

        }

        /// <summary>
        /// Insere máscara em um número de telefone
        /// </summary>
        /// <param name="numberPhone"></param>
        /// <returns>String</returns>
        public static string PhoneMask(this string numberPhone)
        {
            if (numberPhone != null && numberPhone != String.Empty)
            {
                //Se possuir o número zero antes do DDD o mesmo é removido
                if (numberPhone.IndexOf("0", 0) == 0)
                    numberPhone = numberPhone.Remove(0, 1);

                return numberPhone.Insert(0, "(").Insert(3, ")").Insert(8, "-");
            }

            return String.Empty;           
        }

        /// <summary>
        /// Remove a máscara de um cnpj
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static string CnpjRemoveMask(this string cnpj)
        {
            if (cnpj != null && cnpj != String.Empty)
            {
                return cnpj.Replace(".", "").Replace(".", "").Replace("/", "").Replace("-", "");
            }

            return String.Empty;
        }

        public static string PhoneRemoveMask(this string phone)
        {
            if (phone != null && phone != String.Empty)
            {
                return phone.Replace("(", "").Replace(")", "").Replace("-", "");
            }

            return String.Empty;
        }

        public static string CepRemoveMask(this string cep)
        {
            if (cep != null && cep != String.Empty)
            {
                return cep.Replace(".", "").Replace("-", "");
            }

            return String.Empty;
        }

        
    }
}
