using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ClientCertValidator : X509CertificateValidator
    {
        /// <summary>
        /// Implementation of a custom certificate validation on the client side.
        /// Client should consider certificate valid if the given certifiate is not self-signed.
        /// If validation fails, throw an exception with an adequate message.
        /// </summary>
        /// <param name="certificate"> certificate to be validate </param>
        public override void Validate(X509Certificate2 certificate)
        {
            X509Certificate2 clnCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name));
            if (certificate == null)
            {
                Audit.AuthenticationFailed("Nema sertifikat");
                throw new Exception("Client certificate not found.");
            }

            if (!certificate.Subject.Equals(certificate.Issuer))
            {
                Audit.AuthenticationFailed("Certificate is not self-signed.");
                throw new Exception("Certificate is not self-signed.");
            }
            Audit.AuthenticationSuccess(certificate.Subject);
        }
    }
}
