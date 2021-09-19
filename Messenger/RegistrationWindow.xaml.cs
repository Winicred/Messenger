using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Messenger.Core.Utils;
using Messenger.MVVM.Model;
using Messenger.MVVM.ViewModel;
using Application = System.Windows.Application;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Image = System.Drawing.Image;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace Messenger
{
    public partial class RegistrationWindow : Window
    {
        // Startup parameters
        private bool isFirstLaunched = Properties.UserProperties.Default.IsFirstLaunched;
        private bool isLightThemeUsed = Properties.UserProperties.Default.IsLightThemeIsUsed;
        private DatabaseConnection _databaseConnection = new DatabaseConnection();
        private IsolatedStorage _isolatedStorage = new IsolatedStorage();

        public UserModel User { get; set; }
        public RegistrationWindow()
        {
            InitializeComponent();

            switch (isFirstLaunched)
            {
                case true:
                    Width = 800;
                    Height = 600;
                    WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    Background = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                    break;
                case false:
                    Width = Properties.UserProperties.Default.WindowWidthAfterShutdown;
                    Height = Properties.UserProperties.Default.WindowHeightAfterShutdown;
                    Left = Properties.UserProperties.Default.WindowPositionX;
                    Top = Properties.UserProperties.Default.WindowPositionY;
                    break;
            }

            switch (isLightThemeUsed)
            {
                case true:
                    Background = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                    TopBar.Background = new SolidColorBrush(Color.FromRgb(212, 212, 212));
                    MainText.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    DescriptionText.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    MoonIcon.Visibility = Visibility.Visible;
                    SunIcon.Visibility = Visibility.Hidden;
                    break;
                case false:
                    Background = new SolidColorBrush(Color.FromRgb(54, 57, 63));
                    TopBar.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
                    MainText.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 212));
                    DescriptionText.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 212));
                    MoonIcon.Visibility = Visibility.Hidden;
                    SunIcon.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs eventArgs)
        {
            if (eventArgs.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs eventArgs)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs eventArgs)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs eventArgs)
        {
            Properties.UserProperties.Default.WindowWidthAfterShutdown = ActualWidth;
            Properties.UserProperties.Default.WindowHeightAfterShutdown = ActualHeight;
            Properties.UserProperties.Default.WindowPositionX = Left;
            Properties.UserProperties.Default.WindowPositionY = Top;
            Properties.UserProperties.Default.Save();

            Application.Current.Shutdown();
        }

        private void Login(object sender, RoutedEventArgs eventArgs)
        {
            string _username = RegistationUsername.Text;
            long _phoneNumber = Int64.Parse(PhoneNumber.Text);
            _databaseConnection.CreateNewUser(_username, _phoneNumber, "Нет статуса");

            MainWindow.IsFirstLaunched = false;
            PropertiesSettings.SavePropertiesSettings();

            User = new UserModel
            {
                Username = _username,
                PhoneNumber = _phoneNumber,
                Status = "Нет статуса",
                Images = new List<Image>()
            };

            MainViewModel.User = _databaseConnection.InitializeUser(_phoneNumber);
            
            Properties.UserProperties.Default.LogginedUserPhoneNumber = _phoneNumber;
            Properties.UserProperties.Default.Save();
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Activate();
            mainWindow.Show();
            Close();
        }

        private void LoginFromKeyboard(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.Key == Key.Enter)
            {
                Login(sender, eventArgs);
            }
        }

        private void ChangeBackgroundColor(object sender, RoutedEventArgs routedEventArgs)
        {
            ColorAnimation animation = new ColorAnimation();

            if (((SolidColorBrush)Background).Color != Color.FromRgb(224, 224, 224))
            {
                // Dark theme

                MoonIcon.Visibility = Visibility.Visible;
                SunIcon.Visibility = Visibility.Hidden;

                // // Background animation
                // Background = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                // animation.From = Color.FromRgb(54, 57, 63);
                // animation.To = Color.FromRgb(224, 224, 224);
                // animation.Duration = new Duration(TimeSpan.FromSeconds(.25f));
                // Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                //
                // // TopBar animation
                // TopBar.Background = new SolidColorBrush(Color.FromRgb(212, 212, 212));
                // animation.From = Color.FromRgb(27, 27, 27);
                // animation.To = Color.FromRgb(212, 212, 212);
                // animation.Duration = new Duration(TimeSpan.FromSeconds(.25f));
                // TopBar.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                //
                // MainText.Foreground = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                // DescriptionText.Foreground = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                // animation.From = Color.FromRgb(212, 212, 212);
                // animation.To = Color.FromRgb(27, 27, 27);
                // animation.Duration = new Duration(TimeSpan.FromSeconds(.25f));
                // MainText.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                // DescriptionText.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                //
                // Change property value
                Properties.UserProperties.Default.IsLightThemeIsUsed = true;
            }
            else
            {
                // Light theme
                //
                // MoonIcon.Visibility = Visibility.Hidden;
                // SunIcon.Visibility = Visibility.Visible;
                //
                // // Background animation
                // Background = new SolidColorBrush(Color.FromRgb(54, 57, 63));
                // animation.From = Color.FromRgb(224, 224, 224);
                // animation.To = Color.FromRgb(54, 57, 63);
                // animation.Duration = new Duration(TimeSpan.FromSeconds(.25f));
                // Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                //
                // // TopBar animation
                // TopBar.Background = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                // animation.From = Color.FromRgb(212, 212, 212);
                // animation.To = Color.FromRgb(27, 27, 27);
                // animation.Duration = new Duration(TimeSpan.FromSeconds(.25f));
                // TopBar.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                //
                // MainText.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 212));
                // DescriptionText.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 212));
                // animation.From = Color.FromRgb(27, 27, 27);
                // animation.To = Color.FromRgb(212, 212, 212);
                // animation.Duration = new Duration(TimeSpan.FromSeconds(.25f));
                // MainText.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                // DescriptionText.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);


                // Change property value
                Properties.UserProperties.Default.IsLightThemeIsUsed = false;
            }

            Properties.UserProperties.Default.Save();
        }
    }
}