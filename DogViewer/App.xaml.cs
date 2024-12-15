

using DogDatabase;
using DogViewer.Services;


namespace DogViewer
{
    public partial class App : Application
    {
        internal static AlertService AlertService;
        internal static DogApiClient Client;
        internal static DataService Data;
        internal static DbContextDog DogContext;

        // List of dogs that has received a rating by user in this instance of app.
        public List<Dog> RatedDogs;

        // Default dog will be displayed when there is no connection to sql database
        internal static Dog DefaultDog;

        public App(IServiceProvider provider)
        {
            InitializeComponent();
            MainPage = new AppShell();

            DogContext = provider.GetService<DbContextDog>();
            AlertService = provider.GetService<AlertService>();
            Client = provider.GetService<DogApiClient>();
            Data = new DataService(Client, DogContext);
            Utils.SetUpAlerts();
            LoadData();
        }

        private async void LoadData()
        {
            RatedDogs = new();
            DefaultDog = new(1, "australian", "kelpie", "Medium", "Medium", 15, "Mild", 5);
        }

    }
}
