using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    [DataContract]
    public class Suggestion
    {
        [DataMember(Name = "heading")]
        public string Heading { get; set; }

        [DataMember(Name = "lingvoTranslations")]
        public string OriginalTranslations { get; set; }

        [DataMember(Name = "socialTranslations")]
        public string SocialTranslations { get; set; }

        [DataMember(Name = "DictionaryName")]
        public string DictionaryName { get; set; }

        [DataMember(Name = "type")]
        public string WordType { get; set; }

        [DataMember(Name = "srcLangId")]
        public string SorceLanguageId { get; set; }

        [DataMember(Name = "dstLangId")]
        public string TargetLanguageId { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }
    }
}
