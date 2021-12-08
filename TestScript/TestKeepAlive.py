import socket
import time

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect(('localhost', 8080))

s.send('GET / HTTP/1.1\r\nConnnection: Keep-Alive\r\n\r\n'.encode('UTF-8'))
print(s.recv(1024))
print(s.recv(1024))

time.sleep(5)

s.send('GET / HTTP/1.1\r\nConnection: Close\r\n\r\n'.encode('UTF-8'))
print(s.recv(1024))
print(s.recv(1024))
s.shutdown(socket.SHUT_RDWR)