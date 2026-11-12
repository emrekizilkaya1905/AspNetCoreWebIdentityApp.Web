## Cookie Based Authentication
Sadece dis dunyaya datalari sundugumuz api uygulamasi ise token.
React ve angular ise backend ile haberlesmede gene token kullanilir.
Normal bir web uygulamasinda ise cookie kullanilir. 
## Nasil gerceklestiriliyor?
Kullanici web sitesine girdi, ilan vermek istediginde login olmasi gerekiyor.
Login olduktan sonra tarayicida bir cookie olusuyor. Ve bu tarayici bu cookieyi okuyabilir. Hangi domainde olusmus ise sadece o domainde okunabilir. 
Otomatik eklenir her requestte. Tarayici tarafindan cookie de ekleniyor. 
Server side uygulamalarda ekstra bir apiye ihtiyac yoktur.