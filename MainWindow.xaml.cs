using System;
using System.Numerics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Windows.Media;

namespace WpfApp12
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<int> demoStack = new ObservableCollection<int>();
        private string jsonContent = "";

        public MainWindow()
        {
            InitializeComponent();
            lstStack.ItemsSource = demoStack;
        }

        // Задача 10.1 НОК через НОД (Евклид)
        private void BtnCalculateNok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BigInteger a = BigInteger.Parse(txtNokA.Text);
                BigInteger b = BigInteger.Parse(txtNokB.Text);
                BigInteger gcd = Gcd(a, b);
                BigInteger nok = BigInteger.Multiply(a, b) / gcd;
                txtNokResult.Text = "НОД(" + a + "," + b + ") = " + gcd + ", НОК(" + a + "," + b + ") = " + nok;
                statusBar.Content = "НОК вычислен: " + nok;
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

        // Задача 10.2 Решето Эратосфена
        private void BtnFindPrimes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int start = int.Parse(txtEratStart.Text);
                int end = int.Parse(txtEratEnd.Text);
                var primes = SieveOfEratosthenes(start, end);
                lstPrimes.ItemsSource = primes;
                txtEratResult.Text = "Найдено простых чисел: " + primes.Count + " в диапазоне [" + start + ", " + end + "]";
                statusBar.Content = "Найдено " + primes.Count + " простых чисел";
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
            bool[] isPrime = new bool[end + 1];
            for (int i = 2; i <= end; i++) isPrime[i] = true;

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

        // Задача 10.3 Класс Stack
        public class Stack<T>
        {
            private List<T> items = new List<T>();

            public void Push(T item) => items.Add(item);
            public T Pop()
            {
                if (items.Count == 0) throw new InvalidOperationException("Стек пуст");
                T item = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                return item;
            }
            public T Peek()
            {
                if (items.Count == 0) throw new InvalidOperationException("Стек пуст");
                return items[items.Count - 1];
            }
            public bool IsEmpty() => items.Count == 0;
            public int Size() => items.Count;
        }

        private void BtnCheckBrackets_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = txtBrackets.Text;
                bool isValid = CheckBrackets(expression);
                txtBracketsResult.Text = isValid ? "Скобки расставлены правильно ✓" : "Ошибка в скобках ✗";
                txtBracketsResult.Foreground = isValid ? Brushes.Green : Brushes.Red;
                statusBar.Content = "Скобки проверены: " + isValid;
            }
            catch (Exception ex)
            {
                txtBracketsResult.Text = "Ошибка: " + ex.Message;
                txtBracketsResult.Foreground = Brushes.Red;
            }
        }

        private bool CheckBrackets(string expression)
        {
            var stack = new Stack<char>();
            var pairs = new Dictionary<char, char> { { ')', '(' }, { ']', '[' }, { '}', '{' } };

            foreach (char c in expression)
            {
                if (pairs.ContainsValue(c)) stack.Push(c);
                else if (pairs.ContainsKey(c))
                {
                    if (stack.IsEmpty()) return false;
                    if (stack.Pop() != pairs[c]) return false;
                }
            }
            return stack.IsEmpty();
        }

        private void BtnStackPush_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = Interaction.InputBox("Введите число:", "Push");
                if (int.TryParse(input, out int value))
                {
                    demoStack.Add(value);
                    statusBar.Content = "Push: " + value;
                }
            }
            catch { }
        }

        private void BtnStackPop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (demoStack.Count > 0)
                {
                    int value = demoStack[demoStack.Count - 1];
                    demoStack.RemoveAt(demoStack.Count - 1);
                    statusBar.Content = "Pop: " + value;
                }
                else
                {
                    statusBar.Content = "Стек пуст";
                }
            }
            catch (Exception ex)
            {
                statusBar.Content = ex.Message;
            }
        }

        private void BtnStackPeek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (demoStack.Count > 0)
                {
                    int value = demoStack[demoStack.Count - 1];
                    statusBar.Content = "Peek: " + value;
                }
                else
                {
                    statusBar.Content = "Стек пуст";
                }
            }
            catch (Exception ex)
            {
                statusBar.Content = ex.Message;
            }
        }

        // Задача 10.4 Подсчет инверсий (Merge Sort)
        private void BtnCountInversions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var strings = txtInversions.Text.Split(',');
                var array = new List<int>();
                foreach (string s in strings)
                {
                    string trimmed = s.Trim();
                    if (int.TryParse(trimmed, out int num))
                    {
                        array.Add(num);
                    }
                }
                int[] arr = array.ToArray();
                long inversions = CountInversions(arr);
                txtInversionsResult.Text = "Количество инверсий: " + inversions;
                lstInversions.ItemsSource = arr;
                statusBar.Content = "Инверсий найдено: " + inversions;
            }
            catch (Exception ex)
            {
                txtInversionsResult.Text = "Ошибка: " + ex.Message;
                txtInversionsResult.Foreground = Brushes.Red;
            }
        }

        private long CountInversions(int[] array)
        {
            int[] temp = new int[array.Length];
            return MergeSort(array, temp, 0, array.Length - 1);
        }

        private long MergeSort(int[] array, int[] temp, int left, int right)
        {
            long invCount = 0;
            if (left < right)
            {
                int mid = (left + right) / 2;
                invCount += MergeSort(array, temp, left, mid);
                invCount += MergeSort(array, temp, mid + 1, right);
                invCount += Merge(array, temp, left, mid + 1, right);
            }
            return invCount;
        }

        private long Merge(int[] array, int[] temp, int left, int mid, int right)
        {
            int i = left;
            int j = mid;
            int k = left;
            long invCount = 0;

            while (i <= mid - 1 && j <= right)
            {
                if (array[i] <= array[j])
                {
                    temp[k++] = array[i++];
                }
                else
                {
                    temp[k++] = array[j++];
                    invCount += (mid - i);
                }
            }

            while (i <= mid - 1)
            {
                temp[k++] = array[i++];
            }
            while (j <= right)
            {
                temp[k++] = array[j++];
            }

            for (i = left; i <= right; i++)
            {
                array[i] = temp[i];
            }
            return invCount;
        }

        // Задача 10.5 JSON обработка (упрощенная версия без Newtonsoft)
        private void BtnLoadJson_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    jsonContent = File.ReadAllText(dlg.FileName);
                    txtJsonContent.Text = jsonContent;
                    txtJsonStatus.Text = "Загружен файл: " + Path.GetFileName(dlg.FileName);
                    statusBar.Content = "JSON загружен успешно";
                }
                catch (Exception ex)
                {
                    txtJsonStatus.Text = "Ошибка загрузки: " + ex.Message;
                    txtJsonStatus.Foreground = Brushes.Red;
                }
            }
        }

        private void BtnSearchJson_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(jsonContent))
            {
                txtJsonStatus.Text = "Сначала загрузите JSON файл";
                txtJsonStatus.Foreground = Brushes.Red;
                return;
            }

            try
            {
                string field = txtSearchField.Text;
                string value = txtSearchValue.Text;

                // Исправленный паттерн для поиска JSON полей
                string pattern = "\"" + Regex.Escape(field) + "\"\\s*:\\s*\"([^\"\\\\]|\\\\.)*\"";
                var matches = Regex.Matches(jsonContent, pattern, RegexOptions.IgnoreCase);

                var results = new List<string>();
                foreach (Match match in matches)
                {
                    string foundValue = match.Groups[1].Value;
                    if (string.IsNullOrEmpty(value) || foundValue.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add("найдено: " + field + " = " + foundValue);
                    }
                }

                txtJsonStatus.Text = "Найдено результатов: " + results.Count;
                txtJsonStatus.Foreground = Brushes.Blue;
                txtJsonContent.Text = string.Join("\n", results);
                statusBar.Content = "JSON поиск: " + results.Count + " совпадений";
            }
            catch (Exception ex)
            {
                txtJsonStatus.Text = "Ошибка поиска: " + ex.Message;
                txtJsonStatus.Foreground = Brushes.Red;
            }
        }
    }
}
