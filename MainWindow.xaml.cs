using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EncryptionTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        HandleKey mijnSleutel;
        List<HandleKey> sleutels = new List<HandleKey>();
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            loadKeyPic();
            

            vulCboSleutels(getPadSleutels());
        }
        private void loadKeyPic()
        {
            string thisDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string locatieEnBestandsnaam = (thisDirectory + @"\Images\sleutel.png");
            BitmapImage afbeelding = new BitmapImage(new Uri(locatieEnBestandsnaam, UriKind.RelativeOrAbsolute));
            imgSleutelGeactiveerd.Source = afbeelding;
            imgSleutelGeactiveerd.Visibility = Visibility.Hidden;
        }
        private void btnOpenWindowGenereerSleutel_Click(object sender, RoutedEventArgs e)
        {
            GenerateKey FrmGenerateKey = new GenerateKey();
            FrmGenerateKey.ShowDialog();
            vulCboSleutels(getPadSleutels());           
        }

        private void SelectCboKey()
        {
            if (cboKiesSleutel.Items.Count > 0)
            {
                mijnSleutel = sleutels.Where(x => x.MyKeyId == (int)cboKiesSleutel.SelectedValue).First();
                lblActieveSleutel.Content = $"Actieve sleutel: {mijnSleutel.MyKeyName}";
                if (lblActieveSleutel.Content != "")
                {
                    imgSleutelGeactiveerd.Visibility = Visibility.Visible;
                }
            }
        }
        #region Initialisatie sleutels
        private string getPadSleutels()
        {
            string thisDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string keyDirectory = (thisDirectory + @"\AESKeys");
            return keyDirectory;
        }

        private void vulCboSleutels(string pad)
        {
            try
            {
                cboKiesSleutel.ItemsSource = null;
            }
            catch (Exception ex)
            {
                
            }
            string[] keyFiles = Directory.GetFiles(pad);
            sleutels.Clear();
            foreach (string filePad in keyFiles)
            {
                HandleKey vulSleutel = new HandleKey();
                StreamReader reader;
                reader = File.OpenText(filePad);
                string id = reader.ReadLine();
                string naam = reader.ReadLine();
                string k = reader.ReadLine();
                string iv = reader.ReadLine();
                reader.Close();
                vulSleutel.MyKeyId = Convert.ToInt32(id);
                vulSleutel.MyKeyName = naam;
                vulSleutel.MyKey = System.Convert.FromBase64String(k);
                vulSleutel.MyIv = System.Convert.FromBase64String(iv);
                sleutels.Add(vulSleutel);
               
            }
            cboKiesSleutel.ItemsSource = sleutels;
            cboKiesSleutel.DisplayMemberPath = nameof(HandleKey.MyKeyName);
            cboKiesSleutel.SelectedValuePath = nameof(HandleKey.MyKeyId);
            cboKiesSleutel.SelectedIndex = 0;
        }
        #endregion

        private void btnOpenWindowEncryption_Click(object sender, RoutedEventArgs e)
        {
            if (mijnSleutel != null)
            {
            EncryptionWindow frm = new EncryptionWindow(mijnSleutel);
            frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Geen sleutel geselecteerd.");
            }
        }

        private void btnOpenWindowDecryption_Click(object sender, RoutedEventArgs e)
        {
            if (mijnSleutel != null)
            {
                DecryptionWindow frm = new DecryptionWindow(mijnSleutel);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Geen sleutel geselecteerd.");
            }
        }

        private void cboKiesSleutel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCboKey();
        }
    }
}
