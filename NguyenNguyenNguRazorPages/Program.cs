using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<Repositories.ISystemAccountRepo, Repositories.SystemAccountRepo>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Đường dẫn mặc định khi chưa đăng nhập
        options.AccessDeniedPath = "/AccessDenied"; // Đường dẫn khi không đủ quyền
    });
builder.Services.AddScoped<Repositories.INewsArticleRepo, Repositories.NewsArticleRepo>();
builder.Services.AddScoped<Repositories.ICategoryRepo, Repositories.CategoryRepo>();
builder.Services.AddScoped<Repositories.ITagRepo, Repositories.TagRepo>(); // Đăng ký Tag Repository
builder.Services.AddSignalR(); // Đăng ký dịch vụ SignalR
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapHub<NguyenNguyenNguRazorPages.Hubs.NewsHub>("/newshub"); // Định tuyến Hub SignalR

app.Run();
