namespace testmvc.Configuration
{
    using System.Configuration;

    public static class Config
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