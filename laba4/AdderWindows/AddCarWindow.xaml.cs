using System;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4.AdderWindows
{
    /// <summary>
    /// Логика взаимодействия для AddCarWindow.xaml
    /// </summary>
    public partial class AddCarWindow : FluentWindow
    {
        public string Make => MakeBox.Text;
        public string Model => ModelBox.Text;
        public DateTime? YearCreation => YearBox.SelectedDate;
        public string Engine => EngineBox.Text;
        public string Transmission => TransmissionBox.Text;
        public string BodyType => BodyTypeBox.Text;
        public string Color => ColorBox.Text;

        public AddCarWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Make) ||
                    string.IsNullOrWhiteSpace(Model) ||
                    !YearCreation.HasValue ||
                    string.IsNullOrWhiteSpace(Engine) ||
                    string.IsNullOrWhiteSpace(Transmission) ||
                    string.IsNullOrWhiteSpace(BodyType))
                {
                    System.Windows.MessageBox.Show("Заполните все обязательные поля.");
                    return;
                }
                if (YearCreation.Value.Date > DateTime.Now.Date)
                {
                    System.Windows.MessageBox.Show("Год выпуска не может быть в будущем.");
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
