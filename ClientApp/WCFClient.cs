using Common;
using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class WCFClient : ChannelFactory<IWCFContract>, IDisposable
    {
        IWCFContract factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {

            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }





        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public void RegistrujKorisnika()
        {
            try
            {
                Console.WriteLine(factory.RegistrujKorisnika());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to RegistrujKorisnika. Error message : {0}", e.Message);
            }
        }


        public void DodajProjekciju(Projekcija projekcija)
        {
            try
            {
                Console.WriteLine(factory.DodajProjekciju(projekcija));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to DodajProjekciju. Error message : {0}", e.Message);
            }

        }

        public void IzmeniProjekciju(Projekcija projekcija)
        {
            try
            {
                Console.WriteLine(factory.IzmeniProjekciju(projekcija));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to IzmeniProjekciju. Error message : {0}", e.Message);
            }
        }

        public void IzmeniPopust(double noviPopust)
        {
            try
            {
                Console.WriteLine(factory.IzmeniPopust(noviPopust));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to IzmeniPopust. Error message : {0}", e.Message);
            }
        }


        public void UplatiPareNaRacun(double uplata)
        {
            try
            {
                Console.WriteLine(factory.UplatiPareNaRacun(uplata));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to UplatiPareNaRacun. Error message : {0}", e.Message);
            }
        }

        public void NapraviRezervaciju(int idPredstave, int brojKarata)
        {
            try
            {
                Console.WriteLine(factory.NapraviRezervaciju(idPredstave, brojKarata));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to NapraviRezervaciju. Error message : {0}", e.Message);
            }
        }

        public void PlatiRezervaciju(int idRezervacije)
        {
            try
            {
                Console.WriteLine(factory.PlatiRezervaciju(idRezervacije));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to PlatiRezervaciju. Error message : {0}", e.Message);
            }
        }
    }
}
