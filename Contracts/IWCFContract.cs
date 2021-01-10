using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IWCFContract
    {
        [OperationContract]
        string RegistrujKorisnika();

        [OperationContract]
        string DodajProjekciju(Projekcija projekcija);

        [OperationContract]
        string IzmeniProjekciju(Projekcija projekcija);

        [OperationContract]
        string IzmeniPopust(double noviPopust);

        [OperationContract]
        string UplatiPareNaRacun(double uplata);

        [OperationContract]
        string NapraviRezervaciju(int idPredstave, int brojKarata);

        [OperationContract]
        string PlatiRezervaciju(int idRezervacije);
    }
}
