#!flask/bin/python
from flask import Flask, jsonify
from flask import abort
from flask import make_response
from flask import request

app = Flask(__name__)

customers = [
	{
		'name': u'Koehler',
		'orders':
		[
			{
				'orderID': 1,
				'products':
				[
					{
						'product': u'headphones'
					},
					{
						'product': u'laptop'
					}
				]
			},
			{
				'orderID': 2,
				'products':
				[
					{
						'product': u'book'
					},
					{
						'product': u'tablet'
					}
				]
			}
		]
	}
]

# customer
@app.route('/orderService/api/v1.0/customers', methods=['GET'])
def get_customers():
    return jsonify({'customers': customers})

@app.route('/orderService/api/v1.0/customers/<string:customer_name>', methods=['GET'])
def get_customer(customer_name):
    customer = filter(lambda t: t['name'] == customer_name, customers)
    if len(customer) == 0:
        abort(404)
    return jsonify({'customer': customer[0]})

@app.route('/orderService/api/v1.0/customers', methods=['POST'])
def create_customer():
    if not request.json or not 'name' in request.json:
        abort(400)
    customer = {
        'name': request.json['name'],
        'orders': request.json.get('orders', [])
    }
    customers.append(customer)
    return jsonify({'customer': customer}), 201

@app.route('/orderService/api/v1.0/customers/<string:customer_name>', methods=['DELETE'])
def delete_customer(customer_name):
    customer = filter(lambda t: t['name'] == customer_name, customers)
    if len(customer) == 0:
        abort(404)
    customers.remove(customer[0])
    return jsonify({'result': True})

# orders

@app.route('/orderService/api/v1.0/customers/<string:customer_name>/orders', methods=['GET'])
def get_orders(customer_name):
    customer = filter(lambda t: t['name'] == customer_name, customers)
    if len(customer) == 0:
        abort(404)
    orders = customer[0]["orders"]
    if len(orders) == 0:
        abort(404)
    return jsonify({'orders': orders})

@app.route('/orderService/api/v1.0/customers/<string:customer_name>/order/<int:order_id>', methods=['DELETE'])
def delete_order(customer_name, order_id):
    customer = filter(lambda t: t['name'] == customer_name, customers)
    if len(customer) == 0:
        abort(404)
    order = filter(lambda t: t['orderID'] == order_id, customer[0]["orders"])
    if len(order) == 0:
        abort(404)
    customer[0]["orders"].remove(order[0])
    return jsonify({'result': True})

@app.route('/orderService/api/v1.0/customers/<string:customer_name>/order', methods=['POST'])
def create_order(customer_name):
	customer = filter(lambda t: t['name'] == customer_name, customers)
	if len(customer) == 0:
		abort(404)
	if not request.json or not 'products' in request.json:
		abort(400)

	orderID = 1
	if len(customer[0]["orders"]) != 0:
		orderID = customer[0]["orders"][-1]["orderID"] + 1
	order = {
		'orderID': orderID,
		'products': request.json.get('products', [])
	}
	customer[0]["orders"].append(order)
	return jsonify({'order': order}), 201

@app.errorhandler(404)
def not_found(error):
    return make_response(jsonify({'error': 'Not found'}), 404)

@app.errorhandler(400)
def not_found(error):
    return make_response(jsonify({'error': 'Bad Request'}), 400)

if __name__ == '__main__':
    app.run(debug=True)