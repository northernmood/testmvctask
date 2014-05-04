namespace testmvc.Configuration
{
    using System.Configuration;

    public static class Settings
    {
        public static string DefaultCulture
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultCulture"];
            }
        }
    }
}