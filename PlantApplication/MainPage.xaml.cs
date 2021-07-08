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



        async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlantPage());
        }

        private void Start_Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
