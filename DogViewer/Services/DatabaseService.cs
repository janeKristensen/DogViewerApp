using DogDatabase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogViewer.Services
{
    internal class DatabaseService
    {
        public async Task<List<Dog>> FindInDatabase(string serachInput)
        {
            var list = await App.Client.GetBreedsList();
            List<Dog> results = list.FindAll(
            x => x.BreedName.StartsWith(serachInput) ||
            x.SubBreed.StartsWith(serachInput));

            return results;
        }

        public Dog FindSelected(Dog selection)
        {
            var selectedDog = selection;
            try
            {
                selectedDog = App.DogContext.Dogs.Where(
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
            return App.DogContext.Dogs.OrderByDescending(d => d.Stars).Take(number).ToList();
        }

        public Dog UpdateRating(int value, Dog selectedDog)
        {
            selectedDog.AddRating(value);
            
            //App keeps a list of already rated dogs
            var app = Application.Current as App;
            Dog dog = new Dog();

            try
            {
                dog = App.DogContext.Dogs.Where(
                    d => d.BreedName == selectedDog.BreedName &&
                    d.SubBreed == selectedDog.SubBreed).First();
                if (dog != null)
                {
                    dog.Score += selectedDog.Score;
                    dog.Ratings += selectedDog.Ratings;
                    dog.Stars = (Decimal)selectedDog.GetRating();
                    App.DogContext.SaveChanges();
                    app.RatedDogs.Add(dog);
                }
            }
            catch (SqlException ex)
            {
                App.DefaultDog.AddRating(value);
                app = Application.Current as App;
                app.RatedDogs.Add(App.DefaultDog);
            }

            return dog;
        }
    }
}