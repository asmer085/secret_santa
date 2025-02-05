using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using secret_santa.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000") 
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Allow credentials (cookies)
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    await CreateRolesAsync(roleManager);

    await CreateAdminUserAsync(userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "ADMIN", "USER" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

async Task CreateAdminUserAsync(UserManager<IdentityUser> userManager)
{
    const string adminEmail = "admin@gmail.com";
    const string adminPassword = "Admin1!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true 
        };

        var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createUserResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "ADMIN");
            Console.WriteLine("Admin user created successfully.");
        }
        else
        {
            Console.WriteLine("Failed to create admin user.");
        }
    }
    else
    {
        Console.WriteLine("Admin user already exists.");
    }
}