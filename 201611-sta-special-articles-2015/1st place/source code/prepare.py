# -*- coding: utf-8 -*-
# Prepare data sets as described here
# http://scikit-learn.org/stable/modules/generated/sklearn.datasets.load_files.html
 
import json
import codecs
import os
import sys
reload(sys)
sys.setdefaultencoding('utf-8')

# Load training file
with codecs.open('data\sta-special-articles-2015-training.json', encoding='utf-8') as json_data:
    data = json.load(json_data)
# Split to folders
for i in data:
	directory = 'sta-train\\' + str(i['specialCoverage'][0])
	if not os.path.exists(directory):
		os.makedirs(directory)
	file = directory + '\\' +  str(i['id'][0])
	headline = str(i['headline'][0]).encode('utf-8')
	keywords = str(i['keywords'][0]).encode('utf-8')
	
	# Take only first category, seems to be working better than all
	categories = str(i['categories'][0]).encode('utf-8')
	
	# I am not sure if two character words are exluded so make them longer, it does not hurt but seems to works
	categories = categories + categories + categories
	
	# Take only first place, seems to be working better than all
	places = str(i['places'][0]).encode('utf-8')
	
	# Second option with all categories and places in a "clean" way, seems to be working worse so not included in final file but I left it here
	# categories_2 = i['categories']
	# category = ''
	# for cat in categories_2:
		# category = category + ' ' + str(cat) + str(cat) + str(cat)
	# place = ''

	# for p in i['places']:
		# place = place + ' ' + str(p['city']).encode('utf-8') + ' ' + str(p['country']).encode('utf-8')  + ' ' + str(p['code1']).encode('utf-8')  + ' ' + str(p['code2']).encode('utf-8')
	
	try:
		text = str(i['text'][0]).encode('utf-8')
	except KeyError:
		text = ' '

	try:
		lede = str(i['lede'][0]).encode('utf-8')
	except KeyError:
		lede = ' '
		
	with open(file, "w",) as text_file:
		text_file.write(lede + ' ' + places  + ' ' + headline + ' ' + keywords + ' ' + text + ' ' + categories)
