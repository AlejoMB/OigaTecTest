global using DataAccess;
global using Microsoft.EntityFrameworkCore;
global using DataAccess.UnitOfWork;
global using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using jwtOigaSln.ViewModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<OigaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("jwtOigaSln"));
});

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.UseSwagger();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Pagina Inicial!");

app.MapPost("/login", (UserLoginModel user, IUnitOfWork unitOfWork) => Login(user, unitOfWork));


app.MapGet("/get",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard")]
(int id, IUnitOfWork unitOfWork) => Get(id, unitOfWork));

IResult Get(int id, IUnitOfWork unitOfWork)
{
    var user = unitOfWork.User.GetById(id);
    
    if (user == null) return Results.NotFound("Usuario no encontrado");

    return Results.Ok(user);

}

app.MapGet("/List",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard")]
(int id, IUnitOfWork unitOfWork) => List(id, unitOfWork));

IResult List(IUnitOfWork unitOfWork)
{
    var user = unitOfWork.User.GetAll();

    if (user == null) return Results.NotFound("Usuario no encontrado");

    return Results.Ok(user);

}

IResult Login(UserLoginModel user, IUnitOfWork unitOfWork)
{
    if (!string.IsNullOrEmpty(user.UserName) &&
       !string.IsNullOrEmpty(user.Password))
    {
        var loggedInUser = unitOfWork.UserLogin.get(user.UserName, user.Password);
        if (loggedInUser is null) return Results.NotFound("Login Invalido");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.UserName),
            new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
            new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
            new Claim(ClaimTypes.Surname, loggedInUser.SureName),
            new Claim(ClaimTypes.Role, loggedInUser.Role)
        };

        var token = new JwtSecurityToken
        (
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(tokenString);
    }

    return Results.BadRequest("Login Invalido");
}

app.UseSwaggerUI();

app.Run();
