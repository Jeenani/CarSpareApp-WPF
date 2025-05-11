using System;
using System.Linq;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4.AdderWindows
{
    /// <summary>
    /// Логика взаимодействия для AddSupplierWindow.xaml
    /// </summary>
    public partial class AddSupplierWindow : FluentWindow
    {
        public string CompanyName => CompanyNameBox.Text;
        public string ContactPerson => ContactPersonBox.Text;
        public string Phone => PhoneBox.Text;
        public string Email => EmailBox.Text;
        public string Address => AddressBox.Text;

        public AddSupplierWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CompanyName) ||
                    string.IsNullOrWhiteSpace(ContactPerson) ||
                    string.IsNullOrWhiteSpace(Phone) ||
                    string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Address))
                {
                    System.Windows.MessageBox.Show("Заполните все поля.");
                    return;
                }

                string phone = Phone.Trim();
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

                if (!Email.Contains("@") || !Email.Contains("."))
                {
                    System.Windows.MessageBox.Show("Некорректный формат email.");
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
