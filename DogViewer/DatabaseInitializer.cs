using DogDatabase;


namespace DogViewer
{
    // Used to initialize an empty sql database with randomly generated data.
    // This is used once at first run to set up a test database.

    internal static class DatabaseInitializer
    {

        public static void OnInit(DogApiClient client)
        {
            List<string> coatList = new List<string>() {"Short", "Medium", "Long", "Extra long" };
            List<string> sizeList = new List<string>() { "Tiny", "Small", "Medium", "Big", "Very big" };
            List<string> temperList = new List<string>() { "Mild", "Alert", "Protective", "Aggressive" };
            var rand = new Random();

            using (var db = new DbContextDog())
            {
                foreach (Dog dog in client.DogBreedList)
                {
                    dog.CoatLength = coatList[rand.Next(0,4)];
                    dog.AverageAge = rand.Next(5, 25);
                    dog.Size = sizeList[rand.Next(0, 5)];
                    dog.ExcersizeLevel = (byte)rand.Next(1, 6);
                    dog.Temper = temperList[rand.Next(0, 4)];
                    
                    db.Dogs.Add(dog);
                    db.SaveChanges();
                }
            }

        }

       
    }
}
