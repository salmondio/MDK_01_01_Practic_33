using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Practic_33.Classes.Common;
using Practic_33.Models;

namespace Practic_33.Pages.Items
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : UserControl
    {
        /// <summary> Пользователь, которого отображаем </summary>
        Users user;

        /// <summary> Ссылка на главное окно </summary>
        Main main;

        public User(Users user, Main main)
        {
            InitializeComponent();

            // Запоминаем пользователя, которого отображаем
            this.user = user;

            // Запоминаем ссылку на главное окно
            this.main = main;

            // Конвертируем изображение из массива байт в BitmapImage
            imgUser.Source = BitmapFromArrayByte.LoadImage(user.Photo);

            // Присваиваем ФИО
            FIO.Content = user.ToFIO();
        }

        /// <summary> Выбор диалога </summary>
        private void SelectChat(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // При нажатии вызываем метод выбора пользователя на главном окне,
            // передавая выбранного пользователя
            main.SelectUser(user);
        }
    }
}
