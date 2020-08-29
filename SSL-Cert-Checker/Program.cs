using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SSL_Ceritificate_Utilities;
using SSL_Certificate_Utilities;
using Text_File_Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace SSL_Cert_Checker
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider(); 

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            
            Cert_Requester certRequest = serviceProvider.GetRequiredService<Cert_Requester>();
            Cert_Validator certValidator = serviceProvider.GetRequiredService<Cert_Validator>();
          
            string[] lines = System.IO.File.ReadAllLines(config["SitesToAuditFilePath"]);
            Text_File_Logger fileLogger = new Text_File_Logger(config["LogFilePath"]);

            foreach(string line in lines)
            {
                 X509Certificate2 certificate = GetSSLCert(certRequest, line).Result;
                 if(!certValidator.IsCertificateExpirationValid(certificate, Convert.ToDateTime(config["CertificateMustExpireOnOrAfter"])))
                 {
                    fileLogger.WriteToTextFile($"{line} certificate is invalid.");
                 }
            }

            Console.WriteLine("Process Complete");
            Console.ReadLine();
          
        }

        private static async Task<X509Certificate2> GetSSLCert(Cert_Requester cert_Requester, string url)
        {
            var cert = await cert_Requester.GetServerCertificateAsync(url);
            return cert;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
             services.AddLogging(configure => configure.AddConsole())                    
                .AddTransient<Cert_Requester>();

             services.AddLogging(configure => configure.AddConsole())                    
                .AddTransient<Cert_Validator>();
            
        }
    }
}
