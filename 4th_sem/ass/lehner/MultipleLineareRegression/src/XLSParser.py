#!/usr/bin/python
import ArithmParser
import xlrd

def xlsDataFromTerm( term, path ):
	try:
		xlsDoc = xlrd.open_workbook(path)
		resultData = []
		sheet = xlsDoc.sheet_by_index(0)

		for row_idx in range(1, sheet.nrows):
			#replace keys
			resultTerm = replaceKeys(term, sheet, row_idx)
			# check if term is available or not
			if len(resultTerm) > 1:
				result = ArithmParser.parse(resultTerm)
			else:
				result = float(resultTerm[0])
			resultData.append(result)
		return resultData
	except KeyError:
		print 'invalid dictionary key: ', key

def replaceKeys(term, sheet, rowIdx):
	resultTerm = []
	for key in term:
		column = getColumnForKey(key, sheet)
		if column != -1:
			cell = sheet.cell(rowIdx, column)
			resultTerm.append(str(cell.value))
		elif isinstance(key, basestring):
			resultTerm.append(key)
		else:
			resultTerm.append(replaceKeys(key, sheet, rowIdx))
	return resultTerm

def getKeys(path):
	try:
		xlsDoc = xlrd.open_workbook(path)
		sheet = xlsDoc.sheet_by_index(0)
		keyArray = []
		row = sheet.row(0)
		for key in row:
			keyArray.append(key.value)
		return keyArray
	except KeyError:
		print 'could not open file'

def getColumnForKey(key, sheet):
	row = sheet.row(0)
	idx = 0
	for rowKey in row:
		if rowKey.value == key:
			return idx
		idx += 1
	return -1
