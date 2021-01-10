using System;
using System.Collections.Generic;
using Common;
using System.ServiceModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Manager;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
            string srvCertCN = "WCFService";

            /// Define the expected certificate for signing ("<username>_sign" is the expected subject name).
            /// .NET WindowsIdentity class provides information about Windows user running the given process
            string signCertCN = String.Empty;

            /// Define subjectName for certificate used for signing which is not as expected by the service
            string wrongCertCN = String.Empty;

            NetTcpBinding binding2 = new NetTcpBinding();
            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            int id;
            double iznos;
            string ime;
            int br;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address2 = new EndpointAddress(new Uri("net.tcp://localhost:10001/WCFService"),
                                        new X509CertificateEndpointIdentity(srvCert));
           
            using (WCFClient proxy = new WCFClient(binding2, address2))
            {
                proxy.RegistrujKorisnika();
                while (true)
                {
                    Console.WriteLine(" -- (1): UPLATA NA RAČUN -- ");
                    Console.WriteLine(" -- (2): REZERVIŠI -- ");
                    Console.WriteLine(" -- (3): PLATI REZERVACIJU -- ");
                    Console.WriteLine(" -- (4): DODAJ PROJEKCIJU -- ");
                    Console.WriteLine(" -- (5): IZMENI PROJEKCIJU -- ");
                    Console.WriteLine(" -- (6): IZMENI POPUST -- ");
                    ;

                    int choice = Int32.Parse(Console.ReadLine());

                    {
                        switch (choice)
                        {

                            case 1:
                                
                                Console.WriteLine("Unesite iznos: ");
                                iznos = Double.Parse(Console.ReadLine());
                                proxy.UplatiPareNaRacun(iznos);
  
                                break;

                            case 2: 
                                Console.WriteLine("Unesite ID predstave: ");
                                id = Int32.Parse(Console.ReadLine());
                                Console.WriteLine("Unesite broj karata: ");
                                br = Int32.Parse(Console.ReadLine());
                                proxy.NapraviRezervaciju(id, br);
                                break;

                            case 3:
                                Console.WriteLine("Unesite ID predstave: ");
                                id = Int32.Parse(Console.ReadLine());
                                proxy.PlatiRezervaciju(id);                    
                                break;

                            case 4:
                                Projekcija p;
                                Console.WriteLine("Unesite ime predstave: ");
                                ime = Console.ReadLine();
                                Console.WriteLine("Unesite datum predstave(mesec, dan): ");
                                DateTime vreme = DateTime.Now;
                                br = Int32.Parse(Console.ReadLine());
                                vreme.AddMonths(br);
                                br = Int32.Parse(Console.ReadLine());
                                vreme.AddDays(br);
                                Console.WriteLine("Unesite vreme predstave(sat, minuti): ");
                                br = Int32.Parse(Console.ReadLine());
                                vreme.AddHours(br);
                                br = Int32.Parse(Console.ReadLine());
                                vreme.AddMinutes(br);
                                Console.WriteLine("Unesite broj sale: ");
                                br = Int32.Parse(Console.ReadLine());
                                Console.WriteLine("Unesite cenu karte: ");
                                double cena = Double.Parse(Console.ReadLine());
                                p = new Projekcija(ime, vreme, br, cena);
                                proxy.DodajProjekciju(p);
                                break;

                            case 5:
                                Console.WriteLine("Unesite ID predstave: ");
                                id = Int32.Parse(Console.ReadLine());
                                Console.WriteLine("Unesite ime predstave: ");
                                ime = Console.ReadLine();
                                Console.WriteLine("Unesite datum predstave(mesec, dan): ");
                                vreme = DateTime.Now;
                                br = Int32.Parse(Console.ReadLine());
                                vreme.AddMonths(br);
                                br = Int32.Parse(Console.ReadLine());
                                vreme.AddDays(br);
                                Console.WriteLine("Unesite vreme predstave(sat, minuti): ");
                                br = Int32.Parse(Console.ReadLine());
                                vreme.AddHours(br);
                                br = Int32.Parse(Console.ReadLine());
                                vreme.AddMinutes(br);
                                Console.WriteLine("Unesite broj sale: ");
                                br = Int32.Parse(Console.ReadLine());
                                Console.WriteLine("Unesite cenu karte: ");
                                cena = Double.Parse(Console.ReadLine());
                                p = new Projekcija(ime, vreme, br, cena);
                                proxy.IzmeniProjekciju(p);
                                break;

                            case 6:
                                Console.WriteLine("Unesite popust(procenat): ");
                                iznos = Double.Parse(Console.ReadLine());
                                proxy.IzmeniPopust(iznos);
                                break;

                            default:
                                Console.WriteLine("Uneli ste pogresnu komandu!\r\n");
                                break;
                        }
                    }
                }
            }
        }
    }
}
