using Azure;

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
            DogPhotoImg.Source = response;
            SetBreedName(response);
        }

        private async void OnRandomPicClicked(object sender, EventArgs e)
        {
            string response = await App.Client.AsyncFetchRandomImage();
            DogPhotoImg.Source = response;
            SetBreedName(response);
        }

        private async void OnBreedPicClicked(object sender, EventArgs e)
        {
            if (BreedEntry.Text != null)
            {
                string response = await App.Client.AsyncFetchBreedImage(BreedEntry.Text.ToLower());
                DogPhotoImg.Source = response;
                SetBreedName(response); 
            }
        }

        private void SetBreedName(string response)
        {
            string[] breed = response.Split('/');
            lblImgBreedName.Text = breed[^2];
        }

        private void NavigateToDatabasePage(object sender, TappedEventArgs e)
        {
            Shell.Current.GoToAsync($"///DataBasePage?selected={lblImgBreedName.Text}");
        }
    }

}
