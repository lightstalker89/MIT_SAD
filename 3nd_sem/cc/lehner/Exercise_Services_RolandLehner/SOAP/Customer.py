class Customer:
	def __init__(self, name):
		self.name = name
		self.orders = []

	def __str__(self):
		return '[Customer: %s orders: %s]' % (self.name, self.orders)
		