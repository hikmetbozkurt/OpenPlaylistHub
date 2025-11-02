using APP.Domain;
using APP.Models;
using APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Db>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Db")));

builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
