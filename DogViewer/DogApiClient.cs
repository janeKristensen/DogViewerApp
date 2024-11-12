
using System.Text.Json;
using System.Text.Json.Serialization;
using DogDatabase;



namespace DogViewer
{
    public class ApiClient
    {
        public virtual async Task<T?> GetResponse<T>(Uri uri)
        {
            using (HttpClient client = new())
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(responseString);
                }
            }

            return default; 
        }
    }


    internal class ImageResponse
    {
        [JsonPropertyName("message")]
        public string? message { get; set; }
    }


    internal class BreedResponse
    {
        [JsonPropertyName("message")]
        public Dictionary<string, List<string>>? Breeds { get; set; }
    }


    public class DogApiClient : ApiClient
    {
        private ImageResponse? _dogImage;
        private BreedResponse? _breedsResponse;
        public List<Dog>? DogBreedList { get; private set; }

        public DogApiClient()
        {
            DogBreedList = new List<Dog>();
            _breedsResponse = new BreedResponse();
        }

        public void AddToBreedsList(Dog dog)
        {
            DogBreedList.Add(dog);
        }

        public async Task<bool> GetBreedsList()
        {
            Uri uri = new Uri(@"https://dog.ceo/api/breeds/list/all");
            _breedsResponse = await GetResponse<BreedResponse>(uri);
            if (_breedsResponse == null)
            {
                return false;
            }
            else
            {
                SetDogBreedList();
                return true;
            }  
        }

        private async void SetDogBreedList()
        {
            foreach (var kvp in _breedsResponse.Breeds)
            {
                if (kvp.Value.Count == 0)
                {
                    DogBreedList.Add(new Dog(kvp.Key));
                }
                else
                {
                    foreach (string type in kvp.Value)
                    {
                        DogBreedList.Add(new Dog(kvp.Key, type));
                    }
                }
            }
        }

        public async Task<string> AsyncFetchRandomImage()
        {
            Uri uri = new Uri(@"https://dog.ceo/api/breeds/image/random");
            _dogImage = await GetResponse<ImageResponse>(uri);
            return _dogImage.message;
        }  

        public async Task<string> AsyncFetchBreedImage(string breed, string subbreed="")
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            string input = breed.Replace(" ", "") + subbreed;
            var dog = DogBreedList.FindAll(
                x => string.Concat(x.BreedName, x.SubBreed).StartsWith(input) 
                || string.Concat(x.SubBreed, x.BreedName).StartsWith(input));
            
            if (dog.Count() != 0) 
            {
                int randomIndex = rand.Next(0, dog.Count);
                string url = string.Empty;

                if (dog[randomIndex].SubBreed == "")
                    url = $"https://dog.ceo/api/breed/{dog[randomIndex].BreedName}/images/random";
                else
                    url = $"https://dog.ceo/api/breed/{dog[randomIndex].BreedName}/{dog[randomIndex].SubBreed}/images/random";

                Uri uri = new Uri(url);
                _dogImage = await GetResponse<ImageResponse>(uri);
                return _dogImage.message;
            }

            App.AlertService.Alert("Image not found!", "The selected dog breed does not exist in the database. Please try again.", new bool[] { false, true, false});
            return "default";
        }
    }
}
