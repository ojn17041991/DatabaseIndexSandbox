namespace DatabaseIndexSandbox.Abstract.API.Network
{
    internal interface IHttpCaller
    {
        HttpResponseMessage Call(string path);

        Task<HttpResponseMessage> CallAsync(string path);
    }
}
