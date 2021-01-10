using Common;
using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceApp
{
    public class WCFService : IWCFContract
    {
        public string RegistrujKorisnika()
        {
            if (Thread.CurrentPrincipal.IsInRole("RegistrujKorisnika"))
            {
                Audit.AuthorizationSuccess(Thread.CurrentPrincipal.Identity.Name, "RegistrujKorisnika");
                ServerDatabase.ReadData();

                foreach (Korisnik k in ServerDatabase.korisnici)
                {
                    if (k.Username == Thread.CurrentPrincipal.Identity.Name)
                    {
                        return "Korisnik uspesno pronadjen!";
                    }   
                }

                Korisnik novi = new Korisnik(Thread.CurrentPrincipal.Identity.Name, 2000);
                ServerDatabase.korisnici.Add(novi);
                ServerDatabase.UpdateData();

                return "Uspesno kreiran nov nalog za korisnika!";
            }
            else
            {
                Audit.AuthorizationFailed(Thread.CurrentPrincipal.Identity.Name, "RegistrujKorisnika", "Nisi korisnik a ni VIP!");

                throw new FaultException("Nisi korisnik a ni VIP!");
            }
        }

        public string DodajProjekciju(Projekcija projekcija)
        {
            if (Thread.CurrentPrincipal.IsInRole("DodajProjekciju"))
            {
                ServerDatabase.ReadData();

                int maxId = -1;
                foreach (var item in ServerDatabase.projekcije)
                {
                    if (item.Id > maxId)
                    {
                        maxId = item.Id;
                    }
                }
                projekcija.Id = maxId + 1;
                ServerDatabase.projekcije.Add(projekcija);
                ServerDatabase.UpdateData();
                Audit.AuthorizationSuccess(Thread.CurrentPrincipal.Identity.Name, "DodajPrestavu");


                return "Uspesno je dodata predstava.";
            }
            else
            {
                Audit.AuthorizationFailed(Thread.CurrentPrincipal.Identity.Name, "DodajPrestavu", "Nisi admin.");

                throw new FaultException("Nisi Admin!");
            }

        }

        public string IzmeniPopust(double noviPopust)
        {
            if (Thread.CurrentPrincipal.IsInRole("IzmeniPopust"))
            {
                ServerDatabase.ReadData();
                ServerDatabase.popust = noviPopust;
                ServerDatabase.UpdateData();
                Audit.AuthorizationSuccess(Thread.CurrentPrincipal.Identity.Name, "IzmeniPopust");


                return "Uspesno je promenjem popust.";
            }
            else
            {
                Audit.AuthorizationFailed(Thread.CurrentPrincipal.Identity.Name, "IzmeniPopust", "Nisi admin.");

                throw new FaultException("Nisi Admin!");
            }
        }

        public string IzmeniProjekciju(Projekcija projekcija)
        {
            if (Thread.CurrentPrincipal.IsInRole("IzmeniProjekciju"))
            {
                Audit.AuthorizationSuccess(Thread.CurrentPrincipal.Identity.Name, "DodajPrestavu");

                ServerDatabase.ReadData();

                foreach (var item in ServerDatabase.projekcije)
                {
                    if (item.Id == projekcija.Id)
                    {
                        ServerDatabase.projekcije.Remove(item);
                        ServerDatabase.projekcije.Add(projekcija);
                        ServerDatabase.UpdateData();

                        return "Uspesno izmenjena";
                    }
                }




                return "Ne postoji ta predstava.";
            }
            else
            {
                Audit.AuthorizationFailed(Thread.CurrentPrincipal.Identity.Name, "IzmeniProjekciju", "Nisi admin.");

                throw new FaultException("Nisi Admin!");
            }
        }

        public string NapraviRezervaciju(int idProjekcije, int brojKarata)
        {
            if (Thread.CurrentPrincipal.IsInRole("NapraviRezervaciju"))
            {
                Audit.AuthorizationSuccess(Thread.CurrentPrincipal.Identity.Name, "NapraviRezervaciju");

                ServerDatabase.ReadData();
                foreach (var item in ServerDatabase.korisnici)
                {
                    if (item.Username == Thread.CurrentPrincipal.Identity.Name)
                    {
                        int maxid = 0;
                        foreach (var rezervacija in item.Rezervacije)
                        {
                            if (rezervacija.Id > maxid)
                            {
                                maxid = rezervacija.Id;
                            }
                        }
                        foreach (var projekcija in ServerDatabase.projekcije)
                        {
                            if (projekcija.Id == idProjekcije)
                            {
                                item.Rezervacije.Add(new Rezervacija(maxid + 1, idProjekcije, DateTime.Now, brojKarata, StanjeRezervacije.NEPLACENA));
                                ServerDatabase.UpdateData();
                                return "Uspesno rezervisano.";

                            }
                        }
                        return "Nema predstave sa tim ID-om.";
                    }
                }








                return "Korisnik nije registrovan.";
            }
            else
            {
                Audit.AuthorizationFailed(Thread.CurrentPrincipal.Identity.Name, "NapraviRezervaciju", "Nisi korisnik, a ni VIP.");

                throw new FaultException("Nisi korisnik, a ni VIP!");
            }
        }

        public string PlatiRezervaciju(int idRezervacije)
        {
            if (Thread.CurrentPrincipal.IsInRole("PlatiRezervaciju"))
            {
                Audit.AuthorizationSuccess(Thread.CurrentPrincipal.Identity.Name, "PlatiRezervaciju");

                ServerDatabase.ReadData();
                foreach (var item in ServerDatabase.korisnici)
                {
                    if (item.Username == Thread.CurrentPrincipal.Identity.Name)
                    {

                        foreach (var rezervacija in item.Rezervacije)
                        {
                            if (rezervacija.Id == idRezervacije)
                            {
                                foreach (var projekcija in ServerDatabase.projekcije)
                                {
                                    if (projekcija.Id == rezervacija.IdProjekcije)
                                    {
                                        if (Thread.CurrentPrincipal.IsInRole("Popust"))
                                        {
                                            if (rezervacija.Stanje != StanjeRezervacije.PLACENA)
                                            {

                                                if ((projekcija.CenaKarte - ServerDatabase.popust) * rezervacija.KolicinaKarata < item.StanjeNaRacunu)
                                                {
                                                    rezervacija.Stanje = StanjeRezervacije.PLACENA;
                                                    ServerDatabase.UpdateData();
                                                    return "Uspesno placena rezervacija.";

                                                }
                                                else
                                                {
                                                    return "Nema dovoljno para na racunu";
                                                }
                                            }
                                            else
                                            {
                                                return "Rezervacija je vec placena.";
                                            }

                                        }
                                        else
                                        {
                                            if (rezervacija.Stanje != StanjeRezervacije.PLACENA)
                                            {
                                                if (projekcija.CenaKarte * rezervacija.KolicinaKarata < item.StanjeNaRacunu)
                                                {

                                                    rezervacija.Stanje = StanjeRezervacije.PLACENA;
                                                    ServerDatabase.UpdateData();
                                                    return "Uspesno placena rezervacija.";
                                                }
                                                else
                                                {
                                                    return "Nema dovoljno para na racunu";
                                                }
                                            }
                                            else
                                            {
                                                return "Rezervacija je vec placena.";
                                            }
                                        }
                                    }
                                }
                                return "Nema te projekcije.";

                            }
                        }

                        return "Nema rezervacije sa tim ID-om.";
                    }
                }








                return "Korisnik nije registrovan.";
            }
            else
            {
                Audit.AuthorizationFailed(Thread.CurrentPrincipal.Identity.Name, "PlatiRezervaciju", "Nisi korisnik a ni VIP!");

                throw new FaultException("Nisi korisnik a ni VIP!");
            }
        }


        public string UplatiPareNaRacun(double uplata)
        {
            if (Thread.CurrentPrincipal.IsInRole("UplatiPareNaRacun"))
            {
                Audit.AuthorizationSuccess(Thread.CurrentPrincipal.Identity.Name, "UplatiPareNaRacun");

                ServerDatabase.ReadData();

                foreach (var item in ServerDatabase.korisnici)
                {
                    if (item.Username == Thread.CurrentPrincipal.Identity.Name)
                    {
                        item.StanjeNaRacunu += uplata;
                        ServerDatabase.UpdateData();
                        return "Uspesno uplacen novan na racun.";
                    }
                }

                return "Korisnik nije registrovan.";
            }
            else
            {
                Audit.AuthorizationFailed(Thread.CurrentPrincipal.Identity.Name, "UplatiPareNaRacun", "Nisi korisnik a ni VIP!");

                throw new FaultException("Nisi korisnik a ni VIP!");
            }
        }
    }
}
