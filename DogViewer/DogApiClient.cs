using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;


namespace DogViewer
{
    internal class DogApiClient
    {
        private  ImageResponse? _dogImage;
        private BreedResponse? _breedsResponse;
        private List<Dog>? _dogBreedList;

        public async Task<bool> SetBreedsList()
        {
            Uri uri = new Uri(@"https://dog.ceo/api/breeds/list/all");
            using (HttpClient client = new())
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    _breedsResponse = JsonSerializer.Deserialize<BreedResponse>(responseBody);
                }
            }
            _dogBreedList = _breedsResponse.GetDogBreedList();
            return true;    
        }

        public async Task<string> AsyncFetchRandomImage()
        {
            using (HttpClient client = new())
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(@"https://dog.ceo/api/breeds/image/random"));
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    
                    _dogImage = JsonSerializer.Deserialize<ImageResponse>(responseBody);
                    return _dogImage.message;
                }

                return "";
            }
        }

        public async Task<string> AsyncFetchBreedImage(string breed)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            var dog = _dogBreedList.FindAll(x => x.BreedName.StartsWith(breed) || x.Subbreed.StartsWith(breed) || string.Concat(x.BreedName, " ", x.Subbreed).Equals(breed) || string.Concat(x.Subbreed, " ", x.BreedName).Equals(breed));
            
            if (dog != null) 
            {
                int randomIndex = rand.Next(0, dog.Count);
                string uri = string.Empty;

                if (dog[randomIndex].Subbreed == "")
                    uri = $"https://dog.ceo/api/breed/{dog[randomIndex].BreedName}/images/random";
                else
                    uri = $"https://dog.ceo/api/breed/{dog[randomIndex].BreedName}/{dog[randomIndex].Subbreed}/images/random";

                using (HttpClient client = new())
                {
                    HttpResponseMessage response = await client.GetAsync(new Uri(uri));
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        _dogImage = JsonSerializer.Deserialize<ImageResponse>(responseBody);
                        return _dogImage.message;
                    }
                }
            }

            return "";
        }
    }
}
