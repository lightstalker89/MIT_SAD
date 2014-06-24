#!/usr/bin/env python

import socket

UDP_IP = '<broadcast>'
UDP_PORT = 5005
MESSAGE = "Hello, World!"

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
sock.sendto(MESSAGE, (UDP_IP, UDP_PORT))
