//TrainingSet.cs
//Author: Krzysztof Desput
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace STAArticleClassification
{
    /// <summary>
    /// Class that gets training set from a file and stores it 
    /// </summary>
    public class TrainingSet
    {
        /// <summary>
        /// Dictionary of articles obtained from a training set
        /// </summary>
        public Dictionary<int, Article> articles;
        /// <summary>
        /// Path of .json file with training set
        /// </summary>
        private string path = "C:\\Data\\sta-special-articles-2015-training.json";
        /// <summary>
        /// Constructor that gets training set from .json file
        /// </summary>
        public TrainingSet() 
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
