using ClientAPI;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
Options settings = ReadOptions(builder);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {            
            policy.WithOrigins(settings.AllowedOrigins.Split(","));
            policy.WithMethods(settings.AllowedMethods.Split(","));
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);

app.MapGet("/ip", (HttpContext context) =>
{
    string? ip = string.Empty;

    if (string.IsNullOrEmpty(settings.HeaderKey))
    {
        ip = context.Connection.RemoteIpAddress?.ToString();
    }
    else
    {
        context.Request.Headers.TryGetValue(settings.HeaderKey, out Microsoft.Extensions.Primitives.StringValues tempValue);

        ip = tempValue.ToString();
    }

    if(settings.FirstIpOnly && !string.IsNullOrEmpty(ip))
    {
        ip = ip.Split(',')[0];
    }

    return (!string.IsNullOrEmpty(ip))? ip: "cannot get ip";
})
.WithName("GetIP");

app.Run();

static Options ReadOptions(WebApplicationBuilder builder)
{
    return new Options()
    {
        HeaderKey = builder.Configuration["headerKey"],
        AllowedOrigins = builder.Configuration["allowedOrigins"],
        AllowedMethods = builder.Configuration["allowedMethods"],
        FirstIpOnly = builder.Configuration.GetValue<bool>("firstIpOnly", false)
    };
}