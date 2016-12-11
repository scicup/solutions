//Program.cs
//Author: Krzysztof Desput
using System.Linq;

namespace STAArticleClassification
{
    /// <summary>
    /// Main class of the application that uses libsvm library to classify articles
    /// </summary>
    class Program
    {
        /// <summary>
        /// Training set loaded from a file
        /// </summary>
        static TrainingSet trainingSet;
        /// <summary>
        /// Testing set loaded from a file
        /// </summary>
        static TestingSet testingSet;
        /// <summary>
        /// Class containing classifiers that are 100% sure
        /// </summary>
        static SureClassifiers sureClassifiers;
        /// <summary>
        /// Class containing a SVM classifier
        /// </summary>
        static SVMClassifier svmClassifier;
        static void Main(string[] args)
        {
            trainingSet = new TrainingSet();
            testingSet = new TestingSet();
            System.Console.WriteLine("Data loaded");
            System.Console.WriteLine("Training set: " + trainingSet.articles.Count);
            System.Console.WriteLine("Testing set: " + testingSet.articles.Count);

            sureClassifiers = new SureClassifiers(trainingSet, testingSet);

            //1. Looking for previous articles
            foreach (Article article in testingSet.articles.Values.OrderBy(key => key.id[0]))
            {
                if (article.specialCoverage == null) //if article doesn't have specialCoverage
                {
                    int specialCoverage = sureClassifiers.PreviousArticle(article);
                    if (specialCoverage > 0) //if specialCoverage was given by the method
                    {
                        article.specialCoverage = new int[1];
                        article.specialCoverage[0] = specialCoverage;
                    }
                }
            }

            //2. Looking for next articles 
            foreach (Article article in testingSet.articles.Values.OrderByDescending(key => key.id[0]))
            {
                if (article.specialCoverage == null) //if article doesn't have specialCoverage
                {
                    int specialCoverage = sureClassifiers.NextArticle(article);
                    if (specialCoverage > 0) //if specialCoverage was given by the method
                    {
                        article.specialCoverage = new int[1];
                        article.specialCoverage[0] = specialCoverage;
                    }
                }
            }

            //3. Looking for related articles
            foreach (Article article in testingSet.articles.Values.OrderBy(key => key.id[0]))
            {
                if (article.specialCoverage == null) //if article doesn't have specialCoverage
                {
                    int specialCoverage = sureClassifiers.RelatedArticles(article);
                    if (specialCoverage > 0) //if specialCoverage was given by the method
                    {
                        article.specialCoverage = new int[1];
                        article.specialCoverage[0] = specialCoverage;
                    }
                }
            }

            // 4. Getting special coverage from a SVM classifier
            svmClassifier = new SVMClassifier(trainingSet, testingSet);

            foreach (Article article in testingSet.articles.Values)
            {
                if (article.specialCoverage == null) //if article doesn't have specialCoverage
                {
                    article.specialCoverage = new int[1];
                    article.specialCoverage[0] = svmClassifier.Classify(article);
                }
            }

            System.Console.WriteLine("Special coverages given");

            //Write the results to .csv
            ResultsWriter resultsWriter = new ResultsWriter();
            resultsWriter.WriteResults(testingSet);
            System.Console.WriteLine("Results written");
            System.Console.ReadKey();
        }
    }
}
