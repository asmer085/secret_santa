using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using secret_santa.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Configure the database context.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configure CORS.
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

// Configure Identity.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configure application cookies.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None; // Allow cross-site cookies
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are sent over HTTPS
});

var app = builder.Build();
using var noviScope = app.Services.CreateScope();
var dbContext = noviScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
dbContext.Database.Migrate();
// Create roles and admin user on application startup.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    await CreateRolesAsync(roleManager);
    await CreateAdminUserAsync(userManager);
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Helper method to create roles.
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

// Helper method to create the admin user.
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