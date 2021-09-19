using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Messenger.Core;
using Messenger.Core.Utils;
using Messenger.MVVM.Model;
using Application = System.Windows.Forms.Application;
using Timer = System.Timers.Timer;

namespace Messenger.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private DatabaseConnection _databaseConnection = new DatabaseConnection();
        private ServerConnect _serverConnect = new ServerConnect();

        public static ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public RelayCommand SendCommand { get; set; }

        private static int _secondsElapsedFromThePreviousMessage;

        private ContactModel _selectedContact;

        public ContactModel SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                VisibilityIfContactSelected = Visibility.Visible;
                SetTimer();
                OnPropertyChanged();
            }
        }

        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        private string _clearSearchTextBoxText;

        public string ClearSearchTextBoxText
        {
            get => _clearSearchTextBoxText;
            set
            {
                _clearSearchTextBoxText = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibilityIfContactSelected;

        public Visibility VisibilityIfContactSelected
        {
            get => _visibilityIfContactSelected;
            set
            {
                _visibilityIfContactSelected = value;
                OnPropertyChanged();
            }
        }

        public static UserModel User { get; set; }

        public MainViewModel()
        {
            if (SelectedContact == null)
            {
                _visibilityIfContactSelected = Visibility.Hidden;
                OnPropertyChanged();
            }

            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            User = _databaseConnection.InitializeUser(MainWindow.PhoneNumber);
            if (User == null)
            {
                MainWindow.IsFirstLaunched = true;
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }

            User.ImageSource = "https://i.imgur.com/btaOetV.jpg";

            Messages.Add(new MessageModel
            {
                Username = "Username",
                Message = "Hi. My name is Kirill",
                UsernameColor = "#833f8d",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                Time = "00:00",
                IsNativeOrigin = false,
                FirstMessage = true,
            });
            
            Contacts.Add(new ContactModel
            {
                Username = "Stepa",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                OnlineStatus = "не в сети",
                Messages = Messages,
            });

            Contacts.Add(new ContactModel
            {
                Username = "Kirill",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                OnlineStatus = "в сети",
                Messages = Messages
            });

            Contacts.Add(new ContactModel
            {
                Username = "Oleg",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                OnlineStatus = "был недавно",
                Messages = Messages
            });

            Contacts.Add(new ContactModel
            {
                Username = "Vasja",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                OnlineStatus = "был давно",
                Messages = Messages
            });

            DateTime dateTime = DateTime.Now;
            SendCommand = new RelayCommand(o =>
            {
                if (_message != "" && !_message.Contains(" "))
                {
                    if (_secondsElapsedFromThePreviousMessage >= 2 || Messages.Count == 0)
                    {
                        Messages.Add(new MessageModel
                        {
                            Username = User.Username,
                            Message = Message,
                            UsernameColor = "#833f8d",
                            ImageSource = User.ImageSource,
                            Time = dateTime.ToString("HH:mm"),
                            IsNativeOrigin = false,
                            FirstMessage = true,
                        });
                    }
                    else
                    {
                        Messages.Add(new MessageModel
                        {
                            Message = Message,
                            Time = dateTime.ToString("HH:mm"),
                            IsNativeOrigin = false,
                            FirstMessage = false,
                        });
                    }
                }

                _serverConnect.SendMessage("Пользователь (" + User.Username + ") отправил сообщение: (" + Message + ")");

                Message = "";
                _secondsElapsedFromThePreviousMessage = 0;
                OnPropertyChanged();
            });
        }

        public static void SetTimer()
        {
            Timer messageTimer = new Timer();
            messageTimer.Elapsed += OnTimedEvent;
            messageTimer.Interval = 1000;
            messageTimer.AutoReset = true;
            messageTimer.Enabled = true;
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _secondsElapsedFromThePreviousMessage += 1;
        }
    }
}