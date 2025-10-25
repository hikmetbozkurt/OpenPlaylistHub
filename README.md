# OpenPlaylistHub

ASP.NET Core MVC project for managing playlists - developed following the ProductsMVC lecture structure.

---

## Homework 3 Status

### ✅ Completed (HW3 Scope)

#### **1. Solution Structure**
- ✅ Created **CORE** Class Library project (.NET 8)
- ✅ Created **APP** Class Library project (.NET 8)
- ✅ Added project references:
  - `APP → CORE` (APP references CORE)
  - `MVC → APP` (MVC references APP)

#### **2. NuGet Packages Installed**
- ✅ **CORE**: `Microsoft.EntityFrameworkCore` v8.0.0
- ✅ **APP**: `Microsoft.EntityFrameworkCore.Sqlite` v8.0.0, `System.Data.SQLite.Core` v1.0.118
- ✅ **MVC**: `Microsoft.EntityFrameworkCore.Tools` v8.0.0

#### **3. CORE Project - Base Types (Empty Shells)**
Created folder structure matching teacher's ProductsMVC:
```
CORE/
  APP/
    Domain/
      ✅ Entity.cs              (base entity class - empty)
    Models/
      ✅ Request.cs             (base request - empty)
      ✅ Response.cs            (base response - empty)
      ✅ CommandResponse.cs     (command response - empty)
    Services/
      ✅ Service.cs             (abstract generic service - empty)
      MVC/
        ✅ IService.cs          (generic service interface - empty)
```

#### **4. APP Project - Domain Entities (Empty Shells)**

**USERS Module (Required):**
- ✅ `User.cs` (like `Product` in ProductsMVC)
- ✅ `Group.cs` (like `Category` in ProductsMVC)
- ✅ `Role.cs` (like `Store` in ProductsMVC)
- ✅ `UserRole.cs` (join entity, like `ProductStore` in ProductsMVC)
- ✅ `Db.cs` (DbContext - empty)

**Playlists Module (Custom Domain):**
- ✅ `Playlist.cs`
- ✅ `Track.cs`
- ✅ `PlaylistTrack.cs` (join entity)
- ✅ `Artist.cs`
- ✅ `TrackArtist.cs` (join entity)

#### **5. APP Project - DTOs (Empty Shells)**

**USERS Module:**
- ✅ `UserRequest.cs` / `UserResponse.cs`
- ✅ `GroupRequest.cs` / `GroupResponse.cs`
- ✅ `RoleRequest.cs` / `RoleResponse.cs`

**Playlists Module:**
- ✅ `PlaylistRequest.cs` / `PlaylistResponse.cs`
- ✅ `TrackRequest.cs` / `TrackResponse.cs`

#### **6. APP Project - Services (Empty Shells)**
- ✅ `UserService.cs` (like `ProductService` in ProductsMVC)
- ✅ `GroupService.cs` (like `CategoryService` in ProductsMVC)
- ✅ `RoleService.cs` (like `StoreService` in ProductsMVC)
- ✅ `PlaylistService.cs`
- ✅ `TrackService.cs`

#### **7. MVC Project - Configuration Placeholders**
- ✅ SQLite connection string added to `appsettings.json`
- ✅ TODO comments added in `Program.cs` for:
  - DbContext configuration
  - Service DI registrations

---

### ❌ Not Implemented (Reserved for HW4+)

#### **Code Implementation:**
- ❌ NO properties in entity classes
- ❌ NO properties in base types (Entity, Request, Response, CommandResponse)
- ❌ NO DbSet properties in Db.cs
- ❌ NO OnModelCreating configurations
- ❌ NO method implementations in Service.cs
- ❌ NO interface method signatures in IService.cs
- ❌ NO service implementations (UserService, GroupService, etc.)
- ❌ NO DataAnnotations on DTOs
- ❌ NO navigation properties in entities

#### **Database & Migrations:**
- ❌ NO migrations created (`dotnet ef migrations add`)
- ❌ NO database created (`dotnet ef database update`)
- ❌ NO SQLite database file generated

#### **MVC Wiring:**
- ❌ NO actual DI registrations in Program.cs
- ❌ NO AddDbContext implementation
- ❌ NO controllers created (except default HomeController)
- ❌ NO views created (except default Home views)
- ❌ NO scaffolding

---

### 📐 Relationships Planned (To Be Implemented in HW4)

#### **USERS Module:**
1. **One-to-Many:**
   - `Group (1) → (∞) User`
   - One Group can have many Users

2. **Many-to-Many:**
   - `User (∞) ↔ (∞) Role` via `UserRole` join table
   - Users can have multiple Roles, Roles can be assigned to multiple Users

#### **Playlists Module:**
1. **One-to-Many:**
   - `Playlist (1) → (∞) PlaylistTrack`
   - One Playlist can have many Tracks (via join)

2. **Many-to-Many:**
   - `Playlist (∞) ↔ (∞) Track` via `PlaylistTrack`
   - Playlists can contain multiple Tracks, Tracks can appear in multiple Playlists

3. **Many-to-Many (Optional):**
   - `Track (∞) ↔ (∞) Artist` via `TrackArtist`
   - Tracks can have multiple Artists, Artists can have multiple Tracks

#### **Data Types to Include:**
- `string` (names, titles, descriptions)
- `int` (IDs, counts)
- `decimal` (ratings, scores, prices)
- `DateTime` (registration dates, publish dates, birth dates)
- `bool` (IsActive, IsPublic, etc.)

---

### 📚 Project Architecture

This project follows the **N-Layered Architecture** as demonstrated in the teacher's ProductsMVC repository:

- **CORE Layer**: Base types, generic abstractions, interfaces
- **APP Layer**: Domain entities, DTOs, business logic services, DbContext
- **MVC Layer**: Controllers, Views, presentation logic

**Pattern Used:** CQRS (Command Query Response Segregation) via Request/Response DTOs

**Reference:** [Teacher's ProductsMVC Repository](https://github.com/cagilalsac/ProductsMVC)

---

### 🎯 Next Steps (Homework 4)

1. Implement entity properties with proper data types
2. Configure entity relationships in DbContext (OnModelCreating)
3. Add DbSet properties to Db.cs
4. Implement CORE generic Service and IService
5. Implement concrete services (UserService, GroupService, etc.)
6. Add DataAnnotations to Request DTOs
7. Configure DI in Program.cs
8. Run migrations: `dotnet ef migrations add v1` and `dotnet ef database update`
9. Create controllers and views

---

### 🛠️ Technologies

- **.NET 8.0**
- **ASP.NET Core MVC**
- **Entity Framework Core 8.0**
- **SQLite Database**
- **C#**

---

### 📦 Project Structure

```
OpenPlaylistHub/
├── CORE/                    # Base types and abstractions
│   └── APP/
│       ├── Domain/          # Base entity
│       ├── Models/          # Base request/response models
│       └── Services/        # Generic service abstractions
│
├── APP/                     # Application logic
│   ├── Domain/              # Entities and DbContext
│   ├── Models/              # DTOs (Request/Response)
│   └── Services/            # Business logic services
│
├── MVC/                     # Presentation layer
│   ├── Controllers/         # MVC controllers
│   ├── Views/               # Razor views
│   └── wwwroot/             # Static files
│
└── README.md                # This file
```

---

**Course:** .NET MVC Development  
**Assignment:** Homework 3 - Project Structure & Layering  
**Status:** ✅ Complete (HW3 scope only - no implementations yet)

