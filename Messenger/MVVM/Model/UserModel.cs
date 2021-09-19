using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Messenger.MVVM.Model
{
    public class UserModel
    {
        public string Username { get; set; }
        public long PhoneNumber { get; set; }
        public string Status { get; set; }
        public List<Image> Images { get; set; }
        public string ImageSource { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public string OnlineStatus { get; set; }
    }
}