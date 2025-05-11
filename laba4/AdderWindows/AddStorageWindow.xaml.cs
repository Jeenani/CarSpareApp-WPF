using System;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4.AdderWindows
{
    /// <summary>
    /// Логика взаимодействия для AddStorageWindow.xaml
    /// </summary>
    public partial class AddStorageWindow : FluentWindow
    {
        public string HouseName => HouseNameBox.Text;
        public string Location => LocationBox.Text;

        public AddStorageWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(HouseName) || string.IsNullOrWhiteSpace(Location))
                {
                    System.Windows.MessageBox.Show("Заполните все поля.");
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
