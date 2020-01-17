using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RIR
{
    // Создание контракта службы
    [ServiceContract(Namespace = "http://rostov-tfoms.ru/rir", Name = "RIR")]
    public interface IHelloWorldService
    {
        [OperationContract]
        string SayHello();
    }

    public class HelloWorldService : IHelloWorldService
    {
        public string SayHello()
        {
            return string.Format("Hello World!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Объявление адреса и порта на котором будет запущена служба
            Uri baseAddress = new Uri("http://localhost:12345/hello_world"); 
            // Создаём хост службы
            using (ServiceHost host = new ServiceHost(typeof(HelloWorldService), baseAddress))
            try
                {
                    host.AddServiceEndpoint(typeof(IHelloWorldService), new WSHttpBinding(), "Hello World");
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
