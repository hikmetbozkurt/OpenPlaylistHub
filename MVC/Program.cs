var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// TODO HW4: Add DbContext configuration
// Example (to be implemented in HW4):
// builder.Services.AddDbContext<Db>(options => 
//     options.UseSqlite(builder.Configuration.GetConnectionString("Db")));

// TODO HW4: Add service DI registrations
// Example service registrations (to be implemented in HW4):
// builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
// builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
// builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
// builder.Services.AddScoped<IService<PlaylistRequest, PlaylistResponse>, PlaylistService>();
// builder.Services.AddScoped<IService<TrackRequest, TrackResponse>, TrackService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
