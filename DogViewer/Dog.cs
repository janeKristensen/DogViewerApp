using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace DogViewer
{
    internal class ImageResponse
    {
        public string? message {  get; set; }
    }

    internal class BreedResponse
    {
        [JsonPropertyName("message")]
        public Dictionary<string, List<string>>? Breeds { get; set; }
        public List<Dog> _dogBreedsList = new ();
        
        public List<Dog> GetDogBreedList() 
        { 
            foreach(var kvp in Breeds)
            {
                if (kvp.Value.Count == 0)
                {
                    _dogBreedsList.Add(new Dog(kvp.Key));
                }
                else
                {
                    foreach (string type in kvp.Value)
                    {
                        _dogBreedsList.Add(new Dog(kvp.Key, type));
                    }
                }
            }
            return _dogBreedsList;
        }
    }


    internal class Dog
    {
        private int _excersizeLevel;

        public Dog(string breed) { BreedName = breed; }

        public Dog(string breed, string subbreed) 
        { 
            BreedName = breed; 
            Subbreed = subbreed;
        }

        public Dog(string breed, string subbreed, string coatLength, string size, double avgAge, string temper, int excersize) 
        {
            BreedName = breed;
            Subbreed = subbreed;
            CoatLength = coatLength;    
            Size = size;
            AverageAge = avgAge;
            Temper = temper;
            ExcersizeLevel = excersize;
        }

        public string BreedName { get; set; }
        public string Subbreed { get; set; } = string.Empty;
        public string CoatLength { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public double AverageAge { get; set; } = 0;
        public string Temper { get; set; } = string.Empty;
        public int ExcersizeLevel 
        {
            get { return _excersizeLevel; }
            set 
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("Excersize level must be a value between 1 and 5.");

                _excersizeLevel = value;
            }
        }
    }
}
