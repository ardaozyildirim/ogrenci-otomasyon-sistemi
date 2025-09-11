# Öğrenci Yönetim Sistemi

## Proje Açıklaması

Bu öğrenci yönetim sistemi, eğitim kurumlarının öğrenci verilerini etkili bir şekilde yönetmelerini sağlayan tam teşekküllü bir web uygulamasıdır. Sistem; kullanıcı kimlik doğrulama, öğrenci ve öğretmen yönetimi, ders kaydı, not takibi ve yoklama izleme gibi özellikleri içerir.

Uygulama, domain, uygulama mantığı, altyapı ve sunum katmanları için temiz mimari desenini takip eder. Üç kullanıcı tipi (Yönetici, Öğretmen, Öğrenci) ile rol tabanlı erişim kontrolünü uygular.

## İçindekiler
- [Kullanılan Teknolojiler](#kullanılan-teknolojiler)
- [Sistem Özellikleri](#sistem-özellikleri)
- [Gereksinimler](#gereksinimler)
- [Kurulum ve Yapılandırma](#kurulum-ve-yapılandırma)
- [Uygulamayı Çalıştırma](#uygulamayı-çalıştırma)
- [API Dokümantasyonu](#api-dokümantasyonu)
- [Test Kullanıcı Bilgileri](#test-kullanıcı-bilgileri)
- [Frontend Özellikleri](#frontend-özellikleri)
- [Proje Yapısı](#proje-yapısı)
- [Docker Yapılandırması](#docker-yapılandırması)

## Kullanılan Teknolojiler

### Backend
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT Kimlik Doğrulama
- Docker

### Frontend
- Next.js 15
- TypeScript
- Tailwind CSS

### Altyapı
- Docker
- PostgreSQL 16

## Sistem Özellikleri

- **Kullanıcı Yönetimi**: Kayıt, kimlik doğrulama ve rol tabanlı yetkilendirme
- **Öğrenci Yönetimi**: Öğrenci kayıtları için CRUD işlemleri
- **Öğretmen Yönetimi**: Öğretmen kayıtları için CRUD işlemleri
- **Ders Yönetimi**: Ders oluşturma ve yönetimi
- **Not Takibi**: Öğrenci notlarının kaydedilmesi ve alınması
- **Yoklama Sistemi**: Öğrenci yoklamasının takibi
- **Yumuşak Silme**: Tüm varlıklar yumuşak silme özelliğini destekler
- **RESTful API**: İyi dokümante edilmiş API uç noktaları
- **Konteynerleştirme**: Kolay dağıtım için Docker desteği

## Gereksinimler

Başlamadan önce aşağıdaki yazılımların kurulu olduğundan emin olun:
- Docker Desktop
- Git (isteğe bağlı, depoyu klonlamak için)

## Kurulum ve Yapılandırma

1. Depoyu klonlayın:
   ```bash
   git clone <depo-url>
   cd ogrenci-otomasyon-sistemi
   ```

2. Uygulama konteynerleştirme kullandığı için ek kurulum gerekmez.

## Uygulamayı Çalıştırma

### Docker Compose Kullanarak (Önerilen)

1. Tüm servisleri başlatın:
   ```bash
   docker-compose up -d
   ```

2. Uygulamaya şu adreslerden erişebilirsiniz:
   - Frontend: http://localhost:3000
   - API: http://localhost:8080
   - Veritabanı: localhost:5432

3. Uygulamayı durdurmak için:
   ```bash
   docker-compose down
   ```

### Servislerin Özeti

- **PostgreSQL Veritabanı**: 
  - Konteyner adı: `student_management_db`
  - Port: 5432
  - Veritabanı: `student_management_system`
  - Kullanıcı: `postgres`
  - Şifre: `Student123!`

- **API Servisi**:
  - Konteyner adı: `student_management_api`
  - Port: 8080
  - URL: http://localhost:8080

- **Frontend Servisi**:
  - Konteyner adı: `student_management_frontend`
  - Port: 3000
  - URL: http://localhost:3000

## API Dokümantasyonu

API, Swagger kullanılarak dokümante edilmiştir ve şu adreste erişilebilir:
- http://localhost:8080 (API servisi çalışırken)

Dokümantasyon şunları sağlar:
- Tüm uç noktalar hakkında detaylı bilgi
- İstek/yanıt şemaları
- API uç noktalarını doğrudan test etme imkanı
- Her uç nokta için kimlik doğrulama gereksinimleri

## Test Kullanıcı Bilgileri

Sistem, test için önceden yüklenmiş örnek verilerle birlikte gelir:

### Örnek Kullanıcılar

1. **Yönetici Kullanıcı**:
   - E-posta: `admin@system.com`
   - Şifre: `admin123`
   - Rol: Yönetici

2. **Öğretmen Kullanıcı**:
   - Ad: John
   - Soyad: Doe
   - E-posta: `john.doe@university.com`
   - Şifre: (Yeni bir öğretmen hesabı kaydetmeniz veya şifreyi sıfırlamanız gerekir)
   - Rol: Öğretmen

3. **Öğrenci Kullanıcı**:
   - Ad: Alice
   - Soyad: Johnson
   - E-posta: `alice.johnson@student.com`
   - Şifre: (Yeni bir öğrenci hesabı kaydetmeniz veya şifreyi sıfırlamanız gerekir)
   - Rol: Öğrenci

### Kayıt Olma

Yeni kullanıcı oluşturmak için:
1. http://localhost:3000 adresine gidin
2. "Kayıt Ol" bağlantısına tıklayın
3. Gerekli bilgileri girin
4. Uygun rolü seçin (Öğretmen veya Öğrenci)
5. Formu gönderin

### Giriş Yapma

Sisteme giriş yapmak için:
1. http://localhost:3000 adresine gidin
2. "Giriş Yap" bağlantısına tıklayın
3. E-posta ve şifrenizi girin
4. Formu gönderin

**Not:** `admin@system.com` e-posta adresine sahip yönetici kullanıcısı sistemde önceden oluşturulmuştur. Varsayılan şifresi `admin123`'tür. Güvenlik nedeniyle bu şifreyi değiştirmeniz önerilir.

## Frontend Özellikleri

Modern ve profesyonel öğrenci yönetim sistemi frontend'i aşağıdaki özellikleri içerir:

### Tasarım ve Kullanıcı Deneyimi
- Responsive tasarım (mobil, tablet ve masaüstü uyumlu)
- Profesyonel renk paleti ve tipografi
- Gradient efektleri ve gölgelerle modern görünüm
- Akıcı animasyonlar ve geçişler
- Kullanıcı dostu arayüz

### Sayfalar ve Bileşenler
- **Giriş Sayfası**: Güvenli kimlik doğrulama ile profesyonel giriş formu
- **Dashboard**: Kişiselleştirilmiş istatistikler ve aktivite özeti
- **Notlar Sayfası**: Detaylı not listesi ve dağılım grafikleri
- **Yoklama Sayfası**: Yoklama kayıtları ve trend analizi
- **Dersler ve Öğrenciler Sayfaları**: Gelişmekte olan özellikler için profesyonel yer tutucular

### Teknik Özellikler
- Next.js 15 ile oluşturulmuş modern React uygulaması
- TypeScript ile tip güvenliği
- Tailwind CSS ile responsive ve özelleştirilebilir stil sistemi
- JWT tabanlı kimlik doğrulama
- REST API entegrasyonu
- Responsive tasarım prensipleri

## Proje Yapısı

```
ogrenci-otomasyon-sistemi/
├── Backend/
│   ├── StudentManagementSystem.API/          # Web API controller'ları ve başlatma
│   ├── StudentManagementSystem.Application/  # İş mantığı ve DTO'lar
│   ├── StudentManagementSystem.Domain/       # Varlıklar ve domain modelleri
│   ├── StudentManagementSystem.Infrastructure/ # Veritabanı bağlamı ve repository'ler
│   ├── Dockerfile                           # Backend Docker yapılandırması
│   └── docker-compose.yml                   # Docker Compose yapılandırması
├── frontend/                                # Next.js frontend uygulaması
│   ├── components/                          # Yeniden kullanılabilir UI bileşenleri
│   ├── pages/                               # Sayfa bileşenleri
│   ├── services/                            # API servis katmanı
│   ├── utils/                               # Yardımcı fonksiyonlar
│   └── Dockerfile                           # Frontend Docker yapılandırması
├── docker-compose.yml                       # Ana Docker Compose dosyası
└── README.md                               # Bu dosya
```

## Docker Yapılandırması

Uygulama, üç servisi yönetmek için Docker Compose kullanır:

1. **PostgreSQL Veritabanı**: Tüm uygulama verilerini saklar
2. **API Servisi**: RESTful uç noktaları ile .NET Core Web API
3. **Frontend Servisi**: Kullanıcı arayüzü için Next.js uygulaması

Tüm servisler bir Docker ağı üzerinden bağlanır ve birbirleriyle iletişim kurabilirler.

### Ortam Değişkenleri

Uygulama aşağıdaki ortam değişkenlerini kullanır (docker-compose.yml içinde yapılandırılır):

- Veritabanı bağlantı cümlesi
- JWT gizli anahtarı
- Port yapılandırmaları

### Veri Kalıcılığı

PostgreSQL verileri, konteynerler yeniden başlatıldığında veri kaybını önlemek için Docker birimleri kullanılarak kalıcı hale getirilir.

## Geliştirme

Projede Docker olmadan yerel olarak çalışmak için:

### Backend
1. .NET 9 SDK'yı yükleyin
2. Backend dizinine gidin
3. Aşağıdaki komutları çalıştırın:
   ```bash
   dotnet restore
   dotnet ef database update --project StudentManagementSystem.Infrastructure --startup-project StudentManagementSystem.API
   dotnet run --project StudentManagementSystem.API
   ```

### Frontend
1. Node.js'i yükleyin (18 veya üzeri sürüm)
2. frontend dizinine gidin
3. Aşağıdaki komutları çalıştırın:
   ```bash
   npm install
   npm run dev
   ```
