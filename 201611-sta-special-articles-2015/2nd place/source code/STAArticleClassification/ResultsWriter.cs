//ResultsWriter.cs
//Author: Krzysztof Desput
using System.Text;
using System.IO;

namespace STAArticleClassification
{
    /// <summary>
    /// Class to write the results to .csv file
    /// </summary>
    class ResultsWriter
    {
        /// <summary>
        /// Path of created .csv file with results
        /// </summary>
        private string path = "C:\\Results\\sta-special-articles-2015-submission.csv";
        /// <summary>
        /// Writes the results to .csv file
        /// </summary>
        /// <param name="testingSet">Testing set with given special coverages</param>
        public void WriteResults(TestingSet testingSet) 
        {
            StringBuilder csv = new StringBuilder(); //String builder for csv file with results
            csv.AppendLine("id;specialCoverage"); //Header line
            foreach (Article article in testingSet.articles.Values)
            {
                csv.AppendLine(string.Format("{0};{1}", article.id[0], article.specialCoverage[0]));
            }
            File.WriteAllText(@path, csv.ToString()); 
        }
    }
}
