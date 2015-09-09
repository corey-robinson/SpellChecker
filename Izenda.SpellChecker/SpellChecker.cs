using Newtonsoft.Json;
using NHunspell;
using System;
using System.Collections.Generic;

namespace Izenda.SpellChecker
{
    public class SpellChecker
    {
        internal class SpellCheckResults
        {
            [JsonProperty(PropertyName = "outcome")]
            public string Outcome { get; set; }

            [JsonProperty(PropertyName = "data")]
            public List<List<string>> Data { get; set; }
        }
        
        /// <summary>
        /// Checks the spelling of the specified text
        /// </summary>
        /// <param name="affFile">The affFile path</param>
        /// <param name="dicFile">The dictionary file path</param>
        /// <param name="text">The text to spell check</param>
        /// <returns></returns>
        public static string CheckSpelling(string affFile, string dicFile, string text)
        {
            bool success = true;
            List<string> misspelledWords = new List<string>();
            List<List<string>> data = new List<List<string>>();

            try
            {
                using (Hunspell hunspell = new Hunspell(affFile, dicFile))
                {
                    var words = text.Split((char[])null);

                    //check the spelling of each word
                    foreach (var word in words)
                    {
                        if (!hunspell.Spell(word))
                            misspelledWords.Add(word);
                    }

                    data.Add(misspelledWords);
                }
            }
            catch (Exception exception)
            {
                //need to add logging
                success = false;
            }

            var results = new SpellCheckResults
            {
                Outcome = success ? "success" : "error",
                Data = success ? data : null
            };

            return JsonConvert.SerializeObject(results);
        }

        /// <summary>
        /// Gets spelling suggestions for the specified word
        /// </summary>
        /// <param name="affFile">The affFile path</param>
        /// <param name="dicFile">The dictionary file path</param>
        /// <param name="word">The word to get suggestions</param>
        /// <returns>A json list of suggestions for the specified word.</returns>
        public static string GetWordSuggestions(string affFile, string dicFile, string word)
        {
            List<string> suggestions = new List<string>();

            try
            {
                using (Hunspell hunspell = new Hunspell(affFile, dicFile))
                {
                    suggestions = hunspell.Suggest(word);
                }
            }
            catch (Exception exception)
            {
                //need to handle this better and/or add logging
            }

            return JsonConvert.SerializeObject(suggestions);
        }
    }
}
