using System;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4.AdderWindows
{
    /// <summary>
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : FluentWindow
    {
        public string CategoryName => CategoryNameBox.Text;
        public string CategoryDescription => CategoryDescriptionBox.Text;

        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CategoryName))
                {
                    System.Windows.MessageBox.Show("Заполните название категории.");
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
