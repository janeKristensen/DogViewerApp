

using DogDatabase;
using System.Linq.Expressions;
using System.Reflection;

namespace DogViewer
{
    public partial class App : Application
    {
        private static IServiceProvider _serviceProvider;
        internal static AlertService AlertService;
        internal static DogApiClient Client;
        internal static DbContextDog DogContext;

        // List of dogs that has received a rating by user in this instance of app.
        public List<Dog> RatedDogs { get; private set; }

        // Default dog will be displayed when there is no connection to sql database
        internal static Dog DefaultDog;

        public App(IServiceProvider provider)
        {
            InitializeComponent();

            _serviceProvider = provider;
            DogContext = _serviceProvider.GetService<DbContextDog>();

            RatedDogs = new();
            DefaultDog = new(1, "australian", "kelpie", "Medium", "Medium", 15, "Mild", 5);

            MainPage = new AppShell();
            AlertService = _serviceProvider.GetService<AlertService>();

            // Assign methods to delegate methods
            AlertService.alert += DisplayAlert;
            AlertService.alert += LogAlert;
            AlertService.alertConfirm += DisplayAlertConfirmation;

            try
            {
                Client = _serviceProvider.GetService<DogApiClient>();
                if (Client is null)
                    throw new NullReferenceException("Could not connect to Dog API.");

                Load();
            }
            catch (NullReferenceException ex) 
            {
                AlertService.Alert("A critical error occurred", "Could not connect to API", new bool[] { true, false, false });
            }   
        }

        private async void Load()
        {
            if(!await Client.GetBreedsList())
                Client.AddToBreedsList(DefaultDog);

            //First time population of data in empty sql database:
            //DatabaseInitializer.OnInit(Client); 
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Height = 900;
            window.Width = 1600;
            return window;
        }


        #region Error notification methods

        // Pop up alert on main screen
        public static Task DisplayAlert(string title, string message) => 
            Current.MainPage.DisplayAlert(title, message, "OK");

            
        // Log alert in log file
        public static Task LogAlert(string title, string message)
        {
            string path = System.AppContext.BaseDirectory;
            string file = Path.Combine(path, "log.txt");
            using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine($"{System.DateTime.UtcNow} - Error: {title}, {message}");
            }
            return Task.CompletedTask;  

        }

        // Pop alert with confirmation button and log to file
        public static Task<bool> DisplayAlertConfirmation(string title, string message)
        {
            LogAlert(title, message);   
            return Current.MainPage.DisplayAlert(title, message, "OK", "Cancel");
        }

        #endregion
    }
}
