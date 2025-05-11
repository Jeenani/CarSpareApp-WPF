using laba4.AdderWindows;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4
{
    /// <summary>
    /// Логика взаимодействия для CategoriesWindow.xaml
    /// </summary>
    public partial class CategoriesWindow : FluentWindow
    {
        private CarSpareBaseEntities db;

        public CategoriesWindow()
        {
            InitializeComponent();
            db = new CarSpareBaseEntities();
            Loaded += CategoriesWindow_Loaded;
        }
        private async void CategoriesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CategoriesGrid.ItemsSource = await db.Part_Categories.ToListAsync();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var cat in db.Part_Categories.Local)
                {
                    if (string.IsNullOrWhiteSpace(cat.categoryName))
                    {
                        System.Windows.MessageBox.Show("Название категории не должно быть пустым.");
                        return;
                    }
                }
                db.SaveChanges();
                CategoriesGrid.ItemsSource = db.Part_Categories.ToList();
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
                var win = new AddCategoryWindow() { Owner = this };
                if (win.ShowDialog() == true)
                {
                    if (string.IsNullOrWhiteSpace(win.CategoryName))
                    {
                        System.Windows.MessageBox.Show("Название категории не должно быть пустым.");
                        return;
                    }
                    var part_cat = new Part_Categories
                    {
                        categoryName = win.CategoryName,
                        categoryDescription = win.CategoryDescription,
                    };
                    db.Part_Categories.Add(part_cat);
                    db.SaveChanges();
                    CategoriesGrid.ItemsSource = db.Part_Categories.ToList();
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
                var selectedCategory = CategoriesGrid.SelectedItem as Part_Categories;
                if (selectedCategory == null)
                {
                    System.Windows.MessageBox.Show("Выберите категорию для удаления.");
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    $"Удалить категорию: {selectedCategory.categoryName}?",
                    "Подтверждение удаления",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    db.Part_Categories.Remove(selectedCategory);
                    db.SaveChanges();
                    CategoriesGrid.ItemsSource = db.Part_Categories.ToList();
                    System.Windows.MessageBox.Show("Категория удалена.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }

    }
}
