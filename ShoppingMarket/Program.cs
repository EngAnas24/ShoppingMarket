using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingMarket.Data;
using AutoMapper;
using ShoppingMarket.Business;
using ShoppingMarket.Account.Extentions;
using ShoppingMarket.Account.Services;
using ShoppingMarket.Account.Models;
using ShoppingMarket.Account.Repositories.Implementations;
using ShoppingMarket.Account.Repositories.Interfaces;
using ShoppingMarket.Account.Services.Implementations;
using ShoppingMarket.Account.Services.Interfaces;
using ShoppingMarket.Models.JWTHelper;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(o =>
o.UseLazyLoadingProxies().UseSqlServer(builder.
    Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDbContext<MyIdentityDbContext>(o => o.UseSqlServer(builder.
    Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyIdentityDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderDetailsService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<FavoriteService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEmailService, EmailService>(); builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwtAuth();
builder.Services.AddCustomJwtAuth(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
