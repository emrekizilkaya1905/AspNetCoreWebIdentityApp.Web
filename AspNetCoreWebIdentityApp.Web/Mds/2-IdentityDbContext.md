## Identity DB Context
Efcore tarafinda veri tabanimiza karsilik gelen bir sinifimiz var bu dbcontexttir.
Bu dbcontext sinifimiz IdentityDbContext sinifindan turetilir ve bu sayede Identity tarafinda kullanilan tablolar otomatik olarak olusturulur. 
Buradan identitydbuser ve identitydbrole gibi tablolar olusturulur. Bu iki tablo özel oldugu icin set edebiliyoruz.
Diger tablolar icin dbcontext tarafindan olusturabiliriz. 
3 tane parametre aliyor. IdentityDbContext<IdentityUser, IdentityRole, string> seklinde.
3. parametre string olmasi gerekiyor cunku identity tarafinda idler guid olarak tutuluyor. Identity api bizim icin kullanici olustururken random guid degerler olusturacak.
Kullanicinin bir verisi uzerinden yetkilendirme yapiyorsam claimi kullanabilirim.
Bu claimlerdeki key valueleri cookie icerisinde bulunuyor. Bu sekilde de claim bazli yetkiendirme yapabilecegiz.
Facebook, google gibi hesaplardan girdigimizde claimlerde tutabiliriz.
Claim, role, policy bazli yetkilendirme yaparken cookie uzerinden yapacagiz.
Cookie icerisindeki butun datalar claim key-value datalaridir.