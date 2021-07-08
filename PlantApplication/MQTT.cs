using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Threading.Tasks;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Text;
using PlantApplication.Models;



namespace PlantApplication
{
    public class MQTT
    {
        public ManagedMqttClientOptions options;
        

        public MQTT()
        {
            this.options = new ManagedMqttClientOptions();       
        }

        public void SetOptions(string serverAdress, int portNo)
        {
            this.options.ClientOptions = new MqttClientOptions()
            {
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = serverAdress,
                    Port = portNo
                    //Server = "ssongblossom.farmmax.co.kr",
                    //Port = 1883
                }
            };
            this.options.AutoReconnectDelay = TimeSpan.FromSeconds(1);
        }
        public async Task GetMQTTData(ArduinoData returnData)
        {
            try
            {
                var managedClient = new MqttFactory().CreateManagedMqttClient();
                managedClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                {
                    var topic = e.ApplicationMessage.Topic;
                    var payload = e.ApplicationMessage.Payload;
                    string recievedData = Encoding.Default.GetString(payload);
                    Console.WriteLine(recievedData);


                    if (String.Equals(topic, "outTopic"))
                    {
                        ArduinoData.DataProcessor(recievedData, returnData);
                    }

                });

                await managedClient.StartAsync(options);
                await managedClient.SubscribeAsync("outTopic");

                Console.WriteLine("Managed client started.");
                Console.ReadLine();
                await managedClient.UnsubscribeAsync("outTopic");
                await managedClient.StopAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }
    
}
