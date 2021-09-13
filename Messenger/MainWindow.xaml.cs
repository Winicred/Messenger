using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Messenger.Core.Utils;
using Messenger.MVVM.ViewModel;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Messenger
{
    public partial class MainWindow
    {
        private static string WindowTitle = @"Messanger";
        public static bool IsFirstLaunched = Properties.UserProperties.Default.IsFirstLaunched;
        public static long PhoneNumber = Properties.UserProperties.Default.LogginedUserPhoneNumber;

        private readonly DatabaseConnection _databaseConnection = new DatabaseConnection();
        private readonly NotifyIconControl _notifyIcon = new NotifyIconControl();

        public MainWindow()
        {
            switch (IsFirstLaunched)
            {
                case true:
                    RegistrationWindow registrationWindow = new RegistrationWindow();
                    registrationWindow.Show();
                    Close();
                    break;
                case false:
                    InitializeComponent();
                    Title = WindowTitle;
                    break;
            }

            _databaseConnection.DbConnection();

            if (Application.Current.MainWindow != null)
            {
                if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                {
                    ToMaximizedWindow.Visibility = Visibility.Visible;
                    ToNormalWindow.Visibility = Visibility.Hidden;
                    MessageFieldGrid.MinWidth = 250;
                }
                else
                {
                    ToMaximizedWindow.Visibility = Visibility.Hidden;
                    ToNormalWindow.Visibility = Visibility.Visible;
                    MessageFieldGrid.MinWidth = 500;
                }

                PopupSettingsWindow.Visibility = Visibility.Hidden;
                PopupSettings.Visibility = Visibility.Hidden;

                SelectedUserData.Margin = new Thickness(0, 0, -250, 0);

                SelectedUserSettings.Visibility = Visibility.Hidden;

                Width = Properties.UserProperties.Default.WindowWidthAfterShutdown;
                Height = Properties.UserProperties.Default.WindowHeightAfterShutdown;
                Left = Properties.UserProperties.Default.WindowPositionX;
                Top = Properties.UserProperties.Default.WindowPositionY;

                _notifyIcon.InitializeNotifyIcon();
            }
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
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs eventArgs)
        {
            switch (WindowState == WindowState.Maximized)
            {
                case true:
                    WindowState = WindowState.Normal;
                    ToNormalWindow.Visibility = Visibility.Hidden;
                    ToMaximizedWindow.Visibility = Visibility.Visible;
                    MessageFieldGrid.MinWidth = 250;
                    break;
                case false:
                    WindowState = WindowState.Maximized;
                    ToNormalWindow.Visibility = Visibility.Visible;
                    ToMaximizedWindow.Visibility = Visibility.Hidden;
                    MessageFieldGrid.MinWidth = 500;
                    break;
            }
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
            PhoneNumber = 0;
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
            // PhoneCallTask phoneCallTask = new PhoneCallTask();
            //
            // phoneCallTask.PhoneNumber = "2065550123";
            ; // phoneCallTask;.DisplayName = "Gage";
            ; // phoneCallTask;.Show();
        }

        private void OpenSmileWindow(object sender, RoutedEventArgs e)
        {
        }

        private void TakeAPhoto(object sender, RoutedEventArgs e)
        {
        }

        private void OpenPopupSettings(object sender, MouseButtonEventArgs e)
        {
            var transparentBackground = new BrushConverter();
            PopupSettingsWindow.Visibility = Visibility.Visible;
            PopupSettings.Visibility = Visibility.Visible;
        }

        private void HidePopupSettings(object sender, MouseButtonEventArgs e)
        {
            var transparentBackground = new BrushConverter();
            PopupSettingsWindow.Visibility = Visibility.Hidden;
            PopupSettings.Visibility = Visibility.Hidden;
        }

        private void OpenSelectedUserData(object sender, MouseButtonEventArgs e)
        {
            if (SelectedUserData.Margin == new Thickness(0, 0, 0, 0))
            {
                ChatField.Margin = new Thickness(0, 0, 0, 0);
                SelectedUserData.Margin = new Thickness(0, 0, -250, 0);
                Width -= 250;
            }
            else
            {
                ChatField.Margin = new Thickness(0, 0, 250, 0);
                SelectedUserData.Margin = new Thickness(0, 0, 0, 0);
                Width += 250;
            }
        }

        private void CloseSelectedUserData(object sender, RoutedEventArgs e)
        {
            Width -= 250;
            ChatField.Margin = new Thickness(0, 0, 0, 0);
            SelectedUserData.Margin = new Thickness(0, 0, -250, 0);
        }

        private void OpenSelectedUserSettings(object sender, RoutedEventArgs e)
        {
            if (SelectedUserSettings.Visibility == Visibility.Visible)
            {
                SelectedUserSettings.Visibility = Visibility.Hidden;
            }
            else
            {
                SelectedUserSettings.Visibility = Visibility.Visible;
            }
        }
        
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            PopupSettingsWindow.Visibility = Visibility.Hidden;
            PopupSettings.Visibility = Visibility.Hidden;
        }
    }
}