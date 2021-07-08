using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.IO;
using System.Net.Http;

namespace PlantApplication
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlantPage : ContentPage
    {

        string PhotoPath=null;
        string url = "http://ssongblossom.farmmax.co.kr/api/";

        public PlantPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_To_Server(object sender, EventArgs e)
        {
            //No file
            if(PhotoPath == null)
            {
                return;
            }

            try
            {
                var upfilebytes = File.ReadAllBytes(PhotoPath);
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(upfilebytes);

                content.Add(baContent, "File", "filename.ext");

                var response = await client.PostAsync(url, content);
                var responsestr = response.Content.ReadAsStringAsync().Result;

                textPath.Text = responsestr;
            }catch(Exception ex)
            {
                textPath.Text = ex.Message;
                return;
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await TakePhotoAsync();
            imageView.Source = PhotoPath;


        }

        async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
                textPath.Text = PhotoPath;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is now supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }


        
    }
}