
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using DogDatabase;



namespace DogViewer.Services
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

        public List<Dog> DictToList()
        {
            List<Dog> dogBreedList = new();
            foreach (var kvp in Breeds)
            {
                if (kvp.Value.Count == 0)
                    dogBreedList.Add(new Dog(kvp.Key));
                else
                {
                    foreach (string type in kvp.Value)
                    {
                        dogBreedList.Add(new Dog(kvp.Key, type));
                    }
                }
            }

            return dogBreedList;
        }
    }


    public class DogApiClient : ApiClient
    {
        public async Task<List<Dog>> GetBreedsList()
        {
            Uri uri = new Uri(@"https://dog.ceo/api/breeds/list/all");
            var _breedResponse = await GetResponse<BreedResponse>(uri);
            return _breedResponse.DictToList();
        }

        public async Task<string> AsyncFetchRandomImage()
        {
            Uri uri = new Uri(@"https://dog.ceo/api/breeds/image/random");
            var _dogImage = await GetResponse<ImageResponse>(uri);
            return _dogImage.message;
        }

        public async Task<string> AsyncFetchBreedImage(string breed, string subbreed = "")
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            string input = breed.Replace(" ", "").Replace("-", "") + subbreed;
            var list = await GetBreedsList();
            var dog = list.FindAll(
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
                var _dogImage = await GetResponse<ImageResponse>(uri);
                return _dogImage.message;
            }

            App.AlertService.Alert("Image not found!", "The selected dog breed does not exist in the database. Please try again.");
            return "default";
        }
    }
}
