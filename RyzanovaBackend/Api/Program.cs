using Api.Middleware;
using Application.Services;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Database;
using Infrastructure.External.WayForPay;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IThemeRepository, SqlThemeRepository>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<IVideoRepository, SqlVideoRepository>();
builder.Services.AddScoped<VideoService>();
builder.Services.AddScoped<IPhotoRepository, SqlPhotoRepository>();
builder.Services.AddScoped<PhotoService>();

builder.Services.AddScoped<IPaymentRepository, SqlPaymentRepository>();
builder.Services.AddScoped<PaymentService>();

builder.Services.Configure<WayForPayOptions>(
    builder.Configuration.GetSection(WayForPayOptions.SectionName));

builder.Services
    .AddHttpClient<IWayForPayClient, WayForPayClient>()
    .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(15));
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
