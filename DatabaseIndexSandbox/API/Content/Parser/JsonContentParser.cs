using DatabaseIndexSandbox.Abstract.API.Content.Parser;
using System.Text.Json;

namespace DatabaseIndexSandbox.API.Content.Parser
{
    public class JsonContentParser<T> : IContentParser<T>
    {
        public T? Parse(string content)
        {
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}
