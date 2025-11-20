using APP.DataAccess.Context;
using APP.Business.Services;
using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Db>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Db")));

builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
builder.Services.AddScoped<IService<PlaylistRequest, PlaylistResponse>, PlaylistService>();
builder.Services.AddScoped<IService<TrackRequest, TrackResponse>, TrackService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Db>();
    db.Database.EnsureCreated();
}

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