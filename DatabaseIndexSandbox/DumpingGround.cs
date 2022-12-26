namespace DatabaseIndexSandbox
{
    internal class DumpingGround
    {
        // PARALLEL ASYNC CALLS
        //string URL = "https://catfact.ninja";
        //HttpCaller httpCaller = new HttpCaller(URL);
        //Stopwatch stopwatch = Stopwatch.StartNew();
        //stopwatch.Start();
        //IList<Task<HttpResponseMessage>> responses = new List<Task<HttpResponseMessage>>();

        //for (int i = 0; i < 30; i++)
        //{
        //    //HttpResponseMessage response = httpCaller.CallAsync("/breeds");
        //    //Console.WriteLine($"{response.Result.StatusCode} - {stopwatch.ElapsedMilliseconds}"); // 6~ seconds when awaiting each call.

        //    responses.Add(httpCaller.CallAsync("/breeds"));
        //}

        //await Task.WhenAll(responses);
        //foreach (Task<HttpResponseMessage> response in responses)
        //{
        //    Console.WriteLine($"{response.Result.StatusCode} - {stopwatch.ElapsedMilliseconds}"); // 1.5~ seconds when tasks run in parallel.
        //}
    }
}
