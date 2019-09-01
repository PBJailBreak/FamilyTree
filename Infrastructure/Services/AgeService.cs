using System.Net.Http;
using System.Threading.Tasks;
using Core.Contracts.Services;

namespace Infrastructure.Services
{
    public class AgeService : IAgeService
    {
        private readonly HttpClient httpClient;
        public AgeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<int> GetRandomAgeAsync()
        {
            var response = await this.httpClient.GetAsync("https://www.random.org/integers/?num=1&min=0&max=65&col=1&base=10&format=plain&rnd=new");

            if (response.IsSuccessStatusCode)
            {
                return int.Parse(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new System.Exception("Failed to retrieve random number from random.org");
            }
        }
    }
}
