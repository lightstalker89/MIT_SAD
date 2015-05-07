import sqlite3
import ArithmParser

def sqlDataFromTerm( term, path ):
	try:
		keys = getKeys(path)
		conn = sqlite3.connect(path)
		cursor = conn.cursor()
		resultData = []
		# get Table name	
		cursor.execute("SELECT name FROM sqlite_master WHERE type='table';")
		tableName = cursor.fetchone()[0]
		cursor.execute("SELECT * FROM {};".format(tableName))
		count = len(cursor.fetchall())
		for row_idx in range(0, count):
			#replace keys
			resultTerm = replaceKeys(term, cursor, row_idx, keys)
			# check if term is available or not
			if len(resultTerm) > 1:
				result = ArithmParser.parse(resultTerm)
			else:
				result = float(resultTerm[0])
			resultData.append(result)
		conn.close()
		return resultData
	except KeyError:
		print 'invalid dictionary key: ', key

def replaceKeys(term, cursor, rowIdx, keys):
	resultTerm = []
	# get Table name	
	cursor.execute("SELECT name FROM sqlite_master WHERE type='table';")
	tableName = cursor.fetchone()[0]
	for key in term:
		if key in keys:
			cursor.execute('SELECT {} FROM {}'.format(key, tableName))
			data = cursor.fetchall()[rowIdx][0]
			resultTerm.append(data)
		elif isinstance(key, basestring):
			resultTerm.append(key)
		else:
			resultTerm.append(replaceKeys(key, cursor, rowIdx, keys))
	return resultTerm

def getKeys(path):
	try:
		conn = sqlite3.connect(path)
		cursor = conn.cursor()
		# get Table name
		cursor.execute("SELECT name FROM sqlite_master WHERE type='table';")
		tableName = cursor.fetchone()[0]
		# get column names 
		cursor.execute("SELECT * FROM {};".format(tableName))
		columnNames = cursor.description
		keys = []
		for column in columnNames:
			keys.append(column[0])
		conn.close()
		return keys
	except KeyError:
		print 'could not open file'