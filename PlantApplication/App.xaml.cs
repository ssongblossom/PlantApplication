using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PlantApplication;

namespace PlantApplication
{
    public partial class App : Application
    {


        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            MqttTaskService mqttTaskService = new MqttTaskService();
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
