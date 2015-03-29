#!/usr/bin/python
import csv

def csvDataFromKey( key, path ):
	try:
		with open(path, 'rb') as csvfile:
			csvData = csv.DictReader(csvfile)
			resultData = []
			for row in csvData:
				resultData.append(float(row[key]))
			return resultData
	except KeyError:
		print 'invalid dictionary key: ', key