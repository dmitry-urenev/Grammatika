using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    [DataContract]
    public class WordLocation
    {
        [DataMember(Name = "begin")]
        public int Begin { get; set; }

        [DataMember(Name = "end")]
        public int End { get; set; }
    }
}
