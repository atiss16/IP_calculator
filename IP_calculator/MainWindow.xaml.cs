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
using System.Collections;
using System.Globalization;
using static IP_calculator.Transformations;

namespace IP_calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize_ComboboxSubnetMasks();
            Initialize_Table();
            tb_IpAddress.Focus();
        }

        private SolidColorBrush brush_violet = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9e53c9"));
        private SolidColorBrush brush_darkViolet = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5a3073"));
        private SolidColorBrush brush_background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00f73d67"));
        private SolidColorBrush brush_liteViolet = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#bb95f0"));
        private void Initialize_ComboboxSubnetMasks()
        {
            for(int i=0; i<33; i++)
            {
                string mask = i.ToString() + " - " + string.Join(".", Get_DecimalMask(i));
                
                ComboBoxItem cb_item = new ComboBoxItem() { Content = mask };
                cb_SubnetMask.Items.Add(cb_item);
            }
            cb_SubnetMask.SelectedIndex = 24;
        }

        private void Initialize_Table()
        {
            List<string> names = new List<string>()
            {
                "IP-адрес",
                "Маска подсети",
                "Обратная маска",
                "Адрес сети",
                "Широковещательный адрес",
                "Минимальный адрес",
                "Максимальный адрес",
                "Количество узлов"
            };
            for (int i = 0; i < names.Count; i++)
            {
                gr_IpTable.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            }
            
            gr_IpTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(230) });
            gr_IpTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(170) });
            gr_IpTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(290) });

            for(int i=0;i<names.Count;i++)
            {
                Grid_NewLabel(i, 0, "  " + names[i]);
                Grid_NewRichTextBox(i, 1, "decimal" + i, "", 170);
                Grid_NewRichTextBox(i, 2, "binary" + i, "", 290);
            }
        }

        private void Grid_NewRichTextBox(int row, int col, string name, string content, int width)
        {
            RichTextBox rtb = new RichTextBox()
            {
                Name = name,
                Width = width,
                Height = 40,
                IsReadOnly = true,
                Background = brush_background,
                Foreground = new SolidColorBrush(Colors.White),
                Padding = new Thickness(10, 12, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderBrush = brush_darkViolet,
                FontFamily = new FontFamily("seoge UI"),
                FontSize = 14
            };
            rtb.Document.Blocks.Add(new Paragraph(new Run(content)));

            Grid.SetRow(rtb, row);
            Grid.SetColumn(rtb, col);
            gr_IpTable.Children.Add(rtb);
            RegisterName(name, rtb);
        }

        private void Grid_NewLabel(int row, int col, string content)
        {
            Label lbl = new Label()
            {
                Width = 230,
                Height = 40,
                Background = brush_background,
                Foreground = new SolidColorBrush(Colors.White),
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderBrush = brush_darkViolet,
                BorderThickness = new Thickness(1),
                FontFamily = new FontFamily("seoge UI"),
                FontSize = 14,
                Content = content
            };
            Grid.SetRow(lbl, row);
            Grid.SetColumn(lbl, col);
            gr_IpTable.Children.Add(lbl);
        }
        private void btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            lbl_VerificationInfo.Content = "";
            int slashMask = cb_SubnetMask.SelectedIndex;
            List<Address> addresses = new List<Address>();

            Address ip_address = new Address();
            try
            {
                string ip = tb_IpAddress.Text.Trim();
                string[] values = ip.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (values.Count() != 4)
                    throw new Exception();

                ip_address.Decimal = Array.ConvertAll(values, s => byte.Parse(s));
                ip_address.Binary = Get_BinaryFromDecimal(ip_address.Decimal);
            }
            catch(Exception)
            {
                lbl_VerificationInfo.Content = "Введите ip-адрес в формате \"192.168.0.1\"";
                ClearTable();
                return;
            }
                  
            Address subnetMask = new Address();
            subnetMask.Decimal = Get_DecimalMask(cb_SubnetMask.SelectedIndex);
            subnetMask.Binary = Get_BinaryFromDecimal(subnetMask.Decimal);

            addresses.Add(ip_address);
            addresses.Add(subnetMask);
            addresses.Add(Get_Wildcard(subnetMask.Decimal));

            Address network = Get_Network(ip_address.Decimal, slashMask);

            addresses.Add(network);
            addresses.Add(Get_Broadcast(ip_address.Decimal, slashMask));
            addresses.Add(Get_Hostmin(network.Decimal, slashMask));
            addresses.Add(Get_Hostmax(network.Decimal, slashMask));

            for (int i = 0; i < 7; i++)
            {
                //if (slashMask > 30 && i >= 5)//hostmin and hostmax
                //{
                //    RichTextBox tb = (RichTextBox)gr_IpTable.FindName("decimal" + i);
                //    tb.Document.Blocks.Clear();
                //    tb.Document.Blocks.Add(new Paragraph(new Run("Недоступно")));

                //    RichTextBox tb_binary = (RichTextBox)gr_IpTable.FindName("binary" + i);
                //    tb_binary.Document.Blocks.Clear();
                //    continue;
                //}

                //if (slashMask == 32 && i == 4)//broadcast
                //{
                //    RichTextBox tb = (RichTextBox)gr_IpTable.FindName("decimal" + i);
                //    tb.Document.Blocks.Clear();
                //    tb.Document.Blocks.Add(new Paragraph(new Run("Недоступно")));

                //    RichTextBox tb_binary = (RichTextBox)gr_IpTable.FindName("binary" + i);
                //    tb_binary.Document.Blocks.Clear();
                //    continue;
                //}

                RichTextBox tb1 = (RichTextBox)gr_IpTable.FindName("decimal" + i);
                tb1.Document.Blocks.Clear();
                tb1.Document.Blocks.Add(new Paragraph(new Run(string.Join(".", addresses[i].Decimal))));

                RichTextBox tb2 = (RichTextBox)gr_IpTable.FindName("binary" + i);
                tb2.Document.Blocks.Clear();

                string network_part = "";
                string host_part = "";
                for (int j = 0; j < slashMask; j++)
                {
                    network_part += addresses[i].Binary[j];
                    if ((j + 1) % 8 == 0 && j != 31)
                        network_part += '.';
                }
                for (int j = slashMask; j < 32; j++)
                {
                    host_part += addresses[i].Binary[j];
                    if ((j + 1) % 8 == 0 && j != 31)
                        host_part += '.';
                }
                
                Run run = new Run(network_part);
                run.Foreground = Brushes.White;
                Paragraph par = new Paragraph(run);

                run = new Run(host_part);
                run.Foreground = brush_liteViolet;
                par.Inlines.Add(run);
                tb2.Document.Blocks.Add(par);
            }

            RichTextBox tbPossibleHosts = (RichTextBox)gr_IpTable.FindName("decimal" + 7);
            tbPossibleHosts.Document.Blocks.Clear();
            int p = Get_PossibleHosts(slashMask);
            NumberFormatInfo frmt = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalDigits = 0 };
            string possibleHosts = p.ToString("n", frmt);
            tbPossibleHosts.Document.Blocks.Add(new Paragraph(new Run(possibleHosts)));
        }
        private void ClearTable()
        {
            for (int i = 0; i < 8; i++)
            {
                RichTextBox tb1 = (RichTextBox)gr_IpTable.FindName("decimal" + i);
                tb1.Document.Blocks.Clear();

                RichTextBox tb2 = (RichTextBox)gr_IpTable.FindName("binary" + i);
                tb2.Document.Blocks.Clear();
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void tb_IpAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cb_SubnetMask.Focus();
        }

        private void cb_SubnetMask_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_Calculate.Focus();
            if (e.Key == Key.Left)
                tb_IpAddress.Focus();
        }

        private void btn_Calculate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                cb_SubnetMask.Focus();
        }

        private void btn_About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
    }
}
