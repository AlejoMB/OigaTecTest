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
using Domain.Models;

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
(string text, int pageNumber, IUnitOfWork unitOfWork) => Get(text, pageNumber, unitOfWork));

IResult Get(string text,int pageNumber, IUnitOfWork unitOfWork)
{
    int pageSize = 10;
    string accentText = new string(text.Normalize(NormalizationForm.FormD).Where(c => c < 128).ToArray());

    var punctuationText = new String(accentText.Where (ch => Char.IsLetterOrDigit(ch) || Char.IsWhiteSpace(ch)).ToArray());

    var keyWords = punctuationText.Split(" ").ToList();

    
    var users = unitOfWork.User.GetAll().Where(u => keyWords.Any(x => u.FirstName.Contains(x, StringComparison.OrdinalIgnoreCase)) ||
                                                       keyWords.Any(x => u.LastName.Contains(x, StringComparison.OrdinalIgnoreCase)) ||
                                                       keyWords.Any(x => u.UserName.Contains(x, StringComparison.OrdinalIgnoreCase)))
                                           .OrderBy(u => u.FirstName)
                                           .ThenBy(x => x.LastName)
                                           .ThenBy(x => x.UserName);

    if (users == null || !users.Any()) return Results.NotFound("No Result Found");

    double totalPages = ((double)users.Count() / (double)pageSize);
    int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

    if(pageNumber > roundedTotalPages)
    {
        pageNumber = 1;
    }

    var result = new ResultViewModel()
    {
        PageNumber = pageNumber,
        TotalPages = roundedTotalPages,
        Users = users.Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList()
    };

    return Results.Ok(result);

}

app.MapGet("/List",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(IUnitOfWork unitOfWork) => List(unitOfWork));

IResult List(IUnitOfWork unitOfWork)
{
    var users = unitOfWork.User.GetAll();

    return Results.Ok(users);

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
