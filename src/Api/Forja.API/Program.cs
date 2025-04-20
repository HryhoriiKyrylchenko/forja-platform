var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.AddServiceDefaults();

builder.AddInfrastructureServices();

// Add services to the container.

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "RedisInstance";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
       policy =>
       {
           policy.WithOrigins(
                   "http://localhost:3000",
                   "https://localhost:3000",
                   "http://localhost:3003",
                   "https://localhost:3003"
               )
               .AllowCredentials() 
               .AllowAnyHeader()
               .AllowAnyMethod();
       });
});

builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer("keycloak", realm: "forja", options =>
    {
        options.Authority = "http://localhost:8080/realms/forja";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:8080/realms/forja",
            ValidateAudience = false,
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = "preferred_username"
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Request.Cookies.TryGetValue("access_token", out var token);
                context.Token = token;
              
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorizationBuilder();

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor(); 

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Forja API", 
        Version = "v1",
        Description = "API for the Forja platform"
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the token in the format: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Administrator", "SystemAdministrator"));
    options.AddPolicy("UserViewPolicy", policy =>
        policy.RequireRole("Administrator", "SystemAdministrator", "Moderator", "SalesManager", "SupportManager", "AnalyticsManager"));
    options.AddPolicy("UserManagePolicy", policy =>
        policy.RequireRole("Administrator", "SystemAdministrator", "Moderator", "SupportManager"));
    options.AddPolicy("ContentManagePolicy", policy =>
        policy.RequireRole("Administrator", "SystemAdministrator", "Moderator", "ContentManager"));
    options.AddPolicy("ModeratePolicy", policy =>
        policy.RequireRole("Administrator", "SystemAdministrator", "Moderator"));
    options.AddPolicy("StoreManagePolicy", policy =>
        policy.RequireRole("Administrator", "SystemAdministrator", "SalesManager"));
});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();