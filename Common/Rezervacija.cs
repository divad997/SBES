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
    public class Rezervacija
    {
        [DataMember]
        private int id;

        [DataMember]
        private int idProjekcije;

        [DataMember]
        private DateTime vremeRezervacije;

        [DataMember]
        private int kolicinaKarata;

        [DataMember]
        private StanjeRezervacije stanje;

        public int Id { get => id; set => id = value; }
        public int IdProjekcije { get => idProjekcije; set => idProjekcije = value; }
        public DateTime VremeRezervacije { get => vremeRezervacije; set => vremeRezervacije = value; }
        public int KolicinaKarata { get => kolicinaKarata; set => kolicinaKarata = value; }
        public StanjeRezervacije Stanje { get => stanje; set => stanje = value; }

        public Rezervacija(int id, int idProjekcije, DateTime vremeRezervacije, int kolicinaKarata, StanjeRezervacije stanje)
        {
            this.id = id;
            this.idProjekcije = idProjekcije;
            this.vremeRezervacije = vremeRezervacije;
            this.kolicinaKarata = kolicinaKarata;
            this.stanje = stanje;
        }

       
    }
}
