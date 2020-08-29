using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SSL_Ceritificate_Utilities
{
    public class Cert_Requester
    {
        private ILogger _logger;
        private readonly HttpClientHandler _handler;
        private readonly HttpClient _client;
        private X509Certificate2 certificate;

        public Cert_Requester(ILogger<Cert_Requester> logger)
        {
            _logger = logger;
            certificate = null;
            _handler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    certificate = new X509Certificate2(cert.GetRawCertData());
                    return true;
                }
            };
             _client = new HttpClient(_handler);
        }

        public async Task<X509Certificate2> GetServerCertificateAsync(string url)
        {
            _logger.LogInformation($"Sending HttpRequest to {url}");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            
            _logger.LogInformation($"Processing http request response");
            var response = await _client.SendAsync(request);

            _logger.LogInformation("Returning certificate");
            return certificate;
        }
    }
}
