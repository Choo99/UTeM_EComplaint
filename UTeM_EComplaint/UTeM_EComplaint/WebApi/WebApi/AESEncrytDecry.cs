using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;









namespace WebApi
{
    public class AESEncrytDecry
    {


        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }
        public static string DecryptStringAES(string cipherText)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes("30801#4020802070");
                var iv = Encoding.UTF8.GetBytes("&^801040208020sa");

                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
              //  return decriptedFromJavascript;
                return string.Format(decriptedFromJavascript);
            }
            catch (Exception e)
            {
                return "error";
            }
        }

       
        public static string EnecryptStringAES(string cipherText)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes("30801#4020802070");
                var iv = Encoding.UTF8.GetBytes("&^801040208020sa");

                var decriptedFromJavascript = EncryptStringToBytes(cipherText, keybytes, iv);
                string b64ciphertext = Convert.ToBase64String(decriptedFromJavascript);
                return b64ciphertext;
            }
            catch (Exception e)
            {
                return "error";
            }
        }

        public static string DecryptStringAES_notuser(string cipherText, string user)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes("m0801#40208we070");
                var iv = Encoding.UTF8.GetBytes("&^801040208020ta");
                if (user.Length == 5)
                {
                    keybytes = Encoding.UTF8.GetBytes("o0801#402ty02070");
                    iv = Encoding.UTF8.GetBytes("k^801040" + user + "0ua");
                }
                else if (user.Length == 10)
                {
                    keybytes = Encoding.UTF8.GetBytes("20801#4zr0802070");
                    iv = Encoding.UTF8.GetBytes("q^x" + user + "0#a");
                }
                else
                {
                    // var keybytes = Encoding.UTF8.GetBytes("30801#4020802070");
                    // var iv = Encoding.UTF8.GetBytes("&^801040208020sa");
                }
                // var keybytes = Encoding.UTF8.GetBytes("30801#4020802" + user + "070");
                // var iv = Encoding.UTF8.GetBytes("&^80104020802" + user + "0sa");
                // var keybytes = Encoding.UTF8.GetBytes("30801#4020802070");
                // var iv = Encoding.UTF8.GetBytes("&^801040208020sa");

                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                return string.Format(decriptedFromJavascript);
            }
            catch (Exception e)
            {
                return "error";
            }
        }


        public static string EnecryptStringAES_notuser(string cipherText, string user)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes("m0801#40208we070");
                var iv = Encoding.UTF8.GetBytes("&^801040208020ta");
                if (user.Length == 5)
                {
                       keybytes = Encoding.UTF8.GetBytes("o0801#402ty02070");
                       iv = Encoding.UTF8.GetBytes("k^801040" + user + "0ua");
                }
                else if(user.Length == 10)
                {
                     keybytes = Encoding.UTF8.GetBytes("20801#4zr0802070");
                     iv = Encoding.UTF8.GetBytes("q^x" + user + "0#a");
                }
                else
                {
                   // var keybytes = Encoding.UTF8.GetBytes("30801#4020802070");
                   // var iv = Encoding.UTF8.GetBytes("&^801040208020sa");
                }




                var decriptedFromJavascript = EncryptStringToBytes(cipherText, keybytes, iv);
                string b64ciphertext = Convert.ToBase64String(decriptedFromJavascript);
                return b64ciphertext;
            }
            catch (Exception e)
            {
                return "error";
            }
        }





    }

}




//You can find here a simple example to accomplish it.

//Encrypt in node.js:

//var crypto = require('crypto');
//var key = '00000000000000000000000000000000'; //replace with your key
//var iv = '0000000000000000'; //replace with your IV
//var cipher = crypto.createCipheriv('aes256', key, iv)
//var crypted = cipher.update(authorizationKey, 'utf8', 'base64')
//crypted += cipher.final('base64');
//console.log(crypted);

//Decrypt with C#:

//string keyString = "00000000000000000000000000000000"; //replace with your key
//string ivString = "0000000000000000"; //replace with your iv

//byte[] key = Encoding.ASCII.GetBytes(keyString);
//byte[] iv = Encoding.ASCII.GetBytes(ivString);

//using (var rijndaelManaged =
//        new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
//        {
//            rijndaelManaged.BlockSize = 128;
//            rijndaelManaged.KeySize = 256;
//            using (var memoryStream =
//                   new MemoryStream(Convert.FromBase64String(AuthorizationCode)))
//            using (var cryptoStream =
//                   new CryptoStream(memoryStream,
//                       rijndaelManaged.CreateDecryptor(key, iv),
//                       CryptoStreamMode.Read))
//            {
//                return new StreamReader(cryptoStream).ReadToEnd();
//            }
//        }

//Hope this helps.