using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    [DataContract]
    public class Example
    {
        [DataMember(Name = "hasCorrectFoundWordTranslation")]
        public bool HasCorrectFoundWordTranslation { get; set; }

        [DataMember(Name = "sourceFragment")]
        public ExampleFragment SourceFragment { get; set; }

        [DataMember(Name = "targetFragment")]
        public ExampleFragment TargetFragment { get; set; }

        [DataMember(Name = "sourceFoundWords")]
        public List<WordLocation> SourceFoundWords { get; set; }

        [DataMember(Name = "targetFoundWords")]
        public List<WordLocation> TargetFoundWords { get; set; }

        [DataMember(Name = "topics")]
        public string Topics { get; set; }

        [DataMember(Name = "isLingvoContent")]
        public bool IsLingvoContent { get; set; }

        [DataMember(Name = "lingvoDictionaryName")]
        public string LingvoDictionaryName { get; set; }

        [DataMember(Name = "authorNickName")]
        public string AuthorNickName { get; set; }
    }
}
