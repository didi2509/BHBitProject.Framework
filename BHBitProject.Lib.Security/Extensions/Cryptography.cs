using BBP.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BBP.Security.Extensions
{
    public static class Cryptography
    {
        #region [Base64]

        /// <summary>
        /// Criptografa uma string para Base64
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static string EncryptBase64(this string strCorrente)
        {
            return new Base64Crypt().Encrypt(strCorrente);
        }

        /// <summary>
        /// Descriptografa uma string do tipo Base64
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static string DecryptBase64(this string strCorrente)
        {
            return new Base64Crypt().Decrypt(strCorrente);
        }

        /// <summary>
        /// Criptografa um decimal para Base64
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static string EncryptBase64(this decimal strCorrente)
        {
            return new Base64Crypt().Encrypt(strCorrente.ToString());
        }

        /// <summary>
        /// Descriptografa uma string do tipo Base64
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static string DecryptBase64(this decimal strCorrente)
        {
            return new Base64Crypt().Decrypt(strCorrente.ToString());
        }

        /// <summary>
        /// Criptografa um inteiro para Base64
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static string EncryptBase64(this int strCorrente)
        {
            return new Base64Crypt().Encrypt(strCorrente.ToString());
        }

        /// <summary>
        /// Descriptografa uma string do tipo Base64
        /// </summary>
        /// <param name="strCorrente"></param>
        /// <returns></returns>
        public static string DecryptBase64(this int strCorrente)
        {
            return new Base64Crypt().Decrypt(strCorrente.ToString());
        }

        #endregion

        #region Hash

        #region [MD5]

        /// <summary>
        /// Criptografa uma string para MD5
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncryptMD5(string value)
        {
            return MD5Hash.Get(value);
        }


        #endregion

        #region SHA1

        /// <summary>
        /// Compara os hashs
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="hash_"></param>
        /// <returns></returns>
        public static bool IsEqualSHA1Hash(this string strHash, Byte[] hash)
        {
            Byte[] hash_ = strHash.EncryptSHA1();

            if (hash.Length != hash_.Length)
                return false;
            else
            {
                for (int x = 0; x < hash.Length; x++)
                    if (hash[x] != hash_[x])
                        return false;
            }

            return true;
        }

        /// <summary>
        /// Obtém o hash de uma string
        /// </summary>
        /// <param name="MessageString"></param>
        /// <returns></returns>
        public static Byte[] EncryptSHA1(this string MessageString)
        {
            //Create a new instance of the UnicodeEncoding class to 
            //convert the string into an array of Unicode bytes.
            UnicodeEncoding UE = new UnicodeEncoding();

            //Convert the string into an array of bytes.
            byte[] MessageBytes = UE.GetBytes(MessageString);

            //Create a new instance of the SHA1Managed class to create 
            //the hash value.
            SHA1Managed SHhash = new SHA1Managed();

            //Create the hash value from the array of bytes.
            return SHhash.ComputeHash(MessageBytes);
        }

        #endregion

        #region [SHA256]

        /// <summary>
        /// Criptografa uma string para MD5
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncryptSHA256(string value)
        {
            return SHA256Hash.Get(value);
        }


        #endregion

        /// <summary>
        /// Compara os hashs
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="hash_"></param>
        /// <returns></returns>
        public static bool HashIsEqual(this Byte[] hash, Byte[] hash_)
        {
            if (hash.Length != hash_.Length)
                return false;
            else
            {
                for (int x = 0; x < hash.Length; x++)
                    if (hash[x] != hash_[x])
                        return false;
            }

            return true;
        }

        #endregion

        #region GUID

        public static string CreateGUID()
        {
            return Utils.GUID.CreateGUID();
        }

        public static string ParseGUID(this string value)
        {
            return Utils.GUID.ParseGUID(value);
        }

        public static string ParseExactGUID(this string input, string format)
        {
            return Utils.GUID.ParseExactGUID(input, format);
        }

        #endregion

        #region AES

        /// <summary>
        /// Criptografa uma string utilizando a criptografia AES, com uma chave secreta interna
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncrypAES(this string value)
        {
            return new SimpleAES().EncryptToString(value);
        }

        /// <summary>
        /// Descriptografa uma string utilizando a criptografia AES, com uma chave secreta interna
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecrypAES(this string value)
        {
            return new SimpleAES().DecryptString(value);
        }

        /// <summary>
        /// Criptografa uma string utilizando a criptografia AES, de acordo com as chaves informadas
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Key">Array aleatório de números (o mesmo par fornecido para criptografar deverá ser utilizado para descriptofrafar)</param>
        /// <param name="Vector">Array aleatório de números (o mesmo par fornecido para criptografar deverá ser utilizado para descriptofrafar)</param>
        /// <returns></returns>
        public static string EncrypAES(this string value, byte[] Key, byte[] Vector)
        {
            return new SimpleAES(Key,Vector).EncryptToString(value);
        }

        /// <summary>
        /// Descriptografa uma string utilizando a criptografia AES, de acordo com as chaves informadas
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Key">Array aleatório de números (o mesmo par fornecido para criptografar deverá ser utilizado para descriptofrafar)</param>
        /// <param name="Vector">Array aleatório de números (o mesmo par fornecido para criptografar deverá ser utilizado para descriptofrafar)</param>
        /// <returns></returns>
        public static string DecrypAES(this string value, byte[] Key, byte[] Vector)
        {
            return new SimpleAES(Key, Vector).DecryptString(value);
        }

      

        /// <summary>
        /// Criptografa uma string utilizando a criptografia AES, com uma chave secreta interna
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncrypURL(this string value)
        {
            return value.EncrypAES();
        }

        /// <summary>
        /// Descriptografa uma string utilizando a criptografia AES, com uma chave secreta interna
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecrypURL(this string value)
        {
            return value.DecrypAES();
        }

        /// <summary>
        /// Criptografa uma string utilizando a criptografia AES, de acordo com as chaves informadas
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Key">Array aleatório de números (o mesmo par fornecido para criptografar deverá ser utilizado para descriptofrafar)</param>
        /// <param name="Vector">Array aleatório de números (o mesmo par fornecido para criptografar deverá ser utilizado para descriptofrafar)</param>
        /// <returns></returns>
        public static string EncrypURL(this string value, byte[] Key, byte[] Vector)
        {
            return value.EncrypAES(Key,Vector);
        }

        /// <summary>
        /// Descriptografa uma string utilizando a criptografia AES, de acordo com as chaves informadas
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Key">Array aleatório de números (o mesmo par fornecido para criptografar deverá ser utilizado para descriptofrafar)</param>
        /// <param name="Vector">Array aleatório de números (o mesmo par fornecido para criptografar deverá ser utilizado para descriptofrafar)</param>
        /// <returns></returns>
        public static string DecrypURL(this string value, byte[] Key, byte[] Vector)
        {
            return value.DecrypAES(Key,Vector);
        }

        #endregion


    
    
    }
}
