#!/usr/bin/python

from pyparsing import *
import CSVParser
import MLRCalc

keyword = Literal("lm").suppress()
leftBracket,rightBracket,semicolon,comma,equal,tild = map(Suppress, "();,=~")
# variable = Regex('[a-zA-Z_][a-zA-Z_0-9]*')
variable = Word(alphas)
integer = Regex('([+-]?(([1-9][0-9]*)|0+))')

delimiter = Literal("|").suppress()
operator = oneOf("+ - * / [ ] . ^ { } %")

alpha = variable("alpha")
beta = variable("beta")
data = oneOf("csv sql excel")
path = Regex(r'([A-Z][:][\\])?[a-zA-Z_0-9.\\]*')
dataTerm = data + equal + path
dataTerms = delimitedList(dataTerm("dataTerms*"))

termPart  = nestedExpr() | nestedExpr('{','}') | integer | operator
termExpr  = OneOrMore(termPart)
term = (Optional(beta) + Optional(termExpr) + Optional(beta))
variables = delimitedList(term("variables*"))


dsl = keyword + leftBracket + alpha + tild + variables + delimiter + dataTerms + rightBracket
inputString = delimitedList(dsl("inputString*"), delim=semicolon)

#result = dsl.parseString("lm(y ~ x1+2, x2, x3 | csv=mydata,sql=asdf);")
# result = dsl.parseString(r"lm(Sales ~ TV * 2, 4 / Radio, 3 * (Newspaper ^ 2) | csv=advertising.csv);")
code = """
lm(Sales ~ TV * 2, 4 / Radio, 3 * (Newspaper ^ 2) | csv=advertising_test.csv);
lm(Sales ~ TV, Radio, Newspaper | csv=advertising.csv)
"""


result = inputString.parseString(code)
test = code.split(';')
# print result.inputString
# print result.alpha
# print result.variables
# print result.beta
idx = 0
for linearRegression in result.inputString:
	print test[idx].strip()
	for dataSource in linearRegression.dataTerms:
		if dataSource[0] == 'csv':
			dataArray = []
			for term in linearRegression.variables:
				dataArray.append(CSVParser.csvDataFromTerm(term, dataSource[1]))
			xArray = []
			for i in range(len(dataArray)):
				xArray.append(dataArray[i])
			MLRCalc.calcCoeff(CSVParser.csvDataFromTerm([linearRegression.alpha], dataSource[1]), xArray)
			# MLRCalc.calcCoeff([3], [[3,7,4,2], [6,7,3,9]])
			# result = MLRCalc.invert([[2,1,1], [3,2,1], [2,1,2]])
			# print "Inverted Matrix:"
			# print result
		elif dataSource[0] == 'sql':
			print 'asdf'
		elif dataSource[0] == 'excel':	
			print 'asdf'
	idx += 1