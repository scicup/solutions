The solution was built in the following environment:
- Microsoft Visual Studio 2015
- .NET Framework 4.5.2

Libraries used in the solution:
- libsvm.net 2.1.8
- Newtonsoft.Json 9.0.1
- IKVM.OpenJDK.* libriares 7.2.4630.5 (when you install libsvm.net they are included automatically)

The default parameters set in code:
Path of the training set(can be changed in TrainingSet.cs): "C:\Data\sta-special-articles-2015-training.json"
Path of the testing set(can be changed in TestingSet.cs): "C:\Data\sta-special-articles-2015-testing.json"
Path of created results file(can be changed in ResultsWriter.cs): "C:\Results\sta-special-articles-2015-submission.csv"

Executable file's location: \STAArticleClassification\bin\Release\STAArticleClassification.exe