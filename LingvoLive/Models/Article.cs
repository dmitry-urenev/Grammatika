using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    [DataContract]
    public class Article
    {
        [DataMember(Name = "heading")]
        public string Heading { get; set; }

        [DataMember(Name = "dictionary")]
        public string Dictionary { get; set; }

        [DataMember(Name = "bodyHtml")]
        public string BodyHTML { get; set; }
        
    }
}
