using System.Collections.ObjectModel;

namespace Messenger.MVVM.Model
{
    public class UserModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        
    }
}