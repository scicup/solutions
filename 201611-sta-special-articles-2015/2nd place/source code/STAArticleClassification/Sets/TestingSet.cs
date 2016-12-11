//TestingSet.cs
//Author: Krzysztof Desput
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace STAArticleClassification
{
    /// <summary>
    /// Class that gets testing set from a file and stores it 
    /// </summary>
    public class TestingSet
    {
        /// <summary>
        /// Dictionary of articles obtained from a testing set
        /// </summary>
        public Dictionary<int, Article> articles;
        /// <summary>
        /// Path of .json file with testing set
        /// </summary>
        private string path = "C:\\Data\\sta-special-articles-2015-testing.json";
        /// <summary>
        /// Constructor that gets testing set from .json file
        /// </summary>
        public TestingSet()
        {
            string readText = File.ReadAllText(@path);
            JArray trainingSet = JArray.Parse(readText);
            List<JToken> results = trainingSet.Children().ToList();
            articles = new Dictionary<int, Article>();
            foreach (JToken result in results)
            {
                Article article = JsonConvert.DeserializeObject<Article>(result.ToString()); //Getting object from a JSON
                articles.Add(article.id[0], article);
            }
        }
    }
}
