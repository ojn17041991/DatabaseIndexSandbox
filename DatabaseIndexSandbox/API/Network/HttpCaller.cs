using DatabaseIndexSandbox.Abstract.API.Network;

namespace DatabaseIndexSandbox.API.Network
{
    public class HttpCaller : IHttpCaller
    {
        private HttpClient client = new HttpClient();

        public HttpCaller(string rootEndPoint)
        {
            RootEndPoint = rootEndPoint;
        }

        public string RootEndPoint { get; }

        public HttpResponseMessage Call(string path)
        {
            // There is no synchronous GET on HttpClient.
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> CallAsync(string path)
        {
            string url = RootEndPoint + path;
            return await client.GetAsync(url); // The main thread can continue, but we're waiting for this to respond before returning.
        }
    }
}
