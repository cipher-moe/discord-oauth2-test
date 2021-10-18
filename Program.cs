using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace discord_oauth_test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            dotenv.net.DotEnv.Load();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
