using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp
{
    public static class ServerDatabase
    {
        public static List<Projekcija> projekcije = new List<Projekcija>();
        public static List<Korisnik> korisnici = new List<Korisnik>();
        public static double popust = 0;
        public static string serializationFileProjekcije = @".\ResourcesProjekcije";
        public static string serializationFileKorisnici = @".\ResourcesKorisnici";
        public static string serializationFilePopust = @".\ResourcesPopust";

        public static void ReadData()
        {
            try
            {
                using (Stream stream = File.Open(serializationFileProjekcije, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    projekcije = (List<Projekcija>)bformatter.Deserialize(stream);
                }
            }
            catch (Exception exc)
            {
                projekcije = new List<Projekcija>();
            }



            try
            {
                using (Stream stream = File.Open(serializationFileKorisnici, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    korisnici = (List<Korisnik>)bformatter.Deserialize(stream);
                }
            }
            catch (Exception exc)
            {
                korisnici = new List<Korisnik>();
            }

            try
            {
                using (Stream stream = File.Open(serializationFilePopust, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    popust = (double)bformatter.Deserialize(stream);
                }
            }
            catch (Exception exc)
            {
                popust = 0;
            }
        }

        public static void UpdateData()
        {

            using (Stream stream = File.Open(serializationFileProjekcije, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, projekcije);
            }



            using (Stream stream = File.Open(serializationFileKorisnici, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, korisnici);
            }
            using (Stream stream = File.Open(serializationFilePopust, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, popust);
            }

        }
    }
}
