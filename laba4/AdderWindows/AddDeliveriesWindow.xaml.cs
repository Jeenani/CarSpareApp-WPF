using System;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4.AdderWindows
{
    /// <summary>
    /// Логика взаимодействия для AddDeliveriesWindow.xaml
    /// </summary>
    public partial class AddDeliveriesWindow : FluentWindow
    {
        public int? SupplierId => SupplierComboBox.SelectedValue as int?;
        public int? PartId => PartComboBox.SelectedValue as int?;
        public DateTime? DateDeliveliry => DateBox.SelectedDate;
        public int? Quantity => int.TryParse(QuantityBox.Text, out var v) ? v : (int?)null;
        public decimal? UnitPrice => decimal.TryParse(UnitPriceBox.Text, out var v) ? v : (decimal?)null;

        public AddDeliveriesWindow(System.Collections.IEnumerable suppliers, System.Collections.IEnumerable parts)
        {
            InitializeComponent();
            SupplierComboBox.ItemsSource = suppliers;
            PartComboBox.ItemsSource = parts;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!SupplierId.HasValue || !PartId.HasValue || !DateDeliveliry.HasValue || !Quantity.HasValue || !UnitPrice.HasValue)
                {
                    System.Windows.MessageBox.Show("Заполните все поля корректно.");
                    return;
                }
                if (DateDeliveliry.Value.Date > DateTime.Now.Date)
                {
                    System.Windows.MessageBox.Show("Дата поставки не может быть в будущем.");
                    return;
                }
                if (Quantity <= 0)
                {
                    System.Windows.MessageBox.Show("Количество должно быть положительным числом.");
                    return;
                }
                if (UnitPrice < 0)
                {
                    System.Windows.MessageBox.Show("Цена не может быть отрицательной.");
                    return;
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка ввода: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
