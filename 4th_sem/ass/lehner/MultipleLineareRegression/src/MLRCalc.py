# multiply matrix
def multiplyMatrix(m1, m2):
	# result with zero values
	result = []
	for x in range(len(m1)):
		result.append([0] * len(m1))

	# iterate through rows of X
	for i in range(len(m1)):
		# iterate through columns of Y
		for j in range(len(m2[0])):
   			# iterate through rows of Y
			for k in range(len(m2)):
				result[i][j] += m1[i][k] * m2[k][j]
	return result

def invert(matrix):

    # calcMatrix will be used for calculations
    calcMatrix = matrix
    # generate the identity matrix
    identity = []
    #row
    for i in xrange(0, len(calcMatrix)):
        row = []
        # colum
        for j in xrange(0,len(calcMatrix[0])):
            elem = 0
            if i==j:
                elem = 1
            row.append(elem)
        identity.append(row)

    for j in xrange(0, len(calcMatrix[0])):
        # calc row with 1 at first
        # 1 pos = i==j therefore j is indicator for 1 row
        multi = 1.0
        if float(calcMatrix[j][j]) != 1.0:
            multi = 1.0 / float(calcMatrix[j][j])
            for pos in xrange(0, len(calcMatrix[0])):
                calcMatrix[j][pos] = float(calcMatrix[j][pos]) * multi
                identity[j][pos] = float(identity[j][pos]) * multi
        for i in xrange(0, len(calcMatrix)):
            if float(calcMatrix[i][j]) != 0.0 and i!=j:
                multi = - float(calcMatrix[i][j])
                for pos in xrange(0, len(calcMatrix[0])):
                    # j is indicator for 1 row position.
                    calcMatrix[i][pos] = multi * float(calcMatrix[j][pos]) + float(calcMatrix[i][pos])
                    identity[i][pos] = multi * float(identity[j][pos]) + float(identity[i][pos])

    return identity

def calcCoeff(yArray, xArray):
	# create 1 Array
    oneArray = [1] * len(xArray[0])
    transponierteMatrixX = [oneArray]
    for element in xArray:
        transponierteMatrixX.append(element)

    # generate matrixX
    matrixX = []
    for one in oneArray:
        matrixX.append([one])

    for x in xArray:
        for element in range(len(x)):
            matrixX[element].append(x[element])

    #generate matrixY
    matrixY = []
    for element in yArray:
        matrixY.append([element])

    # for element in range(len(yArray)):
    #     matrixY[element].append(yArray[element])

	# calc (X^T*X)^-1
    firstTerm = invert(multiplyMatrix(transponierteMatrixX, matrixX))

    # calc X^T*y
    secondTerm = multiplyMatrix(transponierteMatrixX, matrixY)

    # result
    result = multiplyMatrix(firstTerm, secondTerm)
    print "Coefficient Result:"
    idx = 0
    while idx < len(result):
        print "Beta " + str(idx) + ": " + str(result[idx][0])
        idx += 1


	
