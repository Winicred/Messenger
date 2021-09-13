using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Text.Json;
using Messenger.MVVM.Model;

namespace Messenger.Core.Utils
{
    public class IsolatedStorage
    {
        public void SaveLoginSettingsInIsoStorage(UserModel user)
        {
            IsolatedStorageFile applicationStorageFileForUser = IsolatedStorageFile.GetUserStoreForAssembly();
            IsolatedStorageFileStream applicationStorageStreamForUser = new IsolatedStorageFileStream("settings.txt", FileMode.Create, applicationStorageFileForUser);
            UserModel settings = new UserModel()
            {
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Status = user.Status,
                Contacts = user.Contacts
            };
            byte[] contents = JsonSerializer.SerializeToUtf8Bytes(settings);
 
            using (StreamWriter sw = new StreamWriter(applicationStorageStreamForUser))
            {
                sw.WriteLine(contents);
            }
        }
        private static void ReadSettingsFromIsoStorage()
        {
        }
    }
}