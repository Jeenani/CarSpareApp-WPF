using laba4.AdderWindows;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4
{
    /// <summary>
    /// Логика взаимодействия для SuppliersWindow.xaml
    /// </summary>
    public partial class SuppliersWindow : FluentWindow
    {
        private CarSpareBaseEntities db;

        public SuppliersWindow()
        {
            InitializeComponent();
            db = new CarSpareBaseEntities();
            Loaded += SuppliersWindow_Loaded;
        }
        private async void SuppliersWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SuppliersGrid.ItemsSource = await db.Suppliers.ToListAsync();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            foreach (var supplier in db.Suppliers.Local)
            {
                string phone = supplier.phone?.Trim() ?? "";
                string digits = phone;
                if (!digits.All(char.IsDigit))
                {
                    System.Windows.MessageBox.Show("Телефон должен содержать только цифры.");
                    return;
                }
                if (phone.Length < 12)
                {
                    System.Windows.MessageBox.Show("Номер телефона должен содержать минимум 11 символов", "Предупреждение");
                    return;
                }
                string email = supplier.email?.Trim() ?? "";
                if (!email.Contains("@") || !email.Contains("."))
                {
                    System.Windows.MessageBox.Show($"Email поставщика \"{supplier.supplier_company_Name}\" некорректен.");
                    return;
                }
            }

            db.SaveChanges();
            SuppliersGrid.ItemsSource = db.Suppliers.ToList();
            System.Windows.MessageBox.Show("Изменения сохранены.");
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            db.Dispose();
            this.Close();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddSupplierWindow { Owner = this };
            if (win.ShowDialog() == true)
            {
                string phone = win.Phone?.Trim() ?? "";
                string digits = phone.StartsWith("+") ? phone.Substring(1) : phone;
                if (!digits.All(char.IsDigit))
                {
                    System.Windows.MessageBox.Show("Телефон должен содержать только цифры (после '+').");
                    return;
                }
                if (digits.Length != 11)
                {
                    System.Windows.MessageBox.Show("Телефон должен содержать не менее 11 цифр после '+'.");
                    return;
                }
                string email = win.Email?.Trim() ?? "";
                if (!email.Contains("@") || !email.Contains("."))
                {
                    System.Windows.MessageBox.Show("Некорректный формат email.");
                    return;
                }

                var supplier = new Suppliers
                {
                    supplier_company_Name = win.CompanyName,
                    contact_Person = win.ContactPerson,
                    phone = win.Phone,
                    email = win.Email,
                    address = win.Address
                };
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                SuppliersGrid.ItemsSource = db.Suppliers.ToList();
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedSupplier = SuppliersGrid.SelectedItem as Suppliers;
                if (selectedSupplier == null)
                {
                    System.Windows.MessageBox.Show("Выберите поставщика для удаления.");
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    $"Удалить поставщика: {selectedSupplier.supplier_company_Name}?",
                    "Подтверждение удаления",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    db.Suppliers.Remove(selectedSupplier);
                    db.SaveChanges();
                    SuppliersGrid.ItemsSource = db.Suppliers.ToList();
                    System.Windows.MessageBox.Show("Поставщик удалён.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }

    }
}
