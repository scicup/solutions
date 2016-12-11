//ProblemBuilder.cs
//Author: Krzysztof Desput
using System;
using System.Collections.Generic;
using System.Linq;
using libsvm;

namespace STAArticleClassification
{
    /// <summary>
    /// SVM problem builder for libsvm
    /// </summary>
    public class ProblemBuilder
    {
        /// <summary>
        /// Creates new SVM problem that is used to build the model
        /// </summary>
        /// <param name="x">List of features(bag-of-words) obtained from articles (1 string = 1 article)</param>
        /// <param name="y">Array of special coverages of articles from x</param>
        /// <param name="vocabulary">List of all the features(words) that occur in articles</param>
        /// <returns>Created SVM problem</returns>
        public svm_problem CreateProblem(List<string> x, double[] y, List<string> vocabulary) 
        {
            return new svm_problem
            {
                y = y,
                x = x.Select(xVector => CreateNode(xVector, vocabulary)).ToArray(),
                l = y.Length
            };
        }
        /// <summary>
        /// Creates SVM node that is used in prediction
        /// </summary>
        /// <param name="x">String with the features(bag-of-words) obtained from the article. They are separated by ','</param>
        /// <param name="vocabulary">List of all the features(words) that occur in articles</param>
        /// <returns>Created SVM node</returns>
        public static svm_node[] CreateNode(string x, List<string> vocabulary) 
        {
            var node = new List<svm_node>(vocabulary.Count);

            string[] features = x.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries); //get features from x
            features = features.ToArray();
            for (int i = 0; i < vocabulary.Count; i++)
            {
                int occurence = features.Count(s => String.Equals(s, vocabulary[i], StringComparison.Ordinal)); //how many times does the word from vocabulary occur in x (features)
                if (occurence != 0) 
                {
                    node.Add(new svm_node
                    {
                        index = i + 1,
                        value = occurence
                    });
                }
            }
            return node.ToArray();
        }
    }
}
