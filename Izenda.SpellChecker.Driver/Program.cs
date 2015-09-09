using Newtonsoft.Json;
using System;

namespace Izenda.SpellChecker.Driver
{
    /// <summary>
    /// Driver program used to quickly test the Izenda.SpellChecker api
    /// </summary>
    class Program
    {
        internal class SpellCheckResults
        {
            [JsonProperty(PropertyName = "outcome")]
            public string Outcome { get; set; }

            [JsonProperty(PropertyName = "data")]
            public string[] Data { get; set; }
        }

        static void Main(string[] args)
        {
             var misspelledWords = SpellChecker.CheckSpelling("en_us.aff", "en_us.dic", "This is a simple spelling test.");

             Console.WriteLine("Misspelled Words: ");            
        }
    }
}
