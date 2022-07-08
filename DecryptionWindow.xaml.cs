using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EncryptionTool
{
    /// <summary>
    /// Interaction logic for DecryptionWindow.xaml
    /// </summary>
    public partial class DecryptionWindow : Window
    {
        private HandleKey _myKey;
        private string _filePath;
        private string _sourceFilePath;
        private string _textFileSource;
        private string _imgFileSource;
        private string _saveFilePath;
        private BitmapImage _bitmapImage;

        public DecryptionWindow(HandleKey myKey)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            _myKey = myKey;
            _filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\AESCiphers";

            lblKey.Content = _myKey.MyKeyName;
            rbtnImage.IsChecked = true;
        }

        private void btnChooseFilePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _filePath;
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            string filetext;

            DialogResult result = openFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Stream stream;
                stream = openFileDialog.OpenFile();
                StreamReader reader = new StreamReader(stream);

                filetext = reader.ReadToEnd();
                txtCipher.Text = filetext;
                reader.Close();
                stream.Close();
                if ((bool)rbtnImage.IsChecked)
                {
                    _imgFileSource = filetext;
                }
                else if ((bool)rbtnTextfile.IsChecked)
                {                    
                    _textFileSource = filetext;
                }
                _sourceFilePath = openFileDialog.FileName;
                lblSelectPath.Content = _sourceFilePath;
            }
        }

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rbtnText.IsChecked)
            {
                AesEncryption crypt = new AesEncryption();
                byte[] input;
                try
                {
                    input = Convert.FromBase64String(txtCipher.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Input is niet base64");
                    return;
                }
                string cipher = crypt.DecryptToString(input, _myKey.MyKey, _myKey.MyIv);
                txtPLaintext.Text = cipher;
            }
            if ((bool)rbtnTextfile.IsChecked)
            {
                if (_textFileSource != null)
                {
                    byte[] input;
                    try
                    {
                        input = Convert.FromBase64String(_textFileSource);
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("Input is niet base64");
                        return;
                    }
                    AesEncryption crypt = new AesEncryption();
                    string cipher = crypt.DecryptToString(input, _myKey.MyKey, _myKey.MyIv);
                    txtPLaintext.Text = cipher;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Selecteer een bronbestand");
                }
            }

            if ((bool)rbtnImage.IsChecked)
            {
                if (_imgFileSource != null)
                {
                    byte[] input;
                    try
                    {
                        input = Convert.FromBase64String(_imgFileSource);
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("Input is niet base64");
                        return;
                    }
                    AesEncryption crypt = new AesEncryption();
                    string cipher = crypt.DecryptToString(input, _myKey.MyKey, _myKey.MyIv);
                    BitmapImage bi = new BitmapImage();
                    try
                    {
                        bi = Encode_Decode.byteArrayToBitmapImage(Convert.FromBase64String(cipher));
                    }
                    catch (Exception)
                    {

                        System.Windows.Forms.MessageBox.Show("Kan afbeelding niet decrypteren");
                        return;
                    }
                    imgOrigionalImage.Source = bi;
                    txtPLaintext.Text = cipher;
                    _bitmapImage = bi;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Selecteer een bronbestand");
                }
            }
        }

        private void txtPLaintext_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPLaintext.Clear();
        }

        private void txtPLaintext_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPLaintext.Text == "")
            {
                txtPLaintext.Text = "Plak hier de cyphertekst...";
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (rbtnImage.IsChecked == true)
            {
                imgOrigionalImage.Visibility = Visibility.Visible;
                txtCipher.Visibility = Visibility.Collapsed;
                txtPLaintext.Visibility = Visibility.Collapsed;
                btnSelect.Visibility = Visibility.Visible;
            }
            else if (rbtnTextfile.IsChecked == true)
            {
                imgOrigionalImage.Visibility = Visibility.Collapsed;
                txtPLaintext.Visibility = Visibility.Visible;
                txtCipher.Visibility = Visibility.Collapsed;
                btnSelect.Visibility = Visibility.Visible;
                txtCipher.IsReadOnly = true;
            }
            else
            {
                imgOrigionalImage.Visibility = Visibility.Collapsed;
                txtPLaintext.Visibility = Visibility.Visible;
                txtCipher.Visibility = Visibility.Visible;
                btnSelect.Visibility = Visibility.Collapsed;
                txtCipher.IsReadOnly = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (rbtnTextfile.IsChecked == true || rbtnText.IsChecked == true)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "txt files (*.txt)|*.txt";
                saveFileDialog.InitialDirectory = _saveFilePath;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string plainText = txtPLaintext.Text;
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        sw.Write(plainText);
                    }
                    System.Windows.MessageBox.Show("Bestand is opgeslagen.");
                    _saveFilePath = saveFileDialog.FileName;
                    lblSavePath.Content = _saveFilePath;
                }
            }
            if (rbtnImage.IsChecked == true)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "jpg files (*.jpg)|*.jpg";
                saveFileDialog.InitialDirectory = _saveFilePath;
                ImageFormat format = ImageFormat.Png;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(_bitmapImage));

                    using (var fileStream = new System.IO.FileStream(saveFileDialog.FileName, System.IO.FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                    System.Windows.MessageBox.Show("Bestand is opgeslagen.");
                    _saveFilePath = saveFileDialog.FileName;
                    lblSavePath.Content = _saveFilePath;
                }
            }
        }
    }
}
