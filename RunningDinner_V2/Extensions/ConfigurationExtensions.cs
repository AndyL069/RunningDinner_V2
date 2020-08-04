using Microsoft.Extensions.Configuration;

namespace RunningDinner.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetEmailSettings(this IConfiguration configuration, string key)
        {
            string result = configuration.GetValue<string>("Mailjet:" + key);
            return result;
        }

        public static string GetMapsSettings(this IConfiguration configuration, string key)
        {
            string result = configuration.GetValue<string>("Maps:" + key);
            return result;
        }

        public static string GetAzureStorageSettings(this IConfiguration configuration, string key)
        {
            string result = configuration.GetValue<string>("AzureStorage:" + key);
            return result;
        }

        public static string GetFacebookSettings(this IConfiguration configuration, string key)
        {
            string result = configuration.GetValue<string>("Facebook:" + key);
            return result;
        }

        public static string GetGoogleSettings(this IConfiguration configuration, string key)
        {
            string result = configuration.GetValue<string>("Google:" + key);
            return result;
        }

        public static string GetServerSettings(this IConfiguration configuration, string key)
        {
            string result = configuration.GetValue<string>("Server:" + key);
            return result;
        }
    }
}


