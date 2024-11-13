using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary
{
    public static class CryptographyExtensionMethods
    {
        private static string keyString = "44DD9549-934C-4799-A807-C7429094530E";

        /// <summary>
        /// Get Encrpted Value of Passed value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToEncrypt(this string value)
        {
            return value.ToEncrypt(keyString);
        }

        /// <summary>
        /// Get Decrypted value of passed encrypted string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToDecrypt(this string value)
        {
            return value.Replace(" ", "+").ToDecrypt(keyString);
        }

        /// <summary>
        /// Encrypt value
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToEncrypt(this string strData, string strKey)
        {
            string strValue = "";
            if (!(strKey == ""))
            {
                if (strKey.Length < 16)
                {
                    strKey = strKey.PadRight(16 - strKey.Length, 'X').Substring(0, 16);
                }
                else
                {
                    if (strKey.Length > 16)
                    {
                        strKey = strKey.Substring(0, 16);
                    }
                }
                byte[] byteKey = Encoding.UTF8.GetBytes(strKey.Substring(0, 8));
                byte[] byteVector = Encoding.UTF8.GetBytes(strKey.Substring(8));
                byte[] byteData = Encoding.UTF8.GetBytes(strData);
                DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
                System.IO.MemoryStream objMemoryStream = new System.IO.MemoryStream();
                CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(byteKey, byteVector), CryptoStreamMode.Write);
                objCryptoStream.Write(byteData, 0, byteData.Length);
                objCryptoStream.FlushFinalBlock();
                strValue = Convert.ToBase64String(objMemoryStream.ToArray());
                objDES.Clear();
                objCryptoStream.Clear();
                objCryptoStream.Close();
            }
            else
            {
                strValue = strData;
            }
            return strValue;
        }

        /// <summary>
        /// decrypt value
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToDecrypt(this string strData, string strKey)
        {
            string strValue = "";
            if (!(strData == ""))
            {
                if (strKey.Length < 16)
                {
                    strKey = strKey.PadRight(16 - strKey.Length, 'X').Substring(0, 16);
                }
                else
                {
                    if (strKey.Length > 16)
                    {
                        strKey = strKey.Substring(0, 16);
                    }
                }
                byte[] byteKey = Encoding.UTF8.GetBytes(strKey.Substring(0, 8));
                byte[] byteVector = Encoding.UTF8.GetBytes(strKey.Substring(8));
                byte[] byteData = new byte[strData.Length + 1];
                try
                {
                    byteData = Convert.FromBase64String(strData);
                }
                catch
                {
                    strValue = strData;
                }
                if (strValue == "")
                {
                    try
                    {
                        DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
                        System.IO.MemoryStream objMemoryStream = new System.IO.MemoryStream();
                        CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(byteKey, byteVector), CryptoStreamMode.Write);
                        objCryptoStream.Write(byteData, 0, byteData.Length);
                        objCryptoStream.FlushFinalBlock();
                        System.Text.Encoding objEncoding = System.Text.Encoding.UTF8;
                        strValue = objEncoding.GetString(objMemoryStream.ToArray());
                        objDES.Clear();
                        objCryptoStream.Clear();
                        objCryptoStream.Close();
                    }
                    catch
                    {
                        strValue = "";
                    }
                }
            }
            else
            {
                strValue = strData;
            }
            return strValue;
        }

        /// <summary>
        /// Get Encrpted Value of Passed value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToUrlEncrypt(this string value)
        {
            return value.ToUrlEncrypt(keyString);
        }

        /// <summary>
        /// Get Decrypted value of passed encrypted string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToUrlDecrypt(this string value)
        {
            return value.Replace(" ", "+").ToUrlDecrypt(keyString);
        }

        /// <summary>
        /// Encrypt value
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToUrlEncrypt(this string strData, string strKey)
        {
            string strValue = "";
            if (!(strKey == ""))
            {
                if (strKey.Length < 16)
                {
                    strKey = strKey.PadRight(16 - strKey.Length, 'X').Substring(0, 16);
                }
                else
                {
                    if (strKey.Length > 16)
                    {
                        strKey = strKey.Substring(0, 16);
                    }
                }
                byte[] byteKey = Encoding.UTF8.GetBytes(strKey.Substring(0, 8));
                byte[] byteVector = Encoding.UTF8.GetBytes(strKey.Substring(8));
                byte[] byteData = Encoding.UTF8.GetBytes(strData);
                DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
                System.IO.MemoryStream objMemoryStream = new System.IO.MemoryStream();
                CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(byteKey, byteVector), CryptoStreamMode.Write);
                objCryptoStream.Write(byteData, 0, byteData.Length);
                objCryptoStream.FlushFinalBlock();
                strValue = UrlTokenEncode(objMemoryStream.ToArray());
                objDES.Clear();
                objCryptoStream.Clear();
                objCryptoStream.Close();
            }
            else
            {
                strValue = strData;
            }
            return strValue;
        }

        /// <summary>
        /// decrypt value
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToUrlDecrypt(this string strData, string strKey)
        {
            string strValue = "";
            if (!(strData == ""))
            {
                if (strKey.Length < 16)
                {
                    strKey = strKey.PadRight(16 - strKey.Length, 'X').Substring(0, 16);
                }
                else
                {
                    if (strKey.Length > 16)
                    {
                        strKey = strKey.Substring(0, 16);
                    }
                }
                byte[] byteKey = Encoding.UTF8.GetBytes(strKey.Substring(0, 8));
                byte[] byteVector = Encoding.UTF8.GetBytes(strKey.Substring(8));
                byte[] byteData = new byte[strData.Length + 1];
                try
                {
                    byteData = UrlTokenDecode(strData);
                }
                catch
                {
                    strValue = strData;
                }
                if (strValue == "")
                {
                    try
                    {
                        DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
                        System.IO.MemoryStream objMemoryStream = new System.IO.MemoryStream();
                        CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(byteKey, byteVector), CryptoStreamMode.Write);
                        objCryptoStream.Write(byteData, 0, byteData.Length);
                        objCryptoStream.FlushFinalBlock();
                        System.Text.Encoding objEncoding = System.Text.Encoding.UTF8;
                        strValue = objEncoding.GetString(objMemoryStream.ToArray());
                        objDES.Clear();
                        objCryptoStream.Clear();
                        objCryptoStream.Close();
                    }
                    catch
                    {
                        strValue = "";
                    }
                }
            }
            else
            {
                strValue = strData;
            }
            return strValue;
        }

        /// <summary>
        /// Get HEX string of Passed value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToHex(this string value)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            Byte[] stringBytes = encoding.GetBytes(value);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        /// <summary>
        /// Get string value of passed HEX string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToDeHex(this string value)
        {
            int numberChars = value.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
            }
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            return encoding.GetString(bytes);
        }

        private static string AESkey = "01nf0sensegl0bal";
        private static string AESiv = "01nf0sensegl0bal";

        /// <summary>
        /// Get Encryption by AES
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string AESEncryption(string strData)
        {
            Aes encryptor = Aes.Create();
            encryptor.Key = Encoding.ASCII.GetBytes(AESkey);
            encryptor.IV = Encoding.ASCII.GetBytes(AESiv);

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            string cipherText = String.Empty;
            try
            {
                byte[] plainBytes = Encoding.ASCII.GetBytes(strData);
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();

                byte[] cipherBytes = memoryStream.ToArray();
                cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }

            return cipherText;
        }

        /// <summary>
        /// Get Decryption by AES
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string AESDecryption(string strData)
        {
            Aes encryptor = Aes.Create();
            encryptor.Key = Encoding.ASCII.GetBytes(AESkey);
            encryptor.IV = Encoding.ASCII.GetBytes(AESiv);

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            string plainText = String.Empty;
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(strData);
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();

                byte[] plainBytes = memoryStream.ToArray();
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }

            return plainText;
        }

        private static string UrlTokenEncode(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (input.Length < 1)
                return String.Empty;
            char[] base64Chars = null;

            ////////////////////////////////////////////////////////
            // Step 1: Do a Base64 encoding
            string base64Str = Convert.ToBase64String(input);
            if (base64Str == null)
                return null;

            int endPos;
            ////////////////////////////////////////////////////////
            // Step 2: Find how many padding chars are present in the end
            for (endPos = base64Str.Length; endPos > 0; endPos--)
            {
                if (base64Str[endPos - 1] != '=') // Found a non-padding char!
                {
                    break; // Stop here
                }
            }

            ////////////////////////////////////////////////////////
            // Step 3: Create char array to store all non-padding chars,
            //      plus a char to indicate how many padding chars are needed
            base64Chars = new char[endPos + 1];
            base64Chars[endPos] = (char)((int)'0' + base64Str.Length - endPos); // Store a char at the end, to indicate how many padding chars are needed

            ////////////////////////////////////////////////////////
            // Step 3: Copy in the other chars. Transform the "+" to "-", and "/" to "_"
            for (int iter = 0; iter < endPos; iter++)
            {
                char c = base64Str[iter];

                switch (c)
                {
                    case '+':
                        base64Chars[iter] = '-';
                        break;

                    case '/':
                        base64Chars[iter] = '_';
                        break;

                    case '=':
                        Debug.Assert(false);
                        base64Chars[iter] = c;
                        break;

                    default:
                        base64Chars[iter] = c;
                        break;
                }
            }
            return new string(base64Chars);
        }

        private static byte[] UrlTokenDecode(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int len = input.Length;
            if (len < 1)
                return new byte[0];

            ///////////////////////////////////////////////////////////////////
            // Step 1: Calculate the number of padding chars to append to this string.
            //         The number of padding chars to append is stored in the last char of the string.
            int numPadChars = (int)input[len - 1] - (int)'0';
            if (numPadChars < 0 || numPadChars > 10)
                return null;


            ///////////////////////////////////////////////////////////////////
            // Step 2: Create array to store the chars (not including the last char)
            //          and the padding chars
            char[] base64Chars = new char[len - 1 + numPadChars];


            ////////////////////////////////////////////////////////
            // Step 3: Copy in the chars. Transform the "-" to "+", and "*" to "/"
            for (int iter = 0; iter < len - 1; iter++)
            {
                char c = input[iter];

                switch (c)
                {
                    case '-':
                        base64Chars[iter] = '+';
                        break;

                    case '_':
                        base64Chars[iter] = '/';
                        break;

                    default:
                        base64Chars[iter] = c;
                        break;
                }
            }

            ////////////////////////////////////////////////////////
            // Step 4: Add padding chars
            for (int iter = len - 1; iter < base64Chars.Length; iter++)
            {
                base64Chars[iter] = '=';
            }

            // Do the actual conversion
            return Convert.FromBase64CharArray(base64Chars, 0, base64Chars.Length);
        }

    }
}