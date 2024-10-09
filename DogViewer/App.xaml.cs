

using DogDatabase;

namespace DogViewer
{
    public partial class App : Application
    {
        private static IServiceProvider _serviceProvider;
        public static IAlertService AlertSvc;
        internal static DogApiClient Client;

        public App(IServiceProvider provider)
        {
            InitializeComponent();
            MainPage = new AppShell();
            _serviceProvider = provider;
            AlertSvc = _serviceProvider.GetService<IAlertService>();
            if (AlertSvc is null)
                throw new NullReferenceException();
            Client = _serviceProvider.GetService<DogApiClient>();
            if (Client is null)
                throw new NullReferenceException();
            Load(); 
        }

        private async void Load()
        {
            await Client.GetBreedsList();
            /*using (var db = new DbContextDog())
            {
                foreach (Dog dog in Client.DogBreedList)
                {
                    db.Dogs.Add(dog);
                    db.SaveChanges();
                }
            }*/
        }
    }
}
