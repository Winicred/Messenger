using System;
using System.Collections.ObjectModel;
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

            // ToMaximizedWindow.Visibility = Visibility.Visible; 
            // ToNormalWindow.Visibility = Visibility.Hidden;
            //Width = Properties.UserProperties.Default.WindowWidthAfterShutdown;
            //Height = Properties.UserProperties.Default.WindowHeightAfterShutdown;
            //Left = Properties.UserProperties.Default.WindowPositionX;
            //Top = Properties.UserProperties.Default.WindowPositionY;

            //RegistrationWindowTitle.Content = MainWindow.WindowTitle;
            // RegistrationBlock.Visibility = Visibility.Hidden;
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
                // ToNormalWindow.Visibility = Visibility.Hidden;
                // ToMaximizedWindow.Visibility = Visibility.Visible;
            }
            else
            {
                WindowState = WindowState.Maximized;
                // ToNormalWindow.Visibility = Visibility.Visible;
                // ToMaximizedWindow.Visibility = Visibility.Hidden;
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

        private void RegistrationButton(object sender, RoutedEventArgs eventArgs)
        {
            // LoginBlock.Visibility = Visibility.Hidden;
            // RegistrationBlock.Visibility = Visibility.Visible;
        }

        private void LoginFormButton(object sender, RoutedEventArgs eventArgs)
        {
            // LoginBlock.Visibility = Visibility.Visible;
            // RegistrationBlock.Visibility = Visibility.Hidden;
        }

        private void Login(object sender, RoutedEventArgs eventArgs)
        {
            // string login = UserLogin.Text;
            // string password = UserPassword.Password;

            // if (login == "Kirill" && password == "asd")
            // {
            // ErrorInfo.Content = "Вы вошли!";
            // MainWindow.IsFirstLaunched = false;
            // PropertiesSettings.SavePropertiesSettings();
            // ((MainWindow)Application.Current.MainWindow)?.ShowWindow();
            // Close();
            // }
            // else
            // {
            // ErrorInfo.Content = "Неправильный логин и/или пароль!";
            // }
        }

        private void LoginFromKeyboard(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.Key == Key.Enter)
            {
                Login(sender, eventArgs);
            }
        }

        private void UserRegistration(object sender, RoutedEventArgs routedEventArgs)
        {
            // _newUser = new UserModel
            // {
            //     Login = UserLoginInRegistrationField.Text,
            //     Password = UserPasswordInRegistrationField.Text,
            //     Username = "Kerich",
            //     Email = UserEmailInRegistrationField.Text,
            //     Status = "Some status",
            //     Contacts = new ObservableCollection<ContactModel>()
            // };
            //
            // _databaseConnection.CreateNewUser(_newUser.Login, _newUser.Password, _newUser.Username, _newUser.Email,
            //     _newUser.Status);
            // ErrorInfo.Content = "Вы зарегистрировались";
            //
            // if ((MainWindow)Application.Current.MainWindow != null)
            //     ((MainWindow)Application.Current.MainWindow).Show();
            //
            // MainWindow.IsFirstLaunched = false;
            //
            // Close();
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

        private void NextRegistrationStep(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = new Storyboard();
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();
            thicknessAnimation.BeginTime = new TimeSpan(0);
            thicknessAnimation.SetValue(Storyboard.TargetNameProperty, "FirstRegistrationStep");
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(MarginProperty));

            thicknessAnimation.From = new Thickness(0, 0, 0, 0);
            thicknessAnimation.To = new Thickness(0, 0, 800, 0);
            thicknessAnimation.Duration = new Duration(TimeSpan.FromSeconds(.1f));
            
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Begin(this);

            thicknessAnimation.SetValue(Storyboard.TargetNameProperty, "SecondRegistrationStep");

            thicknessAnimation.From = new Thickness(800, 0, 0, 0);
            thicknessAnimation.To = new Thickness(0, 0, 0, 0);
            thicknessAnimation.Duration = new Duration(TimeSpan.FromSeconds(.1f));

            storyboard.Children.Add(thicknessAnimation);
            storyboard.Begin(this);
        }
    }
}