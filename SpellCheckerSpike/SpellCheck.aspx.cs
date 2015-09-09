using Izenda.SpellChecker;
using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace SpellCheckerSpike
{
    /// <summary>
    /// Asp.NET replacement for SpellChecker.php in the jQuery Spellchecker
    /// library
    /// </summary>
    public partial class SpellCheck : Page
    {
        const string AFF_FILE = "en_US.aff";
        const string DIC_FILE = "en_US.dic";

        public string AffFilePath 
        { 
            get
            {
                return HttpContext.Current.Server.MapPath("~/") + AFF_FILE;
            }
        }

        public string DicFilePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/") + DIC_FILE;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            string action = Request.Params["action"];
            
            if (!DictionaryFilesExist())
            {
                Response.Write("Missing dictionary files...");
                return;
            }

            switch (action)
            {
                case "get_incorrect_words":
                    GetIncorrectWords();
                    break;

                case "get_suggestions":
                    GetSuggestions();
                    break;

                default:
                    Response.Write("Unknown options specifed...");
                    break;
            }
        }

        /// <summary>
        /// Gets a list of incorrect words from the text[] param
        /// </summary>
        private void GetIncorrectWords()
        {
            string text = Request.Params["text[]"];

            if (string.IsNullOrEmpty(text))
            {
                Response.Write("Unknown options specifed...");
                return;
            }

            //expected format: {"outcome":"success","data":[["Izenda","ISVs","real-time","self-service"]]}
            Response.ContentType = "text/plain";
            Response.Write(SpellChecker.CheckSpelling(AffFilePath, DicFilePath, text));     
        }

        /// <summary>
        /// Gets spelling suggestions for the specified word
        /// </summary>
        private void GetSuggestions()
        {
            string word = Request.Params["word"];

            if(string.IsNullOrEmpty(word))
            {
                Response.Write("Unknown options specifed...");
                return;
            }
            
            Response.ContentType = "text/plain";
            Response.Write(SpellChecker.GetWordSuggestions(AffFilePath, DicFilePath, word));                
        }

        /// <summary>
        /// Checks the existence of the files needed for the spell checker to function
        /// </summary>
        /// <returns></returns>
        private bool DictionaryFilesExist()
        {
            if (!File.Exists(AffFilePath) || !File.Exists(DicFilePath))
                return false;

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}