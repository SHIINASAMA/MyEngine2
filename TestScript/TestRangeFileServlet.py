import warnings
warnings.filterwarnings('ignore')
import urllib.request

url = 'http://localhost:8080/index.txt'
# url = 'http://baidu.com/index.html'

header = {
    'Range':'bytes 1-2'
}
request = urllib.request.Request(url, headers = header)
context = urllib.request.urlopen(request).read()
print(context)