using System;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4.AdderWindows
{
    /// <summary>
    /// Логика взаимодействия для AddPartsWindow.xaml
    /// </summary>
    public partial class AddPartsWindow : FluentWindow
    {
        public int? CategoryId => CategoryComboBox.SelectedValue as int?;
        public string PartName => PartNameBox.Text;
        public string PartDescription => PartDescriptionBox.Text;
        public decimal? Price => decimal.TryParse(PriceBox.Text, out var v) ? v : (decimal?)null;
        public string Manufacturer => ManufacturerBox.Text;
        public int? Warranty => int.TryParse(WarrantyBox.Text, out var v) ? v : (int?)null;

        public AddPartsWindow(System.Collections.IEnumerable categories)
        {
            InitializeComponent();
            CategoryComboBox.ItemsSource = categories;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CategoryId.HasValue || string.IsNullOrWhiteSpace(PartName) || !Price.HasValue)
                {
                    System.Windows.MessageBox.Show("Заполните обязательные поля корректно.");
                    return;
                }
                if (Price < 0)
                {
                    System.Windows.MessageBox.Show("Цена не может быть отрицательной.");
                    return;
                }
                if (Warranty.HasValue && Warranty < 0)
                {
                    System.Windows.MessageBox.Show("Гарантия не может быть отрицательной.");
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
