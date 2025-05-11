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
    /// Логика взаимодействия для DeliveriesWindow.xaml
    /// </summary>
    public partial class DeliveriesWindow : FluentWindow
    {
        private CarSpareBaseEntities db;
        public ObservableCollection<Suppliers> SuppliersList { get; set; }
        public ObservableCollection<Spare_Parts> PartsList { get; set; }

        public DeliveriesWindow()
        {
            InitializeComponent();
            db = new CarSpareBaseEntities();
            Loaded += DeliveriesWindow_Loaded;
        }
        private async void DeliveriesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SuppliersList = new ObservableCollection<Suppliers>(await db.Suppliers.ToListAsync());
            PartsList = new ObservableCollection<Spare_Parts>(await db.Spare_Parts.ToListAsync());
            DataContext = this;
            DeliveriesGrid.ItemsSource = await db.Deliveries
                .Include(d => d.Suppliers)
                .Include(d => d.Spare_Parts)
                .ToListAsync();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var delivery in db.Deliveries.Local)
                {
                    if (!delivery.supplier_ID.HasValue || !delivery.part_ID.HasValue || !delivery.dateDeliveliry.HasValue || !delivery.quantity.HasValue || !delivery.unit_Price.HasValue)
                    {
                        System.Windows.MessageBox.Show("Все поля должны быть заполнены.");
                        return;
                    }
                    if (delivery.dateDeliveliry.Value.Date > DateTime.Now.Date)
                    {
                        System.Windows.MessageBox.Show("Дата поставки не может быть в будущем.");
                        return;
                    }
                    if (delivery.quantity <= 0)
                    {
                        System.Windows.MessageBox.Show("Количество должно быть положительным числом.");
                        return;
                    }
                    if (delivery.unit_Price < 0)
                    {
                        System.Windows.MessageBox.Show("Цена не может быть отрицательной.");
                        return;
                    }
                }
                db.SaveChanges();
                DeliveriesGrid.ItemsSource = db.Deliveries.ToList();
                System.Windows.MessageBox.Show("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            db.Dispose();
            this.Close();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new AddDeliveriesWindow(db.Suppliers.ToList(), db.Spare_Parts.ToList()) { Owner = this };
                if (win.ShowDialog() == true)
                {
                    if (!win.SupplierId.HasValue || !win.PartId.HasValue || !win.DateDeliveliry.HasValue || !win.Quantity.HasValue || !win.UnitPrice.HasValue)
                    {
                        System.Windows.MessageBox.Show("Все поля должны быть заполнены.");
                        return;
                    }
                    if (win.DateDeliveliry.Value.Date > DateTime.Now.Date)
                    {
                        System.Windows.MessageBox.Show("Дата поставки не может быть в будущем.");
                        return;
                    }
                    if (win.Quantity <= 0)
                    {
                        System.Windows.MessageBox.Show("Количество должно быть положительным числом.");
                        return;
                    }
                    if (win.UnitPrice < 0)
                    {
                        System.Windows.MessageBox.Show("Цена не может быть отрицательной.");
                        return;
                    }
                    var delivery = new Deliveries
                    {
                        supplier_ID = win.SupplierId,
                        part_ID = win.PartId,
                        dateDeliveliry = win.DateDeliveliry,
                        quantity = win.Quantity,
                        unit_Price = win.UnitPrice,
                    };
                    db.Deliveries.Add(delivery);
                    db.SaveChanges();
                    DeliveriesGrid.ItemsSource = db.Deliveries.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDelivery = DeliveriesGrid.SelectedItem as Deliveries;
                if (selectedDelivery == null)
                {
                    System.Windows.MessageBox.Show("Выберите поставку для удаления.");
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    $"Удалить поставку с ID: {selectedDelivery.delivery_ID}?",
                    "Подтверждение удаления",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    db.Deliveries.Remove(selectedDelivery);
                    db.SaveChanges();
                    DeliveriesGrid.ItemsSource = db.Deliveries.ToList();
                    System.Windows.MessageBox.Show("Поставка удалена.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }

    }
}
