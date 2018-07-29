using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace ContentImporter
{
    public class Configuration
    {
        public bool IsWindows { get; }
        public string Port { get; }
        public string Domain { get; }
        public string URL { get; }
        public string ContentFolder { get; }

        public Configuration() : this(new ConfigurationBuilder().AddEnvironmentVariables().Build()) { }

        public Configuration(IConfigurationRoot configuration)
        {
            this.IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            this.Domain = configuration.GetValue<string>("API_DOMAIN") ?? "*";
            this.Port = configuration.GetValue<string>("API_PORT") ?? "5000";
            this.URL = string.Format($"http://{this.Domain}:{this.Port}");

            this.ContentFolder = configuration.GetValue<string>("CONTENT_FOLDER") ?? @"C:\content";
        }
    }
}
