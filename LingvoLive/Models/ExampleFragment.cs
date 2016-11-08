using System.Runtime.Serialization;

namespace LingvoLive.Models
{
    [DataContract]
    public class ExampleFragment
    {
        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }

        [DataMember(Name = "isOriginal")]
        public bool IsOriginal { get; set; }

        [DataMember(Name = "contentSourceInfo")]
        public SourceInfo ContentSourceInfo { get; set; }
    }
}