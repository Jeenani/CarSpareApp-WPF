using System;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4.AdderWindows
{
    /// <summary>
    /// Логика взаимодействия для AddStockAvailabilityWindow.xaml
    /// </summary>
    public partial class AddStockAvailabilityWindow : FluentWindow
    {
        public int? PartId => PartComboBox.SelectedValue as int?;
        public int? StorageId => StorageComboBox.SelectedValue as int?;
        public int? Quantity => int.TryParse(QuantityBox.Text, out var v) ? v : (int?)null;

        public AddStockAvailabilityWindow(System.Collections.IEnumerable parts, System.Collections.IEnumerable storages)
        {
            InitializeComponent();
            PartComboBox.ItemsSource = parts;
            StorageComboBox.ItemsSource = storages;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!PartId.HasValue || !StorageId.HasValue || !Quantity.HasValue)
                {
                    System.Windows.MessageBox.Show("Заполните все поля корректно.");
                    return;
                }
                if (Quantity <= 0)
                {
                    System.Windows.MessageBox.Show("Количество должно быть положительным числом.");
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
