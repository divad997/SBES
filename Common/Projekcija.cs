using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [Serializable]
    [DataContract]
    public class Projekcija
    {
        [DataMember]
        private int id;
       
        [DataMember]
        private string naziv;

        [DataMember]
        private DateTime vremeProjekcije;

        [DataMember]
        private int sala;

        [DataMember]
        private double cenaKarte;

        public int Id { get => id; set => id = value; }
        public string Naziv { get => naziv; set => naziv = value; }
        public DateTime VremeProjekcije { get => vremeProjekcije; set => vremeProjekcije = value; }
        public int Sala { get => sala; set => sala = value; }
        public double CenaKarte { get => cenaKarte; set => cenaKarte = value; }

        public Projekcija(string naziv, DateTime vremeProjekcije, int sala, double cenaKarte)
        {
            this.Id = -1;
            this.naziv = naziv;
            this.vremeProjekcije = vremeProjekcije;
            this.sala = sala;
            this.cenaKarte = cenaKarte;
        }

        
    }
}
