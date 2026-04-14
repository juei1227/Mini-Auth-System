# Mini-Auth-System

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-purple.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![JWT](https://img.shields.io/badge/JWT-Token-green.svg)](https://jwt.io/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

一個簡單的 .NET 8 Web API 身份驗證系統，實現基本的註冊、登入功能，並使用 JWT Token 進行身份驗證。

## 功能特色

- 🔐 **安全的密碼處理**：使用 Salt + Hash (PBKDF2) 保護密碼
- 🎫 **JWT Token 認證**：無狀態的身份驗證機制，支援過期時間設定
- 📊 **記憶體資料庫**：使用 Entity Framework Core InMemory 進行快速開發測試
- 🔓 **分層權限控制**：公開端點與受保護端點的區分
- 📚 **完整的 API 文檔**：整合 Swagger UI

## 技術架構

- **框架**：.NET 8.0
- **架構模式**：Web API with Controllers
- **身份驗證**：JWT Bearer Token
- **資料庫**：Entity Framework Core (InMemory)
- **密碼雜湊**：PBKDF2 with SHA256
- **API 文檔**：Swagger/OpenAPI

## API 端點

### 公開端點 (無需認證)

- `GET /api/public/info` - 獲取系統資訊
- `GET /api/public/health` - 健康檢查

### 身份驗證端點

- `POST /api/auth/register` - 用戶註冊
- `POST /api/auth/login` - 用戶登入

### 受保護端點 (需要 JWT Token)

- `GET /api/protected/profile` - 獲取用戶個人資料
- `GET /api/protected/data` - 獲取受保護的資料

## 為什麼選擇 JWT 而不是 Session？

### JWT 的優勢

1. **無狀態 (Stateless)**：
   - 不需要在伺服器端儲存 Session 狀態
   - 每個請求都包含完整的身份驗證資訊
   - 有利於水平擴展 (Horizontal Scaling)

2. **跨域支援 (CORS Friendly)**：
   - JWT Token 可以輕鬆在不同域名間傳遞
   - 適合現代的前後端分離架構

3. **效能優化**：
   - 減少資料庫查詢 (無需每次請求都查詢 Session)
   - Token 驗證是 CPU 密集型而非 I/O 密集型

4. **安全性**：
   - Token 有內建過期機制
   - 可以包含額外的聲明 (Claims) 用於授權
   - 支援重新整理 Token 的機制

5. **多平台支援**：
   - 同一個 Token 可以用於 Web、Mobile、API 等不同平台

### Session 的限制

- 需要伺服器端儲存狀態，增加記憶體/資料庫負擔
- 難以在分散式系統中共享 Session
- 跨域請求複雜
- 擴展性較差

## 專案結構

```
MiniAuthSystem/
├── Controllers/          # API 控制器
│   ├── AuthController.cs     # 身份驗證相關端點
│   ├── PublicController.cs   # 公開訪問端點
│   └── ProtectedController.cs # 受保護端點
├── Models/              # 資料模型
│   ├── User.cs              # 用戶模型
│   └── AuthModels.cs        # 認證請求/回應模型
├── Services/            # 業務邏輯服務
│   └── AuthService.cs       # 身份驗證服務
├── AppDbContext.cs      # 資料庫上下文
├── Program.cs           # 應用程式入口點
└── appsettings.json     # 應用程式設定
```

## 快速開始

### 環境需求

- .NET 8.0 SDK
- Visual Studio 2022 或 VS Code

### 執行專案

```bash
# 進入專案目錄
cd MiniAuthSystem

# 還原 NuGet 套件
dotnet restore

# 執行應用程式
dotnet run
```

應用程式將在 `https://localhost:5001` 啟動，並提供 Swagger UI：`https://localhost:5001/swagger`

### 測試 API

1. **註冊新用戶**：
```bash
POST https://localhost:5001/api/auth/register
Content-Type: application/json

{
  "username": "testuser",
  "email": "test@example.com",
  "password": "password123"
}
```

2. **登入獲取 Token**：
```bash
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "password123"
}
```

3. **訪問受保護端點**：
```bash
GET https://localhost:5001/api/protected/profile
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

## 安全性考量

### 密碼安全
- 使用 PBKDF2 演算法進行密碼雜湊
- 每個密碼都有獨特的 Salt
- 迭代次數設定為 10,000 次以增加計算複雜度

### JWT Token 安全
- Token 過期時間設定為 1 小時
- 使用強密鑰進行簽署
- 支援標準的 JWT 聲明

### 生產環境建議
- 將 JWT Secret Key 儲存在環境變數或安全的金鑰管理系統
- 考慮使用 HTTPS
- 實作 Token 重新整理機制
- 添加請求率限制 (Rate Limiting)

## 未來擴展架構

### 多語系支援 (i18n)

1. **新增資源檔案**：
```
Resources/
├── Messages.resx          # 預設語言
├── Messages.zh-TW.resx    # 繁體中文
└── Messages.en-US.resx    # 英文
```

2. **修改服務層**：
```csharp
public class AuthService : IAuthService
{
    private readonly IStringLocalizer<AuthService> _localizer;

    public AuthService(IStringLocalizer<AuthService> localizer)
    {
        _localizer = localizer;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // 使用本地化訊息
        return new AuthResponse
        {
            Success = false,
            Message = _localizer["UsernameAlreadyExists"]
        };
    }
}
```

3. **配置本地化**：
```csharp
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("zh-TW"),
    SupportedCultures = new[] { new CultureInfo("zh-TW"), new CultureInfo("en-US") },
    SupportedUICultures = new[] { new CultureInfo("zh-TW"), new CultureInfo("en-US") }
});
```

### 第三方登入整合 (Google/GitHub OAuth)

1. **安裝 OAuth 套件**：
```bash
dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.GitHub
```

2. **擴展 User 模型**：
```csharp
public class User
{
    // ... 現有欄位

    public string? GoogleId { get; set; }
    public string? GitHubId { get; set; }
    public string? Provider { get; set; } // "Local", "Google", "GitHub"
}
```

3. **新增 OAuth 控制器**：
```csharp
[ApiController]
[Route("api/auth")]
public class OAuthController : ControllerBase
{
    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action("GoogleCallback", "OAuth");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded) return BadRequest();

        // 處理 Google 用戶資訊並建立/更新本地用戶
        var claims = result.Principal.Identities.First().Claims;
        var googleId = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var email = claims.First(c => c.Type == ClaimTypes.Email).Value;

        // ... 建立 JWT Token
    }
}
```

4. **配置 OAuth 在 Program.cs**：
```csharp
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = configuration["Authentication:Google:ClientId"];
        options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    })
    .AddGitHub(options =>
    {
        options.ClientId = configuration["Authentication:GitHub:ClientId"];
        options.ClientSecret = configuration["Authentication:GitHub:ClientSecret"];
    });
```

5. **更新 AuthService**：
```csharp
public async Task<AuthResponse> LoginWithOAuthAsync(string provider, string providerId, string email)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Provider == provider && u.GoogleId == providerId);

    if (user == null)
    {
        // 建立新用戶
        user = new User
        {
            Email = email,
            Provider = provider,
            GoogleId = providerId,
            Username = $"{provider}_{providerId}",
            PasswordHash = "", // OAuth 用戶不需要密碼
            PasswordSalt = ""
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    var token = GenerateJwtToken(user);
    return new AuthResponse
    {
        Success = true,
        Message = "OAuth login successful",
        Token = token,
        ExpiresAt = DateTime.UtcNow.AddHours(1)
    };
}
```

### 其他擴展方向

1. **角色-based 授權 (RBAC)**：
   - 新增 Role 和 Permission 模型
   - 實作 [Authorize(Roles = "Admin")] 特性

2. **Token 重新整理機制**：
   - 新增 Refresh Token 功能
   - 實作 Token 黑名單機制

3. **API 版本控制**：
   - 使用 API Versioning 套件
   - 實作向後相容性

4. **日誌和監控**：
   - 整合 Serilog 或 NLog
   - 添加應用程式指標 (Application Metrics)

5. **快取機制**：
   - 整合 Redis 進行 Token 快取
   - 實作回應快取

## 授權

此專案僅供學習和參考使用。