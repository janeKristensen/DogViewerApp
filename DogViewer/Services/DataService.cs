using DogDatabase;
using Microsoft.Data.SqlClient;


namespace DogViewer.Services
{
    internal class DataService 
    {
        private DogApiClient _client;
        private DbContextDog _dbContext;

        public DataService(DogApiClient client, DbContextDog context) 
        {
            _client = client;
            _dbContext = context;
        }

        public async Task<List<Dog>> GetBreedList()
        {
            var breedList = await _client.GetBreedsList();
            if (breedList == null)
                breedList = new() { App.DefaultDog };
            return breedList;
        }

        public async Task<List<Dog>> SearchBreedList(string searchInput)
        {
            var list = await GetBreedList();
            List<Dog> results = list.FindAll(
                x => x.BreedName.StartsWith(searchInput) ||
                x.SubBreed.StartsWith(searchInput));

            return results;
        }

        public Dog FindSelected(Dog selection)
        {
            Dog selectedDog = new();
            try
            {
                selectedDog = _dbContext.Dogs.Where(
                    d => d.BreedName == selection.BreedName &&
                    d.SubBreed == selection.SubBreed).First();    
            }
            catch (SqlException ex)
            {
                selectedDog = App.DefaultDog;
                App.AlertService.Alert("Connection error", "Could not connect to the database.");
            }

            return selectedDog;
        }

        public List<Dog> SelectTopDogs(int number)
        {
           return _dbContext.Dogs.OrderByDescending(d => d.Stars).Take(number).ToList();
        }

        public Decimal UpdateRating(int value, Dog selectedDog)
        {
            //App keeps a list of already rated dogs
            var app = Application.Current as App;

            selectedDog.AddRating(value);

            try
            {
                var dog = _dbContext.Dogs.Where(
                d => d.BreedName == selectedDog.BreedName &&
                d.SubBreed == selectedDog.SubBreed).First();
                if (dog != null)
                {
                    dog.Score += selectedDog.Score;
                    dog.Ratings += selectedDog.Ratings;
                    dog.Stars = (Decimal)selectedDog.GetRating();
                    _dbContext.SaveChanges();
                    app.RatedDogs.Add(dog);
                }
                return dog.GetRating();
                
            }
            catch (Exception ex) when (ex is SqlException || ex is NullReferenceException)
            {
                if (!app.RatedDogs.Contains(App.DefaultDog))
                {
                    App.DefaultDog.AddRating(value);
                    app.RatedDogs.Add(App.DefaultDog);
                }
                return App.DefaultDog.GetRating();
                
            }
            catch (Exception ex)
            {
                App.AlertService.Alert("Unknown error", "An unknown error occured. Please try again.");
                return 0;
            }
        }
    }
}