using laba4.AdderWindows;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4
{
    /// <summary>
    /// Логика взаимодействия для StoragesWindow.xaml
    /// </summary>
    public partial class StoragesWindow : FluentWindow
    {
        private CarSpareBaseEntities db;

        public StoragesWindow()
        {
            InitializeComponent();
            db = new CarSpareBaseEntities();
            Loaded += StoragesWindow_Loaded;
        }
        private async void StoragesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StoragesGrid.ItemsSource = await db.Storages.ToListAsync();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var storage in db.Storages.Local)
                {
                    if (string.IsNullOrWhiteSpace(storage.houseName) || string.IsNullOrWhiteSpace(storage.Location))
                    {
                        System.Windows.MessageBox.Show("Поля 'Название склада' и 'Расположение' не должны быть пустыми.");
                        return;
                    }
                }
                db.SaveChanges();
                StoragesGrid.ItemsSource = db.Storages.ToList();
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
            var win = new AddStorageWindow { Owner = this };
            if (win.ShowDialog() == true)
            {
                if (string.IsNullOrWhiteSpace(win.HouseName) || string.IsNullOrWhiteSpace(win.Location))
                {
                    System.Windows.MessageBox.Show("Поля 'Название склада' и 'Расположение' не должны быть пустыми.");
                    return;
                }
                var storage = new Storages
                {
                    houseName = win.HouseName,
                    Location = win.Location
                };


                db.Storages.Add(storage);
                db.SaveChanges();
                StoragesGrid.ItemsSource = db.Storages.ToList();
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedStorage = StoragesGrid.SelectedItem as Storages;
                if (selectedStorage == null)
                {
                    System.Windows.MessageBox.Show("Выберите склад для удаления.");
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    $"Удалить склад: {selectedStorage.houseName}?",
                    "Подтверждение удаления",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    db.Storages.Remove(selectedStorage);
                    db.SaveChanges();
                    StoragesGrid.ItemsSource = db.Storages.ToList();
                    System.Windows.MessageBox.Show("Склад удалён.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }

    }
}
