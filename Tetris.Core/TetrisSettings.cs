using Microsoft.Extensions.Configuration;
using System;

namespace Tetris
{
    public static class TetrisSettings
    {
        internal static string EncryptionSecret { get; private set; }

        public static string ForCommands { get; private set; }

        public static string ForQueries { get; private set; }

        internal static string FacebookCheckTokenUrl { get; private set; }

        internal static string FacebookAppId { get; private set; }

        internal static string FacebookAppSecret { get; private set; }

        internal static string GoogleCheckTokenUrl { get; private set; }

        public static Type DatabaseConnectionForQueries { get; private set; }

        public static Type DatabaseConnectionForCommands { get; private set; }

        public static string[] CorsAllowedOrigins { get; private set; }

        public static void LoadConfiguration(IConfiguration configuration)
        {
            ForCommands = configuration.GetString("ConnectionStrings", nameof(ForCommands));
            ForQueries = configuration.GetString("ConnectionStrings", nameof(ForQueries));

            EncryptionSecret = configuration.GetString("AppSettings", nameof(EncryptionSecret));

            FacebookAppId = configuration.GetString("AppSettings", nameof(FacebookAppId));
            FacebookAppSecret = configuration.GetString("AppSettings", nameof(FacebookAppSecret));
            FacebookCheckTokenUrl = configuration.GetString("AppSettings", nameof(FacebookAppSecret));

            GoogleCheckTokenUrl = configuration.GetString("AppSettings", nameof(GoogleCheckTokenUrl));

            CorsAllowedOrigins = configuration.GetStringArray(nameof(CorsAllowedOrigins));
        }

        public static void SetTetrisDatabaseConnectionTypeForCommands(Type type)
        {
            DatabaseConnectionForCommands = type;
        }

        public static void SetTetrisDatabaseConnectionTypeForQueries(Type type)
        {
            DatabaseConnectionForQueries = type;
        }

        private static string GetString(this IConfiguration configuration, string section, string key, bool required = false)
        {
            var s = configuration.GetSection(section);

            if (!s.Exists() && required)
                throw new InvalidOperationException($"The section '{ section }' is not present in the configuration file of your application");

            var value = s[key];

            if (string.IsNullOrEmpty(value) && required)
                throw new InvalidOperationException($"The section '{ section }' does not contains the key '{ key }' in the configuration file of your application");

            return value;
        }

        private static string[] GetStringArray(this IConfiguration configuration, string key, bool required = false)
        {
            var s = configuration.GetSection("AppSettings");

            if (!s.Exists() && required)
                throw new InvalidOperationException($"The section 'AppSettings' is not present in the configuration file of your application");

            var value = s[key];

            if (string.IsNullOrEmpty(value) && required)
                throw new InvalidOperationException($"The section 'AppSettings' does not contains the key '{ key }' in the configuration file of your application");

            return value.Split(",", StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
