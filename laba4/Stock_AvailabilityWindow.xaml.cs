using laba4.AdderWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace laba4
{
    /// <summary>
    /// Логика взаимодействия для Stock_AvailabilityWindow.xaml
    /// </summary>
    public partial class Stock_AvailabilityWindow : FluentWindow
    {
        private CarSpareBaseEntities db;
        public ObservableCollection<Spare_Parts> Parts { get; set; }
        public ObservableCollection<Storages> StoragesList { get; set; }

        public Stock_AvailabilityWindow()
        {
            InitializeComponent();
            db = new CarSpareBaseEntities();
            Loaded += Stock_AvailabilityWindow_Loaded;
        }
        private async void Stock_AvailabilityWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Parts = new ObservableCollection<Spare_Parts>(await db.Spare_Parts.ToListAsync());
            StoragesList = new ObservableCollection<Storages>(await db.Storages.ToListAsync());
            DataContext = this;
            StockGrid.ItemsSource = await db.Stock_Availability.Include(s => s.Spare_Parts).Include(s => s.Storages).ToListAsync();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var stock in db.Stock_Availability.Local)
                {
                    if (!stock.part_ID.HasValue || !stock.storage_ID.HasValue || !stock.quantity.HasValue)
                    {
                        System.Windows.MessageBox.Show("Все поля должны быть заполнены.");
                        return;
                    }
                    if (stock.quantity <= 0)
                    {
                        System.Windows.MessageBox.Show("Количество должно быть положительным числом.");
                        return;
                    }
                }
                db.SaveChanges();
                StockGrid.ItemsSource = db.Stock_Availability.Include(s => s.Spare_Parts).Include(s => s.Storages).ToList();
                System.Windows.MessageBox.Show("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddStockAvailabilityWindow(db.Spare_Parts.ToList(), db.Storages.ToList()) { Owner = this };
            if (win.ShowDialog() == true)
            {
                if (!win.PartId.HasValue || !win.StorageId.HasValue || !win.Quantity.HasValue)
                {
                    System.Windows.MessageBox.Show("Заполните все поля корректно.");
                    return;
                }
                if (win.Quantity <= 0)
                {
                    System.Windows.MessageBox.Show("Количество должно быть положительным числом.");
                    return;
                }
                var stock = new Stock_Availability
                {
                    part_ID = win.PartId,
                    storage_ID = win.StorageId,
                    quantity = win.Quantity,
                };
                db.Stock_Availability.Add(stock);
                db.SaveChanges();
                StockGrid.ItemsSource = db.Stock_Availability.Include(s => s.Spare_Parts).Include(s => s.Storages).ToList();
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            db.Dispose();
            this.Close();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedStock = StockGrid.SelectedItem as Stock_Availability;
                if (selectedStock == null)
                {
                    System.Windows.MessageBox.Show("Выберите запись для удаления.");
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    $"Удалить остаток детали с ID: {selectedStock.part_ID} на складе ID: {selectedStock.storage_ID}?",
                    "Подтверждение удаления",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    db.Stock_Availability.Remove(selectedStock);
                    db.SaveChanges();
                    StockGrid.ItemsSource = db.Stock_Availability.Include(s => s.Spare_Parts).Include(s => s.Storages).ToList();
                    System.Windows.MessageBox.Show("Запись удалена.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }
    }
}
