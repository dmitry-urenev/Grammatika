using LingvoLive.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingvoLive.Models;

namespace LingvoLive
{
    public class TranslationService : LingvoApiService, ITranslationService
    {
        public List<Example> GetExamples(TranslationRequest request)
        {
            return GetExamples(request.SourceLanguage, request.TargetLanguage, request.Text);
        }

        public List<Example> GetExamples(int srcLang, int dstLang, string text)
        {
            throw new NotImplementedException();
        }

        public List<InflectedForm> GetInflectedForm(TranslationRequest request)
        {
            return GetInflectedForm(request.SourceLanguage, request.TargetLanguage, request.Text);
        }

        public List<InflectedForm> GetInflectedForm(int srcLang, int dstLang, string text)
        {
            throw new NotImplementedException();
        }

        public List<Example> GetPhrases(TranslationRequest request)
        {
            return GetPhrases(request.SourceLanguage, request.TargetLanguage, request.Text);
        }

        public List<Example> GetPhrases(int srcLang, int dstLang, string text)
        {
            throw new NotImplementedException();
        }

        public TranslationResponse Translate(TranslationRequest request)
        {
            return Translate(request.SourceLanguage, request.TargetLanguage, request.Text);
        }

        public TranslationResponse Translate(int srcLang, int dstLang, string text)
        {
            throw new NotImplementedException();
        }
    }
}
