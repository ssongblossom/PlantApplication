using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PlantApplication.Models;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;



namespace PlantApplication
{
    
    public partial class MainPage : ContentPage
    {

        String serverAddress = "ssongblossom.farmmax.co.kr";
        int portNo = 1883;
        ArduinoData datas;
        string recievedData;



        public MainPage() 
        {
            InitializeComponent();
            //Application.Current.PageAppearing += async (s, e) => await OnPageAppearing(s,e);
            MqttTaskService mqttTaskService = new MqttTaskService();

            MessagingCenter.Subscribe<string, string>("SessionPayed", "SessionPayed", (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SetText(args); 
                });
            });
        }

        public void SetText(string args)
        {
            if (args.Contains("DHT11"))
                tempText.Text = "";
            tempText.Text += args;
        }
        
        
        
        protected override void OnAppearing()
        {

        }

       

            private void Button_Clicked(object sender, EventArgs e)
        {
            //tempText.Text = $"{datas.NH4}";
        }
        /*

        public async Task GetMQTTData()
        {
            ManagedMqttClientOptions options = new ManagedMqttClientOptions();
            options.ClientOptions = new MqttClientOptions()
            {
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = "ssongblossom.farmmax.co.kr",
                    Port = 1883
                }
            };
            

            //options.AutoReconnectDelay = TimeSpan.FromSeconds(1);
            var managedClient = new MqttFactory().CreateManagedMqttClient();

            try
            {
                managedClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                {
                    tempText.Text = "eventHandler";
                    Console.WriteLine("eventHandler");
                    var topic = e.ApplicationMessage.Topic;
                    var payload = e.ApplicationMessage.Payload;
                    recievedData = Encoding.UTF8.GetString(payload);
                    Console.WriteLine(recievedData);
                    tempText.Text = recievedData;

                    if (String.Equals(topic, "outTopic"))
                    {
                        ArduinoData.DataProcessor(recievedData, datas);
                        tempText.Text = "dataprocessing";
                        Console.WriteLine("dataprocessing");
                    }

                });

                Device.BeginInvokeOnMainThread(() => {
                    tempText.Text = recievedData;
                });

                await managedClient.StartAsync(options);
                await managedClient.SubscribeAsync("outTopic");
                //tempText.Text = "subscribe";

                await managedClient.UnsubscribeAsync("outTopic");
                await managedClient.StopAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        */
    }
}
