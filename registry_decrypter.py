import base64
from Crypto.Cipher import AES

key = 'dYrQM2pRZMYEFEha'.encode('utf-8')
regfile = open('eastreg.ini', 'rb')
content = base64.b64decode(regfile.readline())
aes = AES.new(key, AES.MODE_CBC, iv=bytearray(16))
res = aes.decrypt(content)
print(res)