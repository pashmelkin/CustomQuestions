using System;
using Npgsql;

namespace CustomQuestions.Framework
{
    public static class Extensions
    {
        public static string ToConnectionString(this PostgreSQLSettings postgresSettings)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                ApplicationName = postgresSettings.ApplicationName,
                Database = postgresSettings.Database,
                Username = postgresSettings.Username,
                Password = postgresSettings.Password,
                Port = postgresSettings.Port,
                Host = postgresSettings.Server,
                CommandTimeout = 1048576,
                Timeout = 60
            };

            return builder.ConnectionString;
        }

    }
}
