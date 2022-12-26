using DatabaseIndexSandbox.Abstract.API.Content.Parser;
using System.Xml.Serialization;

namespace DatabaseIndexSandbox.API.Content.Parser
{
    public class XmlContentParser<T> : IContentParser<T>
    {
        internal XmlSerializer serializer = new XmlSerializer(typeof(T));

        public T? Parse(string content)
        {
            using (TextReader reader = new StringReader(content))
            {
                return (T?)serializer.Deserialize(reader);
            }
        }
    }
}
