using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Practic_33.Classes;
using Practic_33.Models;

namespace Practic_33.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public string srcUserImage = "";
        UsersContext usersContext = new UsersContext();
        public Login()
        {
            InitializeComponent();
        }

        private void SelectPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите фото";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                imgUser.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                srcUserImage = openFileDialog.FileName;
            }
        }

        public bool CheckEmpty(string pattern, string input)
        {
            Match m = Regex.Match(input, pattern);

            return m.Success;
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            // Проверяем, что пользователь указал фамилию
            if (!CheckEmpty("^[А-ЯЁ][а-яё]*$", Lastname.Text))
            {
                MessageBox.Show("Укажите фамилию.");
                return;
            }

            // Проверяем, что пользователь указал имя
            if (!CheckEmpty("^[А-ЯЁ][а-яё]*$", Firstname.Text))
            {
                MessageBox.Show("Укажите имя.");
                return;
            }

            // Проверяем, что пользователь указал отчество
            if (!CheckEmpty("^[А-ЯЁ][а-яё]*$", Surname.Text))
            {
                MessageBox.Show("Укажите отчество.");
                return;
            }

            // Проверяем, что пользователь указал изображение
            if (String.IsNullOrEmpty(srcUserImage))
            {
                MessageBox.Show("Выберите изображение.");
                return;
            }

            // Обращаемся к БД и проверяем, что пользователя с такими данными не существует
            if (usersContext.Users.Where(x =>
                    x.Firstname == Firstname.Text &&
                    x.Lastname == Lastname.Text &&
                    x.Surname == Surname.Text).Count() > 0)
            {
                // Получаем пользователя по ФИО
                MainWindow.Instance.LoginUser = usersContext.Users.Where(x =>
                    x.Firstname == Firstname.Text &&
                    x.Lastname == Lastname.Text &&
                    x.Surname == Surname.Text).First();

                // Изменяем пользователю фотографию
                MainWindow.Instance.LoginUser.Photo = File.ReadAllBytes(srcUserImage);

                // Сохраняем изменения
                usersContext.SaveChanges();
            }
            else
            {
                // Добавляем нового пользователя
                usersContext.Users.Add(new Users(
                    Lastname.Text,
                    Firstname.Text,
                    Surname.Text,
                    File.ReadAllBytes(srcUserImage)));

                // Сохраняем изменения
                usersContext.SaveChanges();

                // Получаем пользователя из БД
                MainWindow.Instance.LoginUser = usersContext.Users.Where(x =>
                    x.Firstname == Firstname.Text &&
                    x.Lastname == Lastname.Text &&
                    x.Surname == Surname.Text).First();
            }

            // Открываем главную страницу
            MainWindow.Instance.OpenPages(new Pages.Main());
        }
    }
}
