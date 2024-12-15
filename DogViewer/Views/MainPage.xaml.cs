using Azure;
using System.Text.RegularExpressions;

namespace DogViewer
{
    public partial class MainPage : ContentPage
    {
        public MainPage() 
        {
            InitializeComponent();
            Load();
        }  

        private async void Load()
        {
            string response = await App.Client.AsyncFetchRandomImage();
            SetMainPageDetails(response);
        }


        private async void OnRandomPicClicked(object sender, EventArgs e)
        {
            string response = await App.Client.AsyncFetchRandomImage();
            SetMainPageDetails(response);
        }

        private async void OnBreedPicClicked(object sender, EventArgs e)
        {
            if (BreedEntry.Text != null)
            {
                if (Regex.IsMatch(BreedEntry.Text, @"^[a-zA-Z]+\-?\s?[a-zA-Z]*$"))
                {
                    string response = await App.Client.AsyncFetchBreedImage(BreedEntry.Text.ToLower());
                    SetMainPageDetails(response);
                }
                else
                {
                    App.AlertService.Alert("Entry is not valid.", $"Only characters a-z are allowed.");
                }
                BreedEntry.Text = string.Empty;
            }
        }

        private void SetMainPageDetails(string response)
        {
            if (response == "default")
            {
                DogPhotoImg.Source = "default_dogs.png";
                lblImgBreedName.Text = string.Empty;
            }
            else
            {
                DogPhotoImg.Source = response;
                string[] breed = response.Split('/');
                lblImgBreedName.Text = breed[^2];
            } 
        }

        private void NavigateToDatabasePage(object sender, TappedEventArgs e)
        {
            Shell.Current.GoToAsync($"///DataBasePage?selected={lblImgBreedName.Text}");
        }
    }
}
