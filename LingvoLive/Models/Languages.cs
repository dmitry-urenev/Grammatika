using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive.Models
{
    public class Language
    {
        public Language() { }

        public Language(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public static readonly Language German = new Language(32775, "DE");
        public static readonly Language Russian = new Language(1049, "RU");
    }
}
