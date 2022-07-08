using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Linq;

namespace EncryptionTool
{
    /// <summary>
    /// Interaction logic for GenerateKey.xaml
    /// </summary>
    public partial class GenerateKey : Window
    {
        HandleKey myKeyHandler = new HandleKey();
        public GenerateKey()
        {
            InitializeComponent();
        }

        //Sleutel en IV aanmaken
        //ID bepalen 
        //Method opslaan oproepen
        //Boodschap succes
        private void btnGenereerSleutel_Click(object sender, RoutedEventArgs e)
        {
            string sleutelnaam = txtSleutelNaam.Text;
            myKeyHandler.CreateKey(sleutelnaam);


            string thisDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string keyDirectory = (thisDirectory + @"\AESKeys");

            string[] currentKeys = Directory.GetFiles(keyDirectory);
            List<int> ids = new List<int>();
            foreach (var file in currentKeys)
            {
                StreamReader reader;
                reader = File.OpenText(file);
                string id = reader.ReadLine();
                reader.Close();
                ids.Add(Convert.ToInt32(id));
            }
            int newId;
            int maxId = 0;
            try
            {
                maxId = ids.Max();
            }
            catch (Exception ex)
            {
                maxId = 0;
            }
            
            newId = maxId + 1;
            myKeyHandler.MyKeyId = newId;
            bool succes = SleutelOpslaan(keyDirectory);
            if (succes==true)
            {
                txtResultaat.Text += $"De sleutel {sleutelnaam} is aangemaakt en opgeslagen!\n";
            }
            else
            {
                txtResultaat.Text += $"Er ging iets mis met het opslaan van sleutel: {sleutelnaam}!\n";
            }
        }

        //Omzetten van sleutel en iv naar Base 64
        //Opslaan in AESKeys (folder in project)
        private bool SleutelOpslaan(string pad) //naam, key, iv
        {
            string keyString = Convert.ToBase64String(myKeyHandler.MyKey, 0, myKeyHandler.MyKey.Length);
            string ivString = Convert.ToBase64String(myKeyHandler.MyIv, 0, myKeyHandler.MyIv.Length);
            string uitvoerbestand = pad;
            uitvoerbestand += @$"\key{myKeyHandler.MyKeyId}";


            StreamWriter writer;
            try
            {
                writer = File.CreateText(uitvoerbestand);
                writer.WriteLine(myKeyHandler.MyKeyId.ToString());
                writer.WriteLine(myKeyHandler.MyKeyName);
                writer.WriteLine(keyString);
                writer.WriteLine(ivString);
                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
              
        }

    }
}
