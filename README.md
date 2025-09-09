# Öğrenci Otomasyon Sistemi

Bu proje, stajyer adayların basit bir öğrenci otomasyon sistemi geliştirmelerini hedefler. Proje, temel CRUD işlemleri, kullanıcı yönetimi ve basit raporlamadan oluşmaktadır.

## 🚀 Teknolojiler

- **Backend**: .NET 9
- **ORM**: Entity Framework Core
- **Frontend**: Blazor Server
- **Veritabanı**: PostgreSQL
- **Versiyon Kontrol**: GitHub

## 📁 Proje Yapısı

```
ogrenci-otomasyon-sistemi/
├── Backend/
│   ├── StudentManagementSystem.API/          # Web API Katmanı
│   ├── StudentManagementSystem.Application/ # Uygulama Katmanı (CQRS)
│   ├── StudentManagementSystem.Domain/      # Domain Katmanı (Entities, Enums)
│   ├── StudentManagementSystem.Infrastructure/ # Altyapı Katmanı (DbContext, Services)
│   └── StudentManagementSystem.sln          # Solution Dosyası
├── Frontend/
│   └── StudentManagementSystem.Web/          # Blazor Server Uygulaması
└── README.md
```

## 🏗️ Mimari

Proje **Clean Architecture** prensiplerine uygun olarak tasarlanmıştır:

- **Domain Layer**: İş mantığı ve entity'ler
- **Application Layer**: CQRS pattern ile servisler
- **Infrastructure Layer**: Veritabanı ve dış servisler
- **API Layer**: Web API controllers

## 🛠️ Kurulum ve Çalıştırma

### Gereksinimler

- .NET 9 SDK
- PostgreSQL
- Visual Studio 2022 veya VS Code

### 1. Veritabanı Kurulumu

PostgreSQL'de yeni bir veritabanı oluşturun:

```sql
CREATE DATABASE StudentManagementSystem;
```

### 2. Backend Kurulumu

```bash
cd Backend
dotnet restore
dotnet ef database update --project StudentManagementSystem.Infrastructure --startup-project StudentManagementSystem.API
dotnet run --project StudentManagementSystem.API
```

### 3. Frontend Kurulumu

```bash
cd Frontend/StudentManagementSystem.Web
dotnet restore
dotnet run
```

## 🔐 Test Kullanıcıları

### Admin Kullanıcısı
- **Email**: admin@test.com
- **Şifre**: Admin123!
- **Rol**: Admin

### Öğretmen Kullanıcısı
- **Email**: teacher@test.com
- **Şifre**: Teacher123!
- **Rol**: Teacher

### Öğrenci Kullanıcısı
- **Email**: student@test.com
- **Şifre**: Student123!
- **Rol**: Student

## 📋 Özellikler

### ✅ Tamamlanan Özellikler

- [x] Clean Architecture kurulumu
- [x] Entity Framework Core ve PostgreSQL entegrasyonu
- [x] Domain modelleri (User, Student, Teacher, Course, Grade, Attendance)
- [x] JWT Authentication sistemi
- [x] Swagger API dokümantasyonu
- [x] Blazor Server frontend kurulumu
- [x] CORS konfigürasyonu

### 🚧 Devam Eden Özellikler

- [ ] Authentication ve Authorization implementasyonu
- [ ] CRUD operasyonları (Commands & Queries)
- [ ] Frontend sayfaları
- [ ] Validasyon kuralları
- [ ] Unit testler

## 🔧 API Endpoints

### Authentication
- `POST /api/auth/login` - Kullanıcı girişi
- `POST /api/auth/register` - Kullanıcı kaydı

### Users
- `GET /api/users` - Tüm kullanıcıları listele
- `GET /api/users/{id}` - Kullanıcı detayı
- `PUT /api/users/{id}` - Kullanıcı güncelle
- `DELETE /api/users/{id}` - Kullanıcı sil

### Students
- `GET /api/students` - Öğrenci listesi
- `POST /api/students` - Yeni öğrenci ekle
- `PUT /api/students/{id}` - Öğrenci güncelle
- `DELETE /api/students/{id}` - Öğrenci sil

### Teachers
- `GET /api/teachers` - Öğretmen listesi
- `POST /api/teachers` - Yeni öğretmen ekle
- `PUT /api/teachers/{id}` - Öğretmen güncelle
- `DELETE /api/teachers/{id}` - Öğretmen sil

### Courses
- `GET /api/courses` - Ders listesi
- `POST /api/courses` - Yeni ders ekle
- `PUT /api/courses/{id}` - Ders güncelle
- `DELETE /api/courses/{id}` - Ders sil

## 🧪 Test

```bash
# Backend testleri
cd Backend
dotnet test

# Frontend testleri
cd Frontend/StudentManagementSystem.Web
dotnet test
```

## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

## 👥 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Commit yapın (`git commit -m 'Add some AmazingFeature'`)
4. Push yapın (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📞 İletişim

Proje hakkında sorularınız için issue açabilirsiniz.
