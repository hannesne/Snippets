Certs
====


Install Certbot
----

```Bash
sudo apt-get update
sudo apt-get install software-properties-common
sudo add-apt-repository ppa:certbot/certbot
sudo apt-get update
sudo apt-get install certbot 
```

[Link](https://certbot.eff.org/lets-encrypt/ubuntuxenial-other)

Download cert
----

```Bash
sudo certbot certonly --manual
```

[Link](https://certbot.eff.org/docs/using.html#manual)


Install OpenSSL
----

```Bash
sudo apt-get install openssl
```

Convert from pem to pfx
----

```Bash
openssl pkcs12 -inkey bob_key.pem -in bob_cert.cert -export -out bob_pfx.pfx
```

Convert from pem to cer
----
```Bash
sudo openssl x509 -outform der -in /etc/letsencrypt/live/nel.kiwi/fullchain.pem -out hannestest.cer
```

Base64 encode file content
----

```Bash
cert=$( base64 <certificate path and name>.pfx )
```

```Powershell
[System.Convert]::ToBase64String([System.IO.File]::ReadAllBytes("<certificate path and name>.pfx"))
```