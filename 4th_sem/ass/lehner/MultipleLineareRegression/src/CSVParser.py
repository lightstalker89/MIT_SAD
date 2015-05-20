#!/usr/bin/python
import ArithmParser
import csv

def csvDataFromTerm( term, path ):
	try:
		with open(path, 'rb') as csvfile:
			csvData = csv.DictReader(csvfile)
			resultData = []
			for row in csvData:
				# replace keys
				resultTerm = replaceKeys(term, row)
				# check if term is available or not
				if len(resultTerm) > 1:
					result = ArithmParser.parse(resultTerm)
				else:
					result = float(resultTerm[0])
				resultData.append(result)
			return resultData
	except KeyError:
		print 'invalid dictionary key: ', key

def replaceKeys(term, dataArray):
	resultTerm = []
	for key in term:
		if key in dataArray:
			resultTerm.append(dataArray[key])
		elif isinstance(key, basestring):
			resultTerm.append(key)
		else:
			resultTerm.append(replaceKeys(key, dataArray))
	return resultTerm

def getKeys(path):
	try:
		with open(path, 'rb') as csvfile:
			csvData = csv.DictReader(csvfile)
			return csvData.fieldnames
	except KeyError:
		print 'could not open file'