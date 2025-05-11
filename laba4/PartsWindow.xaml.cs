using laba4.AdderWindows;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4
{
    public partial class PartsWindow : FluentWindow
    {
        private CarSpareBaseEntities db;
        public ObservableCollection<Part_Categories> Categories { get; set; }

        public PartsWindow()
        {
            InitializeComponent();
            db = new CarSpareBaseEntities();
            Loaded += PartsWindow_Loaded;
        }
        private async void PartsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Categories = new ObservableCollection<Part_Categories>(await db.Part_Categories.ToListAsync());
            DataContext = this;
            PartsGrid.ItemsSource = await db.Spare_Parts.Include(p => p.Part_Categories).ToListAsync();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var part in db.Spare_Parts.Local)
                {
                    if (!part.Category_ID.HasValue || string.IsNullOrWhiteSpace(part.partName) || !part.price.HasValue)
                    {
                        System.Windows.MessageBox.Show("Поля 'Категория', 'Название' и 'Цена' обязательны для заполнения.");
                        return;
                    }
                    if (part.price < 0)
                    {
                        System.Windows.MessageBox.Show("Цена не может быть отрицательной.");
                        return;
                    }
                    if (part.warranty_Months.HasValue && part.warranty_Months < 0)
                    {
                        System.Windows.MessageBox.Show("Гарантия не может быть отрицательной.");
                        return;
                    }
                }
                db.SaveChanges();
                PartsGrid.ItemsSource = db.Spare_Parts.Include(p => p.Part_Categories).ToList();
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
            var win = new AddPartsWindow(db.Part_Categories.ToList()) { Owner = this };
            if (win.ShowDialog() == true)
            {
                if (!win.CategoryId.HasValue || string.IsNullOrWhiteSpace(win.PartName) || !win.Price.HasValue)
                {
                    System.Windows.MessageBox.Show("Поля 'Категория', 'Название' и 'Цена' обязательны для заполнения.");
                    return;
                }
                if (win.Price < 0)
                {
                    System.Windows.MessageBox.Show("Цена не может быть отрицательной.");
                    return;
                }
                if (win.Warranty.HasValue && win.Warranty < 0)
                {
                    System.Windows.MessageBox.Show("Гарантия не может быть отрицательной.");
                    return;
                }
                var parts = new Spare_Parts
                {
                    Category_ID = win.CategoryId,
                    partName = win.PartName,
                    partDescription = win.PartDescription,
                    price = win.Price,
                    manufacturer = win.Manufacturer,
                    warranty_Months = win.Warranty,
                };
                db.Spare_Parts.Add(parts);
                db.SaveChanges();
                PartsGrid.ItemsSource = db.Spare_Parts.Include(p => p.Part_Categories).ToList();
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPart = PartsGrid.SelectedItem as Spare_Parts;
                if (selectedPart == null)
                {
                    System.Windows.MessageBox.Show("Выберите деталь для удаления.");
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    $"Удалить деталь: {selectedPart.partName}?",
                    "Подтверждение удаления",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    db.Spare_Parts.Remove(selectedPart);
                    db.SaveChanges();
                    PartsGrid.ItemsSource = db.Spare_Parts.Include(p => p.Part_Categories).ToList();
                    System.Windows.MessageBox.Show("Деталь удалена.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }
    }
}
