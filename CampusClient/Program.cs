using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;

namespace CampusClient
{
    class CampusClient
    {
        static void Main(string[] args)
        {
            try
            {
                ConnectionFactory cf = new ConnectionFactory();
                cf.Uri = "amqps://<user>:<pass>@<server>:5671/";
                cf.Ssl.CertPath = "C:\\keycert.p12";
                cf.Ssl.CertPassphrase = "<ClientCertPass>";
                cf.Ssl.Enabled = true;

                using (IConnection conn = cf.CreateConnection())
                {
                    Console.WriteLine("Connection established");
                    using (IModel ch = conn.CreateModel())
                    {                        
                        ch.BasicQos(0, 1, false);                                   // noAck
                        Subscription sub = new Subscription(ch, "QueueName", true); // last param MUST BE true
                        while (true)
                        {
                            BasicDeliverEventArgs e = sub.Next();
                            byte[] body = e.Body;
                            Console.WriteLine(Encoding.UTF8.GetString(body));
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}