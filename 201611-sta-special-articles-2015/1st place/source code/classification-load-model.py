# -*- coding: utf-8 -*-
import sys
import os
import codecs
import csv
import numpy as np
from sklearn.externals import joblib
from sklearn.datasets import load_files
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.feature_extraction.text import TfidfTransformer
from sklearn.naive_bayes import MultinomialNB
from sklearn.pipeline import Pipeline
from sklearn.linear_model import SGDClassifier
from sklearn.model_selection import GridSearchCV
from sklearn import metrics

def code():
	twenty_train = load_files(container_path = 'sta-train', encoding='utf-8')
	twenty_test = load_files(container_path = 'sta-test', encoding='utf-8')
	
	docs_test = twenty_test.data
	docs_filename = twenty_test.filenames
	
	# Load best classification model to recreate results
	text_clf = joblib.load('models/score = 0.9667805.pkl') 
	predicted = text_clf.predict(docs_test)
	
	# Create submission file
	submission = 'sta-special-articles-2015-submission.csv'
	with open(submission, "w",) as text_file:
		text_file.write('id;specialCoverage\n')
	with open(submission, "a",) as text_file:
		for id, category in zip(docs_filename, predicted):
			text_file.write('%s;%s \n' % (os.path.basename(id), twenty_train.target_names[category]))
	
if __name__ == '__main__':
	reload(sys)
	sys.setdefaultencoding('utf-8')
	codecs.register(lambda name: codecs.lookup('utf-8') if name == 'cp65001' else None)
	code()