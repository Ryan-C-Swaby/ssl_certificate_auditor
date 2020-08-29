using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;

namespace SSL_Certificate_Utilities
{
    public class Cert_Validator
    {
        private ILogger _logger;

        public Cert_Validator(ILogger<Cert_Validator> logger)
        {
            _logger = logger;
        }

        public bool IsCertificateExpirationValid(X509Certificate2 cert, DateTime certMustExpireOnOrAfterDate)
        {
            return GetCertificateExpirationDate(cert) >= certMustExpireOnOrAfterDate;
        }

        private DateTime GetCertificateExpirationDate(X509Certificate2 cert)
        {
            DateTime expiration;
            DateTime.TryParse(cert.GetExpirationDateString(), out expiration);
            return expiration;
        }
    }
}