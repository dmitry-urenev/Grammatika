using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    public class TranslationRequest
    {
        public int SourceLanguage { get; set; }

        public int TargetLanguage { get; set; }

        public string Text { get; set; }
    }
}
