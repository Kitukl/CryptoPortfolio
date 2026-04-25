using CryptoAnalyzer.Portfolio.BLL.Exceptions;
using CryptoAnalyzer.Portfolio.BLL.Queries;
using CryptoAnalyzer.Portfolio.DAL;
using CryptoAnalyzer.Portfolio.DAL.Repositories;
using CryptoAnalyzer.Portfolio.Extensions;
using dotenv.net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(GetCoinNamesQueryHandler).Assembly));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PortfolioContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<ICoinRepository, CoinRepository>();  
builder.Services.AddScoped<IHoldingRepository, HoldingRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "GeckoPrices";
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionsHandler<,>));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddJwtAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JWTOptions>>());

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();