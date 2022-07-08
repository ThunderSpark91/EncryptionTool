using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace EncryptionTool
{
    public static class Encode_Decode
    {
        #region Encode-Decode String to bytes and bytes back to base 64
        public static byte[] StringToBytes(string encodestring)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(encodestring);
            return bytes;
        }

        public static string ByteToBase64(byte[] array)
        {
            string Base64String = Convert.ToBase64String(array);
            return Base64String;

        }
        #endregion

        #region ConvertbytestoString
        public static string BitsToString(byte[] array)
        {
            string bitString = BitConverter.ToString(array);
            return bitString;

        }
        #endregion






        #region Encode-Decode EncodeFileToBase64 and EncodeFile_path_to_bytes
        public static string FileToBase64(string pathfile)
        {
            byte[] bytes = File.ReadAllBytes(pathfile);
            string file = Convert.ToBase64String(bytes);
            return file;
        }

        // Redacted
        public static byte[] File_path_to_bytes(string pathfile)
        {
            string Base64stringfile = FileToBase64(pathfile);
            byte[] bytes = StringToBytes(Base64stringfile);
            return bytes;
        }

        #endregion


        #region to read the txtfile text
        public static string ReadTxtFile(string filePath)
        {
            var reader = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();
            reader.Close();
            var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            writer.Write(text);
            writer.Close();
            return text;
        }
        #endregion

        #region Encode the txt file
        public static byte[] TXTFileToBytes(string filePath)
        {
            string filetext = ReadTxtFile(filePath);
            byte[] bytesoffile = Encoding.UTF8.GetBytes(filetext);
            string base64string = Convert.ToBase64String(bytesoffile);
            byte[] bytes = StringToBytes(base64string);
            return bytes;
        }
        #endregion

        #region Image to byte convert functions and then backwards
        //convert image to bytearray
        public static byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        //convert bytearray to image
        public static BitmapImage byteArrayToBitmapImage(byte[] byteArrayIn)
        {
            BitmapImage bi = new BitmapImage();

            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                ms.Position = 0;

                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
            }
            return bi;
        }


        //convert bytearray to image
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }
        }
        //another easy way to convert image to bytearray
        public static byte[] imgToByteConverter(Image inImg)
        {
            ImageConverter imgCon = new ImageConverter();
            return (byte[])imgCon.ConvertTo(inImg, typeof(byte[]));
        }
        #endregion


        public static byte[] ImageToBytes(string filepath)
        {
            Image img = Image.FromFile(filepath);
            byte[] bArr = imgToByteArray(img);
            return bArr;

        }

        public static string ImageToBase64String(string filepath)
        {
            byte[] bytes = ImageToBytes(filepath);
            string image = ByteToBase64(bytes);
            return image;
        }


        public static string ImageToString(string filepath)
        {
            byte[] bytes = ImageToBytes(filepath);
            string image = BitsToString(bytes);
            return image;
        }




















    }
}
