using Microsoft.Extensions.Configuration;
using System;
using Tetris.Core.Extensions;

namespace Tetris.Core
{
    public static class TetrisSettings
    {
        public static string Zenvia_ApiService { get; private set; }
        public static string Zenvia_ApiKey { get; private set; }

        public static string Smtp_Host { get; private set; }
        public static int Smtp_Port { get; private set; }
        public static string Smtp_User { get; private set; }
        public static string Smtp_Password { get; private set; }

        public static string Secret { get; private set; }

        public static string FacebookAppId { get; private set; }
        public static string FacebookAppSecret { get; private set; }
        public static string FacebookCheckTokenUrl { get; private set; }

        public static string GoogleCheckTokenUrl { get; private set; }

        public static string Endpoints_Main { get; private set; }

        public static string ConnectionStrings_Commands { get; private set; }
        public static string ConnectionStrings_Queries { get; private set; }

        public static Type DatabaseConnectionForQueries { get; private set; }

        public static Type DatabaseConnectionForCommands { get; private set; }

        public static string SmsServiceProvider { get; private set; }
        public static string SmsServiceProvider_KingSmsUrl { get; private set; }

        public static void LoadConfiguration(IConfiguration configuration)
        {
            Console.WriteLine("Configurações carregadas: ");
            Console.WriteLine(configuration.TryToSerialize());

            Zenvia_ApiService = GetString(configuration, "Zenvia", "service");
            Zenvia_ApiKey = GetString(configuration, "Zenvia", "key");

            if (!int.TryParse(configuration.GetString("Smtp", "port"), out int port))
                throw new InvalidOperationException($"A seção 'Smtp' contém um valor inválido para a chave 'port' nos arquivos de configuração da aplicação");

            Smtp_Host = configuration.GetString("Smtp", "host");
            Smtp_Port = port;
            Smtp_User = configuration.GetString("Smtp", "user");
            Smtp_Password = configuration.GetString("Smtp", "password");

            Secret = configuration.GetString("AppSettings", "MoviSecret");
            FacebookAppId = configuration.GetString("AppSettings", "FacebookAppId");
            FacebookAppSecret = configuration.GetString("AppSettings", "FacebookAppSecret");
            FacebookCheckTokenUrl = configuration.GetString("AppSettings", "FacebookCheckTokenUrl");

            GoogleCheckTokenUrl = configuration.GetString("AppSettings", "GoogleCheckTokenUrl");

            SmsServiceProvider = configuration.GetString("AppSettings", nameof(SmsServiceProvider));
            SmsServiceProvider_KingSmsUrl = configuration.GetString("AppSettings", "KingSms.Url");

            Endpoints_Main = configuration.GetString("Endpoints", "main");

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

        private static string GetString(this IConfiguration configuration, string section, string key)
        {
            var s = configuration.GetSection(section);

            if (!s.Exists())
                throw new InvalidOperationException($"A seção '{ section }' não está presente nos arquivo de configuração da aplicação");

            var value = s[key];

            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"A seção '{ section }' não contém a chave '{ key }' nos arquivos de configuração da aplicação");

            return value;
        }
    }
}
