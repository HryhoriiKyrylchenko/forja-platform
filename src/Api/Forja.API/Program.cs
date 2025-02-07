var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddInfrastructureServices();

// Add services to the container.

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Forja API", 
        Version = "v1",
        Description = "API for the Forja platform"
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forja API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();