import operator

# map operator symbols to corresponding arithmetic operations
opn = { "+" : operator.add,
        "-" : operator.sub,
        "*" : operator.mul,
        "/" : operator.truediv,
        "^" : operator.pow }

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
    return float(opn[operator](calcArray[0], calcArray[1]))
            
    # op = s.pop()
    # print "op"
    # print op
    # if op == 'unary -':
    #     return -evaluateStack( s )
    # if op in "+-*/^":
    #     op2 = evaluateStack( s )
    #     op1 = evaluateStack( s )
    #     return opn[op]( op1, op2 )
    # else:
    #     return float( op )