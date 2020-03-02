using Microsoft.Extensions.Configuration;
using System;

namespace Tetris.Core
{
    public static class TetrisSettings
    {
        public static string EncryptionSecret { get; private set; }

        public static string ConnectionStrings_Commands { get; private set; }

        public static string ConnectionStrings_Queries { get; private set; }

        public static Type DatabaseConnectionForQueries { get; private set; }

        public static Type DatabaseConnectionForCommands { get; private set; }

        public static void LoadConfiguration(IConfiguration configuration)
        {
            ConnectionStrings_Commands = configuration.GetString("ConnectionStrings", "commands");
            ConnectionStrings_Queries = configuration.GetString("ConnectionStrings", "queries");
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
    }
}
