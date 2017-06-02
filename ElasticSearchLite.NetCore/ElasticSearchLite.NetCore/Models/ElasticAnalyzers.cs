using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticAnalyzers
    {
        private static List<ElasticAnalyzers> _items = new List<ElasticAnalyzers>();
        public string Name { get; }
        public static IEnumerable<ElasticAnalyzers> Items => _items;
        private ElasticAnalyzers(string name)
        {
            Name = name;
            _items.Add(this);
        }
        /// <summary>
        /// The simple analyzer divides text into terms whenever it encounters a character which is not a letter. It lowercases all terms.
        /// </summary>
        public static ElasticAnalyzers Simple { get; } = new ElasticAnalyzers("simple");
        /// <summary>
        /// The whitespace analyzer breaks text into terms whenever it encounters a whitespace character.
        /// </summary>
        public static ElasticAnalyzers WhiteSpace { get; } = new ElasticAnalyzers("whitespace");
        /// <summary>
        /// The keyword analyzer is a "noop" analyzer which returns the entire input string as a single token.
        /// </summary>
        public static ElasticAnalyzers Keyword { get; } = new ElasticAnalyzers("keyword ");
        /// <summary>
        /// The standard analyzer divides text into terms on word boundaries, as defined by the Unicode Text Segmentation algorithm. 
        /// It removes most punctuation, lowercases terms, and supports removing stop words.
        /// </summary>
        public static ElasticAnalyzers Standard { get; } = new ElasticAnalyzers("standard");
        /// <summary>
        /// The stop analyzer is the same as the simple analyzer but adds support for removing stop words. It defaults to using the _english_ stop words.
        /// </summary>
        public static ElasticAnalyzers Stop { get; } = new ElasticAnalyzers("stop");
        /// <summary>
        /// The pattern analyzer uses a regular expression to split the text into terms. 
        /// The regular expression should match the token separators not the tokens themselves. 
        /// The regular expression defaults to \W+ (or all non-word characters).
        /// </summary>
        public static ElasticAnalyzers Pattern { get; } = new ElasticAnalyzers("pattern");
        /// <summary>
        /// A set of analyzers aimed at analyzing specific language text.
        /// </summary>
        public static ElasticAnalyzers Language { get; } = new ElasticAnalyzers("language");
        /// <summary>
        /// The fingerprint analyzer is a specialist analyzer which creates a fingerprint which can be used for duplicate detection.
        /// </summary>
        public static ElasticAnalyzers Fingerprint { get; } = new ElasticAnalyzers("fingerprint");
    }
}