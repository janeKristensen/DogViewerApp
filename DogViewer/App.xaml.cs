

using DogDatabase;
using DogViewer.Services;
using System.Linq.Expressions;
using System.Reflection;

namespace DogViewer
{
    public partial class App : Application
    {
        internal static AlertService AlertService;
        internal static DogApiClient Client;
        internal static DatabaseService Data;
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
            Data = provider.GetService<DatabaseService>();
            AlertService = provider.GetService<AlertService>();
            Client = provider.GetService<DogApiClient>();

            Utils.SetUpAlerts();
            LoadData();
        }

        private async void LoadData()
        {
            RatedDogs = new();
            DefaultDog = new(1, "australian", "kelpie", "Medium", "Medium", 15, "Mild", 5);

            //First time population of data in empty sql database:
            //DatabaseInitializer.OnInit(Client); 
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Height = 900;
            window.Width = 1400;
            window.X = 50;
            window.Y = 50;
            return window;
        }
    }
}
