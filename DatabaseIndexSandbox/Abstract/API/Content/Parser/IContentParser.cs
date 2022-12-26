namespace DatabaseIndexSandbox.Abstract.API.Content.Parser
{
    internal interface IContentParser<T>
    {
        T? Parse(string content);
    }
}
