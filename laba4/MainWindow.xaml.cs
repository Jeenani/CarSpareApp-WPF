using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace laba4
{
    public partial class MainWindow : FluentWindow
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Suppliers_Click(object sender, RoutedEventArgs e)
        {
            var win = new SuppliersWindow();
            win.ShowDialog();
        }

        private void Storages_Click(object sender, RoutedEventArgs e)
        {
            var win = new StoragesWindow();
            win.ShowDialog();
        }

        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            var win = new CategoriesWindow();
            win.ShowDialog();
        }

        private void Parts_Click(object sender, RoutedEventArgs e)
        {
            var win = new PartsWindow();
            win.ShowDialog();
        }

        private void Deliveries_Click(object sender, RoutedEventArgs e)
        {
            var win = new DeliveriesWindow();
            win.ShowDialog();
        }

        private void Cars_Click(object sender, RoutedEventArgs e)
        {
            var win = new CarsWindow();
            win.ShowDialog();
        }

        private void StockAvailability_Click(object sender, RoutedEventArgs e)
        {
            var win = new Stock_AvailabilityWindow();
            win.ShowDialog();
        }
    }
}
