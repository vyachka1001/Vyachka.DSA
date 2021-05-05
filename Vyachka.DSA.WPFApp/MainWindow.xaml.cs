using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using Vyachka.DSA.AlgorithmImpl;

namespace Vyachka.DSA.WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Sign_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldsFilled("sign"))
            {
                return;
            }

            if (!IsFieldsFilledCorrectly("sign"))
            {
                return;
            }

            BigInteger q = BigInteger.Parse(QValue_textBox.Text);
            BigInteger p = BigInteger.Parse(PValue_textBox.Text);
            BigInteger k = BigInteger.Parse(KValue_textBox.Text);
            BigInteger x = BigInteger.Parse(XValue_textBox.Text);
            BigInteger h = BigInteger.Parse(HValue_textBox.Text);
            byte[] initialMsg = File.ReadAllBytes(FilePath_textBox.Text);

            if (Signer.SignInitialMsg(initialMsg, q, p, k, x, h , out BigInteger r, out BigInteger s))
            {
                Hash_textBox.Text = Helper.CountHashImage(initialMsg).ToString();
                string extension = FilePath_textBox.Text.Substring(FilePath_textBox.Text.Length - 4, 4);
                string filePath = FilePath_textBox.Text[0..^4];
                filePath = filePath + "_signed" + extension;
                File.WriteAllBytes(filePath, initialMsg);
                File.AppendAllText(filePath, ',' + r.ToString() + ',' + s.ToString());
            }
            else
            {
                MessageBox.Show("R or S value is 0. Please, rewrite k value.", "Error with input values", 
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckSign_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldsFilled("check"))
            {
                return;
            }

            if (!IsFieldsFilledCorrectly("check"))
            {
                return;
            }

            BigInteger q = BigInteger.Parse(QValue_textBox.Text);
            BigInteger p = BigInteger.Parse(PValue_textBox.Text);
            BigInteger x = BigInteger.Parse(XValue_textBox.Text);
            BigInteger h = BigInteger.Parse(HValue_textBox.Text);
            byte[] initialMsg = File.ReadAllBytes(FilePath_textBox.Text);

            BigInteger r = 1;
            BigInteger s = 2;
            string result;
            Hash_textBox.Text = Helper.CountHashImage(initialMsg).ToString();

            if (Signer.CheckSign(initialMsg, r, s, q, p, h, x, out BigInteger v))
            {
                result = "EQUAL";
            }
            else
            {
                result = "NOT EQUAL";
            }

            MessageBox.Show($"Checking result:\nr = {r}; v = {v}\nResult is {result}", "Checking result",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool IsFieldsFilled(string action)
        {
            if (QValue_textBox.Text == "" || PValue_textBox.Text == "" || HValue_textBox.Text == "" ||
                XValue_textBox.Text == "" || FilePath_textBox.Text == "")
            {
                MessageBox.Show("Please, fill all input fields!\n(You can only left K empty, " +
                                "if you are checking signature)", "Error with input values", MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                return false;
            }

            if (action == "sign" && KValue_textBox.Text == "")
            {
                MessageBox.Show("Please, fill all input fields!", "Error with input values",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool IsFieldsFilledCorrectly(string action)
        {
            bool isValid = true;
            if (!Checker.MillerRabinTest(BigInteger.Parse(QValue_textBox.Text), 10))
            {
                MessageBox.Show("Q value must be prime.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }

            if (!Checker.CheckP(QValue_textBox.Text, PValue_textBox.Text))
            {
                MessageBox.Show("P value must be prime and (p - 1) mod q = 0", 
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }

            if (!Checker.CheckH(QValue_textBox.Text, PValue_textBox.Text, HValue_textBox.Text))
            {
                MessageBox.Show("H value must be in (1; p - 1) prime and h ^ ((p - 1) / q) mod p > 1",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }

            if (!Checker.CheckIsInInterval("0", QValue_textBox.Text, XValue_textBox.Text))
            {
                MessageBox.Show("X value must be in (0; q)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }

            if (action == "sign" && !Checker.CheckIsInInterval("0", QValue_textBox.Text, KValue_textBox.Text))
            {
                MessageBox.Show("K value must be in (0; q)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }

            if (!File.Exists(FilePath_textBox.Text))
            {
                MessageBox.Show("File with current file path does not exist.", "Error", 
                                MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }

            return isValid;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Text = new string
                (
                    textBox.Text
                           .Where
                            (ch =>
                                ch == '0' || ch == '1' || ch == '2' || ch == '3' ||
                                ch == '4' || ch == '5' || ch == '6' || ch == '7' ||
                                ch == '8' || ch == '9'
                            )
                           .ToArray()
                );
                textBox.SelectionStart = textBox.Text.Length;
                textBox.SelectionLength = 0;
            }
        }
    }
}
