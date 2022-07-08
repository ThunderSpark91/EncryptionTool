using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.IO;
using System.Linq;

namespace EncryptionTool
{
    public class HandleKey
    {

        public HandleKey()
        {
            MyKey = null;
            MyIv = null;
            MyKeyName = string.Empty;
            MyKeyId = -1;
        }
        public HandleKey(byte[] key, byte[] iv, string name, int id)
        {
            MyKey= key;
            MyIv= iv;
            MyKeyName= name;
            MyKeyId= id;
        }

        public byte[] MyKey { get; set; }

        public byte[] MyIv { get; set; }
        public string MyKeyName { get; set; }
        public int MyKeyId { get; set; }

        public void CreateKey(string keyname)
        {
            using (Aes myAes = Aes.Create())
            {
                MyKey = myAes.Key;
                MyIv = myAes.IV;
                MyKeyName = keyname;
            }
        }
    }
}
