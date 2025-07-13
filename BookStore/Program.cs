using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controllers
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnection")));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddControllers();

// CORS - Cross-Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Entity Framework Core (örnek - SQL Server)
// builder.Services.AddDbContext<BookStoreContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication & Authorization
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options => { /* JWT config */ });

// Caching
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

// Logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Custom Services (Dependency Injection)
// builder.Services.AddTransient<IEmailService, EmailService>(); // every time requested
// builder.Services.AddScoped<IBookService, BookService>(); one per request
// builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // one per application lifetime

// API Versioning
// builder.Services.AddApiVersioning();

// Health Checks
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Production Environment
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

// Middleware Pipeline Order (ÖNEMLİ - Sıra önemlidir!)
app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAll");

// Static Files
app.UseStaticFiles();

// Routing
app.UseRouting();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Caching
app.UseResponseCaching();

// Health Checks
app.MapHealthChecks("/health");

// Controllers
app.MapControllers();

// Custom Endpoints
// app.MapGet("/", () => "BookStore API is running!");

app.Run();
