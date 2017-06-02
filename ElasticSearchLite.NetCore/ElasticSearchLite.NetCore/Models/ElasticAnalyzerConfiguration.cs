namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticAnalyzerConfiguration
    {
        /// <summary>
        /// The maximum token length. 
        /// If a token is seen that exceeds this length then it is split at max_token_length intervals.
        /// Defaults to 255.
        /// </summary>
        public int MaxTokenLength { get; set; } = 255;
        /// <summary>
        /// A pre-defined stop words list like _english_ or an array containing a list of stop words. 
        /// Defaults to _none_.
        /// </summary>
        public string StopWords { get; set; } = "_none_";
        /// <summary>
        /// The path to a file containing stop words.
        /// </summary>
        public string StopWordPath { get; set; }
        /// <summary>
        /// A Java regular expression, defaults to \W+.
        /// </summary>
        public string Pattern { get; set; }
        /// <summary>
        /// Java regular expression flags.Flags should be pipe-separated, 
        /// eg "CASE_INSENSITIVE|COMMENTS".
        /// </summary>
        public string Flags { get; set; }
        /// <summary>
        /// Should terms be lowercased or not. Defaults to true.
        /// </summary>
        public bool Lowercase { get; set; } = true;
        /// <summary>
        /// Language to use for the language analyzer, can be english, german, etc...
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// The character to use to concate the terms. Defaults to a space.
        /// </summary>
        public char Separator { get; set; } = ' ';
        /// <summary>
        /// The maximum token size to emit.Defaults to 255. 
        /// Tokens larger than this size will be discarded.
        /// </summary>
        public int MaxOutputSize { get; set; } = 255;
    }
}
