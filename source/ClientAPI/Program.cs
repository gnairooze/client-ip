var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/ip", (HttpContext context) =>
{
    string? ip = string.Empty;
    string headerValue = builder.Configuration["header-key"];

    if (string.IsNullOrEmpty(headerValue))
    {
        ip = context.Connection.RemoteIpAddress?.ToString();
    }
    else
    {
        Microsoft.Extensions.Primitives.StringValues tempValue;
        context.Request.Headers.TryGetValue(headerValue, out tempValue);

        ip = tempValue.ToString();
    }

    return (!string.IsNullOrEmpty(ip))? ip: "cannot get ip";
})
.WithName("GetIP");

app.Run();
