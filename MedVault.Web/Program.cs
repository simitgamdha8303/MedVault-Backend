using MedVault.Web.Extension;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.AddApplicationServices();

// Database
builder.AddDatabase(builder.Configuration);

// Repositories
builder.AddRepositories();

var app = builder.Build();

// Middleware
app.UseApplicationMiddleware();

app.Run();
