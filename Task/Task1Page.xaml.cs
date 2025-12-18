using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp12.View.PageTask
{
    public partial class Task1Page : Page
    {
        public Task1Page()
        {
            InitializeComponent();
        }

        private void BtnCalculateNok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BigInteger a = BigInteger.Parse(txtNokA.Text);
                BigInteger b = BigInteger.Parse(txtNokB.Text);
                BigInteger gcd = Gcd(a, b);
                BigInteger nok = BigInteger.Multiply(a, b) / gcd;

                txtNokResult.Text = $"НОД({a},{b}) = {gcd}, НОК({a},{b}) = {nok}";
                txtNokResult.Foreground = Brushes.LimeGreen;
            }
            catch (Exception ex)
            {
                txtNokResult.Text = "Ошибка: " + ex.Message;
                txtNokResult.Foreground = Brushes.Red;
            }
        }

        private BigInteger Gcd(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
