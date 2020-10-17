using Microsoft.Extensions.Configuration;
using System;

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

        public static string AppSecret { get; private set; }

        public static string FacebookCheckTokenUrl { get; private set; }
        public static string FacebookClientToken { get; private set; }
        public static string FacebookAppId { get; private set; }

        public static string GoogleCheckTokenUrl { get; private set; }

        public static string Endpoints_Main { get; private set; }

        public static string ConnectionStrings_Commands { get; private set; }
        public static string ConnectionStrings_Queries { get; private set; }

        public static Type DatabaseConnectionForQueries { get; private set; }

        public static Type DatabaseConnectionForCommands { get; private set; }

        public static string SmsServiceProvider { get; private set; }
        public static string SmsServiceProvider_KingSmsUrl { get; private set; }


        public static void SetTetrisDatabaseConnectionTypeForCommands(Type type)
        {
            DatabaseConnectionForCommands = type;
        }

        public static void SetTetrisDatabaseConnectionTypeForQueries(Type type)
        {
            DatabaseConnectionForQueries = type;
        }

        public static void LoadTetrisSettings(this IConfiguration configuration)
        {
            ConnectionStrings_Commands = configuration.GetConnectionString("TetrisCommands");
            ConnectionStrings_Queries = configuration.GetConnectionString("TetrisQueries");
            GoogleCheckTokenUrl = configuration["AppSettings:GoogleCheckTokenUrl"];
            FacebookCheckTokenUrl = configuration["AppSettings:FacebookCheckTokenUrl"];
            FacebookClientToken = configuration["AppSettings:FacebookClientToken"];
            FacebookAppId = configuration["AppSettings:FacebookAppId"];
            AppSecret = configuration["AppSettings:AppSecret"];
        }
    }
}
