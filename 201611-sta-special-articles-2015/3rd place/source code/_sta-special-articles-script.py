"""
======================================================
Slovenian Press Agency - Articles' Topic Classification
======================================================

This is my simple yet robust solution for the Slovenian Press Agency challenge on scicup.com

Author: Michal Maternik <michal.maternik@student.pwr.edu.pl>

"""

import json, csv
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.linear_model import SGDClassifier
from sklearn.datasets.base import Bunch
from sklearn.cross_validation import train_test_split
from sklearn import metrics

#import os
#os.chdir('>> PATH CONTAINING sta-special-articles-2015-* FILES <<')

print(__doc__)

submission = open('sta-special-articles-2015-submission.csv', 'r')
training = open('sta-special-articles-2015-training.json', 'r')
testing = open('sta-special-articles-2015-testing.json', 'r')

training_js = json.loads(training.read())
testing_js = json.loads(testing.read())

def loadTrainingData():
    with open('sta-special-articles-2015-training.json', 'r') as training:
        tr = json.loads(training.read())
        return Bunch(data = [json.dumps(elem) for elem in tr],
                     target = [elem.get('specialCoverage')[0] for elem in tr],
                     target_names = []
                     )

def loadTestingData():
    with open('sta-special-articles-2015-testing.json', 'r') as testing:
        ts = json.loads(testing.read())
        return Bunch(data = [json.dumps(elem) for elem in ts],
                     target = [0 for elem in ts],
                     target_names = []
                     )

def saveSubmission(filename, pred):
    with open(filename + '.csv', 'w', newline='') as out:
        writer = csv.writer(out, delimiter=';')
        writer.writerow(['id', 'specialCoverage'])
        for i in range(0, len(pred)):
            id = testing_js[i].get('id')[0]
            writer.writerow([id, pred[i]]) 
    print ("Submission saved in file " + filename + ".csv")
        
data_train = loadTrainingData()
data_test = loadTestingData()

y_train, y_test = data_train.target, data_test.target


# Prepare vectorizer and classifier

vectorizer = TfidfVectorizer(sublinear_tf=True, max_df=0.5, 
                             stop_words='english')

clf = SGDClassifier(alpha=.0001, n_iter=50, penalty="l2")
print(clf)

# Prepare cross validation data

cross_data_train = Bunch(data=[], target=[])
cross_data_test = Bunch(data=[], target=[])

(cross_data_train.data, cross_data_test.data, 
 cross_data_train.target, cross_data_test.target) = train_test_split(
        data_train.data, data_train.target, test_size=0.3, random_state=0)

# Transform, train and validate

print("\nValidating...")

X_cross_train = vectorizer.fit_transform(cross_data_train.data)
X_cross_test = vectorizer.transform(cross_data_test.data)

y_cross_train = cross_data_train.target
y_cross_test = cross_data_test.target

X_cross_train = vectorizer.fit_transform(cross_data_train.data)
X_cross_test = vectorizer.transform(cross_data_test.data)

clf.fit(X_cross_train, y_cross_train)

cross_prediction = clf.predict(X_cross_test)

score = metrics.accuracy_score(y_cross_test, cross_prediction)
print("accuracy:   %0.3f" % score)
print()

# Transform, train and fit the whole data set

print("Solving...")

X_train = vectorizer.fit_transform(data_train.data)
X_test = vectorizer.transform(data_test.data)

clf.fit(X_train, y_train)

prediction = clf.predict(X_test)
print("\nPrediction:")
print(prediction)

saveSubmission('my-submission-sgdc', prediction)

print("\nDone.")