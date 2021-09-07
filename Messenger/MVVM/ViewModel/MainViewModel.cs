﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Messenger.Core;
using Messenger.Core.Utils;
using Messenger.MVVM.Model;
using Timer = System.Timers.Timer;

namespace Messenger.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        
        public RelayCommand SendCommand { get; set; }

        private ContactModel _selectedContact;

        private string _message;
        private static int _secondsElapsedFromThePreviousMessage;
        private UserModel _currentUser;

        public ContactModel SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                SetTimer();
                OnPropertyChanged();
            }
        }
        
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public UserModel User
        {
            get => _currentUser;
            set => _currentUser = value;
        }
        
        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            Contacts.Add(new ContactModel
            {
                Username = "Stepa",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                Messages = Messages
            });

            Contacts.Add(new ContactModel
            {
                Username = "Kirill",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                Messages = Messages
            });

            Contacts.Add(new ContactModel
            {
                Username = "Oleg",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                Messages = Messages
            });

            Contacts.Add(new ContactModel
            {
                Username = "Vasja",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                Messages = Messages
            });
            
            Messages.Add(new MessageModel
            {
                Username = "Kirill",
                Message = "Hi. My name is Kirill",
                UsernameColor = "#833f8d",
                ImageSource = "https://i.imgur.com/0wkVW.jpg",
                Time = "00:00",
                IsNativeOrigin = false,
                FirstMessage = true,
            });

            DateTime dateTime = DateTime.Now;
            SendCommand = new RelayCommand(o =>
            {
                if (_message != "")
                {
                    if (_secondsElapsedFromThePreviousMessage >= 2 || Messages.Count == 0)
                    {
                        Messages.Add(new MessageModel
                        {
                            Username = SelectedContact.Username,
                            Message = Message,
                            UsernameColor = "#833f8d",
                            ImageSource = "https://i.imgur.com/0wkVW.jpg",
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