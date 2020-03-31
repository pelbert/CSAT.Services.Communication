using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace CSAT.Services.Communication.DataCore.Common
{
    public static class EncryptionHelper
    {
        private const string SHA_HASH_SALT = "sda!ls@jkSFKsa";
        private static readonly byte[] AES_KEY = { 123, 217, 219, 37, 24, 26, 85, 45, 114, 42, 27, 162, 37, 219, 222, 9, 241, 24, 175, 91, 207, 53, 196, 148, 24, 26, 17, 218, 131, 236, 153, 2 };
        private static readonly byte[] AES_INITIAL_VECTOR = { 24, 64, 141, 213, 23, 3, 113, 119, 7, 121, 251, 112, 79, 32, 176, 156 };

        private const string PUBLIC_KEY_FORMAT = "-----BEGIN PUBLIC KEY-----\r\n{0}\r\n-----END PUBLIC KEY-----";
        private const string PRIVATE_KEY_FORMAT = "-----BEGIN PRIVATE KEY-----\r\n{0}\r\n-----END PRIVATE KEY-----";

        public static string HashText(string clearText)
        {
            //using code from accepted answer to this SO question
            // http://stackoverflow.com/questions/1300890/md5-hash-with-salt-for-keeping-password-in-db-in-c-sharp
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(clearText);
            byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(SHA_HASH_SALT);
            HMACSHA256 hmac = new HMACSHA256(saltBytes);
            byte[] hashedBytes = hmac.ComputeHash(textBytes);
            return Convert.ToBase64String(hashedBytes);

        }

        //based on MSDN AESManaged class example code available at
        //http://msdn.microsoft.com/en-us/library/system.security.cryptography.aesmanaged(v=vs.110).aspx
        //with some extra tips taken from this question from SO:
        //http://stackoverflow.com/questions/165808/simple-two-way-encryption-for-c-sharp

        //This function takes any string and will return an URL-safe encoded string
        public static string EncryptString(string clearText)
        {
            string retVal;
            byte[] encrypted;
            // Create an AesManaged object 
            // with the specified key and IV. 
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = AES_KEY;
                aesAlg.IV = AES_INITIAL_VECTOR;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(clearText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            retVal = Convert.ToBase64String(encrypted);
            return retVal;
        }

        //This takes a URL-safe encoded string and returns the original plain text decoded string
        //URL encoding is taken care of in both functions to eliminate possible error situations
        public static string DecryptString(string encText)
        {
            string retVal = null;

            if (!string.IsNullOrEmpty(encText))
            {
                byte[] cipherText = Convert.FromBase64String(encText);
                // Create an AesManaged object 
                // with the specified key and IV. 
                using (AesManaged aesAlg = new AesManaged())
                {
                    aesAlg.Key = AES_KEY;
                    aesAlg.IV = AES_INITIAL_VECTOR;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption. 
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                // Read the decrypted bytes from the decrypting stream 
                                // and place them in a string.
                                retVal = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }
            }
            return retVal;
        }

        //More SO help
        //https://stackoverflow.com/questions/28086321/c-sharp-bouncycastle-rsa-encryption-with-public-private-keys
        public static string RsaEncryptWithPublic(string clearText, string publicKey)
        {
            var pem = string.Format(PUBLIC_KEY_FORMAT, publicKey);
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(pem))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;

        }

        public static string RsaEncryptWithPrivate(string clearText, string privateKey)
        {
            var pem = string.Format(PRIVATE_KEY_FORMAT, privateKey);
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(pem))
            {
                var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyPair.Private);
            }

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;
        }

        public static string RsaDecryptWithPrivate(string base64Input, string privateKey)
        {
            var pem = string.Format(PRIVATE_KEY_FORMAT, privateKey);
            var bytesToDecrypt = Convert.FromBase64String(base64Input);

            AsymmetricCipherKeyPair keyPair;
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(pem))
            {
                keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();

                decryptEngine.Init(false, keyPair.Private);
            }

            var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
            return decrypted;
        }

        public static string RsaDecryptWithPublic(string base64Input, string publicKey)
        {
            var pem = string.Format(PUBLIC_KEY_FORMAT, publicKey);
            var bytesToDecrypt = Convert.FromBase64String(base64Input);

            var decryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(pem))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                decryptEngine.Init(false, keyParameter);
            }

            var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
            return decrypted;
        }
    }
}
