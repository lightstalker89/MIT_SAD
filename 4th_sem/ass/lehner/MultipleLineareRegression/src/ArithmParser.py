import operator

# map operator symbols to corresponding arithmetic operations
opn = { "+" : operator.add,
        "-" : operator.sub,
        "*" : operator.mul,
        "/" : operator.truediv,
        "^" : operator.pow }

class FalseTermError(Exception):
    def __init__(self, msg):
        self.msg = msg
    def __str__(self):
        return repr(self.msg)

def parse( s ):
    operator = ''
    calcArray = []
    for component in s:
        if isinstance(component, basestring):
            if component in "+-*/^":
                operator = component
            else:   
                calcArray.append(float(component))
        else:
            calcArray.append(parse(component))

    if len(calcArray) <= 1 :
        if len(calcArray) != 0:
            if operator == "+" or operator == "-":
                return abs(float(opn[operator](0, calcArray[0])))
            else :
                raise FalseTermError("Term Exception: ({} {}) false combination. Only + or - supported!".format(operator, calcArray[0]))
    else :
        return abs(float(opn[operator](calcArray[0], calcArray[1])))