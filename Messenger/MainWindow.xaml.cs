using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        public static bool IsFirstLaunched = Properties.UserProperties.Default.IsFirstLaunched;
        public static long PhoneNumber = Properties.UserProperties.Default.LogginedUserPhoneNumber;
        public string HostName = Dns.GetHostName();

        private readonly DatabaseConnection _databaseConnection = new DatabaseConnection();
        private readonly NotifyIconControl _notifyIcon = new NotifyIconControl();
        
        public MainWindow()
        {
            string localIP = GetMachineData.CurrentIpAddress();
            ServerConnect serverConnect = new ServerConnect();
            try
            {
                serverConnect.SendMessageFromSocket(11000, "Подключился пользователь (" + HostName + ") с ip-адреса (" + localIP + ")");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            IsFirstLaunched = false;
            
            switch (IsFirstLaunched)
            {
                case true:
                    RegistrationWindow registrationWindow = new RegistrationWindow();
                    registrationWindow.Show();
                    Close();
                    break;
                case false:
                    InitializeComponent();
                    break;
            }

            _databaseConnection.DbConnection();

            if (Application.Current.MainWindow != null && !IsFirstLaunched)
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

                // User's windows
                PopupSettingsWindow.Visibility = Visibility.Hidden;
                PopupSettings.Visibility = Visibility.Hidden;
                SelectedUserData.Margin = new Thickness(0, 0, -250, 0);
                SelectedUserSettings.Visibility = Visibility.Hidden;
                SettingsField.Visibility = Visibility.Hidden;
                SettingsWindow.Visibility = Visibility.Hidden;
                PhoneCallWindow.Visibility = Visibility.Hidden;

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

        public void CloseApplication(object sender, MouseEventArgs eventArgs)
        {
            Close();
            _notifyIcon.DetachIcon();
        }

        public void ShowWindow()
        {
            Show();
        }

        public void DisableNotifications(object sender, MouseEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void CallToUser(object sender, RoutedEventArgs eventArgs)
        {
            PhoneCallWindow.Visibility = Visibility.Visible;
        }
        
        private void EndPhoneCall(object sender, RoutedEventArgs eventArgs)
        {
            PhoneCallWindow.Visibility = Visibility.Hidden;
        }

        private void OpenSmileWindow(object sender, RoutedEventArgs eventArgs)
        {

        }

        private void CameraCall(object sender, RoutedEventArgs eventArgs)
        {
            
        }

        private void OpenPopupSettings(object sender, MouseButtonEventArgs eventArgs)
        {
            var transparentBackground = new BrushConverter();
            PopupSettingsWindow.Visibility = Visibility.Visible;
            PopupSettings.Visibility = Visibility.Visible;
        }

        private void HidePopupSettings(object sender, MouseButtonEventArgs eventArgs)
        {
            var transparentBackground = new BrushConverter();
            PopupSettingsWindow.Visibility = Visibility.Hidden;
            PopupSettings.Visibility = Visibility.Hidden;
        }

        private void OpenSelectedUserData(object sender, MouseButtonEventArgs eventArgs)
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

        private void CloseSelectedUserData(object sender, RoutedEventArgs eventArgs)
        {
            Width -= 250;
            ChatField.Margin = new Thickness(0, 0, 0, 0);
            SelectedUserData.Margin = new Thickness(0, 0, -250, 0);
        }

        private void OpenSelectedUserSettings(object sender, RoutedEventArgs eventArgs)
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
        
        private void OpenSettings(object sender, RoutedEventArgs eventArgs)
        {
            PopupSettingsWindow.Visibility = Visibility.Hidden;
            PopupSettings.Visibility = Visibility.Hidden;

            SettingsField.Visibility = Visibility.Visible;
            SettingsWindow.Visibility = Visibility.Visible;
        }

        private void CloseSettingsWindow(object sender, RoutedEventArgs eventArgs)
        {
            SettingsField.Visibility = Visibility.Hidden;
            SettingsWindow.Visibility = Visibility.Hidden;
        }

        private void BrowsePhoto(object sender, EventArgs eventArgs)
        {
            string imageLocation = "";

            try
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*"
                };

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    // MainViewModel.User.Images.Add();
                    
                    // Сделать доавбление фото 
                    
                }
            } catch (Exception e)
            {
                MessageBox.Show("An Error Occured. ERROR message = " + e.Message, "Error", MessageBoxButton.OK);
            }
        }
    }
}