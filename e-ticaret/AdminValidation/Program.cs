using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//appsettings.json dpsyasýndan gerekli token bilgileri alýnýr
var jwtsettings = builder.Configuration.GetSection("jwtSettings");
var key = Encoding.UTF8.GetBytes(jwtsettings["key"]);
var validIssuer = jwtsettings["ValidIssuer"];
var validAudience = jwtsettings["ValidAudience"];
//.......


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, // Issuer doðrulamasý yapýlacaðý söylenir
        ValidateAudience = true, // Audience  doðrulamasý yapýlacaðý söylenir
        ValidateLifetime = true, // Yaþam süresi olacaðýný ve doðrulamasýnýn yapýlacaðý söylenir
        ValidateIssuerSigningKey = true, // SignInKey doðrulamasý yapýlacaðý söylenir
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
