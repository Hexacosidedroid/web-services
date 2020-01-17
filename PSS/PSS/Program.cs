using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography;

namespace PSS
{
    [ServiceContract(Namespace = "http://rostov-tfoms.ru/pss/", Name = "PSS")]
    public interface IPacientCard
    {
        [OperationContract]
        string GetToTheChopper(string FN, string SN, string TN, DateTime BD);
        [OperationContract]
        string SayHelloToMyLittleFriend(string exception);
    }

    public class PacientCard : IPacientCard
    {
        public string GetToTheChopper(string FN, string SN, string TN, DateTime BD)
        {
            string pacient;
            string GenerateHash(string str)  // Участок кода отвечающий за преобразование в 16-и значное число 
            {
                using (var md5Hasher = MD5.Create())  // При помощи md5 образуется хэш функция, от которой мы берём только первые 16 цифр
                {
                    var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
                    return BitConverter.ToString(data).Replace("-", "").Substring(0, 16);
                }
            }
            pacient = FN + " " + SN + " " + TN + " " + BD;
            return GenerateHash(pacient);  //возвращает string
        }
        public string SayHelloToMyLittleFriend(string exception)
        {
            return exception;
            throw new NotImplementedException(); // Выбивает приложение и сообщает исключение с пришедшей к нему строкой   
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:12345/Pacient_card");
            using (ServiceHost host = new ServiceHost(typeof(PacientCard), baseAddress))
            try
                {
                    host.AddServiceEndpoint(typeof(IPacientCard), new WSHttpBinding(), "PacientCard");
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
