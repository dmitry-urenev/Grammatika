using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    [DataContract]
    public class TranslationResponse
    {
        [DataMember(Name = "languagesReversed")]
        public bool LanguagesReversed { get; set; }

        [DataMember(Name = "lingvoArticles")]
        public List<Article> Articles { get; set; }

        public string Suggests { get; set; }

        public List<string> SeeAlsoWordForms { get; set; }

        public string GlossaryUnits { get; set; }

        public string WordByWordTranslation { get; set; }
    }
}
