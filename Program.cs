using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContentImporter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new Configuration();

            var web = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options => options.Limits.MaxRequestBodySize = null)
                .UseUrls(config.URL)
                .Build();

            web.Run();
        }
    }
}
