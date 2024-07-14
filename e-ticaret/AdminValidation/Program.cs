using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//appsettings.json dpsyas�ndan gerekli token bilgileri al�n�r
var jwtsettings = builder.Configuration.GetSection("jwtSettings");
var key = Encoding.UTF8.GetBytes(jwtsettings["key"]);
var validIssuer = jwtsettings["ValidIssuer"];
var validAudience = jwtsettings["ValidAudience"];
//.......


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, // Issuer do�rulamas� yap�laca�� s�ylenir
        ValidateAudience = true, // Audience  do�rulamas� yap�laca�� s�ylenir
        ValidateLifetime = true, // Ya�am s�resi olaca��n� ve do�rulamas�n�n yap�laca�� s�ylenir
        ValidateIssuerSigningKey = true, // SignInKey do�rulamas� yap�laca�� s�ylenir
        ValidIssuer = validIssuer,
        ValidAudience = validAudience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
    


builder.Services.AddControllers();
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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
