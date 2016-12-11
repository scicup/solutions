//SVMClassifier.cs
//Author: Krzysztof Desput
using System;
using System.Collections.Generic;
using System.Linq;
using libsvm;

namespace STAArticleClassification
{
    /// <summary>
    /// Class containing a SVM classifier for articles
    /// </summary>
    public class SVMClassifier
    {
        /// <summary>
        /// Training set loaded from a file
        /// </summary>
        private TrainingSet trainingSet;
        /// <summary>
        /// List of all the features(words) that occur in articles
        /// </summary>
        private HashSet<string> vocabulary;
        /// <summary>
        /// List of features(bag-of-words) obtained from articles (1 string = 1 article)
        /// </summary>
        private List<string> x;
        /// <summary>
        /// Array of special coverages of articles from x
        /// </summary>
        private List<double> y; 
        /// <summary>
        /// Model that is used for predictions
        /// </summary>
        private C_SVC model;
        /// <summary>
        /// Constructor that creates object with given training set and testing set
        /// </summary>
        /// <param name="trainingSet">Training set loaded from a file</param>
        /// <param name="testingSet">Testing set loaded from a file</param>
        public SVMClassifier(TrainingSet trainingSet, TestingSet testingSet)
        {
            this.trainingSet = trainingSet;
            vocabulary = new HashSet<string>();
            x = new List<string>();
            y = new List<double>();

            foreach (Article article in trainingSet.articles.Values) //load data from the training set
            {
                string features = ArticleFeatures(article);
                //add features and special coverages to lists 
                AddFeaturesToVocabulary(features);
                x.Add(features);
                y.Add(article.specialCoverage[0]);
            }

            foreach (Article article in testingSet.articles.Values) //load articles with given specialCoverage from the testing set
            {
                if(article.specialCoverage != null)
                {
                    string features = ArticleFeatures(article);
                    //add features and special coverages to lists
                    AddFeaturesToVocabulary(features);
                    x.Add(features);
                    y.Add(article.specialCoverage[0]);
                }
            }

            //create new problem
            ProblemBuilder problemBuilder = new ProblemBuilder();
            var problem = problemBuilder.CreateProblem(x, y.ToArray(), vocabulary.ToList());

            //create new model using linear kernel
            const int C = 1; //C parameter for C_SVC
            model = new C_SVC(problem, KernelHelper.LinearKernel(), C);
        }
        /// <summary>
        /// Predicts special coverage of the article using built model
        /// </summary>
        /// <param name="article">Article classified by the method</param>
        /// <returns>Special coverage predicted by the model</returns>
        public int Classify(Article article)
        {
            var newX = ProblemBuilder.CreateNode(ArticleFeatures(article), vocabulary.ToList()); //create node
            var predictedY = model.Predict(newX); //get special coverage
            return (int)predictedY;
        }

        /// <summary>
        /// Gets the month when the article was created and the half of this month (e.g. when article was written on 16 March it is "March2")
        /// </summary>
        /// <param name="versioncreated">Date of creation of the article (Unix timestamp in miliseconds)</param>
        /// <returns>Date feature in format [monthName][halfOfTheMonth] e.g. "October1" </returns>
        private string DateFeature(long versioncreated) //get the date feature from article's date of creation (e.g. "September1" for the first half of september)
        {
            DateTime datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            datetime = datetime.AddMilliseconds(versioncreated).ToLocalTime();
            string ret = "";
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            ret = months[datetime.Month - 1]; //get the article's month 
            if (datetime.Day <= 15) //get the half of the month 
                ret += "1";
            else ret += "2";
            return ret;
        }
        /// <summary>
        /// Extracts the features from the article
        /// </summary>
        /// <param name="article">Article to extract features</param>
        /// <returns>String of features(bag-of-words) obtained from article, separated by ','</returns>
        private string ArticleFeatures(Article article)
        {
            string features = ""; //article's features (bag-of-words)
            foreach (string keyword in article.keywords) //add keywords to article's features
            {
                features = features + "," + keyword;
            }
            foreach (string category in article.categories) //add categories to article's features
            {
                features = features + "," + category;
            }
            foreach (Place place in article.places) //add places to article's features
            {
                features = features + "," + place.country + "," + place.city;
            }
            features = features + "," + article.priority[0]; //add priority to article's features

            //add month and half of the month to article's features
            features = features + "," + DateFeature(article.versioncreated[0]);

            return features;
        }
        /// <summary>
        /// Adds features from the string to vocabulary.
        /// </summary>
        /// <param name="features">String of features(bag-of-words) obtained from article, separated by ','</param>
        public void AddFeaturesToVocabulary(string features) //add features to vocabulary
        {
            string[] featuresArray = features.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string feature in featuresArray)
            {
                vocabulary.Add(feature);
            }
        }
    }
}
