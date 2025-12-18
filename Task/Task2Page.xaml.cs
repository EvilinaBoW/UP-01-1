using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp12.View.PageTask
{
    public partial class Task2Page : Page
    {
        public Task2Page()
        {
            InitializeComponent();
        }

        private void BtnFindPrimes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int start = int.Parse(txtEratStart.Text);
                int end = int.Parse(txtEratEnd.Text);

                var primes = SieveOfEratosthenes(start, end);

                lstPrimes.ItemsSource = primes;
                txtEratResult.Text = $"Найдено простых чисел: {primes.Count} в диапазоне [{start}, {end}]";
                txtEratResult.Foreground = Brushes.LimeGreen;
            }
            catch (Exception ex)
            {
                txtEratResult.Text = "Ошибка: " + ex.Message;
                txtEratResult.Foreground = Brushes.Red;
            }
        }

        private List<int> SieveOfEratosthenes(int start, int end)
        {
            if (start < 2) start = 2;

            var primes = new List<int>();
            if (end < 2) return primes;

            bool[] isPrime = new bool[end + 1];
            for (int i = 2; i <= end; i++)
                isPrime[i] = true;

            for (int i = 2; i * i <= end; i++)
            {
                if (isPrime[i])
                {
                    for (int j = i * i; j <= end; j += i)
                        isPrime[j] = false;
                }
            }

            for (int i = Math.Max(2, start); i <= end; i++)
                if (isPrime[i]) primes.Add(i);

            return primes;
        }
    }
}
