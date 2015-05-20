#!/usr/bin/python

from pyparsing import *
import sys
import CSVParser
import XLSParser
import SQLParser
import MLRCalc
import platform

# error classes
class DuplicateKeysError(Exception):
	def __init__(self, msg):
		self.msg = msg
	def __str__(self):
		return repr(self.msg)

class KeyNotFoundError(Exception):
	def __init__(self, msg):
		self.msg = msg
	def __str__(self):
		return repr(self.msg)

# check if key/variable is duplicate
def isKeyDuplicate(variables, sources):
	for variable in variables:
		trueCount = 0
		for source in sources:
			keys = []
			if source[0] == 'csv':
				keys = CSVParser.getKeys(source[1])
			elif source[0] == 'sql':
				keys = SQLParser.getKeys(source[1])
			elif source[0] == 'excel':
				keys = XLSParser.getKeys(source[1])

			if variable.beta:
				if variable.beta in keys:
					trueCount += 1
			# has nested term
			else:
				for var in variable:
					if isinstance(var, basestring) is not True:
						for key in var:
							if key in keys:
								trueCount += 1
		if trueCount > 1:
			msg = 'Duplicated x-value found in sources: '
			for sourceString in sources:
				msg += ' ' + sourceString[1]
			raise DuplicateKeysError(msg)

# get the corresponding datasource for the key/variable
def getDataSourceFromKey(key, dataSources):
	for dataSource in dataSources:
		keys = []
		if dataSource[0] == 'csv':
			keys = CSVParser.getKeys(dataSource[1])
		elif dataSource[0] == 'sql':
			keys = SQLParser.getKeys(dataSource[1])
		elif dataSource[0] == 'excel':	
			keys = XLSParser.getKeys(dataSource[1])

		if isinstance(key, basestring) is not True:
			for keyValue in key:
				if keyValue in keys:
					return dataSource
		else:
			if key in keys:
				return dataSource
	return ''

# definition of the DSL and parsing of the input string
def parseFile(code):
	keyword = Literal("lm").suppress()
	leftBracket,rightBracket,semicolon,comma,equal,tild = map(Suppress, "();,=~")

	variable = Word(alphas)
	digit = Regex('[1-9][0-9]*')

	delimiter = Literal("|").suppress()
	operator = oneOf("+ - * / [ ] . ^ { } %")

	alpha = variable("alpha")
	beta = variable("beta")
	data = oneOf("csv sql excel")
	if platform.system() == "Windows":
		path = Regex(r'([A-Z][:][\\d])?[a-zA-Z_0-9.\\]*')
	else:
		path = Regex(r'[/a-zA-Z_0-9.]*')
	dataTerm = data + equal + path
	dataTerms = delimitedList(dataTerm("dataTerms*"))

	termPart  = nestedExpr() | digit | operator
	termExpr  = OneOrMore(termPart)
	term = (Optional(beta) + Optional(termExpr) + Optional(beta))
	variables = delimitedList(term("variables*"))


	dsl = keyword + leftBracket + alpha + tild + variables + delimiter + dataTerms + rightBracket
	inputString = delimitedList(dsl("inputString*"), delim=semicolon)

	result = inputString.parseString(code)
	rawInput = code.split(';')

	# get values for the key/variables from corresponding datasource
	idx = 0
	for linearRegression in result.inputString:
		isKeyDuplicate(linearRegression.variables, linearRegression.dataTerms)
		print ''
		print rawInput[idx].strip()
		dataArray = []
		for term in linearRegression.variables:
			for key in term:
				data = getDataSourceFromKey(key, linearRegression.dataTerms)
				if data:
					if data[0] == 'csv':
						dataArray.append(CSVParser.csvDataFromTerm(term, data[1]))
					elif data[0] == 'sql':
						dataArray.append(SQLParser.sqlDataFromTerm(term, data[1]))
					elif data[0] == 'excel':
						dataArray.append(XLSParser.xlsDataFromTerm(term, data[1]))				

		xArray = []
		for i in range(len(dataArray)):
			xArray.append(dataArray[i])

		# get data for the Y key/variable
		alphaDataSource = getDataSourceFromKey(linearRegression.alpha, linearRegression.dataTerms)
		if alphaDataSource:
			data = []
			if alphaDataSource[0] == 'csv':
				data = CSVParser.csvDataFromTerm([linearRegression.alpha], alphaDataSource[1])
			elif alphaDataSource[0] == 'sql':
				data = SQLParser.sqlDataFromTerm([linearRegression.alpha], alphaDataSource[1])
			elif alphaDataSource[0] == 'excel':
				data = XLSParser.xlsDataFromTerm([linearRegression.alpha], alphaDataSource[1])
			MLRCalc.calcCoeff(data, xArray)
		else:
			raise KeyNotFoundError('Key not found in sources: ' + linearRegression.alpha)
		idx += 1

# entry level
try:
	if len(sys.argv) == 2:
		filepath = str(sys.argv[1])
		f = open(filepath, "r")
		parseFile(f.read())
	else:
		print "Incorrect Argument!"
except IOError as e:
    print "I/O error({0}): {1}".format(e.errno, e.strerror)
except ValueError:
    print "Could not read data from file."
except:
    print "Unexpected error:", sys.exc_info()[0]
    raise



