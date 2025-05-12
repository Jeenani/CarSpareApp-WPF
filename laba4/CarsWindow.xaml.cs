using laba4.AdderWindows;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Wpf.Ui.Controls;

namespace laba4
{
    /// <summary>
    /// Логика взаимодействия для CarsWindow.xaml
    /// </summary>
    public partial class CarsWindow : FluentWindow
    {
        private CarSpareBaseEntities db;

        public CarsWindow()
        {
            InitializeComponent();
            db = new CarSpareBaseEntities();
            Loaded += CarsWindow_Loaded;

        }
        private async void CarsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CarsGrid.ItemsSource = await db.Cars.ToListAsync();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var car in db.Cars.Local)
                {
                    if (string.IsNullOrWhiteSpace(car.make) ||
                        string.IsNullOrWhiteSpace(car.model) ||
                        string.IsNullOrWhiteSpace(car.engine) ||
                        string.IsNullOrWhiteSpace(car.transmission) ||
                        string.IsNullOrWhiteSpace(car.body_Type))
                    {
                        System.Windows.MessageBox.Show($"ID[{car.car_ID}] - поля не должны быть пустыми.");
                        return;
                    }
                    if (car.yearCreation.HasValue && car.yearCreation.Value.Date > DateTime.Now.Date)
                    {
                        System.Windows.MessageBox.Show("Год выпуска не может быть в будущем.");
                        return;
                    }
                }
                db.SaveChanges();
                CarsGrid.ItemsSource = db.Cars.ToList();
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
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedCar = CarsGrid.SelectedItem as Cars;
                if (selectedCar == null)
                {
                    System.Windows.MessageBox.Show("Выберите автомобиль для удаления.");
                    return;
                }

                var result = System.Windows.MessageBox.Show(
                    $"Удалить автомобиль: {selectedCar.make} {selectedCar.model}?",
                    "Подтверждение удаления",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    db.Cars.Remove(selectedCar);
                    db.SaveChanges();
                    CarsGrid.ItemsSource = db.Cars.ToList();
                    System.Windows.MessageBox.Show("Автомобиль удалён.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new AddCarWindow { Owner = this };
                if (win.ShowDialog() == true)
                {
                    if (string.IsNullOrWhiteSpace(win.Make) ||
                        string.IsNullOrWhiteSpace(win.Model) ||
                        !win.YearCreation.HasValue ||
                        win.YearCreation.Value.Date > DateTime.Now.Date ||
                        string.IsNullOrWhiteSpace(win.Engine) ||
                        string.IsNullOrWhiteSpace(win.Transmission) ||
                        string.IsNullOrWhiteSpace(win.BodyType))
                    {
                        System.Windows.MessageBox.Show("Проверьте корректность заполнения всех обязательных полей. Год не может быть в будущем.");
                        return;
                    }

                    var car = new Cars
                    {
                        make = win.Make.Trim(),
                        model = win.Model.Trim(),
                        yearCreation = win.YearCreation,
                        engine = win.Engine.Trim(),
                        transmission = win.Transmission.Trim(),
                        body_Type = win.BodyType.Trim(),
                        color = string.IsNullOrWhiteSpace(win.Color) ? null : win.Color.Trim()
                    };
                    db.Cars.Add(car);
                    db.SaveChanges();
                    CarsGrid.ItemsSource = db.Cars.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
        }
    }
}
