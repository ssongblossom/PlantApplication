using System;

namespace MqttClientRepositoryLib
{
    public class MqttClientRepository
    {
        Dictionary<string, TopicFilter> _topicFilter;

        private static IMqttClient client;

        public IMqttClient Create(string server, int? port, string userName, string password, List<string> topics)
        {
            _topicFilter = new Dictionary<string, TopicFilter>();

            foreach (var topic in topics)
            {
                TopicFilter topicFilter = new TopicFilter
                {
                    Topic = topic,
                    QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce
                };

                _topicFilter.Add(topic, topicFilter);
            }

            Task.Run(() => MqttClientRunAsync(server, port, userName, password)).Wait();

            return client;
        }

        private async Task MqttClientRunAsync(string server, int? port, string userName, string password)
        {
            try
            {
                var options = new MqttClientOptions
                {
                    ClientId = "YOURCLIENTID",
                    CleanSession = true,
                    ChannelOptions = new MqttClientTcpOptions
                    {
                        Server = server,
                        Port = port
                    },
                    Credentials = new MqttClientCredentials
                    {
                        Username = userName,
                        Password = Encoding.UTF8.GetBytes(password)
                    }
                };

                var factory = new MqttFactory();

                client = factory.CreateMqttClient();

                client.ConnectedHandler = new MqttConnectedHandler(_topicFilter, client);
                client.DisconnectedHandler = new MqttDisconnectedHandler(options, client);

                try
                {
                    await client.ConnectAsync(options);
                }
                catch //(Exception ex)
                {

                }
            }
            catch //(Exception ex)
            {

            }
        }
    }

    public class MqttDisconnectedHandler : IMqttClientDisconnectedHandler
    {
        private IMqttClient _client;
        private MqttClientOptions _options;

        public MqttDisconnectedHandler(MqttClientOptions options, IMqttClient client)
        {
            _options = options;
            _client = client;
        }

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {

            }
            catch
            {

            }
        }
    }

    public class MqttConnectedHandler : IMqttClientConnectedHandler
    {
        private IMqttClient _client;
        private Dictionary<string, TopicFilter> _topicFilter;

        public MqttConnectedHandler(Dictionary<string, TopicFilter> topicFilter, IMqttClient client)
        {
            _topicFilter = topicFilter;
            _client = client;
        }

        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            await _client.SubscribeAsync(_topicFilter.Values.ToArray());
        }
    }
}
    

