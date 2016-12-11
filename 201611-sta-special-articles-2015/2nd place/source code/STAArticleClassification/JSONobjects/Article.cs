//Article.cs
//Author: Krzysztof Desput
namespace STAArticleClassification
{
    /// <summary>
    /// Article that belongs to the set
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Special coverage of the article
        /// </summary>
        public int[] specialCoverage { get; set; }
        /// <summary>
        /// List of places referred in article
        /// </summary>
        public Place[] places { get; set; }
        /// <summary>
        /// Date of creation of the article (Unix timestamp in miliseconds)
        /// </summary>
        public long[] versioncreated { get; set; }
        /// <summary>
        /// Identifiers of articles with similar topic
        /// </summary>
        public int[] related { get; set; }
        /// <summary>
        /// Identifier of the next article in the chain of articles
        /// </summary>
        public int[] next { get; set; }
        /// <summary>
        /// List of keywords of the article
        /// </summary>
        public string[] keywords { get; set; }
        /// <summary>
        /// Identifier of the article
        /// </summary>
        public int[] id { get; set; }
        /// <summary>
        /// The categories the article fits into
        /// </summary>
        public string[] categories { get; set; }
        /// <summary>
        /// Identifier of the previous article in the chain of articles
        /// </summary>
        public int[] previous { get; set; }
        /// <summary>
        /// Priority of the article
        /// </summary>
        public int[] priority { get; set; }
    }
}
