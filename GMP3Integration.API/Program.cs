using FluentValidation;
using GMP3Integration.Infrastructure.Interop;
using GMP3Integration.API.Filters;
using GMP3Integration.API.Middlewares;
using GMP3Integration.Application.Features.Behaviors;
using GMP3Integration.Application.Interfaces;
using GMP3Integration.Application.Options;
using GMP3Integration.Application.Services;
using GMP3Integration.Infrastructure.Services;
using GMP3Integration.Infrastructure.Services.Decorators;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
// Serilog: config + LogContext’tan zenginleştir(CorrelationId/transactionHandle scope’larını alır)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(); // ← default logger yerine Serilog

//builder.Services.AddScoped<Gmp3InteropService>();

builder.Services.AddScoped<GMP3Integration.Application.Interfaces.IGmp3Link,GMP3Integration.Infrastructure.Services.Gmp3Link>();
builder.Services.AddScoped<IGmp3Service, Gmp3InteropService>();
builder.Services.AddScoped<IGmp3Service>(sp =>
{
    var inner = sp.GetRequiredService<Gmp3InteropService>();
    var logger = sp.GetRequiredService<ILogger<ResilientGmp3Service>>();
    return new ResilientGmp3Service(inner, logger);
});

builder.Services.AddRateLimiter(options =>
{
    // Kuyruk dolduğunda dönecek HTTP kodu
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Reddedilince verilecek gövde
    options.OnRejected = async (ctx, token) =>
    {
        var pd = new ProblemDetails
        {
            Title = "Cihaz meşgul",
            Status = StatusCodes.Status429TooManyRequests,
            Detail = "Şu anda cihaz başka bir işlemi yürütüyor. Lütfen isteği kısa bir süre sonra tekrar deneyin.",
            Instance = ctx.HttpContext.Request.Path
        };
        ctx.HttpContext.Response.ContentType = "application/problem+json; charset=utf-8";
        await ctx.HttpContext.Response.WriteAsJsonAsync(pd, token);
    };

    // "device-serial" politikası: aynı anda 1 işlem; 100 istek bekleyebilir (FIFO)
    options.AddPolicy("device-serial", httpContext =>
        RateLimitPartition.GetConcurrencyLimiter(
            partitionKey: "gmp3-device",
            factory: _ => new ConcurrencyLimiterOptions
            {
                PermitLimit = 1,                       
                QueueLimit = 100,                      
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            })
    );
});

// Settings bind
builder.Services
    .AddOptions<Gmp3Options>()
    .Bind(builder.Configuration.GetSection("Gmp3"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.CurrentInterface), "Gmp3:CurrentInterface zorunlu.")
    //.Validate(o => !string.IsNullOrWhiteSpace(o.DllPath), "Gmp3:DllPath zorunlu.")
    .ValidateOnStart(); // Uygulama açılırken kontrol et


//  DI registrations
// Uygulama katmanındaki arayüzü, Infrastructure’daki implementasyonla eşle

builder.Services.AddScoped<TransactionHandleScopeFilter>(); 

builder.Services.AddTransient<IGmp3Service, Gmp3InteropService>();

builder.Services.AddTransient<ITransactionWorkflowService, TransactionWorkflowService>();

//  Swagger & Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        Assembly.Load("GMP3Integration.Application")
    );
});
builder.Services.AddValidatorsFromAssembly(Assembly.Load("GMP3Integration.Application"));
[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
static extern bool SetDllDirectory(string lpPathName);

var dllDir = Path.Combine(AppContext.BaseDirectory, "native", "win-x64");
Directory.SetCurrentDirectory(dllDir);

var xmlPath = Path.Combine(dllDir, "GMP.XML");
Console.WriteLine($"DLL dir: {dllDir} | XML exists: {File.Exists(xmlPath)}");

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("NativeAudit");
NativeAudit.Run(logger);

var nativePath = Path.Combine(AppContext.BaseDirectory, "native", "win-x64");
if (Directory.Exists(nativePath))
{
    SetDllDirectory(nativePath);
    Directory.SetCurrentDirectory(nativePath); // <- XML ve logs için kritik
    app.Logger.LogInformation("Native path: {np}", nativePath);
    app.Logger.LogInformation("CurrentDirectory: {cd}", Directory.GetCurrentDirectory());

    // XML var mı kontrol et
    string[] xmlCandidates = { "GMP.xml", "GMPSmartDLL.xml", "GMPConfig.xml" };
    foreach (var x in xmlCandidates)
        app.Logger.LogInformation("XML exists [{0}]: {1}", x, File.Exists(Path.Combine(nativePath, x)));
}
else
{
    app.Logger.LogWarning("Native path not found: {np}", nativePath);
}

var fwdOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
};
fwdOptions.KnownNetworks.Clear();
fwdOptions.KnownProxies.Clear();
fwdOptions.RequireHeaderSymmetry = false;
fwdOptions.ForwardLimit = null;
fwdOptions.KnownProxies.Add(System.Net.IPAddress.Parse("10.0.0.10")); 
app.UseForwardedHeaders(fwdOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.Use((ctx, next) =>
{
    var proto = ctx.Request.Headers["X-Forwarded-Proto"].ToString();
    if (!string.IsNullOrEmpty(proto) && proto.Equals("http", StringComparison.OrdinalIgnoreCase))
    {
        var host = ctx.Request.Headers["X-Forwarded-Host"].ToString();
        if (string.IsNullOrEmpty(host)) host = ctx.Request.Host.Value;
        var httpsUrl = $"https://{host}{ctx.Request.PathBase}{ctx.Request.Path}{ctx.Request.QueryString}";
        ctx.Response.Redirect(httpsUrl, permanent: false); 
        return Task.CompletedTask;
    }
    return next();
});


app.UseSerilogRequestLogging(opts =>
{
    // İsteğe özel ek alanlar (korelasyon vs.)
    opts.EnrichDiagnosticContext = (diag, http) =>
    {
        // Bizim middleware response header’a yazıyor
        var corrId = http.Response.Headers["X-Correlation-ID"].ToString();
        if (!string.IsNullOrEmpty(corrId))
            diag.Set("correlationId", corrId);

        diag.Set("requestPath", http.Request.Path.Value);
        diag.Set("queryString", http.Request.QueryString.Value);
        diag.Set("method", http.Request.Method);
    };
});

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseRateLimiter();                 // ← ekle
app.UseMiddleware<ApiExceptionMiddleware>();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();