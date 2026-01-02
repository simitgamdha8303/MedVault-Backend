using MedVault.Web.Extension;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.AddApplicationServices();

// Database
builder.AddDatabase(builder.Configuration);

// Repositories
builder.AddRepositories();

// JWT Authentication
builder.AddJwtServices();

var app = builder.Build();

// Middleware
app.UseApplicationMiddleware();

app.Run();
