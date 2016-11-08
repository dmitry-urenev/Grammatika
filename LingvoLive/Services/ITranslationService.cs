using LingvoLive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Services
{
    public interface ITranslationService
    {
        TranslationResponse Translate(TranslationRequest request);
        TranslationResponse Translate(int srcLang, int dstLang, string text);

        List<Example> GetPhrases(TranslationRequest request);
        List<Example> GetPhrases(int srcLang, int dstLang, string text);

        List<Example> GetExamples(TranslationRequest request);
        List<Example> GetExamples(int srcLang, int dstLang, string text);

        List<InflectedForm> GetInflectedForm(TranslationRequest request);
        List<InflectedForm> GetInflectedForm(int srcLang, int dstLang, string text);
    }
}
