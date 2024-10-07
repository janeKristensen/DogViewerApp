namespace DogViewer
{
    public partial class MainPage : ContentPage
    {
        public MainPage() 
        {
            InitializeComponent();
        }  

        private async void OnRandomPicClicked(object sender, EventArgs e)
        {
            DogPhotoImg.Source = await App.Client.AsyncFetchRandomImage(); 
        }

        private async void OnBreedPicClicked(object sender, EventArgs e)
        {
            if(BreedEntry.Text != null)
                DogPhotoImg.Source = await App.Client.AsyncFetchBreedImage(BreedEntry.Text.ToLower());
        }
    }

}
