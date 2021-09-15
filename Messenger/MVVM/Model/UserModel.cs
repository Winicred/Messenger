using System.Collections.ObjectModel;
using System.Drawing;

namespace Messenger.MVVM.Model
{
    public class UserModel
    {
        public string Username { get; set; }
        public long PhoneNumber { get; set; }
        public string Status { get; set; }

        public string ImageSource {  get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
    }
}