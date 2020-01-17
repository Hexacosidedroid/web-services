using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace PSP
{
    [ServiceContract(Namespace = "http://rostov-tfoms.ru/psp/", Name = "PSP")]
    public interface IAreYouWaitingMe
    {
        [OperationContract]
        int IllBeBack(int chislo);
        [OperationContract]
        string ILoveTheSmellOfNapalmInTheMorning(string your);
    }

    public class AreYouWaitingMe : IAreYouWaitingMe
    {
        public int IllBeBack(int chislo)
        {
            chislo = chislo * 2;
            Thread.Sleep(5000);
            return chislo;
        }
        public string ILoveTheSmellOfNapalmInTheMorning(string your)
        {
            string GenRandomString(string Alphabet, int Length)
            {
                Random rnd = new Random();
                StringBuilder sb = new StringBuilder(Length - 1);
                int Position = 0;

                for (int i = 0; i < Length; i++)
                {
                    Position = rnd.Next(0, Alphabet.Length - 1);
                    sb.Append(Alphabet[Position]);
                }             
                return sb.ToString();
            }
            int a;
            string FN;
            string SN;
            string TN;
            DateTime BD;
            a = your.Length;
            if (a == 16)
            {
                FN = GenRandomString("ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮйцукенгшщзхъфывапролджэячсмитьбю", 10);
                SN = GenRandomString("ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮйцукенгшщзхъфывапролджэячсмитьбю", 10);
                TN = GenRandomString("ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮйцукенгшщзхъфывапролджэячсмитьбю", 10);
                BD = DateTime.Now;
            }
            else
            {
                string excep = "Repeat please :D";
                return excep;
            }
            string pacient = FN + " " + SN + " " + TN + " " + BD;
            return pacient; 
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:12345/Card_pacient");
            using (ServiceHost host = new ServiceHost(typeof(AreYouWaitingMe), baseAddress))
                try
                {
                    host.AddServiceEndpoint(typeof(IAreYouWaitingMe), new WSHttpBinding(), "AreYouWaitingMe");
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                    host.Description.Behaviors.Add(smb);
                    host.Open();
                    Console.WriteLine("The service is ready at {0}", baseAddress);
                    Console.WriteLine("Press <Enter> to stop the service.");
                    Console.WriteLine();
                    Console.ReadLine();
                    host.Close();
                }
                catch (CommunicationException ce)
                {
                    Console.WriteLine("An exception occurred: {0}", ce.Message);
                    host.Abort();
                }
        }
    }
}

