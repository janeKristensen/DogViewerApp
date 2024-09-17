namespace DogViewer
{
    public partial class MainPage : ContentPage
    {
        DogApiClient client = new();

        public MainPage() 
        {
            InitializeComponent();
            Load();
        }  

        private async void Load()
        {
           bool success = await client.SetBreedsList();
        }

        private async void OnRandomPicClicked(object sender, EventArgs e)
        {
            DogPhotoImg.Source = await client.AsyncFetchRandomImage(); 
        }

        private async void OnBreedPicClicked(object sender, EventArgs e)
        {
            if(BreedEntry.Text != null)
                DogPhotoImg.Source = await client.AsyncFetchBreedImage(BreedEntry.Text.ToLower());
        }
    }

}
