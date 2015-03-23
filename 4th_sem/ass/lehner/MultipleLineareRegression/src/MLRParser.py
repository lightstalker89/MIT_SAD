#!/usr/bin/python

from pyparsing import *

keyword = Literal("lm").suppress()
leftBracket,rightBracket,semicolon,comma,equal,tild = map(Suppress, "();,=~")
variable = Regex('[a-zA-Z_][a-zA-Z_0-9]*')
integer = Regex('([+-]?(([1-9][0-9]*)|0+))')

delimiter = Literal("|").suppress()
operator = oneOf("+ - * / [ ] . ^ { } %")

alpha = variable("alpha")
data = oneOf("csv sql excel")
path = Regex(r'([A-Z][:][\\])?[a-zA-Z_0-9\\]*')
dataTerm = data + equal + path
dataTerms = delimitedList(dataTerm("dataTerms*"))

termPart  = nestedExpr() | nestedExpr('{','}') | integer | operator
termExpr  = OneOrMore(termPart).setParseAction(keepOriginalText)
term = (Optional(variable) + Optional(termExpr) + Optional(variable))
variables = delimitedList(term("variables*"))


dsl = keyword + leftBracket + alpha + tild + variables + delimiter + dataTerms + rightBracket + semicolon

#result = dsl.parseString("lm(y ~ x1+2, x2, x3 | csv=mydata,sql=asdf);")
result = dsl.parseString(r"lm(Sales ~ TV * 2, 4 / Radio, 3 * (Newspaper ^ 2) | csv=C:\asdf\asdf,sql=F:\hallo\hallo);")
print result.alpha
print result.variables
print result.dataTerms