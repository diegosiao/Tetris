using Microsoft.Extensions.Configuration;
using System;

namespace Tetris
{
    /// <summary>
    /// Tetris application settings
    /// </summary>
    public static class TetrisSettings
    {
        internal static string TetrisEncryptionSecret { get; private set; }

        /// <summary>
        /// The default connection string for read/write operations
        /// </summary>
        public static string TetrisCommands { get; private set; }

        /// <summary>
        /// The default connection string for readonly operations
        /// </summary>
        public static string TetrisQueries { get; private set; }

        internal static string FacebookCheckTokenUrl { get; private set; }

        internal static string FacebookAppId { get; private set; }

        internal static string FacebookAppSecret { get; private set; }

        internal static string GoogleCheckTokenUrl { get; private set; }

        /// <summary>
        /// The concrete type of the default database connection class for queries (e.g.: typeof(MySqlConnection))
        /// </summary>
        public static Type DatabaseConnectionForQueries { get; private set; }

        /// <summary>
        /// The concrete type of the default database connection class for commands (e.g.: typeof(MySqlConnection))
        /// </summary>
        public static Type DatabaseConnectionForCommands { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static string[] CorsAllowedOrigins { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public static void LoadConfiguration(IConfiguration configuration)
        {
            TetrisCommands = configuration.GetString("ConnectionStrings", nameof(TetrisCommands));
            TetrisQueries = configuration.GetString("ConnectionStrings", nameof(TetrisQueries));

            TetrisEncryptionSecret = configuration.GetString("AppSettings", nameof(TetrisEncryptionSecret));

            FacebookAppId = configuration.GetString("AppSettings", nameof(FacebookAppId));
            FacebookAppSecret = configuration.GetString("AppSettings", nameof(FacebookAppSecret));
            FacebookCheckTokenUrl = configuration.GetString("AppSettings", nameof(FacebookAppSecret));

            GoogleCheckTokenUrl = configuration.GetString("AppSettings", nameof(GoogleCheckTokenUrl));

            CorsAllowedOrigins = configuration.GetStringArray(nameof(CorsAllowedOrigins));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public static void SetTetrisDatabaseConnectionTypeForCommands(Type type)
        {
            DatabaseConnectionForCommands = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
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
