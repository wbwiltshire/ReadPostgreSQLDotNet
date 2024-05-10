using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.Binder;
using Npgsql;

namespace ReadPostgreSQLDotNet
{
    public class Program
    {
        //Note: You must change the default language version to at least 7.1
        //      Project->Properties->Build->Advanced Build Settings->Language Version
        public static async Task Main(string[] args)
        {
            IConfiguration config;
            IConfigurationBuilder builder;
            AppSettingsConfiguration settings = new AppSettingsConfiguration();
            NpgsqlConnection conn;
            string query = "SELECT Id, FirstName, LastName, EMail FROM Contact ORDER BY Id;";
            string connString = String.Empty;
            string appSettings = "appsettings.json";

            //Setup configuration
            builder = new ConfigurationBuilder().
                AddJsonFile(appSettings);
            config = builder.Build();
            ConfigurationBinder.Bind(config, settings);
            connString = $"Host={settings.PostgreSQL.Host};Port={settings.PostgreSQL.Port};Username={settings.PostgreSQL.Username};" + 
                $"Password={settings.PostgreSQL.PW};Database={settings.PostgreSQL.Database};SSL Mode={settings.PostgreSQL.SSLMode};Trust Server Certificate=true";

            try
            {
                if (await HostExists(settings.PostgreSQL.Host))
                {
                    using (conn = new NpgsqlConnection(connString))
                    {
                        await conn.OpenAsync();
                        await PrintResults(query, conn);
                    };
                }
                else
                    Console.WriteLine($"Host does not exist: {settings.PostgreSQL.Host}");
            }
            catch (Npgsql.NpgsqlException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }

            Console.WriteLine("\nPress <enter> to end....");

            Console.ReadLine();
        }

        private static async Task PrintResults(string query, NpgsqlConnection conn)
        {
            Console.WriteLine($"Reading from connection: {conn.Host}:{conn.Port}\n");

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    using (var reader = await command.ExecuteReaderAsync())                                         //results in null reference exception
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine($"\t{reader.GetInt32(0)}|{reader.GetString(1)}|{reader.GetString(2)}|{reader.GetString(3)}|");
                        }
                    }
                }

            }
            catch (Npgsql.NpgsqlException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }

        private static async Task<bool> HostExists(string host)
        {
            bool status = false;

            IPHostEntry entry;
            try
            {
                entry = await Dns.GetHostEntryAsync(host);
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return status;
        }
    }
}
