using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    [DataContract]
    public class InflectedForm
    {
        [DataMember(Name = "lexem")]
        public string Lexem { get; set; }

        [DataMember(Name = "partOfSpeech")]
        public string PartOfSpeech { get; set; }

        [DataMember(Name = "paradigmXml")]
        public string ParadigmXml { get; set; }
    }
}
