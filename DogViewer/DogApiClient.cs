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
        private  DogImage? _photo;
        private BreedResponse? _breedsDict;

        public async Task<bool> SetBreedsList()
        {
            Uri uri = new Uri(@"https://dog.ceo/api/breeds/list/all");
            using (HttpClient client = new())
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    _breedsDict = JsonSerializer.Deserialize<BreedResponse>(responseBody);  
                }
            }
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
                    
                    _photo = JsonSerializer.Deserialize<DogImage>(responseBody);
                    return _photo.message;
                }

                return "";
            }
        }

        public async Task<string> AsyncFetchBreedImage(string breed)
        {
            string uri = "";

            if (!_breedsDict.IsInDictionary(breed))
            {
                return "";
            }

            uri = $"https://dog.ceo/api/breed/{breed}/images/random";

            using (HttpClient client = new())
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(uri));
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    _photo = JsonSerializer.Deserialize<DogImage>(responseBody);
                    return _photo.message;
                }

                return "";
            }
        }
    }
}
