using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LingvoLive.Models
{
    [DataContract]
    public class SourceInfo
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "authors")]
        public string Authors { get; set; }

        [DataMember(Name = "copyrights")]
        public List<string> Copyrights { get; set; }

        [DataMember(Name = "linkText")]
        public string LinkText { get; set; }

        [DataMember(Name = "linkUrl")]
        public string LinkUrl { get; set; }

        [DataMember(Name = "links")]
        public List<string> Links { get; set; }

        [DataMember(Name = "isSite")]
        public bool IsSite { get; set; }

        [DataMember(Name = "translators")]
        public string Translators { get; set; }

        [DataMember(Name = "actualDate")]
        public DateTime? ActualDate { get; set; }
    }
}