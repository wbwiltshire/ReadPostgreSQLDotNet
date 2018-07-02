using System;
using System.Collections.Generic;
using System.Text;

namespace ReadPostgreSQLDotNet
{
    public class AppSettingsConfiguration
    {
        public Logging Logging { get; set; }
        public PostgreSQL PostgreSQL { get; set; }
    }

    //Logging Objects
    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }
    }
    public class LogLevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    //Data Objects
    public class PostgreSQL
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string PW { get; set; }
        public string Database { get; set; }
        public string SSLMode { get; set; }
    }
}
