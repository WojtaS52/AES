using ClassLibrary1;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] bufor;
        byte[][] keyBuf;
        public int keySize = 0;
        public MainWindow()
        {

            InitializeComponent();
        }

        private void Generowanie_klucza(object sender, RoutedEventArgs e)
        {
            string lol;
            lol=keysGenerator.keyGenerate(keySize);
            keyTextBox.Text = lol;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }   

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void radio_192bit_Checked(object sender, RoutedEventArgs e)
        {
            keySize = 24;
        }

        private void radio_128bit_Checked(object sender, RoutedEventArgs e)
        {
            keySize = 16;
        }


        private void radio_256bit_Checked(object sender, RoutedEventArgs e)
        {
            keySize = 32;
        }

        private void EncryptTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EncryptBtn_Click(object sender, RoutedEventArgs e)
        {
            keysGenerator log = new keysGenerator();
            DecryptTextBox1.Text = "";
            byte[][] key;

            byte[] data;
            byte[] result;

            data = Encoding.UTF8.GetBytes(keyTextBox.Text);
            //data = Convert.FromHexString(keyTextBox.Text);
            key = log.KeyExpansion(data);

            keyBuf = key;
            EncryptAndDecrypt log2 = new EncryptAndDecrypt(key);
            if (bufor == null)
            {
                data = Encoding.UTF8.GetBytes(EncryptTextBox.Text);
                //data = Convert.FromHexString(EncryptTextBox.Text);
            }
            else
            {
                data = bufor;
            }

            int keySizeBits = keySize * 8;
            result = log2.Encrypt(data, keySizeBits);

            bufor = result;

            //DecryptTextBox1.Text = Encoding.UTF8.GetString(result);
            DecryptTextBox1.Text = BitConverter.ToString(result).Replace("-", "");
            }

        private void DecryptTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DecryptBtn_Click(object sender, RoutedEventArgs e)
        {
            DecryptOnly d;
            EncryptTextBox.Text = "";
            if (keyBuf == null)
            {
                keysGenerator log = new keysGenerator();
                byte[] data;
                byte[][] key;
                data = Encoding.UTF8.GetBytes(keyTextBox.Text);
                //data = Convert.FromHexString(keyTextBox.Text);
                key = log.KeyExpansion(data);
                d = new DecryptOnly(key);
            }
            else
            {
                d = new DecryptOnly(keyBuf);
            }

            if(bufor == null)
            {
                byte[] data;
                //data = Encoding.UTF8.GetBytes(DecryptTextBox1.Text);
                data = Convert.FromHexString(DecryptTextBox1.Text);
                byte[] decrypted;
                int keySizeBits = keySize * 8;
                decrypted = d.Decrypt(data, keySizeBits);
                //EncryptTextBox.Text = Encoding.UTF8.GetString(decrypted);
                EncryptTextBox.Text = BitConverter.ToString(decrypted).Replace("-", "");
                byte[] readText = StringToByteArray(EncryptTextBox.Text);
                EncryptTextBox.Text = Encoding.UTF8.GetString(readText);
                bufor = decrypted;
            }
            else
            {
                byte[] decrypted;
                int keySizeBits = keySize * 8;
                decrypted = d.Decrypt(bufor, keySizeBits);
                //EncryptTextBox.Text = Encoding.UTF8.GetString(decrypted);
                EncryptTextBox.Text = BitConverter.ToString(decrypted).Replace("-", "");
                byte[] readText = StringToByteArray(EncryptTextBox.Text);
                EncryptTextBox.Text = Encoding.UTF8.GetString(readText);
                bufor = decrypted;
            }
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];
            using (var sr = new StringReader(hex))
            {
                for (int i = 0; i < NumberChars; i++)
                    bytes[i] =
                      Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            }
            return bytes;
        }
        private void SaveToFileBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] toSave = bufor; 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if(saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, toSave);
            }
        }

        private void ReadFromFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                byte[] cos = File.ReadAllBytes(ofd.FileName);
                EncryptTextBox.Text = Encoding.UTF8.GetString(cos);
                bufor = cos;
            }
        }

        private void readToDecryptBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                byte[] cos = File.ReadAllBytes(ofd.FileName);
                DecryptTextBox1.Text = BitConverter.ToString(cos).Replace("-", "");
                bufor = cos;
            }
        }

        private void saveDecryptBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] toSave = bufor;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, toSave);
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            bufor = null;
            DecryptTextBox1.Text = null;
            EncryptTextBox.Text = null;
            keyBuf = null;
            keyTextBox.Text = null;
        }
    }
}
