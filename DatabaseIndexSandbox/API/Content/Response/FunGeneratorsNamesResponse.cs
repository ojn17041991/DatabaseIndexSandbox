using DatabaseIndexSandbox.Abstract.API.Content.Response;
using System.Xml.Serialization;

namespace DatabaseIndexSandbox.API.Content.Response
{
    [XmlRoot("response")]
    public class FunGeneratorsResponse : IContent
    {
        [XmlElement("success")]
        public FunGeneratorSuccess success { get; set; } = new FunGeneratorSuccess();

        [XmlElement("contents")]
        public FunGeneratorContents contents { get; set; } = new FunGeneratorContents();
    }

    [XmlRoot("success")]
    public class FunGeneratorSuccess : IContent
    {
        [XmlElement("start")]
        public int start { get; set; }

        [XmlElement("limit")]
        public int limit { get; set; }
    }

    [XmlRoot("contents")]
    public class FunGeneratorContents : IContent
    {
        [XmlElement("category")]
        public string category { get; set; } = string.Empty;

        [XmlArray("names")]
        [XmlArrayItem("text")]
        public List<string> names { get; set; } = new List<string>();
    }
}
