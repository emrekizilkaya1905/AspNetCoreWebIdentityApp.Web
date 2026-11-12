## asp-for
Bu tag sayesinde hem tipini, hem idsini, hem de name'ini otomatik olarak oluşturabiliyoruz. 
Kullanici ile islemleri yapmak icin usermanager sinifini kullanacagiz.
## TempData
2 request arasinda veri tasimak icin kullanilir. Bu veri bir sonraki requestte silinir. ViewBag ve ViewData ise sadece 1 request icin gecerlidir.
## Validation-For
Hatalarin gorulebilmesi icin kullanacagiz. Ilgili alanin altina koyacagiz.
Annotationu kullanmak yetmez. Ayrica controllerda da kontrol etmemiz gerekir.
## Asp-Validation-Summary
Modelonly demek, sadece modeldeki hatalari gosterir. 
Identityresulttaki create async metodu sayesinde ayni kullanici adi ile kayit yapilmaya calisildiginda, hatayi yakalayip, modelstate'e ekleniyor. Bu sekilde asp-validation-summary ile gosterilebilir.
## MVC AREA
Area sayesinde controller ve view'leri ayri klasorlerde tutabiliyoruz. Bu sayede projenin buyuklugune gore, controller ve view'leri ayri ayri klasorlerde tutmak daha kolay oluyor.
## CLIENT SIDE VALIDATION
Backend tarafinda yaptik simdi client tarafinda da yapalim.
RenderSectionAsync metodu sayesinde, ilgili view'de tanimlanan section'lari layoutta render edebiliyoruz. Bu sayede, layoutta tanimlanan scriptler, ilgili view'de de calisiyor.
Validition scripts 2 tane script dosyasi jquery kutuphanesinden sonra calisiyor.
## Password Validation Options
Startup extensions.cs dosyasinda, password validation options ayarlari yapilabilir. Bu sayede, kullanici sifre olustururken, belirli kurallara uymak zorunda kalir. Ornegin, sifre en az 8 karakter uzunlugunda olmalidir, en az bir buyuk harf icermelidir, en az bir kucuk harf icermelidir, en az bir rakam icermelidir ve en az bir ozel karakter icermelidir. Bu ayarlar sayesinde, kullanici sifre olustururken daha guvenli sifreler olusturabilir.
## IPassword Validatior
Username Ahmet ama sifre ahmet olmasin gibi.
## IUserValidator
Veritabininda ayni username varsa otomatikman zaten identity framework bunu engeller.
## IdentityErrorDescriber
IdentityErrorDescriber sinifi sayesinde, identity framework tarafindan uretilen hatalarin mesajlarini degistirebiliriz. Ornegin, kullanici adi zaten var ise, "Bu kullanici adi zaten kullaniliyor" gibi bir mesaj gosterilebilir. Bu sayede, kullaniciya daha anlamli ve kullanisli mesajlar gosterilebilir.
## SiginInAsync
Cookie olusturacagiz. Burada signinmanager sinifini kullacagiz.
urli kullandik cunku kullanici login olduktan sonra, login olmadan onceki sayfaya donmesini istiyoruz.
 Ispersistent: kullancinin bilgilerini cookie de tutulup tutmayacagini belirler. RememberMe: kullanici bilgilerini hatirlamasini(bizim belirledigimiz zure) istiyorsa true, istemiyorsa false olarak ayarlanir.
 model.RememberMe, isPersistent parametresine gider:
- İşaretliyse (true): Kalıcı cookie oluşturulur. Süresi dolmadıysa tarayıcıyı kapatıp açınca giriş devam edebilir.
- İşaretli değilse (false): Oturum cookie’si oluşturulur. Normalde tarayıcı oturumu bitince silinir; ancak tarayıcının oturum geri yükleme özelliği bunu koruyabilir.
  4.parametre : lockoutOnFailure: kullanici bilgilerini yanlis girerse, kullaniciya lockout uygulanip uygulanmayacagini belirler. Biz false olarak ayarladik.
  var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, false);
## LockoutOnFailure
Kullanici bilgilerini yanlis girerse, kullaniciya lockout uygulanip uygulanmayacagini belirler. Biz false olarak ayarladik.
AccessFailedCount: kullanici bilgilerini yanlis girerse, kullaniciya lockout uygulanip uygulanmayacagini belirler. Biz false olarak ayarladik.
LockoutEnabled: Ilgili kullanici da locked out mekanizmasi calisacak mi calismayacak mi onu belirler. Biz false olarak ayarladik.
LockoutEnd: Ne zaman lockout mekanizmasinin bitecegini belirler. Biz null olarak ayarladik.
## Cookie Options
Program.cs dosyasında cookie ayarlarını yapabiliriz. Bu sayede, kullanici bilgilerini cookie de tutulup tutulmayacagini belirleyebiliriz.
Ornegin, cookie'nin HttpOnly olup olmayacagini, expire time span'ini, login path'ini, access denied path'ini ve sliding expiration'ini ayarlayabiliriz. 
Bu ayarlar sayesinde, kullanici bilgilerini daha guvenli bir sekilde tutabiliriz.
 SlidingExpiration özelliği, kullanıcının oturumunun süresini uzatmak için kullanılır. Eğer kullanıcı aktifse, oturum süresi uzatılır.
 Bu sayede, kullanıcı oturumunu kaybetmeden uygulamayı kullanmaya devam edebilir.
 ## Authorize Attribute
 Eger bir controller veya action methoduna [Authorize] attribute'u eklersek, o controller veya action methoduna sadece login olan kullanıcılar erişebilir. Login olmayan kullanıcılar, login sayfasına yönlendirilir.
 Authorize yaptigimizda program.csde login path'ini ayarladigimiz icin, login olmayan kullanici login sayfasina yonlendiriliyor.
 Girdikten sonra returnurl parametresi su an calismitor. Bunun sebebi singup.cshtmlde form taginin action attribute'unde returnurl parametresini gondermedik. Bunu gonderirsek, login olan kullanici login olmadan onceki sayfaya doner.
 Sadece post dersek singup.cshtmlde bu problemi cozuyoruz. Cunku post methodu ile login olan kullanici login olmadan onceki sayfaya donuyor. 
 Burada kritik olan su: Farkli controllera gidersek sikinti yasariz.