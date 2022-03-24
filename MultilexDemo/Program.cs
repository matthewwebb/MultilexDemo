


using FirstDataBank.DrugServer.API;
using FirstDataBank.DrugServer.API.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MultilexDemo
{
    public static class Program
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            CreateHostBuilder(args).Build().RunAsync();

            var mainForm = ServiceProvider?.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDrugServerApi(settings =>
                    {
                        //Get Settings from Config File
                        settings.ConfigurationSourceSettings = context.Configuration.GetSection("FDBDrugServerSettings");
                    });
                    services.AddSingleton<IDrugSystem>(serviceProvider =>
                    {
                        var factory = serviceProvider.GetRequiredService<IDrugSystemFactory>();
                        return factory.CreateSystem();
                    });

                    services.AddScoped<MainForm>();

                    ServiceProvider = services.BuildServiceProvider();
                });
        }

    }
}