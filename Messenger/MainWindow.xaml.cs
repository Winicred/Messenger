using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Messenger.Core.Utils;
using Application = System.Windows.Application;

namespace Messenger
{
    public partial class MainWindow
    {
        public static string WindowTitle = @"Messanger";
        public static bool IsFirstLaunched = Properties.UserProperties.Default.IsFirstLaunched;

        private readonly DatabaseConnection _databaseConnection = new DatabaseConnection();
        private readonly NotifyIconControl _notifyIcon = new NotifyIconControl();
        
        
        public MainWindow()
        {
            switch (IsFirstLaunched)
            {
                case true:
                    RegistrationWindow registrationWindow = new RegistrationWindow();
                    registrationWindow.Show();
                    Visibility = Visibility.Hidden;
                    break;
                case false:
                    InitializeComponent();
                    Title = WindowTitle;
                    break;
            }

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            _notifyIcon.InitializeNotifyIcon();
            _databaseConnection.DbConnection();
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs eventArgs)
        {
            if (eventArgs.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs eventArgs)
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs eventArgs)
        {
            WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs eventArgs)
        {
            Hide();
            ShowInTaskbar = false;
            _notifyIcon.ShowNotificationWhenWindowClosed();
        }

        private void Logout(object sender, RoutedEventArgs eventArgs)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            IsFirstLaunched = true;
            PropertiesSettings.SavePropertiesSettings();
            Close();
        }

        public void OpenWindow(object sender, System.Windows.Forms.MouseEventArgs eventArgs)
        {
            if (eventArgs.Button == MouseButtons.Left)
            {
                switch (ShowInTaskbar)
                {
                    case true:
                        WindowState = WindowState.Normal;
                        break;
                    case false:
                        Show();
                        ShowInTaskbar = true;
                        break;
                }
            }
        }
        public void CloseApplication(object sender, System.Windows.Forms.MouseEventArgs eventArgs)
        {
            Close();
            _notifyIcon.DetachIcon();
        }

        public void ShowWindow()
        {
            Show();
        }

        public void DisableNotifications(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CallToUser(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenSmileWindow(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TakeAPhoto(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}