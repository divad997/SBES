using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    [DataContract]
    public class Korisnik
    {
        [DataMember]
        private string username;

        [DataMember]
        private double stanjeNaRacunu;

        [DataMember]
        private List<Rezervacija> rezervacije;

        public string Username { get => username; set => username = value; }
        public double StanjeNaRacunu { get => stanjeNaRacunu; set => stanjeNaRacunu = value; }
        public List<Rezervacija> Rezervacije { get => rezervacije; set => rezervacije = value; }

        public Korisnik(string username, double stanjeNaRacunu)
        {
            this.username = username;
            this.stanjeNaRacunu = stanjeNaRacunu;
            this.rezervacije = new List<Rezervacija>();
        }

        
    }
}
