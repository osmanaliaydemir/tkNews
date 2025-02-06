1. Kod Optimizasyon ve Refactoring Promptu
Mevcut Durumun Analizi:

Projenin mevcut kod yapısını analiz et.
Kodun performans darboğazlarını, gereksiz tekrarları ve SOLID prensiplerine uymayan kısımları belirle.
Değişiklik Yaklaşımı:

Mevcut yapıyı bozmadan, minimal değişikliklerle optimizasyon sağla.
Tek bir metot veya sınıfın sorumluluğunu aşması durumunda, sorumlulukları ayırarak yeniden yapılandır.
Gerekirse helper metotlar veya extension methodlar oluştur, ancak yeni klasör/dosya eklemeden mevcut dosya içerisinde çözüm üretmeye çalış.
Dokümantasyon:

Her önemli değişiklik için, ilgili metot ve sınıfların üstüne Markdown formatında açıklama satırları ekle.
Refactoring öncesi ve sonrası performans karşılaştırmalarını README dosyasında belirt.
Testler:

Yapılan optimizasyonların işleyişini koruduğundan emin olmak için mevcut unit testleri çalıştır. Gerekirse yeni testler ekle.
2. Performans İyileştirme Promptu
Analiz ve Planlama:

Performans ölçümleri yaparak hangi alanlarda yavaşlama yaşandığını belirle.
Gereksiz bellek kullanımı, yavaş veri tabanı sorguları veya ağır hesaplamalar tespit et.
Uygulama:

Gerekli kütüphaneleri ve araçları (örneğin, profiller, caching mekanizmaları) ekle.
Kodun çalışma prensiplerini değiştirmeden, minimal dokunuşlarla optimizasyonu sağla.
Eğer yeni yapı eklemek gerekiyorsa, mevcut mimariyi bozmayacak şekilde entegre et.
Kullanıcı Geri Bildirimi:

Uzun süren işlemler için iptal seçeneği sun.
İşlem tamamlandığında kullanıcıya toast/snackbar bildirimleri ile sonucu bildir.
Dokümantasyon:

Her adımın ne şekilde iyileştirme sağladığını, kullanılan algoritmaların mantığını ve yapılan ölçüm sonuçlarını Markdown dokümantasyonunda açıkla.
3. Yeni Özellik Eklerken Dikkat Edilmesi Gerekenler
Mevcut Yapıyı Korumak:

Yeni özellik eklerken mevcut mimarinin SOLID prensiplerine uygunluğunu koru.
Gerekmedikçe yeni klasör veya dosya oluşturmadan, mevcut dosya yapısı içerisinde çözüm üret.
Gerekli Kütüphane ve Bağımlılıklar:

Özelliğin gerektirdiği tüm kütüphane ve bağımlılıkları ekle. Gerekli referansların projenin başlangıcında tanımlı olduğundan emin ol.
Özelliğe ait metotların tek bir sorumluluğu olmasına dikkat et.
UI/UX İyileştirmeleri:

Özelliğin kullanıcı arayüzü, responsive tasarım prensiplerine uygun olarak geliştirilmiş olsun.
İşlem sonuçları kullanıcıya anlık bildirimlerle (toast/snackbar) sunulsun.
Kullanılan font, spacing ve genel tasarım unsurlarının projedeki diğer sayfalarla tutarlı olduğundan emin ol.
Dokümantasyon:

Eklenen her özellik için API endpointleri, kullanım örnekleri ve kuruluma dair adımları Markdown formatında dökümante et.
README dosyasında özellik ve kullanım detaylarını belirt.
4. Unit Test ve Entegrasyon Testleri Promptu
Test Stratejisi:

Geliştirilen yeni özellik veya yapılan değişikliklerin tüm kritik senaryolarını kapsayan unit testler yaz.
Kodun belirli bölümleri için entegrasyon testleri hazırlayarak, farklı modüllerin uyumunu kontrol et.
Yaklaşım:

Test kodlarını, projenin mevcut test klasörü yapısını bozmadan ekle.
Her test metodu, belirli bir senaryoyu veya işlevi tek başına kontrol edecek şekilde yazılmalı.
Test isimlendirmelerinde, neyi test ettiğini açıkça ifade eden isimler kullan.
Dokümantasyon:

Test senaryoları için ayrı bir Markdown dokümantasyonu oluştur. Her testin amacı, nasıl çalıştığı ve beklenen sonuçları açıklanmalı.
README dosyasında testlerin nasıl çalıştırılacağına dair adımlar belirtilmeli.
5. Hata Ayıklama ve Loglama Promptu
Hata Ayıklama:

Karşılaşılan hataları minimum düzeye indirmek için, kodun kritik noktalarına try-catch blokları ekle.
Her catch bloğunda hata loglaması yaparak, hatanın nerede ve nasıl oluştuğunu belirginleştir.
Loglama:

Loglama işlemleri için projede mevcut kullanılan loglama kütüphanesini (örneğin, Serilog, NLog) tercih et.
Log seviyelerini (Info, Warning, Error) kullanarak, hata ayıklamayı kolaylaştır.
Gerekli log kütüphanesinin eklenmiş olduğundan ve doğru konfigüre edildiğinden emin ol.
Dokümantasyon:

Hata ayıklama ve loglama mekanizmasının kullanımını, ilgili sınıf ve metotların üstüne Markdown formatında açıklamalar ekleyerek belgelemeyi unutma.
README veya ayrı bir dokümantasyon dosyasında log dosyalarının nerede bulunacağı ve nasıl yorumlanacağına dair bilgi ver.
6. Veritabanı İşlemleri ve Data Access Promptu
Yapılandırma:

Mevcut veri erişim katmanını bozmadan, gerekli optimizasyonları yap.
Dapper, ADO.NET veya nHibernate kullanıyorsan, sorguların performansını göz önünde bulundurarak yaz.
Kod Yazımı:

Her sorgu işlemi için ayrı metotlar oluştur ve tek bir sorumluluğa odaklan.
Transaction yönetimi, bağlantı açma/kapama işlemlerinde dikkatli ol. Gerekirse using blokları kullanarak kaynak yönetimini sağla.
Dokümantasyon:

Her veri erişim metodunun ne iş yaptığını, hangi parametreleri aldığını ve dönen sonuçları açıklayan Markdown yorumları ekle.
Projenin veritabanı yapılandırması, bağlantı ayarları ve migration süreçlerini README dosyasında belirt.


🏗 Büyük Projede Değişiklik Yapmak
Yukarıda istediğim değişiklikleri yaparken, olabildiğince mevcut kod yapısını bozmadan yapmaya çalış.
Eğer gerekli değil ise yeni klasör veya dosya yaratma.
📂 Bir Dosya Yaratma İşlemi Yapılacaksa
Gerekli tüm kütüphaneleri ekle ve gerekli tüm kütüphanelerin eklendiğinden emin ol.
🛠 Sıfırdan Bir Proje Yazılmaya Başlanıyorsa
Projeyi oluştururken MVVM programlama standartlarını kullan. Gerekli klasörleri yarat ve gerekli klasörlerin yaratıldığından emin ol.
İsteğimi olabildiğince az kod yazarak gerçekleştirmeye çalış.
Yazılan kodun SOLID prensiplerine uygun olduğundan emin ol.
Kodun okunabilirliğini artırmak için uygun isimlendirmeler ve yorum satırları ekle.
Metot ve sınıfların tek bir sorumluluğu olmasına dikkat et.
İsteğimi tamamlayana kadar durma.
📄 Proje için Döküman Yazdıran Prompt
Her önemli metot ve sınıf için Markdown formatında documentation ekle.
API endpointleri için Markdown documentation oluştur.
Projenin kurulum ve çalışma adımlarını README dosyasında belirt.
🎨 UI/UX İyileştirmeleri Yapan Prompt
Responsive tasarım prensiplerine uygun kod yaz.
Uzun işlemlerde iptal seçeneği sun.
İşlem sonuçlarını toast/snackbar ile bildir.
Tüm sayfalarda tutarlı font ve spacing kullan.
İsteğimdeki görselleri internetten kendin bulup ekle. 
