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
using System.Windows.Threading;
using Practic_33.Classes;
using Practic_33.Classes.Common;
using Practic_33.Models;

namespace Practic_33.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Users SelectedUser = null;
        public UsersContext usersContext = new UsersContext();
        public MessagesContext messagesContext = new MessagesContext();
        public DispatcherTimer Timer = new DispatcherTimer() { Interval = new System.TimeSpan(0, 0, 3) };
        public Main()
        {
            InitializeComponent();

            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (SelectedUser != null)
            {
                SelectUser(SelectedUser);
            }
        }

        public void LoadUsers()
        {
            foreach (Users user in usersContext.Users)
            {
                if(user.Id != MainWindow.Instance.LoginUser.Id)
                    ParentUsers.Children.Add(new Pages.Items.User(user, this));
            }
        }

        public void SelectUser(Users User)
        {
            // Запоминаем выбранный диалог
            SelectedUser = User;
            // Показываем чат
            Chat.Visibility = Visibility.Visible;
            // Конвертируем изображение пользователя из массива байт в BitmapImage
            imgUser.Source = BitmapFromArrayByte.LoadImage(User.Photo);
            // Отображаем ФИО
            FIO.Content = User.ToFIO();
            // Очищаем сообщения в диалоге
            ParentMessages.Children.Clear();
            // Перебираем сообщения которые :
            // отправил выбранный пользователь авторизованному
            // или отправил авторизованный пользователь выбранному
            // сортируем по ID
            foreach (Messages Message in messagesContext.Messages.Where(x =>
                (x.UserFrom == User.Id && x.UserTo == MainWindow.Instance.LoginUser.Id) ||
                (x.UserFrom == MainWindow.Instance.LoginUser.Id && x.UserTo == User.Id)))
            {
                // Добавляем сообщение в диалог
                ParentMessages.Children.Add(new Pages.Items.Message(Message, usersContext.Users.Where(x => x.Id == Message.UserFrom).First()));
            }
        }

        private void Send(object sender, KeyEventArgs e)
        {
            // Если нажата клавиша Enter
            if (e.Key == Key.Enter)
            {
                // Создаём сообщение, где отправитель — мы, а получатель — выбранный диалог
                Messages message = new Messages(
                    MainWindow.Instance.LoginUser.Id,
                    SelectedUser.Id,
                    Message.Text
                );

                // Добавляем сообщение в контекст
                messagesContext.Messages.Add(message);

                // Сохраняем изменения
                messagesContext.SaveChanges();

                // Добавляем сообщение на экран
                ParentMessages.Children.Add(new Pages.Items.Message(message, MainWindow.Instance.LoginUser));

                // Очищаем поле ввода
                Message.Text = "";
            }
        }
    }
}
