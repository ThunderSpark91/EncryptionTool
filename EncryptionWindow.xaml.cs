using System;
using System.Collections.Generic;
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
    /// Interaction logic for EncryptionWindow.xaml
    /// </summary>
    public partial class EncryptionWindow : Window
    {
        private HandleKey _myKey;
        private string _saveFilePath;
        private string _imgSourceFilePath;
        private string _txtSourceFilePath;
        private string _txtFileContent;


        public EncryptionWindow(HandleKey myKey)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            _myKey = myKey;
            _saveFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\AESCiphers";

            lblSavePath.Content = _saveFilePath;
            lblKey.Content = _myKey.MyKeyName;
            rbtnImage.IsChecked = true;

        }

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rbtnText.IsChecked)
            {
                AesEncryption encrypt = new AesEncryption();
                string cipher = encrypt.EncryptToBase64String(txtPLaintext.Text, _myKey.MyKey, _myKey.MyIv);
                txtCipher.Text = cipher;
            }
            else if ((bool)rbtnTextfile.IsChecked)
            {
                AesEncryption encrypt = new AesEncryption();
                string cipher = encrypt.EncryptToBase64String(_txtFileContent, _myKey.MyKey, _myKey.MyIv);
                txtCipher.Text = cipher;
            }
            else if ((bool)rbtnImage.IsChecked)
            {
                AesEncryption encrypt = new AesEncryption();
                string cipher = encrypt.EncryptToBase64String(Encode_Decode.ImageToBase64String(_imgSourceFilePath), _myKey.MyKey, _myKey.MyIv);
                txtCipher.Text = cipher;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = _saveFilePath;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ciphertext = txtCipher.Text;
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.Write(ciphertext);
                }
                System.Windows.MessageBox.Show("Bestand is opgeslagen.");
                _saveFilePath = saveFileDialog.FileName;
                lblSavePath.Content = _saveFilePath;
            }
        }


        private void txtPLaintext_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPLaintext.Clear();
        }

        private void txtPLaintext_LostFocus(object sender, RoutedEventArgs e)
        {
            if(txtPLaintext.Text == "")
            {
                txtPLaintext.Text = "Typ hier de plaintekst...";
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (rbtnImage.IsChecked == true)
            {
                imgOrigionalImage.Visibility = Visibility.Visible;
                txtPLaintext.Visibility = Visibility.Collapsed;
                btnSelect.Visibility = Visibility.Visible;
                lblSelectPath.Content = _imgSourceFilePath;
            }
            else if (rbtnTextfile.IsChecked == true)
            {
                imgOrigionalImage.Visibility = Visibility.Collapsed;
                txtPLaintext.Visibility = Visibility.Visible;
                btnSelect.Visibility = Visibility.Visible;
                txtPLaintext.IsReadOnly = true;
                txtPLaintext.Text = _txtFileContent;
                lblSelectPath.Content = _txtSourceFilePath;
            }
            else
            {
                imgOrigionalImage.Visibility = Visibility.Collapsed;
                txtPLaintext.Visibility = Visibility.Visible;
                btnSelect.Visibility = Visibility.Collapsed;
                txtPLaintext.IsReadOnly = false;
                txtPLaintext.Text = "Typ hier de Plaintekst";
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _saveFilePath;
            if ((bool)rbtnTextfile.IsChecked)
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                string filetext;

                DialogResult result = openFileDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Stream stream;
                    stream = openFileDialog.OpenFile();
                    StreamReader reader = new StreamReader(stream);

                    filetext = reader.ReadToEnd();
                    _txtFileContent = filetext;
                    txtPLaintext.Text = filetext;
                    reader.Close();
                    stream.Close();
                    _txtSourceFilePath = openFileDialog.FileName;
                    lblSelectPath.Content = _txtSourceFilePath;
                }
            }

            if ((bool)rbtnImage.IsChecked)
            {
                openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";

                DialogResult result = openFileDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string locatieEnBestandsnaam = openFileDialog.FileName;
                    BitmapImage afbeelding = new BitmapImage(new Uri(locatieEnBestandsnaam, UriKind.RelativeOrAbsolute));
                    imgOrigionalImage.Source = afbeelding;
                    _imgSourceFilePath = openFileDialog.FileName;
                    lblSelectPath.Content = _imgSourceFilePath;
                }

            }



        }
    }
}