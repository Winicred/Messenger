namespace Messenger
{
    class PropertiesSettings
    {
        public static void GetPropertiesSettings()
        {
            MainWindow.IsFirstLaunched = Properties.UserProperties.Default.IsFirstLaunched;
        }
        public static void SavePropertiesSettings()
        {
            Properties.UserProperties.Default.IsFirstLaunched = MainWindow.IsFirstLaunched;

            Properties.UserProperties.Default.Save();
        }
    }
}
