using System;
using System.Collections.Generic;
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

namespace Kalkulator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> items = new List<string>();
        public string decimalSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Number_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (string.IsNullOrWhiteSpace(textBlock_screen.Text) && (string)btn?.Content == "0")
            {
                textBlock_screen.Text += $"0{decimalSeparator}";
            }
            else
            {
                textBlock_screen.Text += btn?.Content;
            }
        }

        private void button_plus_minus_Click(object sender, RoutedEventArgs e)
        {
            if (textBlock_screen.Text.Contains("-"))
            {
                textBlock_screen.Text = textBlock_screen.Text.Remove(0, 1);
            }
            else
            {
                textBlock_screen.Text = $"-{textBlock_screen.Text}";
            }
        }

        private void button_DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBlock_screen.Text))
            {
                textBlock_screen.Text = textBlock_screen.Text?.Remove(0);
            }
            items?.Clear();
        }

        private void button_Comma_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBlock_screen.Text) && !textBlock_screen.Text.Contains(decimalSeparator))
            {
                textBlock_screen.Text += decimalSeparator;
            }
        }

        private void button_ActionSymbol(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (!string.IsNullOrWhiteSpace(textBlock_screen.Text) && textBlock_screen.Text != "-")
            {
                items.Add(textBlock_screen.Text);
            }

            if (items.Count > 0)
            {
                if (items[items.Count - 1] == "-" || items[items.Count - 1] == "+" || items[items.Count - 1] == "x" || items[items.Count - 1] == "/")
                {
                    items[items.Count - 1] = (btn?.Content.ToString());
                }
                else
                {
                    items.Add(btn?.Content.ToString());
                }
            }

            textBlock_screen.Text = string.Empty;

        }

        private void button_Solution_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBlock_screen.Text))
            {
                items.Add(textBlock_screen.Text);
                items.Add("=");
            }

            if (items.Count > 0)
            {
                if (items.Count == 1)
                {
                    textBlock_screen.Text = items[0];
                }
                else
                {
                    List<double> itemsNumeric = new List<double>();
                    List<string> symbols = new List<string>();

                    for (int i = 0; i < items.Count; i += 2)
                    {
                        double.TryParse(items[i], out double item);
                        itemsNumeric.Add(item);
                        symbols.Add(items[i + 1]);
                    }

                    for (int i = 0; i < itemsNumeric.Count; i++)
                    {
                        if ((symbols[i] == "x" || symbols[i] == "/") && i != itemsNumeric.Count - 1)
                        {
                            if (symbols[i] == "x")
                            {
                                itemsNumeric[i] *= itemsNumeric[i + 1];
                            }
                            else
                            {
                                if (itemsNumeric[i + 1] == 0)
                                {
                                    MessageBox.Show("You can't divide by 0");
                                    items.Clear();
                                    itemsNumeric.Clear();
                                    symbols.Clear();
                                    textBlock_screen.Text = string.Empty;
                                    return;
                                }
                                itemsNumeric[i] /= itemsNumeric[i + 1];
                            }
                            itemsNumeric.RemoveAt(i + 1);
                            symbols.RemoveAt(i);
                            i = -1;
                        }
                    }

                    double solution = itemsNumeric[0];

                    for (int i = 0; i < itemsNumeric.Count - 1; i++)
                    {
                        if (symbols[i] == "+")
                        {
                            solution += itemsNumeric[i + 1];
                        }

                        if (symbols[i] == "-")
                        {
                            solution -= itemsNumeric[i + 1];
                        }
                    }

                    textBlock_screen.Text = solution.ToString();
                    items.Clear();
                    symbols.Clear();
                }
            }
        }
    }
}
