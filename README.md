# tkNews API

tkNews, modern bir haber/blog platformu için geliştirilmiş bir .NET Core Web API projesidir.

## Özellikler

- 🔐 Kapsamlı Kimlik Doğrulama ve Yetkilendirme
  - JWT tabanlı kimlik doğrulama
  - Rol tabanlı yetkilendirme (RBAC)
  - İzin tabanlı yetkilendirme
  - Refresh token desteği
  - Email doğrulama
  - Şifre sıfırlama

- 📱 API Özellikleri
  - RESTful API tasarımı
  - Swagger/OpenAPI dokümantasyonu
  - CORS desteği
  - Rate limiting
  - Caching

- 🏗️ Mimari
  - Clean Architecture
  - CQRS pattern
  - Repository pattern
  - Unit of Work pattern
  - Domain-Driven Design (DDD) prensipleri

- 🔧 Teknik Altyapı
  - .NET 8.0
  - Entity Framework Core
  - Microsoft SQL Server
  - ASP.NET Core Identity
  - AutoMapper
  - FluentValidation

## Başlangıç

### Gereksinimler

- .NET 8.0 SDK
- SQL Server (LocalDB veya SQL Server Express)
- Visual Studio 2022 / VS Code / JetBrains Rider

### Kurulum

1. Repoyu klonlayın:
```bash
git clone https://github.com/osmanaliaydemir/tkNews.git
```

2. Proje dizinine gidin:
```bash
cd tkNews
```

3. Bağımlılıkları yükleyin:
```bash
dotnet restore
```

4. Veritabanını oluşturun:
```bash
dotnet ef database update --project src/tkNews.Infrastructure --startup-project src/tkNews.API
```

5. Projeyi çalıştırın:
```bash
dotnet run --project src/tkNews.API
```

### Konfigürasyon

`appsettings.json` dosyasında aşağıdaki ayarları yapılandırın:

- Veritabanı bağlantı dizesi
- JWT ayarları
- Email ayarları
- CORS politikaları

## API Dokümantasyonu

API dokümantasyonuna `/swagger` endpoint'inden erişebilirsiniz.

## Katkıda Bulunma

1. Bu repoyu fork edin
2. Feature branch'i oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'feat: add some amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## İletişim

Osman Ali Aydemir - [@osmanaliaydemir](https://github.com/osmanaliaydemir) 