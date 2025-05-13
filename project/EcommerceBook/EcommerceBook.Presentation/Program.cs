using EcommerceBook.Application.Interfaces;
using EcommerceBook.Application.IServices;
using EcommerceBook.Application.Services;
using EcommerceBook.Domain.Interfaces;
using EcommerceBook.Domain.IRepository;
using EcommerceBook.Domain.Repositories;
using EcommerceBook.Infrastructure.Persistence;
using EcommerceBook.Infrastructure.Repositories;
using EcommerceBook.Infrastructure.Services;
using EcommerceBook.Presentation.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Frontend URL
              .AllowAnyMethod() // Allow any HTTP methods (GET, POST, etc.)
              .AllowAnyHeader(); // Allow any headers
    });
});

// 👇 Add Authentication before building app
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthentication(); // 👈 Add this
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});


builder.Services.AddAuthorization(); // 👈 Needed to use [Authorize] attribute

// Update DbContext to use MySQL
builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
// Register Book Service and Repository
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IBookmarkRepository, BookmarkRepository>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IBannerAnnouncementRepository, BannerAnnouncementRepository>();
builder.Services.AddScoped<IBannerAnnouncementService, BannerAnnouncementService>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<IEmailSender, GmailEmailSender>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EcommerceBook API", Version = "v1" });

    // JWT Authentication setup
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOi...\""
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp"); // ✅ APPLY CORS HERE
//app.UseCors("AllowAll");


app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
