using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Receiving;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlantApplication
{
    public class MqttTaskService
    {
        private IMqttClient _mqttClient;
        private readonly string _sessionPayedTopic;
        List<string> topics = new List<string>();

        public MqttTaskService() 
        {
            _sessionPayedTopic = "outTopic";
            topics.Add("outTopic");

            _mqttClient = new MqttClientRepository().Create(
             "152.70.39.197",
             1883,
             //9001,
             "song", "song!", 
             topics // List
             );

            _mqttClient.ApplicationMessageReceivedHandler = new SubscribeCallback(_sessionPayedTopic);
        }

        public void UnSubscribe()
        {
            _mqttClient.ApplicationMessageReceivedHandler = null;
        }
    }

    public class SubscribeCallback : IMqttApplicationMessageReceivedHandler
    {
        private readonly string _sessionPayedTopic;

        public SubscribeCallback(string sessionPayedTopic)
        {
            _sessionPayedTopic = sessionPayedTopic;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            //MessagingCenter.Send("SessionPayed", "SessionPayed", message);

            if (e.ApplicationMessage.Topic == _sessionPayedTopic)
            {
                MessagingCenter.Send("SessionPayed", "SessionPayed", message);
            }

            return Task.CompletedTask;
        }
    }
}
