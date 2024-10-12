﻿

namespace DogViewer
{
    public partial class App : Application
    {
        private static IServiceProvider _serviceProvider;
        public static IAlertService AlertSvc;
        internal static DogApiClient Client;
        internal static List<DogDatabase.Dog> RatedDogs = new();
        internal static DogDatabase.Dog? DefaultDog = new(1, "australian", "kelpie", "Medium", "Medium", 15, "Mild", 5);

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
            //DatabaseInitializer.OnInit(Client);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Height = 900;
            window.Width = 1600;
            return window;
        }
    }
}
