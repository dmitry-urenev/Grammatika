using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    [DataContract]
    public class ExampleResult
    {
        [DataMember(Name = "rows")]
        public List<Example> Examples { get; set; }
    }
}
