using DatabaseIndexSandbox.API.Network;

namespace DatabaseIndexSandbox.API
{
    public class FirstLastNamesGenerator
    {
        private string rootEndPoint = "https://api.fungenerators.com";
        private HttpCaller firstNameCaller;
        private HttpCaller lastNameCaller;

        public FirstLastNamesGenerator()
        {
            firstNameCaller = new HttpCaller(rootEndPoint);
            lastNameCaller = new HttpCaller(rootEndPoint);
        }

        public async Task<string> GetFirstNamesAsync()
        {
            HttpResponseMessage response = await firstNameCaller.CallAsync("/name/generate?category=alien&limit=100");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetLastNamesAsync()
        {
            HttpResponseMessage response = await lastNameCaller.CallAsync("/name/generate?category=pokemon&limit=100");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
