import warnings
warnings.filterwarnings('ignore')
from urllib import request

response = request.urlopen('http://localhost:8080/Test.Log')
context = response.read()
print(context)