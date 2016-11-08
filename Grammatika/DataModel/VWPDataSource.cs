using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace Grammatika.Data
{
    public class VWPItem
    {
        public string Id { get; set; }

        public string Lexeme { get; set; }

        public string DisplayText { get; set; }

        public string Comment { get; set; }

        public bool IsReflexive { get; set; }

        public List<string> Prepositions { get; set; }

        public string Translation { get; set; }

        public List<string> Examples { get; set; }

        public override string ToString()
        {
            return DisplayText ?? Lexeme;
        }

        public string ToPhrase()
        {
            string text = ToString();
            if (IsReflexive)
            {
                if (text.Contains("s "))
                {
                    text = text.Replace("s ", "sich ");
                }
                if (text.Contains("s. "))
                {
                    text = text.Replace("s. ", "sich ");
                }
            }
            if (text.Contains(" jn"))
            {
                text = text.Replace(" jn", " jemanden");
            }
            if (text.Contains(" jm"))
            {
                text = text.Replace(" jm", " jemandem");
            }
            if (text.Contains("jn "))
            {
                text = text.Replace("jn ", " jemanden");
            }
            if (text.Contains("jm "))
            {
                text = text.Replace("jm ", " jemandem");
            }
            if (text.Contains("etwA"))
            {
                text = text.Replace("etwA", "etwas in Akkusativ");
            }
            if (text.Contains("etwD"))
            {
                text = text.Replace("etwD", "etwas in Dativ");
            }
            if (text.Contains("|"))
            {
                text = text.Replace("|", " oder ");
            }
            return text;
        }
    }
    
    public class VWPDataSource
    {
        private static VWPDataSource _dataSource = new VWPDataSource();

        private ObservableCollection<VWPItem> _verbs = new ObservableCollection<VWPItem>();
        public ObservableCollection<VWPItem> Verbs
        {
            get { return this._verbs; }
        }

        public static async Task<IEnumerable<VWPItem>> GetAllVerbsAsync()
        {
            await _dataSource.GetDataAsync();

            return _dataSource.Verbs;
        }

        public static async Task<VWPItem> GetVerbAsync(string uniqueId)
        {
            await _dataSource.GetDataAsync();

            // Simple linear search is acceptable for small data sets
            var matches = _dataSource.Verbs.Where((v) => v.Id.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetDataAsync()
        {
            if (this._verbs.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///Data/VerbsWithPrepositions.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonArray jsonArray = JsonArray.Parse(jsonText);

            foreach (JsonValue val in jsonArray)
            {
                JsonObject obj = val.GetObject();
                VWPItem verb = new VWPItem()
                {
                    Id = obj["Id"].GetString(),
                    Lexeme = obj["Lexeme"].GetString(),
                    Prepositions = obj["Prepositions"].GetArray().Select(o => o.GetString()).ToList(),
                    Examples = obj["Examples"].GetArray().Select(o => o.GetString()).ToList(),
                    Translation = obj["Translation"].GetString()
                };

                if (obj.ContainsKey("DisplayText"))
                    verb.DisplayText = obj["DisplayText"].GetString();

                if (obj.ContainsKey("Reflexive"))
                    verb.IsReflexive = obj["Reflexive"].GetBoolean();

                if (obj.ContainsKey("Comment"))
                    verb.Comment = obj["Comment"].GetString();

                this.Verbs.Add(verb);
            }
        }
    }
}
