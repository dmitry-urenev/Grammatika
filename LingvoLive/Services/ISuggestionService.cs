using LingvoLive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Services
{
    public interface ISuggestionService
    {
        List<Suggestion> GetSuggestions(int srcLang, int dstLang, string text, int count);
    }
}
