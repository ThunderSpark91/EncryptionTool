using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;

namespace EncryptionTool
{
    public class AesEncryption
    {
        public string EncryptToBase64String(string plain, byte[] key, byte[] IV)
        {
            byte[] cipher;
            using (Aes myAes = Aes.Create())
            {
                // Check arguments.
                if (plain == null || plain.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");

                myAes.Key = key;
                myAes.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plain);
                        }
                        cipher = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(cipher);
        }

        public string DecryptToString(byte[] cipher, byte[] key, byte[] IV)
        {
            string decrypted = null;
            using (Aes myAes = Aes.Create())
            {
                myAes.Key = key;
                myAes.IV = IV;

                ICryptoTransform decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDencrypt = new StreamReader(csDecrypt))
                        {
                            try
                            {
                                decrypted = srDencrypt.ReadToEnd();
                            }
                            catch (Exception)
                            {

                                MessageBox.Show("Sleutel komt niet overeen");
                                return "";
                            }

                        }
                    }
                }
            }
            return decrypted;
        }
    }
}
